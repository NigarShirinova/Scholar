# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Upload və static files üçün wwwroot qovluğu yaradılır
RUN mkdir -p /app/wwwroot/uploads

# SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation/Presentation.csproj", "Presentation/"]
COPY ["Business/Business.csproj", "Business/"]
COPY ["Data/Data.csproj", "Data/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore "./Presentation/Presentation.csproj"
COPY . .
WORKDIR "/src/Presentation"
RUN dotnet build "./Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Yenidən mütləq qovluğu yaradırıq əgər build zamanı gəlməyibsə
RUN mkdir -p /app/wwwroot/uploads

# Optional: permission (Linux üçün)
 RUN chmod -R 777 /app/wwwroot/uploads

ENTRYPOINT ["dotnet", "Presentation.dll"]

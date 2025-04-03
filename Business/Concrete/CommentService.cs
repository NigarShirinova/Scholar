using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.ViewModels.Comment;
using Common.Entities;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Business.Concrete
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(ICommentRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AddCommentAsync(CommentCreateVM commentCreateVM)
        {
           
           var comment = new Comment
            {
                FullName = commentCreateVM.FullName,
                Content = commentCreateVM.Content,
                CreatedAt = DateTime.UtcNow 
            };

          
            await _repository.CreateAsync(comment);
            await _unitOfWork.CommitAsync();
            return true;
        }

    }
}

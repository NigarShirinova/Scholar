using Business.Abstract;
using Business.ViewModels.Contact;
using Business.ViewModels.Email;
using Common.Entities;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class ContactMessageService : IContactMessageService
	{
		private readonly IContactMessageRepository _contactMessageRepository;
		private readonly ModelStateDictionary _modelState;
		private readonly IUnitOfWork _unitOfWork;

		public ContactMessageService(IContactMessageRepository contactMessageRepository, IActionContextAccessor actionContextAccessor, IUnitOfWork unitOfWork)
		{
			_contactMessageRepository = contactMessageRepository;
			_modelState = actionContextAccessor.ActionContext.ModelState;
			_unitOfWork = unitOfWork;
		}
		public async Task MarkAsReadAsync(int id)
		{
			var message = await _contactMessageRepository.GetAsync(id);
			if (message != null)
			{
				message.IsRead = true;
				_contactMessageRepository.Update(message);
			}
		}

        public async Task<bool> CreateMessageAsync(ContactMessageCreateVM model)
        {
            var contactMessage = new ContactMessage
            {
                Email = model.Email,
                Name = model.Name,
                Content = model.Content,
                CreatedAt = DateTime.Now,
            };

            await _contactMessageRepository.CreateAsync(contactMessage);
            await _unitOfWork.CommitAsync();

            return true;
        }


    }
	}


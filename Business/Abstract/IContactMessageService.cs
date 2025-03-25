using Business.ViewModels.Contact;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IContactMessageService
	{
		Task<bool> CreateMessageAsync(ContactMessageCreateVM message);
		Task MarkAsReadAsync(int id);
	}
}

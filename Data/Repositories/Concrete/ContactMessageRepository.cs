using Common.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Concrete
{
	public class ContactMessageRepository : BaseRepository<ContactMessage>, IContactMessageRepository
	{
		public ContactMessageRepository(AppDbContext context) : base(context)
		{
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
	public class ContactMessage : BaseEntity
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Content { get; set; }
		public bool IsRead { get; set; } = false;
	}
}

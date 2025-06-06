﻿using System.ComponentModel.DataAnnotations;

namespace Presentation.Areas.Admin.Models.News
{
	public class NewsUpdateVM
	{
		[Required]
		public string Title { get; set; }
		[Required]
		public string Description { get; set; }	
		[Required]
		public string PhotoName { get; set; }
		[Required]
		public string FullContent { get; set; }
	}
}

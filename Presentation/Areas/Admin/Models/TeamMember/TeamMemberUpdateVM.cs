using System.ComponentModel.DataAnnotations;

namespace Presentation.Areas.Admin.Models.TeamMember
{
	public class TeamMemberUpdateVM
	{
		[Required]
		public string FullName { get; set; }
		[Required]
		public string Position { get; set; }
 
       
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string FacebookLink { get; set; }
        [Required]
        public string InstagramLink { get; set; }
        [Required]
        public string LinkedinLink { get; set; }
    }
}

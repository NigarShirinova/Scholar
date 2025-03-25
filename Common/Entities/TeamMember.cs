using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
	public class TeamMember : BaseEntity
	{
        public string FullName { get; set; }
        public string Position { get; set; }
        public string PhotoName { get; set; }
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string LinkedinLink { get; set; }
    }
}

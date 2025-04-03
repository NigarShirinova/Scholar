using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ViewModels.Comment;
using Business.ViewModels.Contact;
using Common.Entities;

namespace Business.ViewModels.Home
{
    public class HomeIndexVM
    {
        public string? UserName { get; set; }
        public List<TeamMember>? teamMembers { get; set; }
        public ContactMessageCreateVM ContactMessage { get; set; }
        public CommentCreateVM Comment { get; set; }
        public List<Common.Entities.Comment> Comments { get; set; }

    }
}

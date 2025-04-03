using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.Comment
{
    public class CommentCreateVM
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Content { get; set; }
    }
}

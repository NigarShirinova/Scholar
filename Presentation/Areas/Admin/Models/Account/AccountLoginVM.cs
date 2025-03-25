using System.ComponentModel.DataAnnotations;

namespace Presentation.Areas.Admin.Models.Controller
{
    public class AccountLoginVM
    {
        [Required(ErrorMessage = "You must enter this!")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must enter this!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

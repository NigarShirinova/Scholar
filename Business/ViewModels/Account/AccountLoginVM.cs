using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.Account
{
    public class AccountLoginVM
    {
        [Required(ErrorMessage = "It is a required field!")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "It is a required field!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

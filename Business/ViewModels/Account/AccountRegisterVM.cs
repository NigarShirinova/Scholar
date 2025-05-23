﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.Account
{
    public class AccountRegisterVM
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "It is a required field!")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "It is a required field!")]
        public string Role { get; set; }

        [Required(ErrorMessage = "It is a required field!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "It is a required field!")]
        [Compare(nameof(Password), ErrorMessage = "Passwords doesn't match!")]
        public string ConfirmPassword { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле імейл є обов'язковим")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле пароль є обов'язковим")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }
    }
}

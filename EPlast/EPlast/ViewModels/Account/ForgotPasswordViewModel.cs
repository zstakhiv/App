using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Поле електронна пошта є обов'язковим")]
        [EmailAddress]
        public string Email { get; set; }
    }
}

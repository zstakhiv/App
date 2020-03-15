using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле Імейл є обов'язковим")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле Пароль є обов'язковим")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле Повторення Пароля є обов'язковим")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Поле Імя є обов'язковим")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле Прізвище є обов'язковим")]
        public string SurName { get; set; }
    }
}

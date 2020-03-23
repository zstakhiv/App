using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Поле поточний пароль є обов'язковим")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Пароль має вміщати мінімум 8 символів", MinimumLength = 8)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Поле новий пароль є обов'язковим")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Пароль має вміщати мінімум 8 символів", MinimumLength = 8)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Поле введіть новий пароль ще раз є обов'язковим")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = 
            "The new password and confirmation password do not match.")]
        [StringLength(100, ErrorMessage = "Пароль має вміщати мінімум 8 символів", MinimumLength = 8)]
        public string ConfirmPassword { get; set; }
    }
}

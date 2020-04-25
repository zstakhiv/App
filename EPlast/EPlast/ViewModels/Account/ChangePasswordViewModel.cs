﻿using System;
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
        [StringLength(100, ErrorMessage = "Пароль має містити цифри та літери, мінімальна довжина повинна складати 8", MinimumLength = 8)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Поле новий пароль є обов'язковим")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Пароль має містити цифри та літери, мінімальна довжина повинна складати 8", MinimumLength = 8)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Поле введіть новий пароль ще раз є обов'язковим")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Новий пароль не співпадає")]
        [StringLength(100, ErrorMessage = "Пароль має містити цифри та літери, мінімальна довжина повинна складати 8", MinimumLength = 8)]
        public string ConfirmPassword { get; set; }
    }
}

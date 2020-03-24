using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels
{
    public class FeedBackViewModel
    {
        [Required(ErrorMessage = "Поле ім'я є обов'язковим")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЇїҐґ]{1,20}((\s+|-)[a-zA-Zа-яА-ЯІіЇїҐґ]{1,20})*$",
            ErrorMessage = "Ім'я має містити тільки літери")]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле номер телефону є обов'язковим")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Поле текст є обов'язковим")]
        public string FeedBackDescription { get; set; }

    }
}

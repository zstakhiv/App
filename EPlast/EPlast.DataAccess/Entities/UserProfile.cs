using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class UserProfile
    {
        public int ID { get; set; }
        [Display(Name="Номер телефону")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Номер телефону повинен складати від 6 до 10 цифр")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата народження")]
        public DateTime DateTime { get; set; }
        public Education Education { get; set; }
        public Nationality Nationality { get; set; }
        public Religion Religion { get; set; }
        public Work Work { get; set; }
        public int? GenderID { get; set; }
        public Gender Gender { get; set; }
        [Display(Name = "Домашня адреса")]
        [MaxLength(50,ErrorMessage = "Адреса не може перевищувати 50 символів")]
        public string Address { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
    }
}

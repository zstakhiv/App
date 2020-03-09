using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class UserProfile
    {
        public int ID { get; set; }
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime DateTime { get; set; }
        public Education Education { get; set; }
        public Nationality Nationality { get; set; }
        public Religion Religion { get; set; }
        public Work Work { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
    }
}

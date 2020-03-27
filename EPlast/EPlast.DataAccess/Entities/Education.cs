using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Education
    {
        public int ID { get; set; }
        [Display(Name = "Місце навчання")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЇїҐґ]{1,20}((\s+|-)[a-zA-Zа-яА-ЯІіЇїҐґ]{1,20})*$",
            ErrorMessage = "Місце навчання має містити тільки літери")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Місце навчання повинне складати від 2 до 50 символів")]

        public string PlaceOfStudy { get; set; }
        [Display(Name = "Спеціальність")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЇїҐґ]{1,20}((\s+|-)[a-zA-Zа-яА-ЯІіЇїҐґ]{1,20})*$",
            ErrorMessage = "Спеціальність має містити тільки літери")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Спеціальність повинна складати від 3 до 50 символів")]
        public string Speciality { get; set; }
        public Degree Degree { get; set; }
        public ICollection<UserProfile> UsersProfiles { get; set; }
    }
}

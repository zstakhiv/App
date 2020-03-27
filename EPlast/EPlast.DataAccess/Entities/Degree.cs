using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Degree
    {
        public int ID { get; set; }

        [Display(Name = "Ступінь")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЇїҐґ]{1,20}((\s+|-)[a-zA-Zа-яА-ЯІіЇїҐґ]{1,20})*$",
            ErrorMessage = "Ступінь має містити тільки літери")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Ступінь повинна складати від 3 до 30 символів")]
        public string Name { get; set; }
        public ICollection<Education> Educations { get; set; }
    }
}

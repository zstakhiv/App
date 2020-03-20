using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Work
    {
        public int ID { get; set; }
        [Display(Name = "Місце праці")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Місце праці повинне складати від 3 до 20 символів")]
        public string PlaceOfwork { get; set; }
        [Display(Name = "Посада")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Посада повинна складати від 3 до 20 символів")]
        public string Position { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}

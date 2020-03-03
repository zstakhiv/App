using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Gender
    {
        public int ID { get; set; }
        [Required, MaxLength(10, ErrorMessage = "Gender name cannot exceed 10 characters")]
        public string GenderName { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}

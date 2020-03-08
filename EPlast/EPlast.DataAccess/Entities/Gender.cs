using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Gender
    {
        public int ID { get; set; }
        [MaxLength(10, ErrorMessage = "Gender name cannot exceed 10 characters")]
        public string Name { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}

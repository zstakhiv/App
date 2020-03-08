using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Religion
    {
        public int ID { get; set; }
        [MaxLength(50, ErrorMessage = "Religion name cannot exceed 50 characters")]
        public string Name { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}

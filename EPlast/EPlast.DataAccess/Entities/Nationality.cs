using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Nationality
    {
        public int ID { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Nationality name cannot exceed 50 characters")]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
        public string NationalityName { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}
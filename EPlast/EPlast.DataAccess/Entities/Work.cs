using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Work
    {
        public int ID { get; set; }
        [Required, MaxLength(50, ErrorMessage = "PlaceOfwork name cannot exceed 50 characters")]
        public string PlaceOfwork { get; set; }
        [Required, MaxLength(50, ErrorMessage = "PlaceOfwork name cannot exceed 50 characters")]
        public string Position { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}

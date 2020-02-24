using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Sex
    {
        public int ID { get; set; }
        [Required, MaxLength(10, ErrorMessage = "Sex name cannot exceed 10 characters")]
        public string SexName { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}

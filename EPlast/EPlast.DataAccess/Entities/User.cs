using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EPlast.DataAccess.Entities
{
    public class User:IdentityUser
    {
        [Required, MaxLength(50, ErrorMessage = "FirstName cannot exceed 50 characters")]
        public string FirstName { get; set; }
        [Required, MaxLength(50, ErrorMessage = "LastName cannot exceed 50 characters")]
        public string LastName { get; set; }
        public Nationality Nationality { get; set; }
        public ICollection<CityMembers> CityMembers { get; set; }
    }
}

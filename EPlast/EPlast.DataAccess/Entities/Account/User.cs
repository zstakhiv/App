using System;
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
        [Required, MaxLength(50, ErrorMessage = "FatherName cannot exceed 50 characters")]
        public string FatherName { get; set; }
        public DateTime RegistredOn { get; set; }

        //добавити звязок з UserProfile
    }
}

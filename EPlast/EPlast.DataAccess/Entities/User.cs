﻿using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class User
    {
        public int ID { get; set; }
        [Required, MaxLength(50, ErrorMessage = "FirstName cannot exceed 50 characters")]
        public string FirstName { get; set; }
        [Required, MaxLength(50, ErrorMessage = "LastName cannot exceed 50 characters")]
        public string LastName { get; set; }
        public Nationality Nationality { get; set; }
    }
}

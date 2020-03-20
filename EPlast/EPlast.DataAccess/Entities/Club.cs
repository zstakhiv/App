﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Club
    {
        public int ID { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Club name cannot exceed 50 characters")]
        public string ClubName { get; set; }
        public string ClubURL { get; set; }
        [MaxLength(1024, ErrorMessage = "Club description cannot exceed 1024 characters")]
        public string Description { get; set; }
        public string Logo { get; set; }
        public ICollection<ClubMembers> ClubMembers { get; set; }
        public ICollection<ClubAdministration> ClubAdministration { get; set; }
    }
}

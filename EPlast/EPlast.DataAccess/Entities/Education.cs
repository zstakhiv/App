﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Education
    {
        public int ID { get; set; }
        [Display(Name = "Місце навчання")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Місце навчання повинне складати від 2 до 30 символів")]

        public string PlaceOfStudy { get; set; }
        [Display(Name = "Спеціальність")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Спеціальність повинна складати від 3 до 20 символів")]
        public string Speciality { get; set; }
        public Degree Degree { get; set; }
        public ICollection<UserProfile> UsersProfiles { get; set; }
    }
}

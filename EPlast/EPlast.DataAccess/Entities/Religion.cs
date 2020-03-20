﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Religion
    {
        public int ID { get; set; }
        [Display(Name = "Віровизнання")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Віровизнання повинне складати від 3 до 20 символів")]
        public string Name { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}

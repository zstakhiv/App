﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Degree
    {
        public int ID { get; set; }

        [Display(Name = "Ступінь")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Ступінь повинна складати від 3 до 20 символів")]
        public string Name { get; set; }
        public ICollection<Education> Educations { get; set; }
    }
}

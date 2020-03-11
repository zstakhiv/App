﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class UserPlastDegreeType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<UserPlastDegree> UserPlastDegrees { get; set; }
    }
}
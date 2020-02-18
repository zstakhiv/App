﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Religion
    {
        public int ID { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Nationality name cannot exceed 50 characters")]
        public string ReligionName { get; set; }
    }
}

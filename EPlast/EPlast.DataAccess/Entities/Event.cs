﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public  class Event
    {
        public int ID { get; set; }
        [Required]
        public string EventName { get; set; }
        [Required]
        public string Description{ get; set; }
        [Required]
        public DateTime EventDateStart { get; set; }
        [Required]
        public DateTime EventDateEnd { get; set; }
        [Required]
        public string Eventlocation { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class SubEventCategory
    {
        public int ID { get; set; }
        [Required]
        public string SubEventCategoryName { get; set; }
        [Required]
        public int EventCategoryID { get; set; }
        public EventCategory EventCategory { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class EventType
    {
        public int ID { get; set; }
        [Required]
        public string EventTypeName { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
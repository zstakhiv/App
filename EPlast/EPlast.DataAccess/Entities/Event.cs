using System;
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
        [Required]
        public int EventCategoryID { get; set; }
        public int EventStatusID { get; set; }
        public EventCategory EventCategory { get; set; }
        public EventStatus EventStatus { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public ICollection<EventGallary> EventGallarys { get; set; }
        public ICollection<EventAdmin> EventAdmins { get; set; }
        public ICollection<EventAdministration> EventAdministrations { get; set; }
    }
}

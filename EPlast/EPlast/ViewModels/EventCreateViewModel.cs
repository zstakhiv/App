using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EPlast.ViewModels.Events
{
    public class EventCreateViewModel
    {
        public Event Event { get; set; }
        public IEnumerable<EventCategory> EventCategory { get; set; }
        public IEnumerable<EventType> EventTypes { get; set; }
        public EventType EventType { get; set; }
        public IEnumerable<EventAdmin> EventAdmins { get; set; }
        public EventAdmin EventAdmin { get;set; }
        public IEnumerable<EventAdministration> EventAdministrations { get; set; }
        public EventAdministration EventAdministration { get; set; }
    }
}

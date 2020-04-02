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
        public IEnumerable<EventType> SubEventCategories { get; set; }
        public EventType SubEventCategory { get; set; }
    }
}

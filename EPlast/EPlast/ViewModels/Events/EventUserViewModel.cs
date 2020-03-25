using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EPlast.ViewModels.Events
{
    public class EventUserViewModel
    {
        public User User { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<Participant> Participants { get; set; }
    }
}

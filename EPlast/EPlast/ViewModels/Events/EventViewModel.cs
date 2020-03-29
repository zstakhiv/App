using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels.Events
{
    public class EventViewModel
    {
        public UserManager<User> user { get; set; }
        public Event Event { get; set; }

        public bool IsUserEventAdmin { get; set;}
        public int ApprovedStatus { get; set;}
        public int UndeterminedStatus { get; set;}
        public int RejectedStatus { get; set; }
        public IEnumerable<Participant> EventParticipants { get; set; }

    }
}

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
        public List<Event> Events { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class EventAdministration
    {
        public int ID { get; set; }
        public string AdministrationType { get; set; }
        public Event Event { get; set; }
    }
}

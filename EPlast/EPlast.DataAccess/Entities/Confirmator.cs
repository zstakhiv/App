using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Entities
{
    public class Confirmator
    {
        public int ID { get; set; }
        public User User { get; set; }
        public ConfirmedUser ConfirmedUser { get; set; }
    }
}

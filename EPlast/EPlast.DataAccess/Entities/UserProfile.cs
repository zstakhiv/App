using System;

namespace EPlast.DataAccess.Entities
{
    public class UserProfile
    {
        public int ID { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime DateTime { get; set; }
        public Education Education { get; set; }
        public Nationality Nationality { get; set; }
        public Religion Religion { get; set; }
        public Work Work { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
    }
}

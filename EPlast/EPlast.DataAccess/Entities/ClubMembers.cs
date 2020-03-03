using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPlast.DataAccess.Entities
{
    public class ClubMembers
    {
        public int ID { get; set; }
        public User User { get; set; }
        public Club Club { get; set; }
        [Required]
        public bool IsApproved { get; set; }
        public ICollection<ClubAdministration> ClubAdministration { get; set; }
    }
}

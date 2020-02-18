using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    class ClubAdministration
    {
        public int ID { get; set; }
        public AdminType AdminType { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Club Club { get; set; } //Mb we don`t need this, because this info is in ClubMembers
        public ClubMembers ClubMembers { get; set; }
    }
}

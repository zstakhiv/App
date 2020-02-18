using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class CityAdministration
    {
        public int ID { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public City City { get; set; }
        public CityMembers CityMembers { get; set; }
    }
}

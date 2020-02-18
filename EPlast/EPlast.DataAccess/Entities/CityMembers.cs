using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class CityMembers
    {
        public int ID { get; set; }
        public User User { get; set; }
        public City City { get; set; }
        [Required]
        public bool IsApproved { get; set; }
        public ICollection<CityAdministration> CityAdministration { get; set; }
    }
}

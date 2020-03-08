using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class CityLegalStatusType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<CityLegalStatus> CityLegalStatuses { get; set; }
    }
}

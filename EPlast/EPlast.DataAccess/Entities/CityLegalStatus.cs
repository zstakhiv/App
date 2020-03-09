using System;

namespace EPlast.DataAccess.Entities
{
    public class CityLegalStatus
    {
        public int Id { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public int CityLegalStatusTypeId { get; set; }
        public CityLegalStatusType LegalStatus { get; set; }

        public DateTime DateStart { get; set; }
    }
}

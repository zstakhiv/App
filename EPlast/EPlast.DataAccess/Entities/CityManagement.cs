using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class CityManagement
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Оберіть правовий статус осередку")]
        public CityLegalStatusType CityLegalStatus { get; set; } 

        public string UserId { get; set; }
        public User User { get; set; }

        public int AnnualReportId { get; set; }
        public AnnualReport AnnualReport { get; set; }
    }
}
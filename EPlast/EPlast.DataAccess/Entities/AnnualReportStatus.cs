using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class AnnualReportStatus
    {
        public int ID { get; set; }

        [DisplayName("Name")]
        [Required, MaxLength(50, ErrorMessage = "Name status cannot exceed 50 characters")]
        public string Name { get; set; }

        public ICollection<AnnualReport> AnnualReports { get; set; }

        public AnnualReportStatus()
        {
            AnnualReports = new List<AnnualReport>();
        }
    }
}
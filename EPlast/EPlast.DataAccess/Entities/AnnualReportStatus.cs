using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class AnnualReportStatus
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<AnnualReport> AnnualReports { get; set; }
    }
}
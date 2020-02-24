using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class AnnualReport
    {
        public int ID { get; set; }

        [DisplayName("Date")]
        [Required]
        public DateTime Date { get; set; }

        [DisplayName("Government funds")]
        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Government funds cannot be negative")]
        public int GovernmentFunds { get; set; }

        [DisplayName("Contributions funds")]
        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Contributions funds cannot be negative")]
        public int ContributionFunds { get; set; }

        [DisplayName("Earnings funds")]
        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Earnings funds cannot be negative")]
        public int PlastFunds { get; set; }

        [DisplayName("Sponsorships funds")]
        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Sponsorships funds cannot be negative")]
        public int SponsorFunds { get; set; }

        [DisplayName("Property list")]
        [Required, MaxLength(500, ErrorMessage = "Property list cannot exceed 500 characters")]
        public string PropertyList { get; set; }

        [DisplayName("Improvement needs")]
        [Required, MaxLength(500, ErrorMessage = "Improvement needs cannot exceed 500 characters")]
        public string ImprovementNeeds { get; set; }

        public int AnnualReportStatusId { get; set; }
        public AnnualReportStatus Status { get; set; }
    }
}
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
        [Required, MaxLength(0, ErrorMessage = "Government funds cannot be negative")]
        public int GovernmentFunds { get; set; }

        [DisplayName("Contributions funds")]
        [Required, MaxLength(0, ErrorMessage = "Contributions funds cannot be negative")]
        public int ContributionFunds { get; set; }

        [DisplayName("Earnings funds")]
        [Required, MaxLength(0, ErrorMessage = "Earnings funds cannot be negative")]
        public int PlastFunds { get; set; }

        [DisplayName("Sponsorships funds")]
        [Required, MaxLength(0, ErrorMessage = "Sponsorships funds cannot be negative")]
        public int SponsorFunds { get; set; }

        [DisplayName("Property list")]
        [Required, MaxLength(500, ErrorMessage = "Property list cannot exceed 500 characters")]
        public string PropertyList { get; set; }

        [DisplayName("Improvement needs")]
        [Required, MaxLength(500, ErrorMessage = "Improvement needs cannot exceed 500 characters")]
        public string ImprovementNeeds { get; set; }
    }
}
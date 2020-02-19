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

        [DisplayName("Sockets count")]
        [Required, MaxLength(0, ErrorMessage = "Sockets count cannot be negative")]
        public int SocketsCount { get; set; }

        [DisplayName("Swarms saved count")]
        [Required, MaxLength(0, ErrorMessage = "Swarms saved count cannot be negative")]
        public int SwarmsSavedCount { get; set; }

        [DisplayName("Not name count")]
        [Required, MaxLength(0, ErrorMessage = "Not name count cannot be negative")]
        public int NotNameCount { get; set; }

        [DisplayName("Birdie socket count")]
        [Required, MaxLength(0, ErrorMessage = "Birdie socket count cannot be negative")]
        public int BirdieSocketCount { get; set; }

        [DisplayName("Beneficiary count")]
        [Required, MaxLength(0, ErrorMessage = "Beneficiary count cannot be negative")]
        public int BeneficiaryCount { get; set; }

        [DisplayName("Improvement needs")]
        [Required, MaxLength(500, ErrorMessage = "Improvement needs cannot exceed 500 characters")]
        public string ImprovementNeeds { get; set; }

        [DisplayName("Government funds")]
        [Required, MaxLength(0, ErrorMessage = "Government funds cannot be negative")]
        public int GovernmentFunds { get; set; }

        [DisplayName("Contribution funds")]
        [Required, MaxLength(0, ErrorMessage = "Contribution funds cannot be negative")]
        public int ContributionFunds { get; set; }

        [DisplayName("Plast funds")]
        [Required, MaxLength(0, ErrorMessage = "Plast funds cannot be negative")]
        public int PlastFunds { get; set; }

        [DisplayName("Sponsor funds")]
        [Required, MaxLength(0, ErrorMessage = "Sponsor funds cannot be negative")]
        public int SponsorFunds { get; set; }

        [DisplayName("Property list")]
        [Required, MaxLength(500, ErrorMessage = "Property list cannot exceed 500 characters")]
        public string PropertyList { get; set; }
    }
}
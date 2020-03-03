using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Statistic
    {
        public int Id { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of gnizd in the city cannot be negative")]
        public int NumberOfGnizd { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of gnizd for Ptashat in the city cannot be negative")]
        public int NumberOfGnizdForPtashat { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of samostiynyh roiv cannot be negative")]
        public int NumberOfSamostiynyhRoiv { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of Ptashat cannot be negative")]
        public int NumberOfPtashat { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of Novatstva cannot be negative")]
        public int NumberOfNovatstva { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of Unatstva Neimenovani cannot be negative")]
        public int NumberOfUnatstvaNeimenovani { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of Unatstva Pryhylnyky cannot be negative")]
        public int NumberOfUnatstvaPryhylnyky { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of Unatstva Uchasnyky cannot be negative")]
        public int NumberOfUnatstvaUchasnyky { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of Unatstva Rozviduvachi cannot be negative")]
        public int NumberOfUnatstvaRozviduvachi { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of Unatstva SkobyVirlytsi cannot be negative")]
        public int NumberOfUnatstvaSkobyVirlytsi { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of Starshi Plastuny Pryhylnyky cannot be negative")]
        public int NumberOfStarshiPlastunyPryhylnyky { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of Starshi Plastuny members cannot be negative")]
        public int NumberOfStarshiPlastunyMembers { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of Seniory Pryhylnyky cannot be negative")]
        public int NumberOfSenioryPryhylnyky { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of Seniory members cannot be negative")]
        public int NumberOfSenioryMembers { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of beneficiary cannot be negative")]
        public int NumberOfBeneficiary { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of Plastpryiat members cannot be negative")]
        public int NumberOfPlastpryiatMembers { get; set; }

        [Required, Range(0, Int32.MaxValue, ErrorMessage = "Number of honorary members cannot be negative")]
        public int NumberOfHonoraryMembers { get; set; }

        public int AnnualReportId { get; set; }
        public AnnualReport AnnualReport { get; set; }
    }
}
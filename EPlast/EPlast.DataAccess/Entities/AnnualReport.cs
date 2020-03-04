using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class AnnualReport
    {
        public int ID { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of seats in city must not be less than 0")]
        public int NumberOfSeatsInCity { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of seats for Ptashat must not be less than 0")]
        public int NumberOfSeatsPtashat { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of independent riy must not be less than 0")]
        public int NumberOfIndependentRiy { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of clubs must not be less than 0")]
        public int NumberOfClubs { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of independent groups must not be less than 0")]
        public int NumberOfIndependentGroups { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of teachers must not be less than 0")]
        public int NumberOfTeachers { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of administrators must not be less than 0")]
        public int NumberOfAdministrators { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of people combining education and administration must not be less than 0")]
        public int NumberOfTeacherAdministrators { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of beneficiaries must not be less than 0")]
        public int NumberOfBeneficiaries { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of Plastpryiat members must not be less than 0")]
        public int NumberOfPlastpryiatMembers { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of honorary members must not be less than 0")]
        public int NumberOfHonoraryMembers { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Public funds must not be less than 0")]
        public int PublicFunds { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Contribution funds must not be less than 0")]
        public int ContributionFunds { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Plast salary must not be less than 0")]
        public int PlastSalary { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Sponsorship funds must not be less than 0")]
        public int SponsorshipFunds { get; set; }

        public string ListProperty { get; set; }

        public string ImprovementNeeds { get; set; }

        public MembersStatistic MembersStatistic { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public int AnnualReportStatusId { get; set; }
        public AnnualReportStatus AnnualReportStatus { get; set; }
    }
}
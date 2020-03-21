using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class AnnualReport
    {
        public int ID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public AnnualReportStatus Status { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfSeatsInCity { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfSeatsPtashat { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfIndependentRiy { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfClubs { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfIndependentGroups { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfTeachers { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfAdministrators { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfTeacherAdministrators { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfBeneficiaries { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfPlastpryiatMembers { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int NumberOfHonoraryMembers { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int PublicFunds { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int ContributionFunds { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int PlastSalary { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int SponsorshipFunds { get; set; }

        public string ListProperty { get; set; }

        public string ImprovementNeeds { get; set; }

        public MembersStatistic MembersStatistic { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
    }
}
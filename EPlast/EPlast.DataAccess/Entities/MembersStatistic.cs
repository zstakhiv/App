using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class MembersStatistic
    {
        public int Id { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of Ptashat must not be less than 0")]
        public int NumberOfPtashata { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of Novatstva must not be less than 0")]
        public int NumberOfNovatstva { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of Unatstva noname must not be less than 0")]
        public int NumberOfUnatstvaNoname { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of Unatstva supporters must not be less than 0")]
        public int NumberOfUnatstvaSupporters { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of Unatstva members must not be less than 0")]
        public int NumberOfUnatstvaMembers { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of Unatstva prospectors must not be less than 0")]
        public int NumberOfUnatstvaProspectors { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of Unatstva skob/virlyts must not be less than 0")]
        public int NumberOfUnatstvaSkobVirlyts { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of Senior Plastyn supporters must not be less than 0")]
        public int NumberOfSeniorPlastynSupporters { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of Senior Plastyn members must not be less than 0")]
        public int NumberOfSeniorPlastynMembers { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of Seigneur supporters must not be less than 0")]
        public int NumberOfSeigneurSupporters { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number of Seigneur members must not be less than 0")]
        public int NumberOfSeigneurMembers { get; set; }

        public int AnnualReportId { get; set; }
        public AnnualReport AnnualReport { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class UserPlastDegree
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int UserPlastDegreeTypeId { get; set; }
        public UserPlastDegreeType UserPlastDegreeType { get; set; }

        [Required]
        public DateTime DateStart { get; set; }

        public DateTime? DateFinish { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Degree
    {
        public int ID { get; set; }

        [MaxLength(50, ErrorMessage = "DegreeName cannot exceed 50 characters")]
        public string Name { get; set; }
        public ICollection<Education> Educations { get; set; }
    }
}

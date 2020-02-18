using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Education
    {
        public int ID { get; set; }

        [Required, MaxLength(50, ErrorMessage = "PlaceOfStudy cannot exceed 50 characters")]
        public string PlaceOfStudy { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Speciality cannot exceed 50 characters")]
        public string Speciality { get; set; }
        
        public Degree Degree { get; set; }
    }
}

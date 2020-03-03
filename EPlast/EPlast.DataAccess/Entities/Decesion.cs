using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Decesion
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DecesionStatus DecesionStatus { get; set; }

        [Required]
        public Organization Organization { get; set; }

        [Required]
        public DecesionTarget DecesionTarget { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
    }
}
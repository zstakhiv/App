using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Decesion
    {
        public int ID { get; set; }

        [Required]
        public DecesionStatus DecesionStatus { get; set; }

        [Required]
        public Organ Organ { get; set; }

        [Required]
        public DecesionTarget DecesionTarget { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
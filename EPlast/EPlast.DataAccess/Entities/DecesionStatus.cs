using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class DecesionStatus
    {
        public int ID { get; set; }

        [DisplayName("Decesion Status Name")]
        [Required, MaxLength(50, ErrorMessage = "Decesion Status Name cannot exceed 50 characters")]
        public string DecesionStatusName { get; set; }
    }
}
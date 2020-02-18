using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Organ
    {
        public int ID { get; set; }

        [DisplayName("Organ Name")]
        [Required, MaxLength(50, ErrorMessage = "Organ  Name cannot exceed 50 characters")]
        public string OrganName { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Organization
    {
        public int ID { get; set; }

        [DisplayName("Organization Name")]
        [Required, MaxLength(50, ErrorMessage = "Organ  Name cannot exceed 50 characters")]
        public string OrganizationName { get; set; }
    }
}
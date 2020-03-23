using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Organization
    {
        public int ID { get; set; }

        [DisplayName("Назва організації")]
        [Required, MaxLength(50, ErrorMessage = "Назва організації має бути меншою 50 символів")]
        public string OrganizationName { get; set; }
    }
}
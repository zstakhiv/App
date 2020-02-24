using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class DecesionTarget
    {
        public int ID { get; set; }

        [DisplayName("Target Name")]
        [Required, MaxLength(50, ErrorMessage = "Target Name cannot exceed 50 characters")]
        public string TargetName { get; set; }
    }
}
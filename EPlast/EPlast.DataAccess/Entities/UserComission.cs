using System;
using System.ComponentModel.DataAnnotations;


namespace EPlast.DataAccess.Entities
{
    public class UserComission
    {
        public int ID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public int UserConfignerID { get; set; }
        [Required]
        public DateTime ComissionDate { get; set; }

    }
}

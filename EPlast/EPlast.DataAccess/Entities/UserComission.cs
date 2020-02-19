using System;
using System.ComponentModel.DataAnnotations;


namespace EPlast.DataAccess.Entities
{
    public class UserComission
    {
        public int ID { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public User UserConfigner { get; set; }
        [Required]
        public DateTime ComissionDate { get; set; }

    }
}

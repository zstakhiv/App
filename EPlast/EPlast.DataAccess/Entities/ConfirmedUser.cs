using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace EPlast.DataAccess.Entities
{
    public class ConfirmedUser
    {
        public int ID { get; set; }
        [Required]
        public User User { get; set; }
        public int? ConfirmatorID { get; set; }
        public Confirmator Confirmator{ get; set; }
        public DateTime ConfirmDate { get; set; }

    }
}

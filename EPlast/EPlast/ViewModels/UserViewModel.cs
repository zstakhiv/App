using EPlast.DataAccess.Entities;
using System.Collections.Generic;

namespace EPlast.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }
        public bool CanManageUserPosition { get; set; }
        public IEnumerable<CityAdministration> UserPositions { get; set; }

        public ICollection<Approver> Approvers { get; set; }

        public UserViewModel()
        {
            Approvers = new List<Approver>();
        }
    }
}

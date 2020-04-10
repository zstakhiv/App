using EPlast.DataAccess.Entities;
using System.Collections.Generic;

namespace EPlast.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }
        public IEnumerable<CityAdministration> UserPositions { get; set; }
        public bool HasAccessToManageUserPositions { get; set; }
        public EditUserViewModel EditView { get; set; }
    }
}

using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels
{
    public class RegionsAndAdmins
    {
        public IEnumerable<City> Cities { get; set; }
        public string City { get; set; }
    }
}

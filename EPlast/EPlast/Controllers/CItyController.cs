using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.Controllers
{
    public class CItyController : Controller
    {
        private readonly DataAccess.Repositories.IRepositoryWrapper _repoWrapper;
        public IActionResult Index()
        {
            List<CityViewModel> cities = new List<CityViewModel>(
                _repoWrapper.City
                .FindAll()
                .Select(city => new CityViewModel { City = city })
                .ToList());
            return View();
        }

        public IActionResult CityProfile()
        {
            return View();
        }
    }
}
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.Controllers
{
    public class CityController : Controller
    {
        private readonly DataAccess.Repositories.IRepositoryWrapper _repoWrapper;
        private readonly IHostingEnvironment _env;
        private UserManager<User> _userManager;
        public CityController(DataAccess.Repositories.IRepositoryWrapper repoWrapper, UserManager<User> userManager, IHostingEnvironment env)
        {
            _userManager = userManager;
            _repoWrapper = repoWrapper;
            _env = env;
        }

        public IActionResult Index()
        {
            List<CityViewModel> cities = new List<CityViewModel>(
                _repoWrapper.City
                .FindAll()
                .Select(city => new CityViewModel { City = city })
                .ToList());

            return View(cities);
        }

        public IActionResult CityProfile(int cityId)
        {
            try
            {
                var city = _repoWrapper.City
                .FindByCondition(q => q.ID == cityId)
                .Include(c => c.CityAdministration)
                    .ThenInclude(t => t.AdminType)
                .Include(c => c.CityAdministration)
                    .ThenInclude(a => a.User)
                .Include(m => m.CityMembers)
                    .ThenInclude(u => u.User)
                .FirstOrDefault();

                var members = city.CityMembers.Where(m => m.EndDate == null && m.StartDate!=null).Take(6).ToList();
                var followers = city.CityMembers.Where(m => m.EndDate == null && m.StartDate == null).Take(6).ToList();

                var cityAdmin = city.CityAdministration
                    .Where(a => a.EndDate == null && a.AdminType.AdminTypeName == "Голова Станиці")
                    .Select(a => a.User)
                    .FirstOrDefault();

                ViewBag.usermanager = _userManager;
                return View(new CityViewModel { City = city, CityAdmin = cityAdmin, Members = members, Followers = followers });

            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
    }
}
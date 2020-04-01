using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;


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

        public IActionResult CityProfile(int cityId)
        {
            try
            {
                var city = _repoWrapper.City
                    .FindByCondition(q => q.ID == cityId)
                    .Include(c => c.CityAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(n => n.CityAdministration)
                    .ThenInclude(t => t.CityMembers)
                    .ThenInclude(us => us.User)
                    .Include(m => m.CityMembers)
                    .ThenInclude(u => u.User)
                    .FirstOrDefault();

                var members = club.ClubMembers.Where(m => m.IsApproved).Take(6).ToList();
                var followers = club.ClubMembers.Where(m => !m.IsApproved).Take(6).ToList();

                var clubAdmin = club.ClubAdministration
                    .Where(a => a.EndDate == null && a.AdminType.AdminTypeName == "Курінний")
                    .Select(a => a.ClubMembers.User)
                    .FirstOrDefault();
                ViewBag.usermanager = _userManager;
                return View(new ClubViewModel { Club = club, ClubAdmin = clubAdmin, Members = members, Followers = followers });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
    }
}
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace EPlast.Controllers
{
    public class ClubController : Controller
    {
        private readonly DataAccess.Repositories.IRepositoryWrapper _repoWrapper;
        private readonly IHostingEnvironment _env;
        private UserManager<User> _userManager;
        public ClubController(DataAccess.Repositories.IRepositoryWrapper repoWrapper, UserManager<User> userManager , IHostingEnvironment env)
        {
            _userManager = userManager;
            _repoWrapper = repoWrapper;
            _env = env;
        }

        public IActionResult Index()
        {
            List<ClubViewModel> clubs = new List<ClubViewModel>(
                _repoWrapper.Club
                .FindAll()
                .Select(club => new ClubViewModel { Club = club })
                .ToList());

            return View(clubs);
        }

        public IActionResult Club(int index)
        {
            try
            {
                 var club = _repoWrapper.Club
                    .FindByCondition(q => q.ID == index)
                    .Include(c => c.ClubAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(n => n.ClubAdministration)
                    .ThenInclude(t => t.ClubMembers)
                    .ThenInclude(us => us.User)
                    .Include(m => m.ClubMembers)
                    .ThenInclude(u => u.User)
                    .FirstOrDefault();

                var members = club.ClubMembers.Where(m => m.IsApproved).Take(6).ToList();
                var followers = club.ClubMembers.Where(m => !m.IsApproved).Take(6).ToList();

                var clubAdmin = club.ClubAdministration
                    .Where(a => a.EndDate == null && a.AdminType.AdminTypeName == "Курінний")
                    .Select(a => a.ClubMembers.User)
                    .FirstOrDefault();
               ViewBag.usermanager = _userManager;
                return View(new ClubViewModel { Club = club, ClubAdmin =  clubAdmin, Members = members, Followers = followers});
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public IActionResult ClubAdmins(int index)
        {
            try
            {
                var club = _repoWrapper.Club
                    .FindByCondition(q => q.ID == index)
                    .Include(c => c.ClubAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(n => n.ClubAdministration)
                    .ThenInclude(t => t.ClubMembers)
                    .ThenInclude(us => us.User)
                    .FirstOrDefault();

                var clubAdmin = club.ClubAdministration
                    .Where(a => a.EndDate == null && a.AdminType.AdminTypeName == "Курінний")
                    .Select(a => a.ClubMembers.User)
                    .FirstOrDefault();
                ViewBag.usermanager = _userManager;
                return View(new ClubViewModel { Club = club, ClubAdmin = clubAdmin });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public IActionResult ClubMembers(int index)
        {
            try
            {
                var club = _repoWrapper.Club
                    .FindByCondition(q => q.ID == index)
                    .Include(c => c.ClubAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(n => n.ClubAdministration)
                    .ThenInclude(t => t.ClubMembers)
                    .ThenInclude(us => us.User)
                    .Include(m => m.ClubMembers)
                    .ThenInclude(u => u.User)
                    .FirstOrDefault();

                var members = club.ClubMembers.Where(m => m.IsApproved).ToList();

                var clubAdmin = club.ClubAdministration
                   .Where(a => a.EndDate == null && a.AdminType.AdminTypeName == "Курінний")
                   .Select(a => a.ClubMembers.User)
                   .FirstOrDefault();
                ViewBag.usermanager = _userManager;
                return View(new ClubViewModel { Club = club, ClubAdmin = clubAdmin, Members = members });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public IActionResult ClubFollowers(int index)
        {
            try
            {
                var club = _repoWrapper.Club
                    .FindByCondition(q => q.ID == index)
                    .Include(c => c.ClubAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(n => n.ClubAdministration)
                    .ThenInclude(t => t.ClubMembers)
                    .ThenInclude(us => us.User)
                    .Include(m => m.ClubMembers)
                    .ThenInclude(u => u.User)
                    .FirstOrDefault();

                var followers = club.ClubMembers.Where(m => !m.IsApproved).ToList();

                var clubAdmin = club.ClubAdministration
                   .Where(a => a.EndDate == null && a.AdminType.AdminTypeName == "Курінний")
                   .Select(a => a.ClubMembers.User)
                   .FirstOrDefault();
                ViewBag.usermanager = _userManager;
                return View(new ClubViewModel { Club = club, ClubAdmin = clubAdmin, Followers = followers });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public IActionResult ClubDescription(int index)
        {
            try
            {
                var club = _repoWrapper.Club
                    .FindByCondition(q => q.ID == index)
                    .FirstOrDefault();

                return View(new ClubViewModel { Club = club });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpGet]
        public IActionResult EditClub(int index)
        {
            try
            {
                var club = _repoWrapper.Club
                    .FindByCondition(q => q.ID == index)
                    .FirstOrDefault();

                return View(new ClubViewModel { Club = club });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public IActionResult EditClub(ClubViewModel model, IFormFile file)
        {
            try 
            {
                var oldImageName = _repoWrapper.Club.FindByCondition(i => i.ID == model.Club.ID).FirstOrDefault().Logo;
                if (file != null && file.Length > 0)
                {
                    var img = Image.FromStream(file.OpenReadStream());
                    var uploads = Path.Combine(_env.WebRootPath, "images\\Club");
                    if (!string.IsNullOrEmpty(oldImageName))
                    {
                        var oldPath = Path.Combine(uploads, oldImageName);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }

                    }

                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    img.Save(filePath);
                    model.Club.Logo = fileName;
                }
                else
                {
                    model.Club.Logo = oldImageName;
                }
                _repoWrapper.Club.Update(model.Club);
                _repoWrapper.Save();
                return RedirectToAction("Club", new { index = model.Club.ID});
            }
            catch (Exception e)
            {
               
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult ChangeIsApprovedStatus(int index, int clubIndex)
        {
            try
            {
                var club = _repoWrapper.Club
                    .FindByCondition(q => q.ID == clubIndex)
                    .Include(m => m.ClubMembers)
                    .ThenInclude(u => u.User)
                    .FirstOrDefault();

                var person = _repoWrapper.ClubMembers
                    .FindByCondition(u => u.ID == index)
                    .FirstOrDefault();
                if (person != null)
                    person.IsApproved = !person.IsApproved;
                _repoWrapper.ClubMembers.Update(person);
                _repoWrapper.Save();

                return RedirectToAction("ClubMembers", new { index = clubIndex });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult ChangeIsApprovedStatusFollowers(int index, int clubIndex)
        {
            try
            {
                var club = _repoWrapper.Club
                    .FindByCondition(q => q.ID == clubIndex)
                    .Include(m => m.ClubMembers)
                    .ThenInclude(u => u.User)
                    .FirstOrDefault();

                var person = _repoWrapper.ClubMembers
                    .FindByCondition(u => u.ID == index)
                    .FirstOrDefault();
                if (person != null)
                    person.IsApproved = !person.IsApproved;

                _repoWrapper.ClubMembers.Update(person);
                _repoWrapper.Save();

                return RedirectToAction("ClubFollowers", new { index = clubIndex });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult ChangeIsApprovedStatusClub(int index, int clubIndex)
        {
            try
            {
                var club = _repoWrapper.Club
                    .FindByCondition(q => q.ID == clubIndex)
                    .Include(m => m.ClubMembers)
                    .ThenInclude(u => u.User)
                    .FirstOrDefault();

                var person = _repoWrapper.ClubMembers
                    .FindByCondition(u => u.ID == index)
                    .FirstOrDefault();
                if (person != null)
                    person.IsApproved = !person.IsApproved;
                _repoWrapper.ClubMembers.Update(person);
                _repoWrapper.Save();

                return RedirectToAction("Club", new { index = clubIndex });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult DeleteFromAdmins(int adminId, int clubIndex)
        {
            ClubAdministration admin = _repoWrapper.GetClubAdministration.FindByCondition(i => i.ID == adminId).FirstOrDefault();
            if (admin != null)
            {
                _repoWrapper.GetClubAdministration.Delete(admin);
                _repoWrapper.Save();
                return RedirectToAction("ClubAdmins", new { index = clubIndex });
            }
            else
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        
        [HttpGet]
        public IActionResult SetEndDateFromAdmins(int adminId, int clubIndex, DateTime enddate)
        {
            try
            {
                ClubAdministration admin = _repoWrapper.GetClubAdministration.FindByCondition(i => i.ID == adminId).FirstOrDefault();

                admin.EndDate = enddate;

                _repoWrapper.GetClubAdministration.Update(admin);
                _repoWrapper.Save();

                return RedirectToAction("ClubAdmins", new { index = clubIndex });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
    }
}
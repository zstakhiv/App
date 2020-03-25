using EPlast.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public ClubController(DataAccess.Repositories.IRepositoryWrapper repoWrapper, IHostingEnvironment env)
        {
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

                return View(new ClubViewModel { Club = club });
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

                return View(new ClubViewModel { Club = club });
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
                .Include(m => m.ClubMembers)
                .ThenInclude(u => u.User)
                .FirstOrDefault();

                return View(new ClubViewModel { Club = club });
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
                .Include(m => m.ClubMembers)
                .ThenInclude(u => u.User)
                .FirstOrDefault();

                return View(new ClubViewModel { Club = club });
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

    }
}
using EPlast.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.Controllers
{
    public class ClubController : Controller
    {
        private readonly DataAccess.Repositories.IRepositoryWrapper _repoWrapper;

        public ClubController(DataAccess.Repositories.IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
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
        public IActionResult ClubAdmins(int index)
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
        public IActionResult ClubMembers(int index)
        {
            var club = _repoWrapper.Club
                .FindByCondition(q => q.ID == index)
                .Include(m => m.ClubMembers)
                .ThenInclude(u => u.User)
                .FirstOrDefault();

            return View(new ClubViewModel { Club = club });
        }
        public IActionResult ClubFollowers(int index)
        {
            var club = _repoWrapper.Club
                .FindByCondition(q => q.ID == index)
                .Include(m => m.ClubMembers)
                .ThenInclude(u => u.User)
                .FirstOrDefault();

            return View(new ClubViewModel { Club = club });
        }
        public IActionResult ClubDescription(int index)
        {
            var club = _repoWrapper.Club
                .FindByCondition(q => q.ID == index)
                .FirstOrDefault();

            return View(new ClubViewModel { Club = club });
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
        /*
        [HttpPost]
        public IActionResult Edit(UserViewModel model, IFormFile file)
        {
            try
            {
                var oldImageName = _repoWrapper.User.FindByCondition(i => i.Id == model.User.Id).FirstOrDefault().ImagePath;
                if (file != null && file.Length > 0)
                {
                    var img = Image.FromStream(file.OpenReadStream());
                    var uploads = Path.Combine(_env.WebRootPath, "images\\Users");
                    if (!string.IsNullOrEmpty(oldImageName) && !string.Equals(oldImageName, "default.png"))
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
                    model.User.ImagePath = fileName;
                }
                else
                {
                    model.User.ImagePath = oldImageName;
                }

                if (model.User.UserProfile.Nationality.ID == 0)
                {
                    string name = model.User.UserProfile.Nationality.Name;
                    if (string.IsNullOrEmpty(name))
                    {
                        model.User.UserProfile.Nationality = null;
                    }
                    else
                    {
                        model.User.UserProfile.Nationality = new Nationality() { Name = name };
                    }
                }

                if (model.User.UserProfile.Religion.ID == 0)
                {
                    string name = model.User.UserProfile.Religion.Name;
                    if (string.IsNullOrEmpty(name))
                    {
                        model.User.UserProfile.Religion = null;
                    }
                    else
                    {
                        model.User.UserProfile.Religion = new Religion() { Name = name };
                    }
                }

                Degree degree = model.User.UserProfile.Education.Degree;
                if (model.User.UserProfile.Education.Degree.ID == 0)
                {
                    string name = model.User.UserProfile.Education.Degree.Name;
                    if (string.IsNullOrEmpty(name))
                    {
                        model.User.UserProfile.Education.Degree = null;
                    }
                    else
                    {
                        model.User.UserProfile.Education.Degree = new Degree() { Name = name };
                    }
                }

                if (model.User.UserProfile.Education.ID == 0)
                {
                    string placeOfStudy = model.User.UserProfile.Education.PlaceOfStudy;
                    string speciality = model.User.UserProfile.Education.Speciality;
                    if (string.IsNullOrEmpty(placeOfStudy) || string.IsNullOrEmpty(speciality))
                    {
                        model.User.UserProfile.Education = null;
                    }
                    else
                    {
                        model.User.UserProfile.Education = new Education() { PlaceOfStudy = placeOfStudy, Speciality = speciality, Degree = degree };
                    }
                }

                if (model.User.UserProfile.Work.ID == 0)
                {
                    string placeOfWork = model.User.UserProfile.Work.PlaceOfwork;
                    string position = model.User.UserProfile.Work.Position;
                    if (string.IsNullOrEmpty(placeOfWork) || string.IsNullOrEmpty(position))
                    {
                        model.User.UserProfile.Work = null;
                    }
                    else
                    {
                        model.User.UserProfile.Work = new Work() { PlaceOfwork = placeOfWork, Position = position };
                    }
                }

                _repoWrapper.UserProfile.Update(model.User.UserProfile);
                _repoWrapper.User.Update(model.User);
                _repoWrapper.Save();
                _logger.LogInformation("User {0} {1} was edited profile and saved in the database", model.User.FirstName, model.User.LastName);
                return RedirectToAction("UserProfile");
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        */
    }
}
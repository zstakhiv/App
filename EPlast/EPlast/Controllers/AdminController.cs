﻿using EPlast.BussinessLayer.ExtensionMethods;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize("Admin")]
    public class AdminController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IRepositoryWrapper repoWrapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _repoWrapper = repoWrapper;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> UsersTable()
        {
            try
            {
                var users = _repoWrapper.User
                    .Include(x => x.UserProfile, x => x.UserPlastDegrees)
                    .ToList();

                var cities = _repoWrapper.City
                    .Include(x => x.Region)
                    .ToList();
                var clubMembers = _repoWrapper.ClubMembers.Include(x => x.Club)
                                                          .ToList();
                var cityMembers = _repoWrapper.CityMembers.Include(x => x.City)
                                                          .ToList();
                List<UserTableViewModel> userTableViewModels = new List<UserTableViewModel>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var cityName = cityMembers.Where(x => x.User.Id.Equals(user.Id) && x.EndDate == null)
                                              .Select(x => x.City.Name)
                                              .LastOrDefault() ?? string.Empty;

                    #region Delete when all users will have UserPlastDegrees automatically

                    if (user.UserPlastDegrees.Count == 0 || cityName.Equals(string.Empty))
                        continue;

                    #endregion Delete when all users will have UserPlastDegrees automatically

                    userTableViewModels.Add(new UserTableViewModel
                    {
                        User = user,
                        ClubName = clubMembers.Where(x => x.UserId.Equals(user.Id) && x.IsApproved == true)
                                              .Select(x => x.Club.ClubName).LastOrDefault() ?? string.Empty,
                        CityName = cityName,
                        RegionName = cities.Where(x => x.Name.Equals(cityName))
                                           .FirstOrDefault()
                                           .Region
                                           .RegionName ?? string.Empty,
                        UserPlastDegreeName = user.UserPlastDegrees.Where(x => x.UserId == user.Id && x.DateFinish == null)
                                                                   .FirstOrDefault()
                                                                   .UserPlastDegreeType
                                                                   .GetDescription() ?? string.Empty,
                        UserRoles = string.Join(", ", roles)
                    });
                }

                return View(userTableViewModels);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        public async Task<IActionResult> Edit(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var admin = _roleManager.Roles.Where(i => i.Name == "Admin");
                var allRoles = _roleManager.Roles.Except(admin).ToList();
                RoleViewModel model = new RoleViewModel
                {
                    UserID = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return PartialView(model);
            }

            return RedirectToAction("HandleError", "Error", new { code = 404 });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);
                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("Index");
            }

            return RedirectToAction("HandleError", "Error", new { code = 404 });
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string userId)
        {
            ViewBag.userId = userId;
            return PartialView();
        }

        public async Task<IActionResult> Delete(string userId)
        {
            if (userId != null)
            {
                User user = _repoWrapper.User.FindByCondition(i => i.Id == userId).FirstOrDefault();
                var roles = await _userManager.GetRolesAsync(user);
                if (user != null && !roles.Contains("Admin"))
                {
                    _repoWrapper.User.Delete(user);
                    _repoWrapper.Save();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("HandleError", "Error", new { code = 505 });
        }
    }
}
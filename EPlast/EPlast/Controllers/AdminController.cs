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
    public class AdminController:Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        public AdminController(RoleManager<IdentityRole> roleManager,UserManager<User> userManager, IRepositoryWrapper repoWrapper) 
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
                User user =_repoWrapper.User.FindByCondition(i => i.Id == userId).FirstOrDefault();
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

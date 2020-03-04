using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using EPlast.DataAccess.Repositories;
using EPlast.Models;
using NLog;
using EPlast.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPlast.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private Logger logger;
        private readonly IRepositoryWrapper _repoWrapper;
        public AccountController(IRepositoryWrapper repoWrapper,UserManager<User> userManager, SignInManager<User> signInManager)
        {
            logger = LogManager.GetCurrentClassLogger();
            _signInManager = signInManager;
            _userManager = userManager;
            _repoWrapper = repoWrapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Edit()
        {
            ViewBag.nationalities = _repoWrapper.Nationality.FindAll();

            ViewBag.genders = (from item in _repoWrapper.Gender.FindAll()
                               select new SelectListItem
                               {
                                   Text = item.GenderName,
                                   Value = item.ID.ToString()
                               });
            try
            {
                var user = _repoWrapper.User.
                FindByCondition(q => q.Id == _userManager.GetUserId(User)).
                Include(i => i.UserProfile).
                ThenInclude(x => x.Nationality ).
                Include(g=>g.UserProfile).
                ThenInclude(g=>g.Gender).
                FirstOrDefault();
                var model = new UserViewModel() { User = user };
                return View(model);
            }
            catch (Exception e)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                });
            }
        }
        [HttpPost]
        public IActionResult EditConfirmed(UserViewModel userVM)
        {
            try
            {
                var nationalities = _repoWrapper.Nationality.FindAll().ToList();
                bool exist = nationalities.Any(x => x.Name.Equals(userVM.User.UserProfile.Nationality.Name));
                if (exist)
                {
                    userVM.User.UserProfile.Nationality.ID = nationalities.Where(x => x.Name.Equals(userVM.User.UserProfile.Nationality.Name))
                        .Select(x => x.ID)
                        .FirstOrDefault();
                }
                _repoWrapper.UserProfile.Attach(userVM.User.UserProfile);
                _repoWrapper.UserProfile.Update(userVM.User.UserProfile);
                _repoWrapper.User.Update(userVM.User);
                _repoWrapper.Save();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                });
            }
        }
        public IActionResult LoginAndRegister()
        {
            return View();
        }

        public async Task<IActionResult> Registered(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");
                return View("LoginAndRegister");
            }

            var user = new User() { Email = registerVM.Email, UserName = registerVM.Name, LastName = registerVM.SurName, EmailConfirmed = true, UserProfile = new UserProfile() { } };
            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Account");
            }

            return View("LoginAndRegister");
        }

        public async Task<IActionResult> LoggedIn(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");
                return RedirectToAction("Index", "Account");
            }
            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                return View("LoginAndRegister");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LoginAndRegister", "Account");
        }
    }
}
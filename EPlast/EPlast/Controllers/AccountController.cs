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
using EPlast.ViewModels;

namespace EPlast.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
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

            var user = new User() { UserName = registerVM.Email, Email = registerVM.Email };
            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("User/GetPage");
            }

            return View("LoginAndRegister");
        }

        public async Task<IActionResult> LoggedIn(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");
                return View("Login");
            }

            var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, false);
            if (result.Succeeded)
            {
                return RedirectToAction("User/GetPage");
            }
            else
                return View("LoginAndRegister");
        }
    }
}
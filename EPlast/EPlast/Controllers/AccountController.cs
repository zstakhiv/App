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
            try
            {
                var user = _repoWrapper.User.
            FindByCondition(q => q.Id == _userManager.GetUserId(User)).
                Include(i => i.UserProfile).
                    ThenInclude(x => x.Nationality).
                Include(g => g.UserProfile).
                    ThenInclude(g => g.Gender).
                Include(g => g.UserProfile).
                    ThenInclude(g => g.Education).
                        ThenInclude(q => q.Degree).
                Include(g => g.UserProfile).
                    ThenInclude(g => g.Religion).
                Include(g => g.UserProfile).
                    ThenInclude(g => g.Work).
                FirstOrDefault();
                ViewBag.genders = (from item in _repoWrapper.Gender.FindAll()
                                   select new SelectListItem
                                   {
                                       Text = item.Name,
                                       Value = item.ID.ToString()
                                   });
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
                if(userVM.User.UserProfile.Nationality.ID == 0)
                {
                    string name= userVM.User.UserProfile.Nationality.Name;
                    if ( name == "")
                    {
                        throw new ArgumentException("Field can`t be empty");
                    }
                    else
                    {
                        userVM.User.UserProfile.Nationality = new Nationality() { Name = name };
                    }
                }

                if (userVM.User.UserProfile.Religion.ID == 0)
                {
                    string name = userVM.User.UserProfile.Religion.Name;
                    if (name == "")
                    {
                        throw new ArgumentException("Field can`t be empty");
                    }
                    else
                    {
                        userVM.User.UserProfile.Religion = new Religion() { Name = name };
                    }
                }

                Degree degree= userVM.User.UserProfile.Education.Degree;
                if (userVM.User.UserProfile.Education.Degree.ID == 0)
                {
                    string name = userVM.User.UserProfile.Education.Degree.Name;
                    if (name == "")
                    {
                        throw new ArgumentException("Field can`t be empty");
                    }
                    else
                    {
                        userVM.User.UserProfile.Education.Degree = new Degree() { Name = name };
                    }
                }

                if (userVM.User.UserProfile.Education.ID == 0)
                {
                    string placeOfStudy = userVM.User.UserProfile.Education.PlaceOfStudy;
                    string speciality = userVM.User.UserProfile.Education.Speciality;
                    if (placeOfStudy == ""  || speciality=="")
                    {
                        throw new ArgumentException("Field can`t be empty");
                    }
                    else
                    {
                        
                        userVM.User.UserProfile.Education = new Education() { PlaceOfStudy = placeOfStudy, Speciality = speciality,Degree=degree};
                    }
                }

                if (userVM.User.UserProfile.Work.ID == 0)
                {
                    string placeOfWork = userVM.User.UserProfile.Work.PlaceOfwork;
                    string position = userVM.User.UserProfile.Work.Position;
                    if (placeOfWork == "" || position == "")
                    {
                        throw new ArgumentException("Field can`t be empty");
                    }
                    else
                    {
                        userVM.User.UserProfile.Work = new Work() { PlaceOfwork = placeOfWork, Position = position };
                    }
                }

                if(userVM.User.UserProfile.PhoneNumber=="" | userVM.User.UserProfile.Address == "" | userVM.User.FatherName=="")
                {
                    throw new ArgumentException("Field can`t be empty");
                }

                //Потрібні правки
                userVM.User.UserProfile.Gender = _repoWrapper.Gender.FindByCondition(x => x.ID == userVM.User.UserProfile.Gender.ID).First();

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
            var user = new User() { Email = registerVM.Email, UserName = registerVM.Name, FirstName = registerVM.Name, LastName = registerVM.SurName, EmailConfirmed = true,
                UserProfile = new UserProfile()
            };

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
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
            ViewBag.religions = _repoWrapper.Religion.FindAll();
            ViewBag.works = _repoWrapper.Work.FindAll();
            ViewBag.educations = _repoWrapper.Education.FindAll();
            ViewBag.degrees = _repoWrapper.Degree.FindAll();


            ViewBag.genders = (from item in _repoWrapper.Gender.FindAll()
                               select new SelectListItem
                               {
                                   Text = item.Name,
                                   Value = item.ID.ToString()
                               });
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
                var nationalities = _repoWrapper.Nationality.FindAll().Include(i=>i.UserProfiles);
                var religions = _repoWrapper.Religion.FindAll().Include(i => i.UserProfiles);
                var works = _repoWrapper.Work.FindAll().Include(i => i.UserProfiles);
                var degrees = _repoWrapper.Degree.FindAll().Include(i=>i.Educations);
                var educations = _repoWrapper.Education.FindAll().Include(i => i.UsersProfiles).Include(i=>i.Degree);

                var nationality = nationalities.FirstOrDefault(x => x.Name.Contains(userVM.User.UserProfile.Nationality.Name));
                if (nationality != null)
                {
                    userVM.User.UserProfile.Nationality.ID = nationality.ID;
                }

                var religion = religions.FirstOrDefault(x => x.Name.Contains(userVM.User.UserProfile.Religion.Name));
                if (religion != null)
                {
                    userVM.User.UserProfile.Religion.ID = religion.ID;
                }

                var work = works.FirstOrDefault(x => x.PlaceOfwork.Contains(userVM.User.UserProfile.Work.PlaceOfwork) && x.Position.Contains(userVM.User.UserProfile.Work.Position));
                if (work != null)
                {
                    userVM.User.UserProfile.Work.ID = work.ID;
                }

                var education = educations.FirstOrDefault(x => x.PlaceOfStudy.Contains(userVM.User.UserProfile.Education.PlaceOfStudy) && x.Speciality.Contains(userVM.User.UserProfile.Education.Speciality));
                if (education != null)
                {
                    userVM.User.UserProfile.Education.ID = education.ID;
                }

                var degree = degrees.FirstOrDefault(x => x.Name.Contains(userVM.User.UserProfile.Education.Degree.Name));
                if (degree != null)
                {
                    userVM.User.UserProfile.Education.Degree.ID = degree.ID;
                }

                _repoWrapper.Gender.Attach(userVM.User.UserProfile.Gender);
                _repoWrapper.Nationality.Attach(userVM.User.UserProfile.Nationality);
                _repoWrapper.Religion.Attach(userVM.User.UserProfile.Religion);
                _repoWrapper.Work.Attach(userVM.User.UserProfile.Work);
                _repoWrapper.Degree.Attach(userVM.User.UserProfile.Education.Degree);
                _repoWrapper.Education.Attach(userVM.User.UserProfile.Education);

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

            var user = new User() { Email = registerVM.Email, UserName = registerVM.Name, LastName = registerVM.SurName, EmailConfirmed = true,
                UserProfile = new UserProfile()
                {
                    Education = new Education { Degree = new Degree() },
                    Work = new Work (),
                    Religion = new Religion (),
                    Nationality = new Nationality (),
                    Gender = new Gender ()
                }
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
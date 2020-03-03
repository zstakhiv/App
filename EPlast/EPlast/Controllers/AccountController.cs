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
using EPlast.DataAccess.Entities.Account;
using EPlast.Models;
using NLog;
using Microsoft.EntityFrameworkCore;

namespace EPlast.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private Logger logger;
        private readonly IRepositoryWrapper _repoWrapper;
        public AccountController(IRepositoryWrapper repoWrapper)
        {
            logger = LogManager.GetCurrentClassLogger();
            _repoWrapper = repoWrapper;
        }

        public IActionResult Index()
        {
            var work=new Work() { PlaceOfwork = "55555555555555555",Position="vasv"};
            UserProfile userProfile = new UserProfile() { Address = "sdava", PhoneNumber = 5145, Work = work };
            _repoWrapper.UserProfile.Create(userProfile);
            _repoWrapper.Save();
            try
            {
                var res=_repoWrapper.UserProfile.FindAll().Include(i=>i.Work);
                //var user = _userService.GetUserProfile(User);
                //var model = _mapper.Map<UserProfile, UserProfileViewModel>(user);
                return View();
            }
            catch (Exception e)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                   // Exception = e
                });
            }
            return View();
        }
        
        public UserProfile createUsers()
        {
            var _degree = new Degree() { DegreeName = "Бакалавр" };
            var userProfile = new UserProfile()
            {
                Nationality = new Nationality() { Name = "Українець" },
                Gender = new Gender() { GenderName = "Чоловіча" },
                Religion = new Religion() { ReligionName = "Християнин" },
                Education = new Education() { PlaceOfStudy = "ЛНУ", Speciality = "Комп.науки", Degree = _degree },
                Work = new Work() { PlaceOfwork = "SoftServe", Position = "Директор" },
                Address = "Stryiska",
                PhoneNumber = 123456789
            };
            var user = new User() { FirstName = "Іван", LastName = "Іваненко", FatherName = "Іванович", Email = "ivan333@gmail.com", UserProfile = userProfile };
            userProfile.User = user;
            _repoWrapper.UserProfile.Create(userProfile);
            _repoWrapper.Save();
            return userProfile;
        }
        public IActionResult Edit()
        {
            var user=createUsers();
            var model = new UserProfileViewModel() { UserProfile=user};
            try
            {
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
        public IActionResult EditConfirmed(UserProfileViewModel user)
        {
            
            try
            {
                _repoWrapper.UserProfile.Update(user.UserProfile);
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
    }
}
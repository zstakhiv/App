using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Microsoft.AspNetCore.Authorization;
using EPlast.DataAccess.Repositories.Contracts;
using EPlast.DataAccess.Repositories;
using EPlast.Models;
using EPlast.BussinessLayer;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using EPlast.BussinessLayer.Interfaces;
using System.Web;

namespace EPlast.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ILogger _logger;
        private readonly IEmailConfirmation _emailConfirmation;
        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IRepositoryWrapper repoWrapper,
            ILogger<AccountController> logger, IEmailConfirmation emailConfirmation)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _repoWrapper = repoWrapper;
            _emailConfirmation = emailConfirmation;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LoginAndRegister()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConfirmedEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Дані введені неправильно");
                return View("LoginAndRegister");
            }

            var registeredUser = await _userManager.FindByEmailAsync(registerVM.Email);
            if (registeredUser != null)
            {
                ModelState.AddModelError("", "Користувач з введеною електронною поштою вже зареєстрований в системі");
                return View("LoginAndRegister");
            }
            else
            {
                var user = new User()
                {
                    Email = registerVM.Email,
                    UserName = registerVM.Email,
                    LastName = registerVM.SurName,
                    FirstName = registerVM.Name,
                    UserProfile = new UserProfile()
                };

                var result = await _userManager.CreateAsync(user, registerVM.Password);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Пароль має містити щонайменше 8 символів, цифри та букви");
                    return View("LoginAndRegister");
                }
                else
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(
                        nameof(ConfirmingEmail),
                        "Account",
                        new { code = code, userId = user.Id },
                        protocol: HttpContext.Request.Scheme);

                    await _emailConfirmation.SendEmailAsync(registerVM.Email, "Підтвердьте вашу реєстрацію",
                        $"Підтвердіть реєстрацію, перейшовши по силці :  <a href='{confirmationLink}'>тут</a> ");

                    return View("AcceptingEmail");
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmingEmail(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("ConfirmedEmail", "Account");
            else
                return View("Error");
        }

        public async Task<IActionResult> Logging(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginVM.Email);
                if(user == null)
                {
                    ModelState.AddModelError("", "Ви не зареєструвались, або не підтвердили свою електронну пошту");
                    return View("LoginAndRegister");
                }
                else
                {
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("", "Ви не підтвердили свою електронну пошту, будь ласка увійдіть та зробіть підтвердження");
                        return View("LoginAndRegister");
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("UserProfile", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Ви ввели неправильний пароль, спробуйте ще раз");
                    return View("LoginAndRegister");
                }
            }
            return View("LoginAndRegister");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LoginAndRegister", "Account");
        }

        [HttpGet]
        public IActionResult UserProfile()
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
            var model = new UserViewModel { User = user };
            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("HandleError", "Error", new { code = 505 });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(string id)
        {

            if (!_repoWrapper.Gender.FindAll().Any())
            {
                _repoWrapper.Gender.Create(new Gender { Name = "" });
                _repoWrapper.Gender.Create(new Gender { Name = "Чоловік" });
                _repoWrapper.Gender.Create(new Gender { Name = "Жінка" });
                _repoWrapper.Save();
            }
            //!!
           
            try
            {
                var user = _repoWrapper.User.
            FindByCondition(q => q.Id == id).
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
                _logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditConfirmed(UserViewModel userVM)
        {
            try
            {
                if (userVM.User.UserProfile.Nationality.ID == 0)
                {
                    string name = userVM.User.UserProfile.Nationality.Name;
                    if(string.IsNullOrEmpty(name))
                    {
                        userVM.User.UserProfile.Nationality = null;
                    }
                    else
                    {
                        userVM.User.UserProfile.Nationality = new Nationality() { Name = name };
                    }
                }

                if (userVM.User.UserProfile.Religion.ID == 0)
                {
                    string name = userVM.User.UserProfile.Religion.Name;
                    if(string.IsNullOrEmpty(name))
                    {
                        userVM.User.UserProfile.Religion = null;
                    }
                    else
                    {
                        userVM.User.UserProfile.Religion = new Religion() { Name = name };
                    }
                }

                Degree degree = userVM.User.UserProfile.Education.Degree;
                if (userVM.User.UserProfile.Education.Degree.ID == 0)
                {
                    string name = userVM.User.UserProfile.Education.Degree.Name;
                    if (string.IsNullOrEmpty(name))
                    {
                        userVM.User.UserProfile.Education.Degree = null;
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
                    if (string.IsNullOrEmpty(placeOfStudy) || string.IsNullOrEmpty(speciality))
                    {
                        userVM.User.UserProfile.Education = null;
                    }
                    else
                    {
                        userVM.User.UserProfile.Education = new Education() { PlaceOfStudy = placeOfStudy, Speciality = speciality, Degree = degree };
                    }
                }

                if (userVM.User.UserProfile.Work.ID == 0)
                {
                    string placeOfWork = userVM.User.UserProfile.Work.PlaceOfwork;
                    string position = userVM.User.UserProfile.Work.Position;
                    if (string.IsNullOrEmpty(placeOfWork) || string.IsNullOrEmpty(position))
                    {
                        userVM.User.UserProfile.Work = null;
                    }
                    else
                    {
                        userVM.User.UserProfile.Work = new Work() { PlaceOfwork = placeOfWork, Position = position };
                    }
                }

                //!!
                userVM.User.UserProfile.Gender = _repoWrapper.Gender.FindByCondition(x => x.ID == userVM.User.UserProfile.Gender.ID).First();

                _repoWrapper.UserProfile.Update(userVM.User.UserProfile);
                _repoWrapper.User.Update(userVM.User);
                _repoWrapper.Save();
                _logger.LogInformation("User {0} {1} was edited profile and saved in the database", userVM.User.FirstName, userVM.User.LastName);
                return RedirectToAction("UserProfile");
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotpasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotpasswordVM.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    ModelState.AddModelError("", "Користувачча з таким імейлом немає в системі, або користувач не підтвердив свою реєстрацію");
                    return View("ForgotPassword");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(
                    nameof(ResetPassword), 
                    "Account", 
                    new { userId = user.Id, code = HttpUtility.UrlEncode(code) }, 
                    protocol: HttpContext.Request.Scheme);
                await _emailConfirmation.SendEmailAsync(forgotpasswordVM.Email, "Reset Password",
                    $"Для скидування пароля перейдіть за : <a href='{callbackUrl}'>посиланням</a>");
                return View("ForgotPasswordConfirmation");
            }
            return View("ForgotPassword");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View("ResetPassword");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetpasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(resetpasswordVM);
            }
            var user = await _userManager.FindByEmailAsync(resetpasswordVM.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Користувачча з таким імейлом немає в системі, або користувач не підтвердив свою реєстрацію");
                return View("ResetPassword");
            }
            var result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(resetpasswordVM.Code), resetpasswordVM.Password);
            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }
            else
            {
                ModelState.AddModelError("", "Проблеми зі скидуванням пароля");
                return View("ResetPassword");
            }
        }
    }
}
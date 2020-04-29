﻿using EPlast.BussinessLayer.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.BussinessLayer.AccessManagers.Interfaces;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Ical.Net.DataTypes;

namespace EPlast.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ILogger _logger;
        private readonly IEmailConfirmation _emailConfirmation;
        private readonly IHostingEnvironment _env;
        private readonly IUserAccessManager _userAccessManager;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IRepositoryWrapper repoWrapper,
            ILogger<AccountController> logger,
            IEmailConfirmation emailConfirmation,
            IHostingEnvironment env,
            IUserAccessManager userAccessManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repoWrapper = repoWrapper;
            _logger = logger;
            _emailConfirmation = emailConfirmation;
            _env = env;
            _userAccessManager = userAccessManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            try
            {
                LoginViewModel loginmVM = new LoginViewModel
                {
                    ReturnUrl = returnUrl,
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                };
                return View(loginmVM);
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginVM, string returnUrl)
        {
            try
            {
                loginVM.ReturnUrl = returnUrl;
                loginVM.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(loginVM.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Ви не зареєстровані в системі, або не підтвердили свою електронну пошту");
                        return View(loginVM);
                    }
                    else
                    {
                        if (!await _userManager.IsEmailConfirmedAsync(user))
                        {
                            ModelState.AddModelError("", "Ваш акаунт не підтверджений, будь ласка увійдіть та зробіть підтвердження");
                            return View(loginVM);
                        }
                    }

                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
                    if (result.IsLockedOut)
                    {
                        return RedirectToAction("AccountLocked", "Account");
                    }

                    if (result.Succeeded)
                    {
                        return RedirectToAction("UserProfile", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ви ввели неправильний пароль, спробуйте ще раз");
                        return View(loginVM);
                    }
                }
                return View("Login", loginVM);
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Дані введені неправильно");
                    return View("Register");
                }

                var registeredUser = await _userManager.FindByEmailAsync(registerVM.Email);
                if (registeredUser != null)
                {
                    ModelState.AddModelError("", "Користувач з введеною електронною поштою вже зареєстрований в системі, " +
                        "можливо він не підтвердив свою реєстрацію");
                    return View("Register");
                }
                else
                {
                    var user = new User()
                    {
                        Email = registerVM.Email,
                        UserName = registerVM.Email,
                        LastName = registerVM.SurName,
                        FirstName = registerVM.Name,
                        RegistredOn = DateTime.Now,
                        ImagePath = "default.png",
                        SocialNetworking = false,
                        UserProfile = new UserProfile()
                    };

                    var result = await _userManager.CreateAsync(user, registerVM.Password);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Пароль має містити цифри та літери, мінімальна довжина повинна складати 8");
                        return View("Register");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Прихильник");
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action(
                            nameof(ConfirmingEmail),
                            "Account",
                            new { code = code, userId = user.Id },
                            protocol: HttpContext.Request.Scheme);

                        user.EmailSendedOnRegister = DateTime.Now;
                        await _userManager.UpdateAsync(user);
                        await _emailConfirmation.SendEmailAsync(registerVM.Email, "Підтвердження реєстрації ",
                            $"Підтвердіть реєстрацію, перейшовши за :  <a href='{confirmationLink}'>посиланням</a> ", "Адміністрація сайту EPlast");

                        return View("AcceptingEmail");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet]
        public IActionResult ConfirmedEmail()
        {
            return View("ConfirmedEmail");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResendEmailForRegistering(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(
                nameof(ConfirmingEmail),
                "Account",
                new { code = code, userId = user.Id },
                protocol: HttpContext.Request.Scheme);

            user.EmailSendedOnRegister = DateTime.Now;
            await _userManager.UpdateAsync(user);
            await _emailConfirmation.SendEmailAsync(user.Email, "Підтвердження реєстрації ",
                $"Підтвердіть реєстрацію, перейшовши за :  <a href='{confirmationLink}'>посиланням</a> ", "Адміністрація сайту EPlast");
            return View("ResendEmailConfirmation");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmingEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
            IDateTime dateTimeConfirming = new DateTimeHelper();
            var totalTime = dateTimeConfirming.GetCurrentTime().Subtract(user.EmailSendedOnRegister).TotalMinutes;
            if (totalTime < 180)
            {
                if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(code))
                {
                    return RedirectToAction("HandleError", "Error", new { code = 500 });
                }

                var result = await _userManager.ConfirmEmailAsync(user, code);

                if (result.Succeeded)
                {
                    return RedirectToAction("ConfirmedEmail", "Account");
                }
                else
                {
                    return RedirectToAction("HandleError", "Error", new { code = 500 });
                }
            }
            else
            {
                return View("ConfirmEmailNotAllowed", user);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccountLocked()
        {
            return View("AccountLocked");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
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
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(forgotpasswordVM.Email);
                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        ModelState.AddModelError("", "Користувача із заданою електронною поштою немає в системі або він не підтвердив свою реєстрацію");
                        return View("ForgotPassword");
                    }

                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Action(
                        nameof(ResetPassword),
                        "Account",
                        new { userId = user.Id, code = HttpUtility.UrlEncode(code) },
                        protocol: HttpContext.Request.Scheme);

                    user.EmailSendedOnForgotPassword = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                    await _emailConfirmation.SendEmailAsync(forgotpasswordVM.Email, "Скидування пароля",
                        $"Для скидування пароля перейдіть за : <a href='{callbackUrl}'>посиланням</a>", "Адміністрація сайту EPlast");
                    return View("ForgotPasswordConfirmation");
                }
                return View("ForgotPassword");
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userId, string code = null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }

            IDateTime dateTimeResetingPassword = new DateTimeHelper();
            dateTimeResetingPassword.GetCurrentTime();
            var totalTime = dateTimeResetingPassword.GetCurrentTime().Subtract(user.EmailSendedOnForgotPassword).TotalMinutes;
            if (totalTime < 180)
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    return RedirectToAction("HandleError", "Error", new { code = 500 });
                }
                else
                {
                    return View("ResetPassword");
                }
            }
            else
            {
                return View("ResetPasswordNotAllowed", user);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetpasswordVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("ResetPassword");
                }
                var user = await _userManager.FindByEmailAsync(resetpasswordVM.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Користувача із заданою електронною поштою немає в системі або він не підтвердив свою реєстрацію");
                    return View("ResetPassword");
                }
                var result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(resetpasswordVM.Code), resetpasswordVM.Password);
                if (result.Succeeded)
                {
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                    }
                    return View("ResetPasswordConfirmation");
                }
                else
                {
                    ModelState.AddModelError("", "Проблеми зі скидуванням пароля або введений новий пароль повинен вміщати 8символів, включаючи літери та цифри");
                    return View("ResetPassword");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = user.SocialNetworking;
            if (result != true)
            {
                return View("ChangePassword");
            }
            else
            {
                return View("ChangePasswordNotAllowed");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        return RedirectToAction("Login");
                    }
                    var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword,
                        model.NewPassword);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Проблема зі зміною пароля, можливо неправильно введений старий пароль");
                        return View("ChangePassword");
                    }
                    await _signInManager.RefreshSignInAsync(user);
                    return View("ChangePasswordConfirmation");
                }
                else
                {
                    return View("ChangePassword");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Account",
                new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            try
            {
                returnUrl = returnUrl ?? Url.Content("~/Account/UserProfile");
                LoginViewModel loginViewModel = new LoginViewModel
                {
                    ReturnUrl = returnUrl,
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                };

                if (remoteError != null)
                {
                    ModelState.AddModelError(string.Empty, $"Error from external provider : {remoteError}");
                    return View("Login", loginViewModel);
                }
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    ModelState.AddModelError(string.Empty, "Error loading external login information");
                    return View("Login", loginViewModel);
                }

                var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                    info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
                if (signInResult.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                    if (info.LoginProvider.ToString() == "Google")
                    {
                        if (email != null)
                        {
                            var user = await _userManager.FindByEmailAsync(email);
                            if (user == null)
                            {
                                user = new User
                                {
                                    SocialNetworking = true,
                                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                                    Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                                    FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                                    LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                                    ImagePath = "default.png",
                                    EmailConfirmed = true,
                                    RegistredOn = DateTime.Now,
                                    UserProfile = new UserProfile()
                                };
                                await _userManager.CreateAsync(user);
                                await _emailConfirmation.SendEmailAsync(user.Email, "Повідомлення про реєстрацію",
                            "Ви зареєструвались в системі EPlast використовуючи свій Google-акаунт ", "Адміністрація сайту EPlast");
                            }
                            await _userManager.AddToRoleAsync(user, "Прихильник");
                            await _userManager.AddLoginAsync(user, info);
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    else if (info.LoginProvider.ToString() == "Facebook")
                    {
                        var nameIdentifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                        var identifierForSearching = email ?? nameIdentifier;
                        var user = _userManager.Users.FirstOrDefault(u => u.UserName == identifierForSearching);
                        if (user == null)
                        {
                            user = new User
                            {
                                SocialNetworking = true,
                                UserName = (email ?? nameIdentifier),
                                FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                                Email = (email ?? "facebookdefaultmail@gmail.com"),
                                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                                ImagePath = "default.png",
                                EmailConfirmed = true,
                                RegistredOn = DateTime.Now,
                                UserProfile = new UserProfile()
                            };
                            await _userManager.CreateAsync(user);
                        }
                        await _userManager.AddToRoleAsync(user, "Прихильник");
                        await _userManager.AddLoginAsync(user, info);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    return View("Error");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        public IActionResult UserProfile(string userId)
        {
            try
            {
                var _currentUserId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    userId = _currentUserId;
                    _logger.Log(LogLevel.Information, "UserId is not null");
                }
                var user = _repoWrapper.User.
                FindByCondition(q => q.Id == userId).
                    Include(i => i.UserProfile).
                        ThenInclude(x => x.Nationality).
                    Include(g => g.UserProfile).
                    ThenInclude(g => g.Gender).
                    Include(g => g.UserProfile).
                        ThenInclude(g => g.Education).
                    Include(g => g.UserProfile).
                        ThenInclude(g => g.Degree).
                    Include(g => g.UserProfile).
                        ThenInclude(g => g.Religion).
                    Include(g => g.UserProfile).
                        ThenInclude(g => g.Work).
                    Include(x => x.ConfirmedUsers).
                        ThenInclude(q => (q as ConfirmedUser).Approver).
                        ThenInclude(q => q.User).
                    FirstOrDefault();
                var userPositions = _repoWrapper.CityAdministration
                    .FindByCondition(ca => ca.UserId == userId)
                        .Include(ca => ca.AdminType)
                        .Include(ca => ca.City);

                var edit = Edit(userId);
                if (edit == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = 500 });
                }

                var _canApprove = user.ConfirmedUsers.Count < 3
                    && !user.ConfirmedUsers.Any(x => x.Approver.UserID == _currentUserId)
                    && !(_currentUserId == userId);

                TimeSpan _timeToJoinPlast = CheckOrAddPlastunRole(user).Result;

                if (user != null)
                {
                    var model = new UserViewModel
                    {
                        User = user,
                        UserPositions = userPositions,
                        HasAccessToManageUserPositions = _userAccessManager.HasAccess(_userManager.GetUserId(User), userId),
                        EditView = edit,
                        canApprove = _canApprove,
                        timeToJoinPlast = _timeToJoinPlast
                    };

                    return View(model);
                }
                _logger.Log(LogLevel.Error, $"Can`t find this user:{userId}, or smth else");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        private async Task<TimeSpan> CheckOrAddPlastunRole(User user)
        {
            try
            {
                var _timeToJoinPlast = user.RegistredOn.AddYears(1) - DateTime.Now;
                if (_timeToJoinPlast <= TimeSpan.Zero)
                {
                    var us = await _userManager.FindByIdAsync(user.Id);
                    await _userManager.AddToRoleAsync(us, "Пластун");
                    return TimeSpan.Zero;
                }
                return _timeToJoinPlast;
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }

        public IActionResult ApproveUser(string userId)
        {
            if (userId != null)
            {
                var id = _userManager.GetUserId(User);

                var conUs = new ConfirmedUser { UserID = userId, ConfirmDate = DateTime.Now };
                var appUs = new Approver { UserID = id, ConfirmedUser = conUs };
                conUs.Approver = appUs;

                _repoWrapper.ConfirmedUser.Create(conUs);
                _repoWrapper.Save();
                return RedirectToAction("UserProfile", "Account", new { userId = userId });
            }
            return RedirectToAction("HandleError", "Error", new { code = 505 });
        }

        public IActionResult ApproverDelete(string userId)
        {
            var id = _userManager.GetUserId(User);
            var user = _repoWrapper.User.FindByCondition(x => x.Id == userId).
                Include(x => x.ConfirmedUsers).
                        ThenInclude(q => (q as ConfirmedUser).Approver).
                        ThenInclude(q => q.User).
                        FirstOrDefault();
            var t = user.ConfirmedUsers;
            var confUser = user.ConfirmedUsers.Where(x => x.Approver.UserID == id).FirstOrDefault();
            _repoWrapper.ConfirmedUser.Delete(confUser);
            _repoWrapper.Save();
            return RedirectToAction("UserProfile", "Account", new { userId = userId });
        }

        private EditUserViewModel Edit(string id)
        {
            if (!_repoWrapper.Gender.FindAll().Any())
            {
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
                Include(g => g.UserProfile).
                    ThenInclude(g => g.Degree).
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
                var placeOfStudyUnique = _repoWrapper.Education.FindAll().GroupBy(x => x.PlaceOfStudy).Select(x => x.FirstOrDefault()).ToList();
                var specialityUnique = _repoWrapper.Education.FindAll().GroupBy(x => x.Speciality).Select(x => x.FirstOrDefault()).ToList();
                var placeOfWorkUnique = _repoWrapper.Work.FindAll().GroupBy(x => x.PlaceOfwork).Select(x => x.FirstOrDefault()).ToList();
                var positionUnique = _repoWrapper.Work.FindAll().GroupBy(x => x.Position).Select(x => x.FirstOrDefault()).ToList();

                var educView = new EducationViewModel { PlaceOfStudyID = user.UserProfile.EducationId, SpecialityID = user.UserProfile.EducationId, PlaceOfStudyList = placeOfStudyUnique, SpecialityList = specialityUnique };
                var workView = new WorkViewModel { PlaceOfWorkID = user.UserProfile.WorkId, PositionID = user.UserProfile.WorkId, PlaceOfWorkList = placeOfWorkUnique, PositionList = positionUnique };
                var model = new EditUserViewModel()
                {
                    User = user,
                    Nationalities = _repoWrapper.Nationality.FindAll(),
                    Religions = _repoWrapper.Religion.FindAll(),
                    EducationView = educView,
                    WorkView = workView,
                    Degrees = _repoWrapper.Degree.FindAll(),
                };

                return model;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(EditUserViewModel model, IFormFile file)
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

                //Nationality
                if (model.User.UserProfile.NationalityId == null)
                {
                    if (string.IsNullOrEmpty(model.User.UserProfile.Nationality.Name))
                    {
                        model.User.UserProfile.Nationality = null;
                    }
                }
                else
                {
                    model.User.UserProfile.Nationality = null;
                }

                //Religion
                if (model.User.UserProfile.ReligionId == null)
                {
                    if (string.IsNullOrEmpty(model.User.UserProfile.Religion.Name))
                    {
                        model.User.UserProfile.Religion = null;
                    }
                }
                else
                {
                    model.User.UserProfile.Religion = null;
                }

                //Degree
                if (model.User.UserProfile.DegreeId == null)
                {
                    if (string.IsNullOrEmpty(model.User.UserProfile.Degree.Name))
                    {
                        model.User.UserProfile.Degree = null;
                    }
                }
                else
                {
                    model.User.UserProfile.Degree = null;
                }

                //Education
                if (model.EducationView.SpecialityID == model.EducationView.PlaceOfStudyID)
                {
                    model.User.UserProfile.EducationId = model.EducationView.SpecialityID;
                }
                else
                {
                    var spec = _repoWrapper.Education.FindByCondition(x => x.ID == model.EducationView.SpecialityID).FirstOrDefault();
                    var placeStudy = _repoWrapper.Education.FindByCondition(x => x.ID == model.EducationView.PlaceOfStudyID).FirstOrDefault();
                    if (spec != null && spec.PlaceOfStudy == model.User.UserProfile.Education.PlaceOfStudy)
                    {
                        model.User.UserProfile.EducationId = spec.ID;
                    }
                    else if (placeStudy != null && placeStudy.Speciality == model.User.UserProfile.Education.Speciality)
                    {
                        model.User.UserProfile.EducationId = placeStudy.ID;
                    }
                    else
                    {
                        model.User.UserProfile.EducationId = null;
                    }
                }

                if (model.User.UserProfile.EducationId == null || model.User.UserProfile.EducationId == 0)
                {
                    if (string.IsNullOrEmpty(model.User.UserProfile.Education.PlaceOfStudy) && string.IsNullOrEmpty(model.User.UserProfile.Education.Speciality))
                    {
                        model.User.UserProfile.Education = null;
                        model.User.UserProfile.EducationId = null;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(model.User.UserProfile.Education.PlaceOfStudy) || string.IsNullOrEmpty(model.User.UserProfile.Education.Speciality))
                    {
                        model.User.UserProfile.EducationId = null;
                    }
                    else
                    {
                        model.User.UserProfile.Education = null;
                    }
                }

                //Work
                if (model.WorkView.PositionID == model.WorkView.PlaceOfWorkID)
                {
                    model.User.UserProfile.WorkId = model.WorkView.PositionID;
                }
                else
                {
                    var placeWork = _repoWrapper.Work.FindByCondition(x => x.ID == model.WorkView.PlaceOfWorkID).FirstOrDefault();
                    var position = _repoWrapper.Work.FindByCondition(x => x.ID == model.WorkView.PositionID).FirstOrDefault();
                    if (placeWork != null && placeWork.Position == model.User.UserProfile.Work.Position)
                    {
                        model.User.UserProfile.WorkId = placeWork.ID;
                    }
                    else if (position != null && position.PlaceOfwork == model.User.UserProfile.Work.PlaceOfwork)
                    {
                        model.User.UserProfile.WorkId = position.ID;
                    }
                    else
                    {
                        model.User.UserProfile.WorkId = null;
                    }
                }

                if (model.User.UserProfile.WorkId == null || model.User.UserProfile.WorkId == 0)
                {
                    if (string.IsNullOrEmpty(model.User.UserProfile.Work.PlaceOfwork) && string.IsNullOrEmpty(model.User.UserProfile.Work.Position))
                    {
                        model.User.UserProfile.Work = null;
                        model.User.UserProfile.WorkId = null;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(model.User.UserProfile.Work.PlaceOfwork) || string.IsNullOrEmpty(model.User.UserProfile.Work.Position))
                    {
                        model.User.UserProfile.WorkId = null;
                    }
                    else
                    {
                        model.User.UserProfile.Work = null;
                    }
                }

                _repoWrapper.User.Update(model.User);
                _repoWrapper.UserProfile.Update(model.User.UserProfile);
                _repoWrapper.Save();
                _logger.LogInformation("User {0} {1} was edited profile and saved in the database", model.User.FirstName, model.User.LastName);
                return RedirectToAction("UserProfile");
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу, Голова Станиці")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            try
            {
                CityAdministration cityAdministration = _repoWrapper.CityAdministration
                    .FindByCondition(ca => ca.ID == id)
                        .Include(ca => ca.AdminType)
                        .Include(ca => ca.User)
                    .First();
                var userId = _userManager.GetUserId(User);
                if (!_userAccessManager.HasAccess(userId, cityAdministration.UserId))
                {
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                }
                if (cityAdministration.EndDate == null)
                {
                    await _userManager.RemoveFromRoleAsync(cityAdministration.User, cityAdministration.AdminType.AdminTypeName);
                }
                _repoWrapper.CityAdministration.Delete(cityAdministration);
                _repoWrapper.Save();
                return Ok("Діловодство успішно видалено!");
            }
            catch
            {
                return NotFound("Не вдалося видалити діловодство!");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу, Голова Станиці")]
        public async Task<IActionResult> EndPosition(int id)
        {
            try
            {
                CityAdministration cityAdministration = _repoWrapper.CityAdministration
                    .FindByCondition(ca => ca.ID == id)
                        .Include(ca => ca.AdminType)
                        .Include(ca => ca.User)
                    .First();
                var userId = _userManager.GetUserId(User);
                if (!_userAccessManager.HasAccess(userId, cityAdministration.UserId))
                {
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                }
                cityAdministration.EndDate = DateTime.Today;
                _repoWrapper.CityAdministration.Update(cityAdministration);
                _repoWrapper.Save();
                await _userManager.RemoveFromRoleAsync(cityAdministration.User, cityAdministration.AdminType.AdminTypeName);
                return Ok("Каденцію діловодства успішно завершено!");
            }
            catch
            {
                return NotFound("Не вдалося завершити каденцію діловодства!");
            }
        }
    }

    public interface IDateTime
    {
        DateTime GetCurrentTime();
    }

    public class DateTimeHelper : IDateTime
    {
        DateTime IDateTime.GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
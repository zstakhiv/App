using EPlast.BussinessLayer.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        private readonly IHostingEnvironment _env;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IRepositoryWrapper repoWrapper,
            ILogger<AccountController> logger,
            IEmailConfirmation emailConfirmation,
            IHostingEnvironment env)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _repoWrapper = repoWrapper;
            _emailConfirmation = emailConfirmation;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConfirmedEmail()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccountLocked()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.IsEmailConfirmedAsync(user);
            if (result)
            {
                return View("ChangePassword");
            }
            else
            {
                ModelState.AddModelError("", "Незареєстрований користувач не може змінити пароль");
                return RedirectToAction("UserProfile", "Account");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Дані введені неправильно");
                return View("Register");
            }

            var registeredUser = await _userManager.FindByEmailAsync(registerVM.Email);
            if (registeredUser != null)
            {
                ModelState.AddModelError("", "Користувач з введеною електронною поштою вже зареєстрований в системі");
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
                    ImagePath = "default.png",
                    UserProfile = new UserProfile()
                };

                var result = await _userManager.CreateAsync(user, registerVM.Password);
                await _userManager.AddToRoleAsync(user, "Користувач");

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Пароль має містити щонайменше 8 символів, цифри та літери");
                    return View("Register");
                }
                else
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(
                        nameof(ConfirmingEmail),
                        "Account",
                        new { code = code, userId = user.Id },
                        protocol: HttpContext.Request.Scheme);

                    await _emailConfirmation.SendEmailAsync(registerVM.Email, "Підтвердження реєстрації ",
                        $"Підтвердіть реєстрацію, перейшовши за :  <a href='{confirmationLink}'>посиланням</a> ", "Адміністрація сайту EPlast");

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
            {
                //Цей код повинен знаходитись тут(замість 99 рядка) при релізі проекту
                //await _userManager.AddToRoleAsync(user, "Користувач");
                return RedirectToAction("ConfirmedEmail", "Account");
            }
            else
                return View("Error");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginVM, string returnUrl)
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
            return View("Login");
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
                var model = new EditUserViewModel()
                {
                    User = user,
                    Nationalities = _repoWrapper.Nationality.FindAll(),
                    Religions = _repoWrapper.Religion.FindAll(),
                    Educations = _repoWrapper.Education.FindAll(),
                    Works = _repoWrapper.Work.FindAll(),
                    Degrees = _repoWrapper.Degree.FindAll()
                };

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
                    ModelState.AddModelError("", "Користувача із заданою електронною поштою немає в системі або він не підтвердив свою реєстрацію");
                    return View("ForgotPassword");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(
                    nameof(ResetPassword),
                    "Account",
                    new { userId = user.Id, code = HttpUtility.UrlEncode(code) },
                    protocol: HttpContext.Request.Scheme);
                await _emailConfirmation.SendEmailAsync(forgotpasswordVM.Email, "Скидування пароля",
                    $"Для скидування пароля перейдіть за : <a href='{callbackUrl}'>посиланням</a>", "Адміністрація сайту EPlast");
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

        [HttpPost]
        [AllowAnonymous]
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
            returnUrl = returnUrl ?? Url.Content("~/Account/UserProfile");
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider : {remoteError}");
                return View("Login");
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
                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                            LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                            ImagePath = "default.png",
                            UserProfile = new UserProfile(),
                        };
                        await _userManager.CreateAsync(user);
                    }
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
                ViewBag.ErrorTitle = $"Email claim not received from : {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on Pragim@PragimTech.com";

                return View("Error");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if(user == null)
                {
                    return RedirectToAction("Login");
                }
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword,
                    model.NewPassword);
                if (!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await _signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }
            return View(model);
        }
    }
}
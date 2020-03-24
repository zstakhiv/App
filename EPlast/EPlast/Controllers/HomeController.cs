using EPlast.BussinessLayer.Interfaces;
using EPlast.Models;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EPlast.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailConfirmation _emailConfirmation;

        public HomeController(IEmailConfirmation emailConfirmation)
        {
            _emailConfirmation = emailConfirmation;
        }

        public IActionResult Index()
        {
            return View();
            //return StatusCode(500);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AboutPLAST()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View("Views/Home/Contacts.cshtml");
        }

        public IActionResult FAQ()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("User/GetPage")]
        public IActionResult GetInformation()
        {
            return View("Views/Account/LoginAndRegister.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> SendContacts(ContactsViewModel contactsViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Дані введені неправильно");
                return View("Contacts");
            }

            await _emailConfirmation.SendEmailAsync("eplastdmnstrtr@gmail.com", 
                "Питання користувачів",
                "Питання",
                contactsViewModel.Email);

            return View("Contacts");
        }





        /*[HttpPost]
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
*/


    }
}
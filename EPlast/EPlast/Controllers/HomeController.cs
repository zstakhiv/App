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
            return View("Views/Account/Login.cshtml");
        }

        [HttpGet]
        public IActionResult FeedBackSended()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendContacts(ContactsViewModel contactsViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Дані введені неправильно");
                    return View("Contacts");
                }
                else
                {
                    await _emailConfirmation.SendEmailAsync("eplastdmnstrtr@gmail.com",
                    "Питання користувачів",
                     $"Контактні дані користувача : Електронна пошта {contactsViewModel.Email}, Ім'я {contactsViewModel.Name}, Телефон {contactsViewModel.PhoneNumber}" +
                     $"  Опис питання : {contactsViewModel.FeedBackDescription}",
                     contactsViewModel.Email);
                }
                return RedirectToAction("FeedBackSended", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
    }
}
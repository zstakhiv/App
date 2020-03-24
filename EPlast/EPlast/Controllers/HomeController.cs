using EPlast.BussinessLayer.Interfaces;
using EPlast.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace EPlast.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmailConfirmation _emailConfirmation;

        public HomeController(ILogger logger, IEmailConfirmation emailConfirmation)
        {
            _logger = logger;
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
            return View();
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

         

    }
}
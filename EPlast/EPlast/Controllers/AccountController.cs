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

namespace EPlast.Controllerss
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

       
        

        public IActionResult Index()
        {
            _logger.LogDebug("Indexxxxxxxxxxxxxxxxxxxxxxxxxx");
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
    }
}
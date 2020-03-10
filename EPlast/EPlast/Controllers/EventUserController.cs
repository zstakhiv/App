using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.Controllers
{
    public class EventUserController : Controller
    {
        public IActionResult EventUser()
        {
            return View();
        }
    }
}
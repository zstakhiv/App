﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.Controllers
{
    public class CItyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CityProfile()
        {
            return View();
        }
    }
}
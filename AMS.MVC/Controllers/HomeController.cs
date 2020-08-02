﻿using System.Diagnostics;
using AMS.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AMS.MVC.Controllers
{
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}

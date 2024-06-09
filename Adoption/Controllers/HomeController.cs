using Adoption.Areas.Identity.Data;
using Adoption.Data;
using Adoption.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace Adoption.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
      
        private readonly Data.AdoptionContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, Data.AdoptionContext context)
        {
            _context = context;
            _logger = logger;
            this._userManager = userManager;
            
        }

        public IActionResult Index()
        {
            IEnumerable<Dog> dogs = _context.Dogs.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITO.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITO.Controllers
{
    public class DivisionEnentController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        readonly UserManager<User> _userManager;
        private readonly AllContext db;
        IWebHostEnvironment _appEnvironment;

        public DivisionEnentController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, AllContext context, IWebHostEnvironment appEnvironment)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            db = context;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

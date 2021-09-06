using DotacionWEBCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DotacionWEBCore.Controllers
{
    public class EnDesarrolloController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly DatabaseContext _context;


        [Authorize]
        public IActionResult EnDesarrollo()
        {
            return View();
        }

    }

}



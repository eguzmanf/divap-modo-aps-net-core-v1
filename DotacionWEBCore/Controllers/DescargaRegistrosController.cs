using DotacionWEBCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DotacionWEBCore.Controllers
{
    public class DescargaRegistrosController : Controller
    {

        private readonly IConfiguration configuration;
        private readonly DatabaseContext _context;

        public DescargaRegistrosController(DatabaseContext context, IConfiguration config)
        {
            _context = context;
            configuration = config;
        }

        public IActionResult DescargaRegistros()
        {
            return View();
        }


    }
}



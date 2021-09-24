using DotacionWEBCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace DotacionWEBCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly DatabaseContext _context;
        private readonly ISession _session;


        // ------- definiendo conexion a base de datos ------- //
        public HomeController(DatabaseContext context, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.configuration = config;
            _session = httpContextAccessor.HttpContext.Session;

        }
        [Authorize]
        public IActionResult Index()
        {
            // var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            // HttpContext.Session.SetString("Role1", Perfil_U.ToString());
            // _session.SetString("Role2", Perfil_U.ToString());
            // ViewBag.Role = Perfil_U;
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            //obtiene datos del pop up //
            string Connstr1 = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn1 = new SqlConnection(Connstr1);
            conn1.Open();
            string query2 = "SELECT [Mensaje] FROM .[dbo].[DOTACION_Mensaje] WHERE [ID_mensaje] = 1";
            SqlCommand cmd2 = new SqlCommand(query2, conn1);
            var valorMensaje = cmd2.ExecuteScalar();
            ViewData["Mensaje"] = valorMensaje;
            return View();
        }

        public IActionResult Privacy()
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public FileResult Cargamanual()
        {

            //--------------------CODIGO PARA CARGAR MANUAL------------------------// 
            string Connstr1 = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn1 = new SqlConnection(Connstr1);
            // byte[] bytes = System.IO.File.ReadAllBytes("C:/Users/cesarrivera/Desktop/César Rivera Serrano/2019/11 - DOTACION/Manual -MoDo APS v1.0.pdf");
            byte[] bytes = System.IO.File.ReadAllBytes("C:/Manual-MoDo-APS_v2.0.pdf");
            conn1.Open();
            // string query = "UPDATE [dbo].[DOTACION_Archivos] set [Archivo] = @archivo, [direccion] = 'C:/Users/cesarrivera/Desktop/Manual -MoDo APS v1.0.pdf' WHERE IdArchivo = 1";
            string query = "UPDATE [dbo].[DOTACION_Archivos] set [Archivo] = @archivo, [direccion] = 'C:/Manual-MoDo-APS_v2.0.pdf' WHERE IdArchivo = 1";
            SqlCommand cmd = new SqlCommand(query, conn1);
            cmd.Parameters.AddWithValue("@archivo", bytes);
            cmd.ExecuteReader();
            conn1.Close();
            ModelState.AddModelError("File", "Manual cargado");
            return Cargamanual();
        }

        public FileResult CargaExcel()
        {

            //--------------------CODIGO PARA CARGAR eXCEL DE CARGA------------------------// 
            string Connstr1 = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn1 = new SqlConnection(Connstr1);
            byte[] bytes = System.IO.File.ReadAllBytes("C:/Users/cesarrivera/Desktop/César Rivera Serrano/2019/11 - DOTACION/Prueba nuevo formato/Araucania Norte.xlsx");
            conn1.Open();
            string query = "UPDATE [dbo].[DOTACION_Archivos] set [Archivo] = @archivo, [direccion] = 'C:/Users/cesarrivera/Desktop/César Rivera Serrano/2019/11 - DOTACION/Prueba nuevo formato/Araucania Norte.xlsx' WHERE IdArchivo = 30";
            SqlCommand cmd = new SqlCommand(query, conn1);
            cmd.Parameters.AddWithValue("@archivo", bytes);
            cmd.ExecuteReader();
            conn1.Close();
            ModelState.AddModelError("File", "Excel cargado");
            return CargaExcel();
        }

        public FileResult ReadMe()
        {

            string Connstr1 = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn1 = new SqlConnection(Connstr1);
            conn1.Open();
            string query = "SELECT TOP 1 [Archivo] FROM .[dbo].[DOTACION_Archivos] where IdArchivo = 1";
            SqlCommand cmd = new SqlCommand(query, conn1);
            SqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            byte[] bytes = (Byte[])rdr["Archivo"];
            System.IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/manual.pdf", bytes);
            conn1.Close();
            return File(bytes, "application/pdf");
        }


        public FileResult CargaMasiva()

        {
            string Connstr1 = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn1 = new SqlConnection(Connstr1);
            conn1.Open();
            string query = "SELECT TOP 1 [Archivo] FROM .[dbo].[DOTACION_Archivos] where IdServicio = 2";
            SqlCommand cmd = new SqlCommand(query, conn1);
            SqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            byte[] bytes = (Byte[])rdr["Archivo"];
            System.IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/Excel.xlsx", bytes);
            conn1.Close();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

       [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExampleModalLoading()
        {
            //obtiene datos del pop up //
            string Connstr1 = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn1 = new SqlConnection(Connstr1);
            conn1.Open();
            string query2 = "SELECT DISTINCT [Mensaje] FROM .[dbo].[DOTACION_Mensaje] WHERE [ID_mensaje] = 1";
            SqlCommand cmd2 = new SqlCommand(query2, conn1);
            var valorMensaje = cmd2.ExecuteScalar();
            //ViewData["Mensaje"] = "prueba de texto para ver como funciona este asunto";
            return View();
        }
       
    }

}



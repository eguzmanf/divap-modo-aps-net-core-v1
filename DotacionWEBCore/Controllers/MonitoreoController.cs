using DotacionWEBCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DotacionWEBCore.Controllers
{
    public class MonitoreoController : Controller
    {

        private readonly IConfiguration configuration;
        private readonly DatabaseContext _context;

        public MonitoreoController(DatabaseContext context, IConfiguration config)
        {
            _context = context;
            this.configuration = config;
        }

        public IActionResult Monitoreo()
        {

            //--Conecta a SQL--//
            string Connstr = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(Connstr);
            conn.Open();

            // obtiene Rut del usuario logeado//
            string userId = User.Identity.Name;
            ViewData["Rut"] = userId;

            //Obtiene perfil del usuario//
            string Perfil = "(SELECT TOP 1 [ID_Perfil] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + userId + "')";
            SqlCommand cmdPerfil = new SqlCommand(Perfil, conn);
            var SQLPerfil = cmdPerfil.ExecuteScalar();
            int Perfil_U = Int32.Parse(SQLPerfil.ToString());


            // Muestra datos según Perfil //
            if (Perfil_U == 1)
            //Muestra Resultado Perfil Servicio //
            {
                //Obtiene IdServicio del usuario//
                string IdServicio = "(SELECT TOP 1 [IdServicio] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + userId + "')";
                SqlCommand cmdIdServicio = new SqlCommand(IdServicio, conn);
                var SQLIdServicio = cmdIdServicio.ExecuteScalar();

                //Obtiene total de registros IdServicio del usuario//
                string Registro = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados] where [IdServicio] = " + SQLIdServicio;
                SqlCommand cmdRegistros = new SqlCommand(Registro, conn);
                var TotalRegistros = cmdRegistros.ExecuteScalar();
                ViewData["TotalRegistros"] = TotalRegistros;

                //Obtiene total de registros de Servicio del usuario//
                string RegistroServicio = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados] where [IdServicio] = " + SQLIdServicio;
                SqlCommand cmdRegistrosServicio = new SqlCommand(RegistroServicio, conn);
                var TotalRegistrosServicio = cmdRegistrosServicio.ExecuteScalar();
                ViewData["RegistrosComunaServicio"] = TotalRegistrosServicio;

                //Obtiene detalle de registros IdServicio del usuario//

                List<ListaRegistros> MonitoreoRegistros_S = new List<ListaRegistros>();
                MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados
                                      where Resultados.ID_Servicio == Int32.Parse(SQLIdServicio.ToString())
                orderby Resultados.Rut, Resultados.Comuna, Resultados.Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                      select Resultados).ToList();
                ViewBag.Resultados = MonitoreoRegistros_S.ToList();
            }

            else
            {
                //Obtiene IdServicio del usuario//
                string IdServicio = "(SELECT TOP 1 [IdServicio] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + userId + "')";
                SqlCommand cmdIdServicio = new SqlCommand(IdServicio, conn);
                var SQLIdServicio = cmdIdServicio.ExecuteScalar();

                //Obtiene IdComuna del usuario//
                string IdComuna = "(SELECT TOP 1 [ID_Comuna_U] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + userId + "')";
                SqlCommand cmdIdComuna = new SqlCommand(IdComuna, conn);
                var SQLIdComuna = cmdIdComuna.ExecuteScalar();

                //Obtiene total de registros IdServicio del usuario//
                string Registro = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados] where [IdServicio] = " + SQLIdServicio;
                SqlCommand cmdRegistros = new SqlCommand(Registro, conn);
                var TotalRegistros = cmdRegistros.ExecuteScalar();
                ViewData["TotalRegistros"] = TotalRegistros;

                //Obtiene total de registros de comuna del usuario//
                string RegistroComuna = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados] where [IdComuna] = " + SQLIdComuna;
                SqlCommand cmdRegistrosComuna = new SqlCommand(RegistroComuna, conn);
                var TotalRegistrosComuna = cmdRegistrosComuna.ExecuteScalar();
                ViewData["RegistrosComunaServicio"] = TotalRegistrosComuna;

                //Obtiene detalle de registros IdComuna del usuario//

                List<ListaRegistros> MonitoreoRegistros_C = new List<ListaRegistros>();
                MonitoreoRegistros_C = (from Resultados in _context.DOTACION_Registros_resultados
                                      where Resultados.ID_Comuna == Int32.Parse(SQLIdComuna.ToString())
                orderby Resultados.Rut, Resultados.Comuna, Resultados.Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                      select Resultados).ToList();
                ViewBag.Resultados = MonitoreoRegistros_C.ToList();


            }


            conn.Close();
            return View();
        }


    }
}
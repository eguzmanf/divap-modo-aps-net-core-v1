using DotacionWEBCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace DotacionWEBCore.Controllers
{
    public class EliminaRegistrosController : Controller
    {
        // ------- definiendo conexion a base de datos ------- //
        private readonly IConfiguration configuration;
        private readonly DatabaseContext _context;
        private int Id_Validacion_Registro;

        public EliminaRegistrosController(DatabaseContext context, IConfiguration config)
        {
            _context = context;
            configuration = config;

        }


        [HttpGet]
        public IActionResult EliminaRegistros(string ID)
        {
            //--conecta a SQL--//
            string Connstr = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(Connstr);
            conn.Open();

            string ID_Validacion = "SELECT ID_Registro FROM .[dbo].[DOTACION_Registros_resultados] where [ID] = '" + ID + "'";
            SqlCommand cmdID_ValidacionRegistro = new SqlCommand(ID_Validacion, conn);
            var SQLIDValidacionRegistro = cmdID_ValidacionRegistro.ExecuteScalar();
            Id_Validacion_Registro = Int32.Parse(SQLIDValidacionRegistro.ToString());


            string query = "UPDATE .[dbo].[DOTACION_Registros] SET [Activo] = 1 WHERE  ID_Registro = " + Id_Validacion_Registro;
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteReader();
            conn.Close();
                        
            ModelState.AddModelError("Error", "Se ha eliminado el Registro");
      
            return RedirectToAction("ActualizaRegistros", "ActualizaRegistros");
        }
    }
}



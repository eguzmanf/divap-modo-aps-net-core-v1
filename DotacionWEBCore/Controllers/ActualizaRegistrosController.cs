using DotacionWEBCore.Models;
using DotacionWEBCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace DotacionWEBCore.Controllers
{
    public class ActualizaRegistrosController : Controller
    {
        private static string userId;

        // ------- definiendo conexion a base de datos ------- //
        private readonly IConfiguration configuration;
        private readonly DatabaseContext _context;
        private int Perfil_U;
        private SqlConnection conn;
        private int Id_Validacion_Registro;
        public static string Filtro;

        // ------- definiendo conexion a base de datos ------- //
        public ActualizaRegistrosController(DatabaseContext context, IConfiguration config)
        {
            _context = context;
            configuration = config;
        }

        [HttpGet]
        public async Task<IActionResult> ActualizaRegistros(ListadoViewModel<ListaRegistros> modelo)
        {
            //--conecta a SQL--//
            string Connstr = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(Connstr);
            conn.Open();

            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();
            /*
            //Obtiene perfil del usuario//
            string Perfil = "SELECT TOP 1 [ID_Perfil] FROM [dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdPerfil = new SqlCommand(Perfil, conn);
            var SQLPerfil = cmdPerfil.ExecuteScalar();
            Perfil_U = Int32.Parse(SQLPerfil.ToString());
            */

            var SQLIdServicio = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.IdServicio).First();
            /*
            //Obtiene IdServicio del usuario//
            string IdServicio = "SELECT TOP 1 [IdServicio] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdIdServicio = new SqlCommand(IdServicio, conn);
            var SQLIdServicio = cmdIdServicio.ExecuteScalar();
            */

            var SQLIdComuna = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Comuna_U).First();
            /*
            //Obtiene IdComuna del usuario//
            string IdComuna = "SELECT TOP 1 [ID_Comuna_U] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdIdComuna = new SqlCommand(IdComuna, conn);
            var SQLIdComuna = cmdIdComuna.ExecuteScalar();
            */

            var TotalRegistros = _context.DOTACION_Registros_resultados.Where(s => s.ID_Servicio == SQLIdServicio && s.Activo == 0).Count();
            ViewData["TotalRegistrosServicio"] = TotalRegistros;
            /*
            //Obtiene total de registros del servicio del usuario//
            string Registro = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados] where [ID_Servicio] = " + SQLIdServicio + " AND [Activo] = 0";
            SqlCommand cmdRegistros = new SqlCommand(Registro, conn);
            var TotalRegistros = cmdRegistros.ExecuteScalar();
            ViewData["TotalRegistrosServicio"] = TotalRegistros;
            */

            var TotalRegistrosServicio = _context.DOTACION_Registros_resultados.Where(s => s.ID_Servicio == SQLIdServicio && s.Validado == 0 && s.Activo == 0).Count();
            ViewData["TotalRegistrosServicioPorValidar"] = TotalRegistrosServicio;
            /*
            //Obtiene total de registros por validar del Servicio del usuario//
            string RegistroServicio = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados] where [ID_Servicio] = " + SQLIdServicio + " and [Validado] = 0 AND [Activo] = 0";
            SqlCommand cmdRegistrosServicio = new SqlCommand(RegistroServicio, conn);
            var TotalRegistrosServicio = cmdRegistrosServicio.ExecuteScalar();
            ViewData["TotalRegistrosServicioPorValidar"] = TotalRegistrosServicio;
            */

            var TotalRegistroUsuario = _context.DOTACION_Registros_resultados.Where(s => s.ID_Comuna == SQLIdComuna && s.Activo == 0).Count();
            ViewData["TotalRegistrosUsuario"] = TotalRegistroUsuario;
            /*
            //Obtiene total de registros de la comuna del usuario//
            string RegistroUsuario = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados] where [ID_Comuna] = '" + SQLIdComuna + "' AND [Activo] = 0";
            SqlCommand cmdRegistrosUsuario = new SqlCommand(RegistroUsuario, conn);
            var TotalRegistroUsuario = cmdRegistrosUsuario.ExecuteScalar();
            ViewData["TotalRegistrosUsuario"] = TotalRegistroUsuario;
            */

            var TotalRegistroUsuarioval = _context.DOTACION_Registros_resultados.Where(s => s.ID_Comuna == SQLIdComuna && s.Validado == 0 && s.Activo == 0).Count();
            ViewData["TotalRegistrosUsuarioPorValidar"] = TotalRegistroUsuarioval;
            /*
            //Obtiene total de registros por validar de la comuna del usuario//
            string RegistroUsuarioval = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados] where [ID_Comuna] = '" + SQLIdComuna + "' and [Validado] = 0  AND [Activo] = 0";
            SqlCommand cmdRegistrosUsuarioval = new SqlCommand(RegistroUsuarioval, conn);
            var TotalRegistroUsuarioval = cmdRegistrosUsuarioval.ExecuteScalar();
            ViewData["TotalRegistrosUsuarioPorValidar"] = TotalRegistroUsuarioval;
            */

            var TotalRegistrosMinsal = _context.DOTACION_Registros_resultados.Where(s => s.Activo == 0).Count();
            ViewData["TotalRegistrosMinsal"] = TotalRegistrosMinsal;

            var TotalRegistrosMinsalPorValidar = _context.DOTACION_Registros_resultados.Where(s => s.Validado == 0 && s.Activo == 0).Count();
            ViewData["TotalRegistrosMinsalPorValidar"] = TotalRegistrosMinsalPorValidar;

            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //Perfil_U = 2;

            {
                ViewBag.ID_PerfilUsuario = Perfil_U;
                ViewBag.IdServicioUsuario = SQLIdServicio;
                ViewBag.IdComunaUsuario = SQLIdComuna;

                if(Perfil_U == 3)
                {
                    //Muestra Resultado Perfil Administrador del Sistema //
                    ViewData["Perfil"] = "Administrador del Sistema";

                    var registrosResultados = _context.DOTACION_Registros_resultados.Select(b => b).Where(s => s.Activo == 0).OrderBy(x => x.Rut);
                    // ViewBag.Resultados = registrosResultados;

                    var registrosEliminados = _context.DOTACION_Registros_resultados.Select(b => b).Where(s => s.Activo == 1).OrderBy(x => x.Rut).ToList();
                    ViewBag.Eliminados = registrosEliminados;

                    var numeroPagina = modelo.Pagina ?? 1;
                    var registros = await registrosResultados.ToPagedListAsync(numeroPagina, 30);
                    modelo.Registros = registros;

                    return View(modelo);

                }
                else if (Perfil_U == 1)
                {
                    //Muestra Resultado Perfil Servicio //
                    ViewData["Perfil"] = "Administrador de Servicio";

                    var registrosResultados = _context.DOTACION_Registros_resultados.Select(b => b).Where(s => s.ID_Servicio == SQLIdServicio && s.Activo == 0).OrderBy(x => x.Rut);
                    // ViewBag.Resultados = registrosResultados;
                    

                    /*
                    //Obtiene detalle de registros IdServicio del usuario//
                    List<ListaRegistros> MonitoreoRegistros_S = new List<ListaRegistros>();
                    MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados
                                            where Resultados.ID_Servicio == Int32.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 0
                                            orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                            select Resultados).ToList();
                    ViewBag.Resultados = MonitoreoRegistros_S.ToList();
                    */


                    var registrosEliminados = _context.DOTACION_Registros_resultados.Select(b => b).Where(s => s.ID_Servicio == SQLIdServicio && s.Activo == 1).OrderBy(x => x.Rut).ToList();
                    ViewBag.Eliminados = registrosEliminados;

                    /*
                   //Obtiene detalle de registros eliminados IdServicio del usuario//
                   List <ListaRegistros> MonitoreoRegistroseliminados_S = new List<ListaRegistros>();
                    MonitoreoRegistroseliminados_S = (from Resultados in _context.DOTACION_Registros_resultados
                                            where Resultados.ID_Servicio == Int32.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 1
                                            orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                            select Resultados).ToList();
                    ViewBag.Eliminados = MonitoreoRegistroseliminados_S.ToList();
                    */

                    var numeroPagina = modelo.Pagina ?? 1;
                    var registros = await registrosResultados.ToPagedListAsync(numeroPagina, 20);
                    modelo.Registros = registros;

                    return View(modelo);

                }
                else if (Perfil_U == 2)
                {
                    ViewData["Perfil"] = "Usuario Comunal";

                    //Obtiene detalle de registros IdComuna del usuario//
                    List<ListaRegistros> registrosResultados = new List<ListaRegistros>();
                    registrosResultados = (from Resultados in _context.DOTACION_Registros_resultados
                                            where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 0
                                            orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                            select Resultados).ToList();
                    // ViewBag.Resultados = registrosResultados.ToList();


                    //Obtiene detalle de registros eliminados IdComuna del usuario//
                    List<ListaRegistros> MonitoreoRegistrosEliminados_C = new List<ListaRegistros>();
                    MonitoreoRegistrosEliminados_C = (from Resultados in _context.DOTACION_Registros_resultados
                                            where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 1
                                            orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                            select Resultados).ToList();
                    ViewBag.Eliminados = MonitoreoRegistrosEliminados_C.ToList();

                    var numeroPagina = modelo.Pagina ?? 1;
                    var registros = await registrosResultados.ToPagedListAsync(numeroPagina, 15);
                    modelo.Registros = registros;

                    return View(modelo);

                }
                conn.Close();
                return View(modelo);
            }
        }

        public IActionResult Excel()
        //--Descarga de excel --//

        {
            //--conecta a SQL1--//
            string Connstr1 = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn1 = new SqlConnection(Connstr1);
            conn1.Open();

            // obtiene Rut del usuario logeado//
            userId = User.Identity.Name;

            //Obtiene perfil del usuario//
            string Perfil = "SELECT TOP 1 [ID_Perfil] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdPerfil = new SqlCommand(Perfil, conn1);
            var SQLPerfil = cmdPerfil.ExecuteScalar();
            Perfil_U = Int32.Parse(SQLPerfil.ToString());

            //Obtiene IdServicio del usuario//
            string IdServicio = "SELECT TOP 1 [IdServicio] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdIdServicio = new SqlCommand(IdServicio, conn1);
            var SQLIdServicio = cmdIdServicio.ExecuteScalar();

            //Obtiene IdComuna del usuario//
            string IdComuna = "SELECT TOP 1 [ID_Comuna_U] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdIdComuna = new SqlCommand(IdComuna, conn1);
            var SQLIdComuna = cmdIdComuna.ExecuteScalar();

            conn1.Close();

            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //Perfil_U = 2;

            if (Perfil_U == 1)
            {
                //Exporta Excel perfil Servicio //
                {
                    var comlumHeadrs = new string[]
                {
                "N°",
                "Servicio de Salud",
                "Comuna",
                "Establecimiento",
                "Administración",
                "Run",
                "DV",
                "Apellido Paterno",
                "Apellido Materno",
                "Nombre",
                "Sexo",
                "Fecha Nacimiento",
                "Nacionalidad",
                "Ley Contratación",
                "Tipo de contrato",
                "Categoría",
                "Nivel Carrera",
                "Profesión",
                "Especialidad",
                "Cargo",
                "Asig. Chofer",
                "Jornada",
                "Años de Serv.",
                "Fecha Ingreso",
                "Bienios",
                "Previsión",
                "Isapre",
                "Sueldo Base comunal",
                "Total Haberes",
                "Validado",
                "Revisado"
                };

                    byte[] result;

                    using (var package = new ExcelPackage())
                    {
                        // Agrega una hoja al libro de trabajo de excel

                        var worksheet = package.Workbook.Worksheets.Add("Validacion - Servicio"); //nombre de la hoja excel
                        using (var cells = worksheet.Cells[1, 1, 1, 31])
                        {
                            cells.Style.Font.Bold = true;
                        }

                        //Agrega encabezados
                        for (var i = 0; i < comlumHeadrs.Count(); i++)
                        {
                            worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                        }

                        //Agegraga celdas

                        var ContadorRegistros = 2;

                        List<ListaRegistros> MonitoreoRegistros_S = new List<ListaRegistros>();
                        MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Servicio == int.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 0
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_S.ToList();

                        foreach (var RegistrosExcel in ViewBag.Resultados)
                        {

                            worksheet.Cells["A" + ContadorRegistros].Value = ContadorRegistros - 1;
                            worksheet.Cells["B" + ContadorRegistros].Value = RegistrosExcel.Servicio;
                            worksheet.Cells["C" + ContadorRegistros].Value = RegistrosExcel.Comuna;
                            worksheet.Cells["D" + ContadorRegistros].Value = RegistrosExcel.Establecimiento;
                            worksheet.Cells["E" + ContadorRegistros].Value = RegistrosExcel.Administracion;
                            worksheet.Cells["F" + ContadorRegistros].Value = RegistrosExcel.Rut;
                            worksheet.Cells["G" + ContadorRegistros].Value = RegistrosExcel.DV;
                            worksheet.Cells["H" + ContadorRegistros].Value = RegistrosExcel.Apellido_Paterno;
                            worksheet.Cells["I" + ContadorRegistros].Value = RegistrosExcel.Apellido_Materno;
                            worksheet.Cells["J" + ContadorRegistros].Value = RegistrosExcel.Nombre;
                            worksheet.Cells["K" + ContadorRegistros].Value = RegistrosExcel.Sexo;
                            worksheet.Cells["L" + ContadorRegistros].Value = RegistrosExcel.Fecha_Nacimiento_Texto;
                            worksheet.Cells["M" + ContadorRegistros].Value = RegistrosExcel.Nacionalidad;
                            worksheet.Cells["N" + ContadorRegistros].Value = RegistrosExcel.Ley;
                            worksheet.Cells["O" + ContadorRegistros].Value = RegistrosExcel.Tipo_contrato;
                            worksheet.Cells["P" + ContadorRegistros].Value = RegistrosExcel.Categoria;
                            worksheet.Cells["Q" + ContadorRegistros].Value = RegistrosExcel.Nivel_Carrera;
                            worksheet.Cells["R" + ContadorRegistros].Value = RegistrosExcel.Profesion;
                            worksheet.Cells["S" + ContadorRegistros].Value = RegistrosExcel.Especialidad;
                            worksheet.Cells["T" + ContadorRegistros].Value = RegistrosExcel.Cargo;
                            worksheet.Cells["U" + ContadorRegistros].Value = RegistrosExcel.Funciones_Chofer;
                            worksheet.Cells["V" + ContadorRegistros].Value = RegistrosExcel.Jornada;
                            worksheet.Cells["W" + ContadorRegistros].Value = RegistrosExcel.Anos_Servicio;
                            worksheet.Cells["X" + ContadorRegistros].Value = RegistrosExcel.Fecha_Ingreso_Texto;
                            worksheet.Cells["Y" + ContadorRegistros].Value = RegistrosExcel.Bienios;
                            worksheet.Cells["Z" + ContadorRegistros].Value = RegistrosExcel.Tipo_Prevision;
                            worksheet.Cells["AA" + ContadorRegistros].Value = RegistrosExcel.Tipo_Isapre;
                            worksheet.Cells["AB" + ContadorRegistros].Value = RegistrosExcel.SBase_Comunal;
                            worksheet.Cells["AC" + ContadorRegistros].Value = RegistrosExcel.Haberes;
                            worksheet.Cells["AD" + ContadorRegistros].Value = RegistrosExcel.ValidadoTexto;
                            worksheet.Cells["AE" + ContadorRegistros].Value = RegistrosExcel.RevisadoTexto;


                            ContadorRegistros++;


                        }
                        result = package.GetAsByteArray();
                    }
                    return File(result, "application/ms-excel", $"ModoAPS_Registros_Servicio.xlsx");
                }
            }
            else if(Perfil_U == 2)
            {
                //Exporta Excel perfil Comuna //
                {
                    var comlumHeadrs = new string[]
                {
                "N°",
                "Servicio de Salud",
                "Comuna",
                "Establecimiento",
                "Administración",
                "Run",
                "DV",
                "Apellido Paterno",
                "Apellido Materno",
                "Nombre",
                "Sexo",
                "Fecha Nacimiento",
                "Nacionalidad",
                "Ley Contratación",
                "Tipo de contrato",
                "Categoría",
                "Nivel Carrera",
                "Profesión",
                "Especialidad",
                "Cargo",
                "Asig. Chofer",
                "Jornada",
                "Años de Serv.",
                "Fecha Ingreso",
                "Bienios",
                "Previsión",
                "Isapre",
                "Sueldo Base comunal",
                "Total Haberes",
                "Validado",
                "Revisado"
                };

                    byte[] result;

                    using (var package = new ExcelPackage())
                    {
                        // Agrega una hoja al libro de trabajo de excel

                        var worksheet = package.Workbook.Worksheets.Add("Validacion - Comuna"); //nombre de la hoja excel
                        using (var cells = worksheet.Cells[1, 1, 1, 31])
                        {
                            cells.Style.Font.Bold = true;
                        }

                        //Agrega encabezados
                        for (var i = 0; i < comlumHeadrs.Count(); i++)
                        {
                            worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                        }

                        //Agegraga celdas

                        var ContadorRegistros = 2;

                        List<ListaRegistros> MonitoreoRegistros_S = new List<ListaRegistros>();
                        MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 0
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_S.ToList();

                        foreach (var RegistrosExcel in ViewBag.Resultados)
                        {
                            worksheet.Cells["A" + ContadorRegistros].Value = ContadorRegistros - 1;
                            worksheet.Cells["B" + ContadorRegistros].Value = RegistrosExcel.Servicio;
                            worksheet.Cells["C" + ContadorRegistros].Value = RegistrosExcel.Comuna;
                            worksheet.Cells["D" + ContadorRegistros].Value = RegistrosExcel.Establecimiento;
                            worksheet.Cells["E" + ContadorRegistros].Value = RegistrosExcel.Administracion;
                            worksheet.Cells["F" + ContadorRegistros].Value = RegistrosExcel.Rut;
                            worksheet.Cells["G" + ContadorRegistros].Value = RegistrosExcel.DV;
                            worksheet.Cells["H" + ContadorRegistros].Value = RegistrosExcel.Apellido_Paterno;
                            worksheet.Cells["I" + ContadorRegistros].Value = RegistrosExcel.Apellido_Materno;
                            worksheet.Cells["J" + ContadorRegistros].Value = RegistrosExcel.Nombre;
                            worksheet.Cells["K" + ContadorRegistros].Value = RegistrosExcel.Sexo;
                            worksheet.Cells["L" + ContadorRegistros].Value = RegistrosExcel.Fecha_Nacimiento_Texto;
                            worksheet.Cells["M" + ContadorRegistros].Value = RegistrosExcel.Nacionalidad;
                            worksheet.Cells["N" + ContadorRegistros].Value = RegistrosExcel.Ley;
                            worksheet.Cells["O" + ContadorRegistros].Value = RegistrosExcel.Tipo_contrato;
                            worksheet.Cells["P" + ContadorRegistros].Value = RegistrosExcel.Categoria;
                            worksheet.Cells["Q" + ContadorRegistros].Value = RegistrosExcel.Nivel_Carrera;
                            worksheet.Cells["R" + ContadorRegistros].Value = RegistrosExcel.Profesion;
                            worksheet.Cells["S" + ContadorRegistros].Value = RegistrosExcel.Especialidad;
                            worksheet.Cells["T" + ContadorRegistros].Value = RegistrosExcel.Cargo;
                            worksheet.Cells["U" + ContadorRegistros].Value = RegistrosExcel.Funciones_Chofer;
                            worksheet.Cells["V" + ContadorRegistros].Value = RegistrosExcel.Jornada;
                            worksheet.Cells["W" + ContadorRegistros].Value = RegistrosExcel.Anos_Servicio;
                            worksheet.Cells["X" + ContadorRegistros].Value = RegistrosExcel.Fecha_Ingreso_Texto;
                            worksheet.Cells["Y" + ContadorRegistros].Value = RegistrosExcel.Bienios;
                            worksheet.Cells["Z" + ContadorRegistros].Value = RegistrosExcel.Tipo_Prevision;
                            worksheet.Cells["AA" + ContadorRegistros].Value = RegistrosExcel.Tipo_Isapre;
                            worksheet.Cells["AB" + ContadorRegistros].Value = RegistrosExcel.SBase_Comunal;
                            worksheet.Cells["AC" + ContadorRegistros].Value = RegistrosExcel.Haberes;
                            worksheet.Cells["AD" + ContadorRegistros].Value = RegistrosExcel.ValidadoTexto;
                            worksheet.Cells["AE" + ContadorRegistros].Value = RegistrosExcel.RevisadoTexto;

                            ContadorRegistros++;
                        }
                        result = package.GetAsByteArray();
                    }
                    return File(result, "application/ms-excel", $"ModoAPS_Registros_Comuna.xlsx");
                }
            } 
            else
            {
                //Exporta Excel perfil Minsal //
                {
                    var comlumHeadrs = new string[]
                {
                "N°",
                "Servicio de Salud",
                "Comuna",
                "Establecimiento",
                "Administración",
                "Run",
                "DV",
                "Apellido Paterno",
                "Apellido Materno",
                "Nombre",
                "Sexo",
                "Fecha Nacimiento",
                "Nacionalidad",
                "Ley Contratación",
                "Tipo de contrato",
                "Categoría",
                "Nivel Carrera",
                "Profesión",
                "Especialidad",
                "Cargo",
                "Asig. Chofer",
                "Jornada",
                "Años de Serv.",
                "Fecha Ingreso",
                "Bienios",
                "Previsión",
                "Isapre",
                "Sueldo Base comunal",
                "Total Haberes",
                "Validado",
                "Revisado"
                };

                    byte[] result;

                    using (var package = new ExcelPackage())
                    {
                        // Agrega una hoja al libro de trabajo de excel

                        var worksheet = package.Workbook.Worksheets.Add("Validacion - Minsal"); //nombre de la hoja excel
                        using (var cells = worksheet.Cells[1, 1, 1, 31])
                        {
                            cells.Style.Font.Bold = true;
                        }

                        //Agrega encabezados
                        for (var i = 0; i < comlumHeadrs.Count(); i++)
                        {
                            worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                        }

                        //Agegraga celdas

                        var ContadorRegistros = 2;

                        List<ListaRegistros> MonitoreoRegistros_S = new List<ListaRegistros>();
                        MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.Activo == 0
                                                orderby Resultados.Rut, Resultados.ID_Servicio, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_S.ToList();

                        foreach (var RegistrosExcel in ViewBag.Resultados)
                        {
                            worksheet.Cells["A" + ContadorRegistros].Value = ContadorRegistros - 1;
                            worksheet.Cells["B" + ContadorRegistros].Value = RegistrosExcel.Servicio;
                            worksheet.Cells["C" + ContadorRegistros].Value = RegistrosExcel.Comuna;
                            worksheet.Cells["D" + ContadorRegistros].Value = RegistrosExcel.Establecimiento;
                            worksheet.Cells["E" + ContadorRegistros].Value = RegistrosExcel.Administracion;
                            worksheet.Cells["F" + ContadorRegistros].Value = RegistrosExcel.Rut;
                            worksheet.Cells["G" + ContadorRegistros].Value = RegistrosExcel.DV;
                            worksheet.Cells["H" + ContadorRegistros].Value = RegistrosExcel.Apellido_Paterno;
                            worksheet.Cells["I" + ContadorRegistros].Value = RegistrosExcel.Apellido_Materno;
                            worksheet.Cells["J" + ContadorRegistros].Value = RegistrosExcel.Nombre;
                            worksheet.Cells["K" + ContadorRegistros].Value = RegistrosExcel.Sexo;
                            worksheet.Cells["L" + ContadorRegistros].Value = RegistrosExcel.Fecha_Nacimiento_Texto;
                            worksheet.Cells["M" + ContadorRegistros].Value = RegistrosExcel.Nacionalidad;
                            worksheet.Cells["N" + ContadorRegistros].Value = RegistrosExcel.Ley;
                            worksheet.Cells["O" + ContadorRegistros].Value = RegistrosExcel.Tipo_contrato;
                            worksheet.Cells["P" + ContadorRegistros].Value = RegistrosExcel.Categoria;
                            worksheet.Cells["Q" + ContadorRegistros].Value = RegistrosExcel.Nivel_Carrera;
                            worksheet.Cells["R" + ContadorRegistros].Value = RegistrosExcel.Profesion;
                            worksheet.Cells["S" + ContadorRegistros].Value = RegistrosExcel.Especialidad;
                            worksheet.Cells["T" + ContadorRegistros].Value = RegistrosExcel.Cargo;
                            worksheet.Cells["U" + ContadorRegistros].Value = RegistrosExcel.Funciones_Chofer;
                            worksheet.Cells["V" + ContadorRegistros].Value = RegistrosExcel.Jornada;
                            worksheet.Cells["W" + ContadorRegistros].Value = RegistrosExcel.Anos_Servicio;
                            worksheet.Cells["X" + ContadorRegistros].Value = RegistrosExcel.Fecha_Ingreso_Texto;
                            worksheet.Cells["Y" + ContadorRegistros].Value = RegistrosExcel.Bienios;
                            worksheet.Cells["Z" + ContadorRegistros].Value = RegistrosExcel.Tipo_Prevision;
                            worksheet.Cells["AA" + ContadorRegistros].Value = RegistrosExcel.Tipo_Isapre;
                            worksheet.Cells["AB" + ContadorRegistros].Value = RegistrosExcel.SBase_Comunal;
                            worksheet.Cells["AC" + ContadorRegistros].Value = RegistrosExcel.Haberes;
                            worksheet.Cells["AD" + ContadorRegistros].Value = RegistrosExcel.ValidadoTexto;
                            worksheet.Cells["AE" + ContadorRegistros].Value = RegistrosExcel.RevisadoTexto;

                            ContadorRegistros++;
                        }
                        result = package.GetAsByteArray();
                    }
                    return File(result, "application/ms-excel", $"ModoAPS_Registros_Minsal.xlsx");
                }
            }

        }


        public JsonResult GetComunas(int IdServicio)
        {
            List<ListaComunas> Comunalist = new List<ListaComunas>();

            //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
            Comunalist = (from ComunasBD in _context.DOTACION_Comunas
                          where ComunasBD.ID_Comuna == IdServicio
                          orderby ComunasBD.Comuna
                          select ComunasBD).ToList();

            //--------------Insertando select item en la lista --------------
            Comunalist.Insert(0, new ListaComunas { ID_Comuna = 0, Comuna = "" });

            //--------------Asignando Categorylist a viewBag.ListofCategory --------------
            return Json(new SelectList(Comunalist, "IdComuna", "Comuna"));
        }

        public JsonResult GetEstablecimientos(int ID_Comuna)
        {
            List<ListaEstablecimientos> EstablecimientoList = new List<ListaEstablecimientos>();

            //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
            EstablecimientoList = (from EstablecimientosBD in _context.DOTACION_Establecimientos
                                   where EstablecimientosBD.ID_Comuna == ID_Comuna
                                   orderby EstablecimientosBD.Establecimiento
                                   select EstablecimientosBD).ToList();

            //--------------Insertando select item en la lista --------------
            EstablecimientoList.Insert(0, new ListaEstablecimientos { CodigoNuevo = 0, Establecimiento = "" });

            //--------------Asignando Categorylist a viewBag.ListofCategory --------------
            return Json(new SelectList(EstablecimientoList, "CodigoNuevo", "Establecimiento"));
        }

        /*    public JsonResult GetEstablecimientosMadres(int IdComuna, int Codigonuevo)
            {
                List<ListaEstablecimientosMadres> EstablecimientoMadreList = new List<ListaEstablecimientosMadres>();

                //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
                EstablecimientoMadreList = (from EstablecimientosBD in _context.DOTACION_EstablecimientosMadre
                                       where EstablecimientosBD.IdComuna == IdComuna && EstablecimientosBD.CodigoNuevoMadre != Codigonuevo
                                            orderby EstablecimientosBD.EstablecimientoMadre
                                       select EstablecimientosBD).ToList();

                //--------------Insertando select item en la lista --------------
                EstablecimientoMadreList.Insert(0, new ListaEstablecimientosMadres { CodigoNuevoMadre = 0, EstablecimientoMadre = "No Aplica" });

                //--------------Asignando Categorylist a viewBag.ListofCategory --------------
                return Json(new SelectList(EstablecimientoMadreList, "CodigoNuevoMadre", "EstablecimientoMadre"));
            }
        */

        public JsonResult GetCategorias(String Categoria)
        {
            List<ListaProfesiones> CategoriaList = new List<ListaProfesiones>();

            //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
            CategoriaList = (from ProfesionesBD in _context.DOTACION_Profesion
                             where ProfesionesBD.Categoria == Categoria
                             orderby ProfesionesBD.Profesion
                             select ProfesionesBD).ToList();

            //--------------Insertando select item en la lista --------------
            CategoriaList.Insert(0, new ListaProfesiones { Profesion = "" });

            //--------------Asignando Categorylist a viewBag.ListofCategory --------------
            return Json(new SelectList(CategoriaList, "Profesion", "Profesion").Distinct());
        }

        public JsonResult GetAsignacionChofer(String Cargo)
        {
            List<ListaChofer> ListadoChofer = new List<ListaChofer>();
            ListadoChofer = (from Chofer in _context.DOTACION_Chofer
                             where Chofer.Cargo == Cargo
                             select Chofer).ToList();

            ListadoChofer.Insert(0, new ListaChofer { Funciones_Chofer = "" });

            return Json(new SelectList(ListadoChofer, "Funciones_Chofer", "Funciones_Chofer").Distinct());
        }


        public JsonResult GetEspecialidad(String Profesion)
        {
            List<ListaEspecialidad> EspecialidadList = new List<ListaEspecialidad>();

            //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
            EspecialidadList = (from EspecialidadesBD in _context.DOTACION_Especialidad
                                where EspecialidadesBD.Profesion == Profesion
                                orderby EspecialidadesBD.Especialidad
                                select EspecialidadesBD).ToList();

            //--------------Insertando select item en la lista --------------
            EspecialidadList.Insert(0, new ListaEspecialidad { Especialidad = "" });

            //--------------Asignando Categorylist a viewBag.ListofCategory --------------
            return Json(new SelectList(EspecialidadList, "Especialidad", "Especialidad").Distinct());
        }

        public JsonResult GetContrato(String Ley)
        {
            List<ListaContratos> ContratosList = new List<ListaContratos>();

            //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
            ContratosList = (from ContratosBD in _context.DOTACION_Contrato
                             where ContratosBD.Ley == Ley
                             orderby ContratosBD.Tipo_contrato
                             select ContratosBD).ToList();

            //--------------Insertando select item en la lista --------------
            ContratosList.Insert(0, new ListaContratos { Tipo_contrato = "" });

            //--------------Asignando Categorylist a viewBag.ListofCategory --------------
            return Json(new SelectList(ContratosList, "Tipo_contrato", "Tipo_contrato").Distinct());
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Order = 2)]
        public async Task<IActionResult> ActualizaRegistros(string[] checkAll, ListadoViewModel<ListaRegistros> modelo)

        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            var idServicioSaludAPS = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.IdServicio).First();
            var idComunaAPS = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Comuna_U).First();

            var TotalRegistros = _context.DOTACION_Registros_resultados.Where(s => s.ID_Servicio == idServicioSaludAPS && s.Activo == 0).Count();
            ViewData["TotalRegistrosServicio"] = TotalRegistros;

            var TotalRegistrosServicio = _context.DOTACION_Registros_resultados.Where(s => s.ID_Servicio == idServicioSaludAPS && s.Validado == 0 && s.Activo == 0).Count();
            ViewData["TotalRegistrosServicioPorValidar"] = TotalRegistrosServicio;

            var TotalRegistroUsuario = _context.DOTACION_Registros_resultados.Where(s => s.ID_Comuna == idComunaAPS && s.Activo == 0).Count();
            ViewData["TotalRegistrosUsuario"] = TotalRegistroUsuario;

            var TotalRegistroUsuarioval = _context.DOTACION_Registros_resultados.Where(s => s.ID_Comuna == idComunaAPS && s.Validado == 0 && s.Activo == 0).Count();
            ViewData["TotalRegistrosUsuarioPorValidar"] = TotalRegistroUsuarioval;

            var TotalRegistrosMinsal = _context.DOTACION_Registros_resultados.Where(s => s.Activo == 0).Count();
            ViewData["TotalRegistrosMinsal"] = TotalRegistrosMinsal;

            var TotalRegistrosMinsalPorValidar = _context.DOTACION_Registros_resultados.Where(s => s.Validado == 0 && s.Activo == 0).Count();
            ViewData["TotalRegistrosMinsalPorValidar"] = TotalRegistrosMinsalPorValidar;

            //--conecta a SQL2--//
            string Connstr2 = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn2 = new SqlConnection(Connstr2);
            conn2.Open();

            //Obtiene perfil del usuario//
            string Perfil = "SELECT TOP 1 [ID_Perfil] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdPerfil = new SqlCommand(Perfil, conn2);
            var SQLPerfil = cmdPerfil.ExecuteScalar();
            Perfil_U = Int32.Parse(SQLPerfil.ToString());
            conn2.Close();

            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //Perfil_U = 2;



            ViewBag.ID_PerfilUsuario = Perfil_U;

            {

                // Muestra datos según Perfil //
                if (Perfil_U == 1 || Perfil_U == 3)
                //Muestra Resultado Perfil Servicio y Perfil Minsal //
                {

                    foreach (var item in checkAll)
                    {
                        conn2.Open();


                        string ID_Validacion1 = "SELECT ID_Registro FROM .[dbo].[DOTACION_Registros_resultados] where [ID] = '" + item + "'";
                        SqlCommand cmdID_Validacion1Registro = new SqlCommand(ID_Validacion1, conn2);
                        var SQLIDValidacion1Registro = cmdID_Validacion1Registro.ExecuteScalar();
                        Id_Validacion_Registro = Int32.Parse(SQLIDValidacion1Registro.ToString());


                        string query = "UPDATE .[dbo].[DOTACION_Registros] SET [Validado] = 1, [Fecha_Validacion] = GETDATE(), [usuario_validador] =  '" + User.Identity.Name + "' WHERE  ID_Registro = " + Id_Validacion_Registro + " and Revisado = 1 and Activo = 0";
                        SqlCommand cmd = new SqlCommand(query, conn2);
                        cmd.ExecuteReader();
                        conn2.Close();
                    }

                    conn2.Open();
                    //Obtiene IdServicio del usuario//
                    string IdServicio = "SELECT TOP 1 [IdServicio] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
                    SqlCommand cmdIdServicio = new SqlCommand(IdServicio, conn2);
                    var SQLIdServicio = cmdIdServicio.ExecuteScalar();
                    conn2.Close();

                    if (!string.IsNullOrEmpty(Filtro))
                    {
                        if (Perfil_U == 1)
                        {
                            ViewData["Perfil"] = "Administrador de Servicio";

                            //Obtiene detalle de registros IdServicio del usuario//
                            List<ListaRegistros> MonitoreoRegistros_S = new List<ListaRegistros>();
                            MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados
                                                    where Resultados.ID_Servicio == int.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 0 && Resultados.Rut == Filtro.ToString()
                                                    orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                    select Resultados).ToList();
                            ViewBag.Resultados = MonitoreoRegistros_S.ToList();

                            var numeroPagina = modelo.Pagina ?? 1;
                            var registros = await MonitoreoRegistros_S.ToPagedListAsync(numeroPagina, 20);
                            modelo.Registros = registros;

                            //Obtiene detalle de registros eliminados IdServicio del usuario//
                            List<ListaRegistros> MonitoreoRegistroseliminados_S = new List<ListaRegistros>();
                            MonitoreoRegistroseliminados_S = (from Resultados in _context.DOTACION_Registros_resultados
                                                              where Resultados.ID_Servicio == Int32.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 1
                                                              orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                              select Resultados).ToList();
                            ViewBag.Eliminados = MonitoreoRegistroseliminados_S.ToList();

                        } else if(Perfil_U == 3)
                        {
                            ViewData["Perfil"] = "Administrador del Sistema MINSAL";

                            var registrosX = _context.DOTACION_Registros_resultados.Select(b => b).Where(s => s.Activo == 0 && s.Rut == Filtro.ToString()).OrderBy(x => x.Rut).ToList();
                            ViewBag.Resultados = registrosX;

                            var numeroPagina = modelo.Pagina ?? 1;
                            var registros = await registrosX.ToPagedListAsync(numeroPagina, 30);
                            modelo.Registros = registros;

                            var registrosEliminados = _context.DOTACION_Registros_resultados.Select(b => b).Where(s => s.Activo == 1).OrderBy(x => x.Rut).ToList();
                            ViewBag.Eliminados = registrosEliminados;
                        }
                    }
                    else
                    {
                        if(Perfil_U == 1)
                        {
                            ViewData["Perfil"] = "Administrador de Servicio";

                            //Obtiene detalle de registros IdServicio del usuario//
                            List<ListaRegistros> MonitoreoRegistros_S = new List<ListaRegistros>();
                            MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados
                                                    where Resultados.ID_Servicio == int.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 0
                                                    orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                    select Resultados).ToList();
                            ViewBag.Resultados = MonitoreoRegistros_S.ToList();

                            var numeroPagina = modelo.Pagina ?? 1;
                            var registros = await MonitoreoRegistros_S.ToPagedListAsync(numeroPagina, 20);
                            modelo.Registros = registros;

                            //Obtiene detalle de registros eliminados IdServicio del usuario//
                            List<ListaRegistros> MonitoreoRegistroseliminados_S = new List<ListaRegistros>();
                            MonitoreoRegistroseliminados_S = (from Resultados in _context.DOTACION_Registros_resultados
                                                    where Resultados.ID_Servicio == Int32.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 1
                                                    orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                    select Resultados).ToList();
                            ViewBag.Eliminados = MonitoreoRegistroseliminados_S.ToList();
                        
                        } else if(Perfil_U == 3)
                        {
                            ViewData["Perfil"] = "Administrador del Sistema MINSAL";

                            var registrosX = _context.DOTACION_Registros_resultados.Select(b => b).Where(s => s.Activo == 0).OrderBy(x => x.Rut).ToList();
                            ViewBag.Resultados = registrosX;

                            var numeroPagina = modelo.Pagina ?? 1;
                            var registros = await registrosX.ToPagedListAsync(numeroPagina, 30);
                            modelo.Registros = registros;

                            var registrosEliminados = _context.DOTACION_Registros_resultados.Select(b => b).Where(s => s.Activo == 1).OrderBy(x => x.Rut).ToList();
                            ViewBag.Eliminados = registrosEliminados;
                        }
                        
                    }

                }
                else if(Perfil_U == 2)
                {
                    foreach (var item in checkAll)
                    {
                        conn2.Open();


                        string ID_Validacion1 = "SELECT ID_Registro FROM .[dbo].[DOTACION_Registros_resultados] where [ID] = '" + item + "'";
                        SqlCommand cmdID_Validacion1Registro = new SqlCommand(ID_Validacion1, conn2);
                        var SQLIDValidacion1Registro = cmdID_Validacion1Registro.ExecuteScalar();
                        Id_Validacion_Registro = Int32.Parse(SQLIDValidacion1Registro.ToString());


                        string query = "UPDATE .[dbo].[DOTACION_Registros] SET [Revisado] = 1, [Fecha_Revision] = GETDATE(), [usuario_revisor] =  '" + User.Identity.Name + "' WHERE  ID_Registro = " + Id_Validacion_Registro;
                        SqlCommand cmd = new SqlCommand(query, conn2);
                        cmd.ExecuteReader();
                        conn2.Close();
                    }

                    conn2.Open();
                    //Obtiene IdComuna del usuario//
                    string IdComuna = "SELECT TOP 1 [ID_Comuna_U] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
                    SqlCommand cmdIdComuna = new SqlCommand(IdComuna, conn2);
                    var SQLIdComuna = cmdIdComuna.ExecuteScalar();
                    conn2.Close();

                    if (!string.IsNullOrEmpty(Filtro))
                    {

                        ViewData["Perfil"] = "Usuario Comunal";

                        //Obtiene detalle de registros IdServicio del usuario//

                        List<ListaRegistros> MonitoreoRegistros_C = new List<ListaRegistros>();
                        MonitoreoRegistros_C = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 0 && Resultados.Rut == Filtro.ToString()
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_C.ToList();

                        var numeroPagina = modelo.Pagina ?? 1;
                        var registros = await MonitoreoRegistros_C.ToPagedListAsync(numeroPagina, 15);
                        modelo.Registros = registros;

                        //Obtiene detalle de registros eliminados IdComuna del usuario//
                        List<ListaRegistros> MonitoreoRegistrosEliminados_C = new List<ListaRegistros>();
                        MonitoreoRegistrosEliminados_C = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 1
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Eliminados = MonitoreoRegistrosEliminados_C.ToList();
                    }
                    else
                    {
                        ViewData["Perfil"] = "Usuario Comunal";

                        //Obtiene detalle de registros IdServicio del usuario//

                        List<ListaRegistros> MonitoreoRegistros_C = new List<ListaRegistros>();
                        MonitoreoRegistros_C = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 0
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_C.ToList();

                        var numeroPagina = modelo.Pagina ?? 1;
                        var registros = await MonitoreoRegistros_C.ToPagedListAsync(numeroPagina, 15);
                        modelo.Registros = registros;

                        //Obtiene detalle de registros eliminados IdComuna del usuario//
                        List<ListaRegistros> MonitoreoRegistrosEliminados_C = new List<ListaRegistros>();
                        MonitoreoRegistrosEliminados_C = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 1
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Eliminados = MonitoreoRegistrosEliminados_C.ToList();
                    }

                }

            }
            return View("ActualizaRegistros", modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken(Order = 2)]
        public async Task<IActionResult> EliminaRegistros(string[] checkAll, ListadoViewModel<ListaRegistros> modelo)

        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            //--conecta a SQL--//
            string Connstr = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn2 = new SqlConnection(Connstr);
            conn2.Open();

            //Obtiene perfil del usuario//
            string Perfil = "SELECT TOP 1 [ID_Perfil] FROM [dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdPerfil = new SqlCommand(Perfil, conn2);
            var SQLPerfil = cmdPerfil.ExecuteScalar();
            Perfil_U = Int32.Parse(SQLPerfil.ToString());

            //Obtiene IdServicio del usuario//
            string IdServicio = "SELECT TOP 1 [IdServicio] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdIdServicio = new SqlCommand(IdServicio, conn2);
            var SQLIdServicio = cmdIdServicio.ExecuteScalar();

            //Obtiene IdComuna del usuario//
            string IdComuna = "SELECT TOP 1 [ID_Comuna_U] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdIdComuna = new SqlCommand(IdComuna, conn2);
            var SQLIdComuna = cmdIdComuna.ExecuteScalar();

            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //Perfil_U = 2;
            conn2.Close();


            ViewBag.ID_PerfilUsuario = Perfil_U;

            {

                // Muestra datos según Perfil //
                if (Perfil_U == 1)
                //Muestra Resultado Perfil Servicio //
                {

                }
                else

                {
                    foreach (var item in checkAll)
                    {
                        conn2.Open();
                        string ID_Validacion = "SELECT ID_Registro FROM .[dbo].[DOTACION_Registros_resultados] where [ID] = '" + item + "'";
                        SqlCommand cmdID_ValidacionRegistro = new SqlCommand(ID_Validacion, conn2);
                        var SQLIDValidacionRegistro = cmdID_ValidacionRegistro.ExecuteScalar();
                        Id_Validacion_Registro = Int32.Parse(SQLIDValidacionRegistro.ToString());


                        string query = "UPDATE .[dbo].[DOTACION_Registros] SET [Activo] = 1 WHERE  ID_Registro = " + Id_Validacion_Registro;
                        SqlCommand cmd = new SqlCommand(query, conn2);
                        cmd.ExecuteReader();
                        conn2.Close();
                    }
         
                    ModelState.AddModelError("Error", "Se han desactivado los Registros");

                    if (!string.IsNullOrEmpty(Filtro))
                    {

                        ViewData["Perfil"] = "Usuario Comunal";

                        //Obtiene detalle de registros IdServicio del usuario//

                        List<ListaRegistros> MonitoreoRegistros_C = new List<ListaRegistros>();
                        MonitoreoRegistros_C = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 0 && Resultados.Rut == Filtro.ToString()
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_C.ToList();

                        var paginasX = 20;

                        if (Perfil_U == 1) {
                            paginasX = 20;
                        } else if (Perfil_U == 2) {
                            paginasX = 15;
                        } else if (Perfil_U == 3) {
                            paginasX = 30;
                        }
                        
                        var numeroPagina = modelo.Pagina ?? 1;
                        var registros = await MonitoreoRegistros_C.ToPagedListAsync(numeroPagina, paginasX);
                        modelo.Registros = registros;

                        //Obtiene detalle de registros eliminados IdComuna del usuario//
                        List<ListaRegistros> MonitoreoRegistrosEliminados_C = new List<ListaRegistros>();
                        MonitoreoRegistrosEliminados_C = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 1
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Eliminados = MonitoreoRegistrosEliminados_C.ToList();
                    }
                    else
                    {
                        ViewData["Perfil"] = "Usuario Comunal";

                        //Obtiene detalle de registros IdServicio del usuario//

                        List<ListaRegistros> MonitoreoRegistros_C = new List<ListaRegistros>();
                        MonitoreoRegistros_C = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 0
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_C.ToList();

                        var paginasX = 20;

                        if (Perfil_U == 1)
                        {
                            paginasX = 20;
                        }
                        else if (Perfil_U == 2)
                        {
                            paginasX = 15;
                        }
                        else if (Perfil_U == 3)
                        {
                            paginasX = 30;
                        }

                        var numeroPagina = modelo.Pagina ?? 1;
                        var registros = await MonitoreoRegistros_C.ToPagedListAsync(numeroPagina, paginasX);
                        modelo.Registros = registros;

                        //Obtiene detalle de registros eliminados IdComuna del usuario//
                        List<ListaRegistros> MonitoreoRegistrosEliminados_C = new List<ListaRegistros>();
                        MonitoreoRegistrosEliminados_C = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 1
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Eliminados = MonitoreoRegistrosEliminados_C.ToList();
                    }

                    conn2.Open();

                    //Obtiene total de registros del servicio del usuario//
                    string Registro = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados] where [ID_Servicio] = " + SQLIdServicio + " AND [Activo] = 0";
                    SqlCommand cmdRegistros = new SqlCommand(Registro, conn2);
                    var TotalRegistros = cmdRegistros.ExecuteScalar();
                    ViewData["TotalRegistrosServicio"] = TotalRegistros;

                    //Obtiene total de registros por validar del Servicio del usuario//
                    string RegistroServicio = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados] where [ID_Servicio] = " + SQLIdServicio + " and [Validado] = 0 AND [Activo] = 0";
                    SqlCommand cmdRegistrosServicio = new SqlCommand(RegistroServicio, conn2);
                    var TotalRegistrosServicio = cmdRegistrosServicio.ExecuteScalar();
                    ViewData["TotalRegistrosServicioPorValidar"] = TotalRegistrosServicio;

                    //Obtiene total de registros de la comuna del usuario//
                    string RegistroUsuario = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados] where [ID_Comuna] = '" + SQLIdComuna + "' AND [Activo] = 0";
                    SqlCommand cmdRegistrosUsuario = new SqlCommand(RegistroUsuario, conn2);
                    var TotalRegistroUsuario = cmdRegistrosUsuario.ExecuteScalar();
                    ViewData["TotalRegistrosUsuario"] = TotalRegistroUsuario;

                    //Obtiene total de registros por validar de la comuna del usuario//
                    string RegistroUsuarioval = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados] where [ID_Comuna] = '" + SQLIdComuna + "' and [Validado] = 0  AND [Activo] = 0";
                    SqlCommand cmdRegistrosUsuarioval = new SqlCommand(RegistroUsuarioval, conn2);
                    var TotalRegistroUsuarioval = cmdRegistrosUsuarioval.ExecuteScalar();
                    ViewData["TotalRegistrosUsuarioPorValidar"] = TotalRegistroUsuarioval;

                    var TotalRegistrosMinsal = _context.DOTACION_Registros_resultados.Where(s => s.Activo == 0).Count();
                    ViewData["TotalRegistrosMinsal"] = TotalRegistrosMinsal;
                        
                    var TotalRegistrosMinsalPorValidar = _context.DOTACION_Registros_resultados.Where(s => s.Validado == 0 && s.Activo == 0).Count();
                    ViewData["TotalRegistrosMinsalPorValidar"] = TotalRegistrosMinsalPorValidar;
                        
                    conn2.Close();

                }

            }
            return View("ActualizaRegistros", modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Filtrar(string searchString, ListadoViewModel<ListaRegistros> modelo)
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            var idServicioSaludAPS = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.IdServicio).First();
            var idComunaAPS = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Comuna_U).First();

            var TotalRegistros = _context.DOTACION_Registros_resultados.Where(s => s.ID_Servicio == idServicioSaludAPS && s.Activo == 0).Count();
            ViewData["TotalRegistrosServicio"] = TotalRegistros;

            var TotalRegistrosServicio = _context.DOTACION_Registros_resultados.Where(s => s.ID_Servicio == idServicioSaludAPS && s.Validado == 0 && s.Activo == 0).Count();
            ViewData["TotalRegistrosServicioPorValidar"] = TotalRegistrosServicio;

            var TotalRegistroUsuario = _context.DOTACION_Registros_resultados.Where(s => s.ID_Comuna == idComunaAPS && s.Activo == 0).Count();
            ViewData["TotalRegistrosUsuario"] = TotalRegistroUsuario;

            var TotalRegistroUsuarioval = _context.DOTACION_Registros_resultados.Where(s => s.ID_Comuna == idComunaAPS && s.Validado == 0 && s.Activo == 0).Count();
            ViewData["TotalRegistrosUsuarioPorValidar"] = TotalRegistroUsuarioval;

            var TotalRegistrosMinsal = _context.DOTACION_Registros_resultados.Where(s => s.Activo == 0).Count();
            ViewData["TotalRegistrosMinsal"] = TotalRegistrosMinsal;

            var TotalRegistrosMinsalPorValidar = _context.DOTACION_Registros_resultados.Where(s => s.Validado == 0 && s.Activo == 0).Count();
            ViewData["TotalRegistrosMinsalPorValidar"] = TotalRegistrosMinsalPorValidar;

            //--conecta a SQL1--//
            string Connstr2 = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn2 = new SqlConnection(Connstr2);
            conn2.Open();

            //Obtiene perfil del usuario//
            string Perfil = "SELECT TOP 1 [ID_Perfil] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdPerfil = new SqlCommand(Perfil, conn2);
            var SQLPerfil = cmdPerfil.ExecuteScalar();
            Perfil_U = Int32.Parse(SQLPerfil.ToString());

            //Obtiene IdServicio del usuario//
            string IdServicio = "SELECT TOP 1 [IdServicio] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdIdServicio = new SqlCommand(IdServicio, conn2);
            var SQLIdServicio = cmdIdServicio.ExecuteScalar();

            //Obtiene IdComuna del usuario//
            string IdComuna = "SELECT TOP 1 [ID_Comuna_U] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdIdComuna = new SqlCommand(IdComuna, conn2);
            var SQLIdComuna = cmdIdComuna.ExecuteScalar();

            conn2.Close();


            ViewData["Perfil"] = "Administrador de Servicio";

            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //Perfil_U = 2;

            ViewBag.ID_PerfilUsuario = Perfil_U;

            {

                // Muestra datos según Perfil //
                if (Perfil_U == 1)
                //Muestra Resultado Perfil Servicio //
                {
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        ViewData["Perfil"] = "Administrador de Servicio";

                        //Obtiene detalle de registros IdServicio del usuario//
                        List<ListaRegistros> MonitoreoRegistros_S = new List<ListaRegistros>();
                        MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Servicio == int.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 0 && Resultados.Rut == searchString.ToString()
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_S.ToList();

                        var numeroPagina = modelo.Pagina ?? 1;
                        var registros = await MonitoreoRegistros_S.ToPagedListAsync(numeroPagina, 20);
                        modelo.Registros = registros;

                        //Obtiene detalle de registros eliminados IdServicio del usuario//
                        List<ListaRegistros> MonitoreoRegistroseliminados_S = new List<ListaRegistros>();
                        MonitoreoRegistroseliminados_S = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Servicio == Int32.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 1
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Eliminados = MonitoreoRegistroseliminados_S.ToList();

                    }
                    else
                    {
                        ViewData["Perfil"] = "Administrador de Servicio";

                        //Obtiene detalle de registros IdServicio del usuario//
                        List<ListaRegistros> MonitoreoRegistros_S = new List<ListaRegistros>();
                        MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Servicio == int.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 0
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_S.ToList();

                        var numeroPagina = modelo.Pagina ?? 1;
                        var registros = await MonitoreoRegistros_S.ToPagedListAsync(numeroPagina, 20);
                        modelo.Registros = registros;

                        //Obtiene detalle de registros eliminados IdServicio del usuario//
                        List<ListaRegistros> MonitoreoRegistroseliminados_S = new List<ListaRegistros>();
                        MonitoreoRegistroseliminados_S = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Servicio == Int32.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 1
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Eliminados = MonitoreoRegistroseliminados_S.ToList();

                    }
                } 
                else if(Perfil_U == 3)
                {
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        ViewData["Perfil"] = "Administrador del Sistema MINSAL";

                        var registrosX = _context.DOTACION_Registros_resultados.Select(b => b).Where(s => s.Activo == 0 && s.Rut == searchString.ToString()).OrderBy(x => x.Rut).ToList();
                        ViewBag.Resultados = registrosX;

                        var numeroPagina = modelo.Pagina ?? 1;
                        var registros = await registrosX.ToPagedListAsync(numeroPagina, 30);
                        modelo.Registros = registros;

                        var registrosEliminados = _context.DOTACION_Registros_resultados.Select(b => b).Where(s => s.Activo == 1).OrderBy(x => x.Rut).ToList();
                        ViewBag.Eliminados = registrosEliminados;


                    } else
                    {
                        ViewData["Perfil"] = "Administrador del Sistema MINSAL";

                        var registrosX = _context.DOTACION_Registros_resultados.Select(b => b).Where(s => s.Activo == 0).OrderBy(x => x.Rut).ToList();
                        ViewBag.Resultados = registrosX;

                        var numeroPagina = modelo.Pagina ?? 1;
                        var registros = await registrosX.ToPagedListAsync(numeroPagina, 30);
                        modelo.Registros = registros;

                        var registrosEliminados = _context.DOTACION_Registros_resultados.Select(b => b).Where(s => s.Activo == 1).OrderBy(x => x.Rut).ToList();
                        ViewBag.Eliminados = registrosEliminados;

                    }

                }
                else if(Perfil_U == 2)
                {
                    if (!string.IsNullOrEmpty(searchString))
                    {

                        ViewData["Perfil"] = "Usuario Comunal";

                        //Obtiene detalle de registros IdServicio del usuario//

                        List<ListaRegistros> MonitoreoRegistros_C = new List<ListaRegistros>();
                        MonitoreoRegistros_C = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 0 && Resultados.Rut == searchString.ToString()
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_C.ToList();

                        var numeroPagina = modelo.Pagina ?? 1;
                        var registros = await MonitoreoRegistros_C.ToPagedListAsync(numeroPagina, 15);
                        modelo.Registros = registros;

                        //Obtiene detalle de registros eliminados IdComuna del usuario//
                        List<ListaRegistros> MonitoreoRegistrosEliminados_C = new List<ListaRegistros>();
                        MonitoreoRegistrosEliminados_C = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 1
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Eliminados = MonitoreoRegistrosEliminados_C.ToList();

                    }
                    else 
                    {
                        ViewData["Perfil"] = "Usuario Comunal";

                        //Obtiene detalle de registros IdServicio del usuario//

                        List<ListaRegistros> MonitoreoRegistros_C = new List<ListaRegistros>();
                        MonitoreoRegistros_C = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 0
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_C.ToList();

                        var numeroPagina = modelo.Pagina ?? 1;
                        var registros = await MonitoreoRegistros_C.ToPagedListAsync(numeroPagina, 15);
                        modelo.Registros = registros;

                        //Obtiene detalle de registros eliminados IdComuna del usuario//
                        List<ListaRegistros> MonitoreoRegistrosEliminados_C = new List<ListaRegistros>();
                        MonitoreoRegistrosEliminados_C = (from Resultados in _context.DOTACION_Registros_resultados
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 1
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Eliminados = MonitoreoRegistrosEliminados_C.ToList();

                    }
                }
                Filtro = searchString;
                return View("ActualizaRegistros", modelo);
            }
        }

    }
}
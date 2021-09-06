using DotacionWEBCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DotacionWEBCore.Controllers
{
    public class IncentivoRetiroController : Controller
    {
        private static string userId;

        // ------- definiendo conexion a base de datos ------- //
        private readonly IConfiguration configuration;
        private readonly DatabaseContext _context;
        private int Perfil_U;
        private SqlConnection conn;
        private int Id_Validacion_Registro; string RutPostulaRetiroRegistro;
        public static string Filtro;

        // ------- definiendo conexion a base de datos ------- //
        public IncentivoRetiroController(DatabaseContext context, IConfiguration config)
        {
            _context = context;
            configuration = config;

        }
        [Authorize]
        public IActionResult IncentivoRetiro()
        {

            //--conecta a SQL--//
            string Connstr = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(Connstr);
            conn.Open();

            // obtiene Rut del usuario logeado//
            userId = User.Identity.Name;
            //ViewData["Rut"] = userId;

            //Obtiene perfil del usuario//
            string Perfil = "SELECT TOP 1 [ID_Perfil] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdPerfil = new SqlCommand(Perfil, conn);
            var SQLPerfil = cmdPerfil.ExecuteScalar();
            Perfil_U = Int32.Parse(SQLPerfil.ToString());

            //Obtiene IdServicio del usuario//
            string IdServicio = "SELECT TOP 1 [IdServicio] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdIdServicio = new SqlCommand(IdServicio, conn);
            var SQLIdServicio = cmdIdServicio.ExecuteScalar();

            //Obtiene IdComuna del usuario//
            string IdComuna = "SELECT TOP 1 [ID_Comuna_U] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
            SqlCommand cmdIdComuna = new SqlCommand(IdComuna, conn);
            var SQLIdComuna = cmdIdComuna.ExecuteScalar();

            //Obtiene total de registros que pueden postular a retiro voluntario del servicio del usuario//
            string Registro = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados_IR] where [ID_Servicio] = " + SQLIdServicio;
            SqlCommand cmdRegistros = new SqlCommand(Registro, conn);
            var TotalRegistros = cmdRegistros.ExecuteScalar();
            ViewData["TotalRegistrosServicio"] = TotalRegistros;

            //Obtiene total de registros que pueden postular al retiro voluntario de la com,una del usuario//
            string RegistroUsuario = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados_IR] where [ID_Comuna] = " + SQLIdComuna;
            SqlCommand cmdRegistrosUsuario = new SqlCommand(RegistroUsuario, conn);
            var TotalRegistroUsuario = cmdRegistrosUsuario.ExecuteScalar();
            ViewData["TotalRegistrosUsuario"] = TotalRegistroUsuario;

            conn.Close();

            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //forzando el perfil para poder realizar pruebas //
            //Perfil_U = 2;

            {
                ViewBag.ID_PerfilUsuario = Perfil_U;
                ViewBag.IdServicioUsuario = SQLIdServicio;
                ViewBag.IdComunaUsuario = SQLIdComuna;

                // Muestra datos según Perfil //
                if (Perfil_U == 1)
                //Muestra Resultado Perfil Servicio //
                {
                    ViewData["Perfil"] = "Administrador de Servicio";

                    //Obtiene detalle de registros IdServicio del usuario//
                    List<ListaRegistrosIR> MonitoreoRegistros_S = new List<ListaRegistrosIR>();
                    MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados_IR
                                            where Resultados.ID_Servicio == int.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 0
                                            orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                            select Resultados).ToList();
                    ViewBag.Resultados = MonitoreoRegistros_S.ToList();
                }

                else
                {
                    ViewData["Perfil"] = "Usuario Comunal";

                    //Obtiene detalle de registros IdServicio del usuario//

                    List<ListaRegistrosIR> MonitoreoRegistros_C = new List<ListaRegistrosIR>();
                    MonitoreoRegistros_C = (from Resultados in _context.DOTACION_Registros_resultados_IR
                                            where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 0
                                            orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                            select Resultados).ToList();
                    ViewBag.Resultados = MonitoreoRegistros_C.ToList();

                }
                
                return View();

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
                "Servicio",
                "Comuna",
                "Establecimiento",
                "Administracion",
                "Rut",
                "DV",
                "Apellido Paterno",
                "Apellido Materno",
                "Nombre",
                "Sexo",
                "Fecha Nacimiento",
                "Nacionalidad",
                "Categoría",
                "Profesión",
                "Especialidad",
                "Cargo",
                "Asig. Chofer",
                "Contratación",
                "Tipo de contrato",
                "Jornada",
                "Fecha Ingreso",
                "Años de Serv.",
                "Bienios",
                "Carrera Func.",
                "Previsión",
                "Isapre",
                "Base comunal",
                "Haberes",
                "Validado",
                "Revisado",
                "PostulaRetiro"
                };

                    byte[] result;

                    using (var package = new ExcelPackage())
                    {
                        // Agrega una hoja al libro de trabajo de excel

                        var worksheet = package.Workbook.Worksheets.Add("Retiro voluntario - servicio"); //nombre de la hoja excel
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

                        List<ListaRegistrosIR> MonitoreoRegistros_S = new List<ListaRegistrosIR>();
                        MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados_IR
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
                            worksheet.Cells["N" + ContadorRegistros].Value = RegistrosExcel.Categoria;
                            worksheet.Cells["O" + ContadorRegistros].Value = RegistrosExcel.Profesion;
                            worksheet.Cells["P" + ContadorRegistros].Value = RegistrosExcel.Especialidad;
                            worksheet.Cells["Q" + ContadorRegistros].Value = RegistrosExcel.Cargo;
                            worksheet.Cells["R" + ContadorRegistros].Value = RegistrosExcel.Funciones_Chofer;
                            worksheet.Cells["S" + ContadorRegistros].Value = RegistrosExcel.Ley;
                            worksheet.Cells["T" + ContadorRegistros].Value = RegistrosExcel.Tipo_contrato;
                            worksheet.Cells["U" + ContadorRegistros].Value = RegistrosExcel.Jornada;
                            worksheet.Cells["V" + ContadorRegistros].Value = RegistrosExcel.Fecha_Ingreso_Texto;
                            worksheet.Cells["W" + ContadorRegistros].Value = RegistrosExcel.Anos_Servicio;
                            worksheet.Cells["X" + ContadorRegistros].Value = RegistrosExcel.Bienios;
                            worksheet.Cells["Y" + ContadorRegistros].Value = RegistrosExcel.Nivel_Carrera;
                            worksheet.Cells["Z" + ContadorRegistros].Value = RegistrosExcel.Tipo_Prevision;
                            worksheet.Cells["AA" + ContadorRegistros].Value = RegistrosExcel.Tipo_Isapre;
                            worksheet.Cells["AB" + ContadorRegistros].Value = RegistrosExcel.SBase_Comunal;
                            worksheet.Cells["AC" + ContadorRegistros].Value = RegistrosExcel.Haberes;
                            worksheet.Cells["AD" + ContadorRegistros].Value = RegistrosExcel.ValidadoTexto;
                            worksheet.Cells["AE" + ContadorRegistros].Value = RegistrosExcel.RevisadoTexto;
                            worksheet.Cells["AF" + ContadorRegistros].Value = RegistrosExcel.PostulaRetiroTexto;


                            ContadorRegistros++;


                        }
                        result = package.GetAsByteArray();
                    }
                    return File(result, "application/ms-excel", $"ModoAPS_Retiro_Voluntario_Servicio.xlsx");
                }
            }
            else

            {
                //Exporta Excel perfil Servicio //
                {
                    var comlumHeadrs = new string[]
                {
                "N°",
                "Servicio",
                "Comuna",
                "Establecimiento",
                "Administracion",
                "Rut",
                "DV",
                "Apellido Paterno",
                "Apellido Materno",
                "Nombre",
                "Sexo",
                "Fecha Nacimiento",
                "Nacionalidad",
                "Categoría",
                "Profesión",
                "Especialidad",
                "Cargo",
                "Asig. Chofer",
                "Contratación",
                "Tipo de contrato",
                "Jornada",
                "Fecha Ingreso",
                "Años de Serv.",
                "Bienios",
                "Carrera Func.",
                "Previsión",
                "Isapre",
                "Base comunal",
                "Haberes",
                "Validado",
                "Revisado",
                "PostulaRetiro"
                };

                    byte[] result;

                    using (var package = new ExcelPackage())
                    {
                        // Agrega una hoja al libro de trabajo de excel

                        var worksheet = package.Workbook.Worksheets.Add("Retiro voluntario - Comuna"); //nombre de la hoja excel
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

                        List<ListaRegistrosIR> MonitoreoRegistros_S = new List<ListaRegistrosIR>();
                        MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados_IR
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
                            worksheet.Cells["N" + ContadorRegistros].Value = RegistrosExcel.Categoria;
                            worksheet.Cells["O" + ContadorRegistros].Value = RegistrosExcel.Profesion;
                            worksheet.Cells["P" + ContadorRegistros].Value = RegistrosExcel.Especialidad;
                            worksheet.Cells["Q" + ContadorRegistros].Value = RegistrosExcel.Cargo;
                            worksheet.Cells["R" + ContadorRegistros].Value = RegistrosExcel.Funciones_Chofer;
                            worksheet.Cells["S" + ContadorRegistros].Value = RegistrosExcel.Ley;
                            worksheet.Cells["T" + ContadorRegistros].Value = RegistrosExcel.Tipo_contrato;
                            worksheet.Cells["U" + ContadorRegistros].Value = RegistrosExcel.Jornada;
                            worksheet.Cells["V" + ContadorRegistros].Value = RegistrosExcel.Fecha_Ingreso_Texto;
                            worksheet.Cells["W" + ContadorRegistros].Value = RegistrosExcel.Anos_Servicio;
                            worksheet.Cells["X" + ContadorRegistros].Value = RegistrosExcel.Bienios;
                            worksheet.Cells["Y" + ContadorRegistros].Value = RegistrosExcel.Nivel_Carrera;
                            worksheet.Cells["Z" + ContadorRegistros].Value = RegistrosExcel.Tipo_Prevision;
                            worksheet.Cells["AA" + ContadorRegistros].Value = RegistrosExcel.Tipo_Isapre;
                            worksheet.Cells["AB" + ContadorRegistros].Value = RegistrosExcel.SBase_Comunal;
                            worksheet.Cells["AC" + ContadorRegistros].Value = RegistrosExcel.Haberes;
                            worksheet.Cells["AD" + ContadorRegistros].Value = RegistrosExcel.ValidadoTexto;
                            worksheet.Cells["AE" + ContadorRegistros].Value = RegistrosExcel.RevisadoTexto;
                            worksheet.Cells["AF" + ContadorRegistros].Value = RegistrosExcel.PostulaRetiroTexto;

                            ContadorRegistros++;
                        }
                        result = package.GetAsByteArray();
                    }
                    return File(result, "application/ms-excel", $"ModoAPS_Retiro_Voluntario_Comuna.xlsx");
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
        public IActionResult PostulaRetiroRegistros(string[] checkAll)

        {

            //--conecta a SQL2--//
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

            //Obtiene total de registros que pueden postular a retiro voluntario del servicio del usuario//
            string Registro = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados_IR] where [ID_Servicio] = " + SQLIdServicio;
            SqlCommand cmdRegistros = new SqlCommand(Registro, conn2);
            var TotalRegistros = cmdRegistros.ExecuteScalar();
            ViewData["TotalRegistrosServicio"] = TotalRegistros;

            //Obtiene total de registros que pueden postular al retiro voluntario de la com,una del usuario//
            string RegistroUsuario = "SELECT count([ID_Registro]) FROM .[dbo].[DOTACION_Registros_resultados_IR] where [ID_Comuna] = " + SQLIdComuna;
            SqlCommand cmdRegistrosUsuario = new SqlCommand(RegistroUsuario, conn2);
            var TotalRegistroUsuario = cmdRegistrosUsuario.ExecuteScalar();
            ViewData["TotalRegistrosUsuario"] = TotalRegistroUsuario;

            conn2.Close();

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

                    foreach (var item in checkAll)
                    {
                        conn2.Open();

                        string ID_Validacion1 = "SELECT ID_Registro FROM .[dbo].[DOTACION_Registros_resultados_IR] where [ID] = '" + item + "'";
                        SqlCommand cmdID_Validacion1Registro = new SqlCommand(ID_Validacion1, conn2);
                        var SQLIDValidacion1Registro = cmdID_Validacion1Registro.ExecuteScalar();
                        Id_Validacion_Registro = Int32.Parse(SQLIDValidacion1Registro.ToString());

                        string RutPostulaRetiro = "SELECT Rut FROM .[dbo].[DOTACION_Registros_resultados_IR] where [ID] = '" + item + "'";
                        SqlCommand cmdRutPostulaRetiroRegistro = new SqlCommand(RutPostulaRetiro, conn2);
                        var SQLRutPostulaRetiroRegistro = cmdRutPostulaRetiroRegistro.ExecuteScalar();
                        RutPostulaRetiroRegistro = SQLRutPostulaRetiroRegistro.ToString();

                        string ValidaRut = "SELECT Rut FROM.[dbo].[DOTACION_Registros_postulacion_IR] where [Rut] = '" + RutPostulaRetiroRegistro + "'";
                        SqlCommand cmdValidaRutRegistro = new SqlCommand(ValidaRut, conn2);
                        var SQLValidaRutRegistro = cmdValidaRutRegistro.ExecuteScalar();

                        if (SQLValidaRutRegistro == null)
                        {
                            string query = "INSERT INTO [dbo].[DOTACION_registros_postulacion_IR] ([Rut],[DV] ,[Apellido_Paterno],[Apellido_Materno],[Nombre],[Sexo],[Fecha_Nacimiento],[Nacionalidad],[Ley],[Fecha_Postulacion],[usuario_postulador]) (SELECT DISTINCT [Rut],[DV],[Apellido_Paterno],[Apellido_Materno],[Nombre],[Sexo],[Fecha_Nacimiento],[Nacionalidad],[Ley],getdate(),'" + User.Identity.Name + "' FROM [dbo].[DOTACION_registros] DRPIR WHERE [Rut] = '" + RutPostulaRetiroRegistro + "' AND Ley = 'Ley 19.378')";
                            SqlCommand cmd = new SqlCommand(query, conn2);
                            cmd.ExecuteReader();
                            conn2.Close();

                            conn2.Open();
                            string query1 = "UPDATE [dbo].[DOTACION_Registros] SET [PostulaRetiro] = 1, [Fecha_Postulacion] = GETDATE(), [usuario_postulador] =  '" + User.Identity.Name + "' WHERE  Rut = '" + RutPostulaRetiroRegistro + "'";
                            SqlCommand cmd1 = new SqlCommand(query1, conn2);
                            cmd1.ExecuteReader();
                            conn2.Close();
                        }
                        else
                        {
                            string query = "UPDATE [dbo].[DOTACION_registros_postulacion_IR] SET [Fecha_Postulacion] = GETDATE(),[usuario_postulador] = '" + User.Identity.Name + "' WHERE [Rut] = '" + RutPostulaRetiroRegistro + "'";
                            SqlCommand cmd = new SqlCommand(query, conn2);
                            cmd.ExecuteReader();
                            conn2.Close();

                            conn2.Open();
                            string query1 = "UPDATE [dbo].[DOTACION_Registros] SET [PostulaRetiro] = 1, [Fecha_Postulacion] = GETDATE(), [usuario_postulador] =  '" + User.Identity.Name + "' WHERE  Rut = '" + RutPostulaRetiroRegistro + "'";
                            SqlCommand cmd1 = new SqlCommand(query1, conn2);
                            cmd1.ExecuteReader();
                            conn2.Close();

                        }
                    }


                    if (!string.IsNullOrEmpty(Filtro))
                    {
                        ViewData["Perfil"] = "Administrador de Servicio";

                        //Obtiene detalle de registros IdServicio del usuario//
                        List<ListaRegistrosIR> MonitoreoRegistros_S = new List<ListaRegistrosIR>();
                        MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados_IR
                                                where Resultados.ID_Servicio == int.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 0 && Resultados.Rut == Filtro.ToString()
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_S.ToList();
                    }
                    else
                    {
                        ViewData["Perfil"] = "Administrador de Servicio";

                        //Obtiene detalle de registros IdServicio del usuario//
                        List<ListaRegistrosIR> MonitoreoRegistros_S = new List<ListaRegistrosIR>();
                        MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados_IR
                                                where Resultados.ID_Servicio == int.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 0
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_S.ToList();
                    }

                }
                else 
                {
                    foreach (var item in checkAll)
                    {
                        conn2.Open();

                        string ID_Validacion1 = "SELECT ID_Registro FROM .[dbo].[DOTACION_Registros_resultados_IR] where [ID] = '" + item + "'";
                        SqlCommand cmdID_Validacion1Registro = new SqlCommand(ID_Validacion1, conn2);
                        var SQLIDValidacion1Registro = cmdID_Validacion1Registro.ExecuteScalar();
                        Id_Validacion_Registro = Int32.Parse(SQLIDValidacion1Registro.ToString());

                        string RutPostulaRetiro = "SELECT Rut FROM .[dbo].[DOTACION_Registros_resultados_IR] where [ID] = '" + item + "'";
                        SqlCommand cmdRutPostulaRetiroRegistro = new SqlCommand(RutPostulaRetiro, conn2);
                        var SQLRutPostulaRetiroRegistro = cmdRutPostulaRetiroRegistro.ExecuteScalar();
                        RutPostulaRetiroRegistro = SQLRutPostulaRetiroRegistro.ToString();

                        string ValidaRut = "SELECT Rut FROM.[dbo].[DOTACION_Registros_postulacion_IR] where [Rut] = '" + RutPostulaRetiroRegistro + "'";
                        SqlCommand cmdValidaRutRegistro = new SqlCommand(ValidaRut, conn2);
                        var SQLValidaRutRegistro = cmdValidaRutRegistro.ExecuteScalar();

                        if (SQLValidaRutRegistro == null)
                        {
                            string query = "INSERT INTO [dbo].[DOTACION_registros_postulacion_IR] ([Rut],[DV] ,[Apellido_Paterno],[Apellido_Materno],[Nombre],[Sexo],[Fecha_Nacimiento],[Nacionalidad],[Ley],[Fecha_Postulacion],[usuario_postulador]) (SELECT DISTINCT [Rut],[DV],[Apellido_Paterno],[Apellido_Materno],[Nombre],[Sexo],[Fecha_Nacimiento],[Nacionalidad],[Ley],getdate(),'" + User.Identity.Name + "' FROM [dbo].[DOTACION_registros] DRPIR WHERE [Rut] = '" + RutPostulaRetiroRegistro + "' AND Ley = 'Ley 19.378')";
                            SqlCommand cmd = new SqlCommand(query, conn2);
                            cmd.ExecuteReader();
                            conn2.Close();

                            conn2.Open();
                            string query1 = "UPDATE [dbo].[DOTACION_Registros] SET [PostulaRetiro] = 1, [Fecha_Postulacion] = GETDATE(), [usuario_postulador] =  '" + User.Identity.Name + "' WHERE  Rut = '" + RutPostulaRetiroRegistro + "'";
                            SqlCommand cmd1 = new SqlCommand(query1, conn2);
                            cmd1.ExecuteReader();
                            conn2.Close();
                        }
                        else
                        {
                            string query = "UPDATE [dbo].[DOTACION_registros_postulacion_IR] SET [Fecha_Postulacion] = GETDATE(),[usuario_postulador] = '" + User.Identity.Name + "' WHERE [Rut] = '" + RutPostulaRetiroRegistro + "'";
                            SqlCommand cmd = new SqlCommand(query, conn2);
                            cmd.ExecuteReader();
                            conn2.Close();

                            conn2.Open();
                            string query1 = "UPDATE [dbo].[DOTACION_Registros] SET [PostulaRetiro] = 1, [Fecha_Postulacion] = GETDATE(), [usuario_postulador] =  '" + User.Identity.Name + "' WHERE  Rut = '" + RutPostulaRetiroRegistro + "'";
                            SqlCommand cmd1 = new SqlCommand(query1, conn2);
                            cmd1.ExecuteReader();
                            conn2.Close();

                        }
                    }

                    if (!string.IsNullOrEmpty(Filtro))
                    {

                        ViewData["Perfil"] = "Usuario Comunal";

                        //Obtiene detalle de registros IdServicio del usuario//

                        List<ListaRegistrosIR> MonitoreoRegistros_C = new List<ListaRegistrosIR>();
                        MonitoreoRegistros_C = (from Resultados in _context.DOTACION_Registros_resultados_IR
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 0 && Resultados.Rut == Filtro.ToString()
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_C.ToList();
                    }
                    else
                    {
                        ViewData["Perfil"] = "Usuario Comunal";

                        //Obtiene detalle de registros IdServicio del usuario//

                        List<ListaRegistrosIR> MonitoreoRegistros_C = new List<ListaRegistrosIR>();
                        MonitoreoRegistros_C = (from Resultados in _context.DOTACION_Registros_resultados_IR
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 0
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_C.ToList();
                    }

                }

            }
        return View("IncentivoRetiro", "IncentivoRetiro");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Filtrar(string searchString)
        {

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


            //ViewData["Perfil"] = "Administrador de Servicio";

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
                        List<ListaRegistrosIR> MonitoreoRegistros_S = new List<ListaRegistrosIR>();
                        MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados_IR
                                                where Resultados.ID_Servicio == int.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 0 && Resultados.Rut == searchString.ToString()
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_S.ToList();
                    }
                    else
                    {
                        ViewData["Perfil"] = "Administrador de Servicio";

                        //Obtiene detalle de registros IdServicio del usuario//
                        List<ListaRegistrosIR> MonitoreoRegistros_S = new List<ListaRegistrosIR>();
                        MonitoreoRegistros_S = (from Resultados in _context.DOTACION_Registros_resultados_IR
                                                where Resultados.ID_Servicio == int.Parse(SQLIdServicio.ToString()) && Resultados.Activo == 0
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_S.ToList();
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(searchString))
                    {

                        ViewData["Perfil"] = "Usuario Comunal";

                        //Obtiene detalle de registros IdServicio del usuario//

                        List<ListaRegistrosIR> MonitoreoRegistros_C = new List<ListaRegistrosIR>();
                        MonitoreoRegistros_C = (from Resultados in _context.DOTACION_Registros_resultados_IR
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 0 && Resultados.Rut == searchString.ToString()
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_C.ToList();
                    }
                    else 
                    {
                        ViewData["Perfil"] = "Usuario Comunal";

                        //Obtiene detalle de registros IdServicio del usuario//

                        List<ListaRegistrosIR> MonitoreoRegistros_C = new List<ListaRegistrosIR>();
                        MonitoreoRegistros_C = (from Resultados in _context.DOTACION_Registros_resultados_IR
                                                where Resultados.ID_Comuna == int.Parse(SQLIdComuna.ToString()) && Resultados.Activo == 0
                                                orderby Resultados.Rut, Resultados.ID_Comuna, Resultados.ID_Establecimiento, Resultados.Ley, Resultados.Tipo_contrato
                                                select Resultados).ToList();
                        ViewBag.Resultados = MonitoreoRegistros_C.ToList();
                    }
                }
                Filtro = searchString;
                return View("IncentivoRetiro", "IncentivoRetiro");
            }
        }

    }
}


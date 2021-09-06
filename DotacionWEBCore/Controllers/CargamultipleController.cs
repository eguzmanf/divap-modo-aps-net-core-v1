using DotacionWEBCore.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Linq;
using System.Globalization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Vereyon.Web;

namespace DotacionWEBCore.Controllers

{
    public class CargamultipleController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly IConfiguration configuration;
        private readonly DatabaseContext _context;
        private static DataTable resultado; int SumJornada = 0; int SumJornadaBD = 0; int cont = 0; int contSumaJornada = 0; int contSumaJornadaBD = 0; int contRepetidoExcel = 0; int contRepetido = 0; int contServicioInexistente = 0; int contServicio = 0; int contComuna = 0; int contEstablecimiento = 0; int contAdministracion = 0; int contRUT = 0; int contDV = 0; int contAPaterno = 0; int contAMaterno = 0; int contNombre = 0; int contSexo = 0; int contFecha_Nacimiento = 0; int contNacionalidad = 0; int contCategoria = 0; int contContratoCategoria = 0; int contContratoProfesion = 0; int contProfesion = 0; int contEspecialidad = 0; int contCargo = 0; int contLey = 0; int contContrato = 0; int contChofer = 0; int contJornada = 0; int contFechaIngreso = 0; int contAñosServicio; int contBienios; int contNivel_Carrera; int contSBase_Comunal = 0; int contHaberes = 0; int contTipo_Prevision = 0; int contTipo_Isapre = 0; int validaTexto = 0; int contRegistroEliminadoError = 0; DateTime validaFecha;
        private static Boolean RevisionExcel; Boolean SumaJornadaError; Boolean SumaJornadaBDError; Boolean RepetidoExcelError; Boolean RepetidoError; Boolean ServicioError; Boolean ComunaError; Boolean EstablecimientoError; Boolean AdministracionError; Boolean RUTError; Boolean DVError; Boolean APaternoError; Boolean AMaternoError; Boolean NombreError; Boolean SexoError; Boolean Fecha_NacimientoError; Boolean NacionalidadError; Boolean CategoriaError; Boolean ContratoCategoriaError; Boolean ContratoProfesionError; Boolean ProfesionError; Boolean EspecialidadError; Boolean CargoError; Boolean LeyError; Boolean ContratoError; Boolean ChoferError; Boolean JornadaError; Boolean FechaIngresoError; Boolean AñosServicioError; Boolean BieniosError; Boolean Nivel_CarreraError; Boolean SBase_ComunalError; Boolean HaberesError; Boolean Tipo_PrevisionError; Boolean Tipo_IsapreError; Boolean CargaError; Boolean RegistroEliminadoError;


        public CargamultipleController(DatabaseContext context, IConfiguration config, IFlashMessage flashMessage)
        {
            _context = context;
            this.configuration = config;
            _flashMessage = flashMessage;
        }

        public IActionResult Cargamultiple()
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            if (Perfil_U == 3)
            {
                return View();
            }
            else 
            {
                _flashMessage.Danger("Acceso denegado, Usted no tiene los permisos necesarios para acceder a este Recurso!");
                return RedirectToAction("Index", "Home", null);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Cargamultiple(IFormFile upload)
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (ModelState.IsValid)
            {

                if (upload != null && upload.Length > 0)

                {

                    // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                    // to get started. This is how we avoid dependencies on ACE or Interop:
                    Stream stream = upload.OpenReadStream();

                    // We return the interface, so that
                    IExcelDataReader reader = null;

                    if (upload.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateReader(stream);
                        var excel = ExcelReaderFactory.CreateReader(stream);
                    }
                    else if (upload.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        var excel = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }

                    else
                    {
                        ModelState.AddModelError("File", "El archivo seleccionado no es un Excel válido para cargar registros");
                        return View();

                    }

                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }

                    };

                    DataSet result = reader.AsDataSet(conf);
                    resultado = result.Tables["Carga"];
                    if (resultado == null)
                    {
                        ModelState.AddModelError("File", "El archivo seleccionado no contiene la hoja 'Carga'");
                    }
                    //reader.Close();
                    else
                    {
                        if (resultado.Columns.Count > 29)

                        {
                            ModelState.AddModelError("File", "El archivo seleccionado contiene mas columnas de las permitidas");
                            return View();
                        }

                        else if (resultado.Columns.Count < 29)

                        {
                            ModelState.AddModelError("File", "El archivo seleccionado contiene menos columnas de las permitidas");
                            return View();
                        }
                    }
                    return View(resultado);
                }

                else
                {
                    object result = null;


                    if (result != null)

                    {
                        ModelState.AddModelError("File", "Seleccione el archivo que va a cargar en la base de datos");
                    }

                    else
                    {
                        if (resultado.Columns.Count > 29)

                        {
                            ModelState.AddModelError("File", "El archivo seleccionado contiene mas columnas de las permitidas");
                        }

                        else if (resultado.Columns.Count < 29)

                        {
                            ModelState.AddModelError("File", "El archivo seleccionado contiene menos columnas de las permitidas");
                        }

                        else

                        {


                            //--pone a las variables que detectan error en falso y las variables de conteo en 1 para asimilarlas al Excel--//
                            SumaJornadaBDError = false;
                            SumaJornadaError = false;
                            RepetidoExcelError = false;
                            RepetidoError = false;
                            ServicioError = false;
                            ComunaError = false;
                            EstablecimientoError = false;
                            //EstablecimientoMadreError = false;
                            //TipoEstablecimientoError = false;
                            AdministracionError = false;
                            RUTError = false;
                            DVError = false;
                            APaternoError = false;
                            AMaternoError = false;
                            NombreError = false;
                            SexoError = false;
                            Fecha_NacimientoError = false;
                            NacionalidadError = false;
                            CategoriaError = false;
                            ContratoCategoriaError = false;
                            ContratoProfesionError = false;
                            ProfesionError = false;
                            EspecialidadError = false;
                            CargoError = false;
                            LeyError = false;
                            ContratoError = false;
                            ChoferError = false;
                            JornadaError = false;
                            FechaIngresoError = false;
                            AñosServicioError = false;
                            BieniosError = false;
                            Nivel_CarreraError = false;
                            SBase_ComunalError = false;
                            HaberesError = false;
                            Tipo_PrevisionError = false;
                            Tipo_IsapreError = false;
                            CargaError = false;
                            RegistroEliminadoError = false;


                            contSumaJornadaBD = 0;
                            contSumaJornada = 0;
                            contRepetidoExcel = 0;
                            contRepetido = 0;
                            contServicio = 0;
                            contComuna = 0;
                            contEstablecimiento = 0;
                            //contEstablecimientoMadre = 1;
                            //contTipoEstablecimiento = 1;
                            contAdministracion = 0;
                            contRUT = 0;
                            contDV = 0;
                            contAPaterno = 0;
                            contAMaterno = 0;
                            contNombre = 0;
                            contSexo = 0;
                            contFecha_Nacimiento = 0;
                            contNacionalidad = 0;
                            contCategoria = 0;
                            contContratoCategoria = 0;
                            contContratoProfesion = 0;
                            contProfesion = 0;
                            contEspecialidad = 0;
                            contCargo = 0;
                            contLey = 0;
                            contContrato = 0;
                            contChofer = 0;
                            contJornada = 0;
                            contFechaIngreso = 0;
                            contAñosServicio = 0;
                            contBienios = 0;
                            contNivel_Carrera = 0;
                            contSBase_Comunal = 0;
                            contHaberes = 0;
                            contTipo_Prevision = 0;
                            contTipo_Isapre = 0;
                            validaTexto = 0;
                            contRegistroEliminadoError = 0;


                            //--Conecta a SQL--//
                            string Connstr = configuration.GetConnectionString("DefaultConnection");
                            SqlConnection conn = new SqlConnection(Connstr);

                            foreach (DataRow row in resultado.Rows)
                            {
                                //--cuenta la fila del primer error--//
                                if (SumaJornadaBDError == false)
                                {
                                    contSumaJornadaBD++;
                                }
                                if (SumaJornadaError == false)
                                {
                                    contSumaJornada++;
                                }
                                if (RepetidoExcelError == false)
                                {
                                    contRepetidoExcel++;
                                }

                                if (RepetidoError == false)
                                {
                                    contRepetido++;
                                }

                                if (ServicioError == false)
                                {
                                    contServicio++;
                                }

                                if (ComunaError == false)
                                {
                                    contComuna++;
                                }

                                if (EstablecimientoError == false)
                                {
                                    contEstablecimiento++;
                                }

                                /* if (EstablecimientoMadreError == false)
                                {
                                    contEstablecimientoMadre++;
                                }

                                if (TipoEstablecimientoError == false)
                                {
                                    contTipoEstablecimiento++;
                                }
                                */
                                if (AdministracionError == false)
                                {
                                    contAdministracion++;
                                }

                                if (RUTError == false)
                                {
                                    contRUT++;
                                }

                                if (DVError == false)
                                {
                                    contDV++;
                                }

                                if (APaternoError == false)
                                {
                                    contAPaterno++;
                                }

                                if (AMaternoError == false)
                                {
                                    contAMaterno++;
                                }

                                if (NombreError == false)
                                {
                                    contNombre++;
                                }

                                if (SexoError == false)
                                {
                                    contSexo++;
                                }

                                if (Fecha_NacimientoError == false)
                                {
                                    contFecha_Nacimiento++;
                                }

                                if (NacionalidadError == false)
                                {
                                    contNacionalidad++;
                                }

                                if (CategoriaError == false)
                                {
                                    contCategoria++;
                                }

                                if (ContratoCategoriaError == false)
                                {
                                    contContratoCategoria++;
                                }

                                if (ContratoProfesionError == false)
                                {
                                    contContratoProfesion++;
                                }

                                if (ProfesionError == false)
                                {
                                    contProfesion++;
                                }

                                if (EspecialidadError == false)
                                {
                                    contEspecialidad++;
                                }

                                if (CargoError == false)
                                {
                                    contCargo++;
                                }

                                if (LeyError == false)
                                {
                                    contLey++;
                                }

                                if (ContratoError == false)
                                {
                                    contContrato++;
                                }

                                if (ChoferError == false)
                                {
                                    contChofer++;
                                }

                                if (JornadaError == false)
                                {
                                    contJornada++;
                                }

                                if (FechaIngresoError == false)
                                {
                                    contFechaIngreso++;
                                }

                                if (AñosServicioError == false)
                                {
                                    contAñosServicio++;
                                }

                                if (BieniosError == false)
                                {
                                    contBienios++;
                                }

                                if (Nivel_CarreraError == false)
                                {
                                    contNivel_Carrera++;
                                }

                                if (SBase_ComunalError == false)
                                {
                                    contSBase_Comunal++;
                                }

                                if (HaberesError == false)
                                {
                                    contHaberes++;
                                }

                                if (Tipo_PrevisionError == false)
                                {
                                    contTipo_Prevision++;
                                }

                                if (Tipo_IsapreError == false)
                                {
                                    contTipo_Isapre++;
                                }

                                if (RegistroEliminadoError == false)
                                {
                                    contRegistroEliminadoError++;
                                }


                                //--Compara los datos del excel con la información alamcenadas en las bases de datos--//
                                conn.Open();

                                /*
                                // CultureInfo culture = new CultureInfo("es-ES");
                                    String fechaNacimientoExcell = row[11].ToString();
                                    DateTime fechaNac1 = Convert.ToDateTime(fechaNacimientoExcell, System.Globalization.CultureInfo.InvariantCulture);
                                    DateTime fechaNacimientoDate = DateTime.ParseExact(fechaNac1.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                    //
                                    String fechaIngresoExcell = row[21].ToString();
                                    DateTime fechaIngreso1 = Convert.ToDateTime(fechaIngresoExcell, System.Globalization.CultureInfo.InvariantCulture);
                                    DateTime fechaIngresoDate = DateTime.ParseExact(fechaIngreso1.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                */

                                //verifica si el registro está eliminado (activo = 1)//
                                string EliminadoID = "SELECT ISNULL((SELECT SUM([Activo]) FROM [dbo].[DOTACION_Registros] where [ID_Establecimiento] = (Select [CodigoNuevo] FROM [dbo].[DOTACION_Establecimientos] where Establecimiento = '" + row[3].ToString() + "' and [Id_Comuna1] = (SELECT [ID_Comuna1] FROM [dbo].[DOTACION_Comunas] where [Comuna] ='" + row[2].ToString() + "')) and [ID_Servicio] = (SELECT [ID_Servicio] FROM [dbo].[DOTACION_Servicios] where Servicio = '" + row[1].ToString() + "') and [Id_Comuna] = (SELECT [ID_Comuna1] FROM [dbo].[DOTACION_Comunas] where [Comuna] ='" + row[2].ToString() + "') and [Administracion] = '" + row[4].ToString() + "' and [Rut] = '" + row[5].ToString() + "' and [DV] = '" + row[6].ToString() + "' and [Apellido_Paterno] = upper('" + row[7].ToString() + "') and [Apellido_Materno] = upper('" + row[8].ToString() + "') and [Nombre] = upper('" + row[9].ToString() + "') and [Sexo] = '" + row[10].ToString() + "' and [Fecha_Nacimiento] ='" + row[11].ToString() + "' and [Nacionalidad] = '" + row[12].ToString() + "' and [Categoria] = '" + row[13].ToString() + "' and [Profesion] = '" + row[14].ToString() + "' and [Especialidad] = '" + row[15].ToString() + "' and [Cargo] = '" + row[16].ToString() + "' and [Funciones_Chofer] = '" + row[17].ToString() + "' and [Ley] = '" + row[18].ToString() + "' and [Tipo_contrato] = '" + row[19].ToString() + "' and [Jornada] = '" + row[20].ToString() + "' and [Fecha_Ingreso] = '" + row[21].ToString() + "' and [Anos_Servicio] = '" + row[22].ToString() + "' and [Bienios] = '" + row[23].ToString() + "' and [Nivel_Carrera] = '" + row[24].ToString() + "' and [Tipo_Prevision] = '" + row[25].ToString() + "' and [Tipo_Isapre] = '" + row[26].ToString() + "' and [SBase_Comunal] = '" + row[27].ToString() + "' and [Haberes] = '" + row[28].ToString() + "' and [Activo] = 1),0)";
                                SqlCommand cmdEliminado = new SqlCommand(EliminadoID, conn);
                                var RegistroEliminado = cmdEliminado.ExecuteScalar();

                                // valida Jornada > 44 //
                                string Jornada44 = "SELECT ISNULL((SELECT sum([Jornada]) FROM .[dbo].[DOTACION_Registros_resultados] where [Rut] = '" + row[5].ToString() + "' and ([Ley] = 'Ley 19.378' or [Ley] = 'Código del Trabajo') and [Activo] = 0),0)";
                                SqlCommand cmdJornada44 = new SqlCommand(Jornada44, conn);
                                var MasJornada44 = cmdJornada44.ExecuteScalar();

                                // valida registro repetido //var valorEstablecimiento = cmd2.ExecuteScalar();
                                string RegistroRepetido = "SELECT [ID_Servicio], [ID_Comuna], [ID_Establecimiento],[Rut],[Tipo_Contrato] FROM [dbo].[DOTACION_Registros] where [ID_Establecimiento] = (Select [CodigoNuevo] FROM [dbo].[DOTACION_Establecimientos] where Establecimiento = '" + row[3].ToString() + "' and [Id_Comuna1] = (SELECT [ID_Comuna1] FROM [dbo].[DOTACION_Comunas] where [Comuna] ='" + row[2].ToString() + "')) and [ID_Servicio] = (SELECT [ID_Servicio] FROM [dbo].[DOTACION_Servicios] where Servicio = '" + row[1].ToString() + "') and [Id_Comuna] = (SELECT [ID_Comuna1] FROM [dbo].[DOTACION_Comunas] where [Comuna] ='" + row[2].ToString() + "') and [Rut] = '" + row[5].ToString() + "' and [Ley] = '" + row[18].ToString() + "' and [Tipo_Contrato] = '" + row[19].ToString() + "' and [Activo] = 0";
                                SqlCommand cmdRepetido = new SqlCommand(RegistroRepetido, conn);
                                var Repetido = cmdRepetido.ExecuteScalar();

                                // valida Servicio //
                                string query = "SELECT ISNULL((SELECT DISTINCT [Servicio] FROM [dbo].[DOTACION_Servicios] WHERE [Servicio] = '" + row[1].ToString() + "'),0)";
                                SqlCommand cmd = new SqlCommand(query, conn);
                                var valorServicio = cmd.ExecuteScalar();

                                // valida Comuna //
                                string query1 = "SELECT ISNULL((SELECT DISTINCT [Comuna] FROM [dbo].[DOTACION_Comunas] WHERE [Comuna] = '" + row[2].ToString() + "'),0)";
                                SqlCommand cmd1 = new SqlCommand(query1, conn);
                                var valorComuna = cmd1.ExecuteScalar();

                                // valida Establecimiento //
                                string query2 = "SELECT ISNULL((SELECT DISTINCT [Establecimiento] FROM [dbo].[DOTACION_Establecimientos] WHERE [Establecimiento] = '" + row[3].ToString() + "' and [ID_Comuna1] = (SELECT [ID_Comuna1] FROM [dbo].[DOTACION_Comunas] where [Comuna] ='" + row[2].ToString() + "')),0)";
                                SqlCommand cmd2 = new SqlCommand(query2, conn);
                                var valorEstablecimiento = cmd2.ExecuteScalar();

                                /*
                                // valida Establecimiento Madre //
                                string query3 = "SELECT ISNULL((SELECT DISTINCT [CodigoNuevoMadre] FROM [dbo].[DOTACION_EstablecimientosMadre] WHERE [CodigoNuevoMadre] = '" + row[3].ToString() + "' and [IdComuna1] = '" + row[1].ToString() + "'),0)";
                                SqlCommand cmd3 = new SqlCommand(query3, conn);
                                var valorEstablecimientoMadre = cmd3.ExecuteScalar();

                                // valida Tipo Establecimiento //
                                string query4 = "SELECT ISNULL((SELECT DISTINCT [Tipo_Establecimiento] FROM [dbo].[DOTACION_Tipo_Establecimiento] WHERE [Tipo_Establecimiento] = '" + row[4].ToString() + "'),'0')";
                                SqlCommand cmd4 = new SqlCommand(query4, conn);
                                var valorTipoEstablecimiento = cmd4.ExecuteScalar();
                                */

                                // valida Administración //
                                string query3 = "SELECT ISNULL((SELECT DISTINCT [Administracion] FROM [dbo].[DOTACION_Administracion] WHERE [Administracion] = '" + row[4].ToString() + "'),'0')";
                                SqlCommand cmd3 = new SqlCommand(query3, conn);
                                var valorAdministracion = cmd3.ExecuteScalar();

                                // valida DV //
                                string query5 = "SELECT ISNULL((SELECT DISTINCT [DV] FROM [dbo].[DOTACION_DV] WHERE [DV] = '" + row[6].ToString() + "'),'0')";
                                SqlCommand cmd5 = new SqlCommand(query5, conn);
                                var valorDV = cmd5.ExecuteScalar();

                                // valida sexo //
                                string query9 = "SELECT ISNULL((SELECT DISTINCT [Sexo] FROM [dbo].[DOTACION_Sexo] WHERE [Sexo] = '" + row[10].ToString() + "'),0)";
                                SqlCommand cmd9 = new SqlCommand(query9, conn);
                                var valorSexo = cmd9.ExecuteScalar();

                                // valida Nacionalidad //
                                string query11 = "SELECT ISNULL((SELECT DISTINCT [Nacionalidad] FROM [dbo].[DOTACION_Nacionalidad] WHERE [Nacionalidad] = '" + row[12].ToString() + "'),0)";
                                SqlCommand cmd11 = new SqlCommand(query11, conn);
                                var valorNacionalidad = cmd11.ExecuteScalar();

                                // valida sexo //
                                string query12 = "SELECT ISNULL((SELECT DISTINCT [Categoria] FROM [dbo].[DOTACION_Categoria] WHERE [Categoria] = '" + row[13].ToString() + "'),0)";
                                SqlCommand cmd12 = new SqlCommand(query12, conn);
                                var valorCategoria = cmd12.ExecuteScalar();

                                // valida profesion //
                                string query13 = "SELECT ISNULL((SELECT DISTINCT [Profesion] FROM [dbo].[DOTACION_Profesion] WHERE [Profesion] = '" + row[14].ToString() + "' AND [Categoria] = '" + row[13].ToString() + "'),0)";
                                SqlCommand cmd13 = new SqlCommand(query13, conn);
                                var valorProfesion = cmd13.ExecuteScalar();

                                // valida especialidad //
                                string query14 = "SELECT ISNULL((SELECT DISTINCT [Especialidad] FROM [dbo].[DOTACION_Especialidad] WHERE [Especialidad] = '" + row[15].ToString() + "' AND [Profesion] = '" + row[14].ToString() + "'),0)";
                                SqlCommand cmd14 = new SqlCommand(query14, conn);
                                var valorEspecialidad = cmd14.ExecuteScalar();

                                // valida cargo //
                                string query15 = "SELECT ISNULL((SELECT DISTINCT [Cargo] FROM [dbo].[DOTACION_Cargo] WHERE [Cargo] = '" + row[16].ToString() + "'),0)";
                                SqlCommand cmd15 = new SqlCommand(query15, conn);
                                var valorCargo = cmd15.ExecuteScalar();

                                // valida chofer //
                                string query16 = "SELECT ISNULL((SELECT DISTINCT [Funciones_Chofer] FROM [dbo].[DOTACION_Chofer] WHERE [Funciones_Chofer] = '" + row[17].ToString() + "' AND [Cargo] = '" + row[16].ToString() + "'),0)";
                                SqlCommand cmd16 = new SqlCommand(query16, conn);
                                var valorChofer = cmd16.ExecuteScalar();

                                // valida ley //
                                string query17 = "SELECT ISNULL((SELECT DISTINCT [Ley] FROM [dbo].[DOTACION_Ley] WHERE [Ley] = '" + row[18].ToString() + "'),0)";
                                SqlCommand cmd17 = new SqlCommand(query17, conn);
                                var valorLey = cmd17.ExecuteScalar();

                                // valida contrato //
                                string query18 = "SELECT ISNULL((SELECT DISTINCT [Tipo_Contrato] FROM [dbo].[DOTACION_Contrato] WHERE [Tipo_contrato] = '" + row[19].ToString() + "' AND [Ley] = '" + row[18].ToString() + "'),0)";
                                SqlCommand cmd18 = new SqlCommand(query18, conn);
                                var valorContrato = cmd18.ExecuteScalar();

                                // valida Años Servicio //
                                string query21 = "SELECT ISNULL((SELECT DISTINCT [Anos_Servicio] FROM [dbo].[DOTACION_Anos_Servicio] WHERE [Anos_Servicio] = '" + row[22].ToString() + "'),0)";
                                SqlCommand cmd21 = new SqlCommand(query21, conn);
                                var valorAnos_Servicion = cmd21.ExecuteScalar();

                                // valida Bienios //
                                string query22 = "SELECT ISNULL((SELECT DISTINCT [Bienios] FROM [dbo].[DOTACION_Bienios] WHERE [Bienios] = '" + row[23].ToString() + "'),0)";
                                SqlCommand cmd22 = new SqlCommand(query22, conn);
                                var valorBienios = cmd22.ExecuteScalar();

                                // valida Carrera Funcionaria //
                                string query23 = "SELECT ISNULL((SELECT DISTINCT [Nivel_Carrera] FROM [dbo].[DOTACION_Nivel_Carrera] WHERE [Nivel_Carrera] = '" + row[24].ToString() + "'),0)";
                                SqlCommand cmd23 = new SqlCommand(query23, conn);
                                var valorNivel_Carrera = cmd23.ExecuteScalar();

                                // valida Previsión //
                                string query26 = "SELECT ISNULL((SELECT DISTINCT [Tipo_Prevision] FROM [dbo].[DOTACION_Tipo_prevision] WHERE [Tipo_Prevision] = '" + row[25].ToString() + "'),0)";
                                SqlCommand cmd26 = new SqlCommand(query26, conn);
                                var valorTipo_Prevision = cmd26.ExecuteScalar();

                                // valida Isapre //
                                string query27 = "SELECT ISNULL((SELECT DISTINCT [Tipo_Isapre] FROM [dbo].[DOTACION_Tipo_Isapre] WHERE [Tipo_Isapre] = '" + row[26].ToString() + "'),0)";
                                SqlCommand cmd27 = new SqlCommand(query27, conn);
                                var valorTipo_Isapre = cmd27.ExecuteScalar();

                                conn.Close();

                                //--Cambia el estado de la variable que identifica el error--//


                                foreach (DataRow rowPost in resultado.Rows)
                                {
                                    //--Registro repetido en el excel--//
                                    if (row[3].ToString() == rowPost[3].ToString() && row[5].ToString() == rowPost[5].ToString() && row[18].ToString() == rowPost[18].ToString() && row[19].ToString() == rowPost[19].ToString() && row != null && row != rowPost)
                                    {
                                        RepetidoExcelError = true;
                                    }

                                }


                                foreach (DataRow rowPost in resultado.Rows)
                                {


                                    if (row[18].ToString() == "Ley 19.378" || row[18].ToString() == "Código del Trabajo")
                                    {
                                        if (row[5].ToString() == rowPost[5].ToString() && row != null)
                                        {
                                            SumJornada = SumJornada + Int32.Parse(rowPost[20].ToString());
                                            if (SumJornada > 44)
                                            {
                                                SumaJornadaError = true;
                                            }
                                            else
                                            {

                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                    else
                                    {

                                    }


                                }
                                SumJornada = 0;

                                //--Registro repetido en la base de datos--//
                                if (Int32.Parse(RegistroEliminado.ToString()) != 0)
                                {
                                    RegistroEliminadoError = true;
                                }
                                //--Registro repetido en la base de datos--//
                                if (Repetido != null && RegistroEliminadoError == false)
                                {
                                    RepetidoError = true;
                                }
                                //--Errores en el Servicio--//
                                else if (row[1].ToString() != valorServicio.ToString())
                                {
                                    ServicioError = true;
                                }


                                //--Errores en la comuna--//
                                if (row[2].ToString() != valorComuna.ToString() && ServicioError == false)
                                {
                                    ComunaError = true;
                                }

                                //--Errores en el establecmiento--//
                                if (row[3].ToString() != valorEstablecimiento.ToString() && ComunaError == false)
                                {
                                    EstablecimientoError = true;
                                }

                                //--Errores en el establecmiento madre--//
                                /* if (row[3].ToString() == "No Aplica" && ComunaError == false)
                                {

                                }
                                else if (row[3].ToString() != valorEstablecimientoMadre.ToString() && ComunaError == false)
                                {
                                    EstablecimientoMadreError = true;
                                }
                                */

                                //--Errores en la Administración--//
                                if (row[4].ToString() != valorAdministracion.ToString())
                                {
                                    AdministracionError = true;
                                }


                                //--Errores en el rut--//
                                if (int.TryParse(row[5].ToString(), out validaTexto))
                                {
                                    if (row[5].ToString().Length < 5 || row[5].ToString().Length > 8 || row[5].ToString().Substring(0, 1) == "0")
                                    {
                                        RUTError = true;
                                    }
                                }
                                else
                                {
                                    RUTError = true;
                                }

                                //--Errores en el DV--//
                                if (row[6].ToString() != valorDV.ToString())
                                {
                                    DVError = true;
                                }

                                //--Errores en el Apellido Paterno--//
                                if (row[7].ToString().Contains("0") || row[7].ToString().Contains("1") || row[7].ToString().Contains("2") || row[7].ToString().Contains("3") || row[7].ToString().Contains("4") || row[7].ToString().Contains("5") || row[7].ToString().Contains("6") || row[7].ToString().Contains("7") || row[7].ToString().Contains("8") || row[7].ToString().Contains("9") || row[7].ToString().Contains("@") || row[7].ToString() == "")
                                {
                                    APaternoError = true;
                                }

                                //--Errores en el Apellido Materno--//
                                if (row[8].ToString().Contains("0") || row[8].ToString().Contains("1") || row[8].ToString().Contains("2") || row[8].ToString().Contains("3") || row[8].ToString().Contains("4") || row[8].ToString().Contains("5") || row[8].ToString().Contains("6") || row[8].ToString().Contains("7") || row[8].ToString().Contains("8") || row[8].ToString().Contains("9") || row[8].ToString().Contains("@") || row[8].ToString() == "")
                                {
                                    AMaternoError = true;
                                }

                                //--Errores en el Nombre--//
                                if (row[9].ToString().Contains("0") || row[9].ToString().Contains("1") || row[9].ToString().Contains("2") || row[9].ToString().Contains("3") || row[9].ToString().Contains("4") || row[9].ToString().Contains("5") || row[9].ToString().Contains("6") || row[9].ToString().Contains("7") || row[9].ToString().Contains("8") || row[9].ToString().Contains("9") || row[9].ToString().Contains("@") || row[9].ToString() == "")
                                {
                                    NombreError = true;
                                }

                                //--Errores en el Sexo--//
                                if (row[10].ToString() != valorSexo.ToString())
                                {
                                    SexoError = true;
                                }

                                //--Errores en la fecha de nacimiento--//
                                if (DateTime.TryParse(row[11].ToString(), out validaFecha))
                                {

                                    if (DateTime.Parse(row[11].ToString()) > DateTime.Today.AddYears(-18) || DateTime.Parse(row[11].ToString()) < new DateTime(1919, 1, 1, 0, 0, 0))
                                    {
                                        Fecha_NacimientoError = true;
                                    }
                                }
                                else
                                {
                                    Fecha_NacimientoError = true;
                                }

                                //--Errores en la Nacionalidad--//
                                if (row[12].ToString() != valorNacionalidad.ToString())
                                {
                                    NacionalidadError = true;
                                }

                                //--Errores en la Categoria--//
                                if (row[13].ToString() != valorCategoria.ToString())
                                {
                                    CategoriaError = true;
                                }

                                //--Errores en la Categoria - selección de codigo del trabajo--//
                                if (row[13].ToString() != "N/A" && row[18].ToString() != "Ley 19.378")
                                {
                                    ContratoCategoriaError = true;
                                }

                                //--Errores en la Profesión--//
                                if (row[14].ToString() != valorProfesion.ToString() && CategoriaError == false)
                                {
                                    ProfesionError = true;
                                }

                                //--Errores en la Especialidad--//
                                if (row[15].ToString() != valorEspecialidad.ToString() && ProfesionError == false)
                                {
                                    EspecialidadError = true;
                                }

                                //--Errores en el cargo--//
                                if (row[16].ToString() != valorCargo.ToString())
                                {
                                    CargoError = true;
                                }

                                //--Errores en la asignación de chofer--//
                                if (row[17].ToString() != valorChofer.ToString() && CargoError == false)
                                {
                                    ChoferError = true;
                                }

                                //--Errores en la Ley--//
                                if (row[18].ToString() != valorLey.ToString())
                                {
                                    LeyError = true;
                                }

                                //--Errores en el tipo de contrato--//
                                if (row[19].ToString() != valorContrato.ToString() && LeyError == false)
                                {
                                    ContratoError = true;
                                }

                                //--Errores en la jornada--//
                                if (int.TryParse(row[20].ToString(), out validaTexto))
                                {
                                    if (Int32.Parse(row[20].ToString()) < 1 || Int32.Parse(row[20].ToString()) > 44 && row[18].ToString() != "Honorario")
                                    {
                                        JornadaError = true;
                                    }
                                    else
                                    {
                                        //--Registro suma mas de 44 en la base de datos--//
                                        if (row[18].ToString() == "Ley 19.378" || row[18].ToString() == "Código del Trabajo")
                                        {
                                            if (int.Parse(MasJornada44.ToString()) + Int32.Parse(row[20].ToString()) > 44 && RepetidoError == false && JornadaError == false)
                                            {
                                                SumaJornadaBDError = true;
                                            }
                                            else
                                            {

                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                                else
                                {
                                    JornadaError = true;
                                }


                                    //--Errores en la fecha de ingreso--//
                                    if (DateTime.TryParse(row[21].ToString(), out validaFecha))
                                    {
                                        if (DateTime.Parse(row[21].ToString()) > DateTime.Today || DateTime.Parse(row[21].ToString()) < new DateTime(1973, 1, 1, 0, 0, 0))
                                        {
                                            FechaIngresoError = true;
                                        }
                                    }
                                    else
                                    {
                                        FechaIngresoError = true;
                                    }

                                    //--Errores en los años de servicio--//
                                    if (int.TryParse(row[22].ToString(), out validaTexto))
                                    {
                                        if (Int32.Parse(row[22].ToString()) < 0 || Int32.Parse(row[22].ToString()) > 60 || row[22].ToString() == "")
                                        {
                                            AñosServicioError = true;
                                        }
                                    }
                                    else
                                    {
                                        AñosServicioError = true;
                                    }

                                    //--Errores en los bienios--//
                                    if (int.TryParse(row[23].ToString(), out validaTexto))
                                    {
                                        if (Int32.Parse(row[23].ToString()) < 0 || Int32.Parse(row[23].ToString()) > 15 || row[23].ToString() == "")
                                        {
                                            BieniosError = true;
                                        }
                                    }
                                    else
                                    {
                                        BieniosError = true;
                                    }

                                    //--Errores en el nivel de la carrera--//
                                    if (int.TryParse(row[24].ToString(), out validaTexto))
                                    {
                                        if (Int32.Parse(row[24].ToString()) < 0 || Int32.Parse(row[24].ToString()) > 15)
                                        {
                                            Nivel_CarreraError = true;
                                        }
                                    }
                                    else
                                    {
                                        Nivel_CarreraError = true;
                                    }


                                    //--Errores en el tipo de previsión--//
                                    if (row[25].ToString() != valorTipo_Prevision.ToString())
                                    {
                                        Tipo_PrevisionError = true;
                                    }

                                    //--Errores en el tipo de previsión--//
                                    if (row[26].ToString() != valorTipo_Isapre.ToString())
                                    {
                                        Tipo_IsapreError = true;
                                    }


                                    //--Errores en la Base Comunal--//
                                    if (int.TryParse(row[27].ToString(), out validaTexto))
                                    {
                                        if (Int32.Parse(row[27].ToString()) < 0 || Int32.Parse(row[27].ToString()) > 90000000)
                                        {
                                            SBase_ComunalError = true;
                                        }
                                    }
                                    else
                                    {
                                        SBase_ComunalError = true;
                                    }

                                    //--Errores en la haberes--//
                                    if (int.TryParse(row[28].ToString(), out validaTexto))
                                    {
                                        if (Int32.Parse(row[28].ToString()) < 0 || Int32.Parse(row[28].ToString()) > 90000000)
                                        {
                                            HaberesError = true;
                                        }
                                    }
                                    else
                                    {
                                        HaberesError = true;
                                    }


                                //--muestra los mensajes de error--//
                                    if (RegistroEliminadoError == true)
                                    {
                                        ModelState.AddModelError("Error", "Entre los registros inactivos ya existe un registro que contiene la misma información - Fila " + contServicio);
                                    }

                                    if (RepetidoError == true)
                                    {
                                        ModelState.AddModelError("Error", "En lo datos almacenados, ya existe un registro que contiene el mismo contrato, para el mismo rut, en el mismo establecimiento - Fila " + contServicio);
                                    }
                                    if (SumaJornadaBDError == true)
                                    {
                                        ModelState.AddModelError("Error", "La suma de la jornada de este registro y de los que se encuentra almacenados,  exceden las 44 horas para el contrato proveniente de la Ley 19.738, para el mismo rut - Fila " + contSumaJornadaBD);
                                    }
                                    if (ServicioError == true)
                                    {
                                        ModelState.AddModelError("Error", "El servicio ingresado no existe - Columna B - Fila " + contServicio);
                                    }
                                    if (ComunaError == true)
                                    {
                                        ModelState.AddModelError("Error", "La comuna no existe o no pertenece al servicio ingresado - Columna C - Fila " + contComuna);
                                    }
                                    if (EstablecimientoError == true)
                                    {
                                        ModelState.AddModelError("Error", "El establecimiento no existe o no pertenece a la comuna ingresada - Columna D - Fila " + contEstablecimiento);
                                    }
                                    /*
                                    if (EstablecimientoMadreError == true)
                                    {
                                        ModelState.AddModelError("Error", "El establecimiento Madre no existe o no pertenece a la comuna ingresada - fila " + contEstablecimientoMadre);
                                    }
                                    if (TipoEstablecimientoError == true)
                                    {
                                        ModelState.AddModelError("Error", "El Tipo de establecimiento solo puede ser Rural o Urbano - fila " + contTipoEstablecimiento);
                                    }
                                    */
                                    if (AdministracionError == true)
                                    {
                                        ModelState.AddModelError("Error", "El dato ingresado en la columna de administración no es válido - Columna E - Fila " + contAdministracion);
                                    }
                                    if (RUTError == true)
                                    {
                                        ModelState.AddModelError("Error", "El rut esta vacio, tiene mas de 8 digitos, contiene puntos o empieza con 0 - Columna F - Fila " + contRUT);
                                    }
                                    if (DVError == true)
                                    {
                                        ModelState.AddModelError("Error", "El digito verificador esta vacio, contiene un valor mayor a 9 o una letra distinta a k - Columna G - Fila " + contDV);
                                    }
                                    if (APaternoError == true)
                                    {
                                        ModelState.AddModelError("Error", "El apellido paterno esta vacio o contiene un número - Columna H - Fila " + contAPaterno);
                                    }
                                    if (AMaternoError == true)
                                    {
                                        ModelState.AddModelError("Error", "El apellido materno esta vacio o contiene un número - Columna I - Fila " + contAMaterno);
                                    }
                                    if (NombreError == true)
                                    {
                                        ModelState.AddModelError("Error", "El nombre esta vacio o contiene un número - Columna J - Fila " + contNombre);
                                    }
                                    if (SexoError == true)
                                    {
                                        ModelState.AddModelError("Error", "El sexo contiene un valor distinto al listado permitido o está vacio - Columna K - Fila " + contSexo);
                                    }
                                    if (Fecha_NacimientoError == true)
                                    {
                                        ModelState.AddModelError("Error", "La fecha de nacimiento es anterior al 01-01-1919, menor a 18 años desde la fecha en la que ingresa el registro o está vacía - Columna L - Fila " + contFecha_Nacimiento);
                                    }
                                    if (NacionalidadError == true)
                                    {
                                        ModelState.AddModelError("Error", "La nacionalidad ingresada no esta incluido en el listado de nacionalidades, o se ingresó un espacio en blanco - Columna M - Fila " + contNacionalidad);
                                    }
                                    if (CategoriaError == true)
                                    {
                                        ModelState.AddModelError("Error", "La categoria ingresada no esta incluida en el listado de categorias, o se ingresó un espacio en blanco - Columna N - Fila " + contCategoria);
                                    }
                                    if (ContratoCategoriaError == true)
                                    {
                                        ModelState.AddModelError("Error", "Si especifica un contrato por código del trabajo u honorarios, en la categoría debe especificar N/A - Columna N - Fila " + contContratoCategoria);
                                    }
                                    if (ContratoProfesionError == true)
                                    {
                                        ModelState.AddModelError("Error", "Si especifica un contrato por código del trabajo u Honorarios, en la profesion debe especificar N/A  - Columna O - Fila " + contContratoProfesion);
                                    }
                                    if (ProfesionError == true)
                                    {
                                        ModelState.AddModelError("Error", "La profesión ingresada no esta incluida en el listado de profesiones, esta en blanco o no corresponde a la categoría ingresada - Columna O - Fila " + contProfesion);
                                    }
                                    if (EspecialidadError == true)
                                    {
                                        ModelState.AddModelError("Error", "La especialidad ingresada no esta incluida en el listado de especialidades, esta en blanco o no corresponde a la profesión ingresada - Columna P - Fila " + contEspecialidad);
                                    }
                                    if (CargoError == true)
                                    {
                                        ModelState.AddModelError("Error", "El cargo ingresado no esta incluido en el listado de cargos o está en blanco - Columna Q - Fila " + contCargo);
                                    }
                                    if (ChoferError == true)
                                    {
                                        ModelState.AddModelError("Error", "La Asignación de chofer ingresada no esta incluida en el listado, esta en blanco o no corresponde al cargo ingresado - Columna R - Fila " + contChofer);
                                    }
                                    if (LeyError == true)
                                    {
                                        ModelState.AddModelError("Error", "La contratación ingresada no esta incluida en el listado o está en blanco - Columna S - Fila " + contLey);
                                    }
                                    if (ContratoError == true)
                                    {
                                        ModelState.AddModelError("Error", "El tipo de contrato no esta incluido en la lista, está en blanco o no corresponde a la contratación ingresada - Columna T - Fila " + contContrato);
                                    }
                                    if (JornadaError == true)
                                    {
                                        ModelState.AddModelError("Error", "Se ingresó mas de 44 horas de jornada o la jornada está en blanco - Columna U - Fila " + contJornada);
                                    }
                                    if (FechaIngresoError == true)
                                    {
                                        ModelState.AddModelError("Error", "La fecha de ingreso es posterior a la fecha actual o está en blanco - Columna V - Fila " + contFechaIngreso);
                                    }
                                    if (AñosServicioError == true)
                                    {
                                        ModelState.AddModelError("Error", "La cantidad de años de servicio tiene decimales, es menor a 0, mayor a 60 años o se dejaron en blanco - Columna W - Fila " + contAñosServicio);
                                    }
                                    if (BieniosError == true)
                                    {
                                        ModelState.AddModelError("Error", "La cantidad de bienios tiene decimales, es menor a 0, mayor a 15 años o se dejaron en blanco - Columna X - Fila " + contBienios);
                                    }
                                    if (Nivel_CarreraError == true)
                                    {
                                        ModelState.AddModelError("Error", "Los años de carrera funcionaria tiene decimales, es menor a 1, mayor a 15 años o se dejaron en blanco - Columna Y - Fila " + contNivel_Carrera);
                                    }
                                    if (SBase_ComunalError == true)
                                    {
                                        ModelState.AddModelError("Error", "La base comunal esta vacia, es negativa o mayor a 90.000.000 $ - Columna AB - Fila " + contSBase_Comunal);
                                    }
                                    if (HaberesError == true)
                                    {
                                        ModelState.AddModelError("Error", "Los haberes estan vacios, es negativo o mayor a 90.000.000 $ - Columna AC - Fila " + contHaberes);
                                    }
                                    if (Tipo_PrevisionError == true)
                                    {
                                        ModelState.AddModelError("Error", "La previsión ingresada no esta incluida en el listado o está en blanco - Columna Z - Fila  " + contTipo_Prevision);
                                    }
                                    if (Tipo_IsapreError == true)
                                    {
                                        ModelState.AddModelError("Error", "La isapre ingresada no esta incluida en el listado o está en blanco - Columna AA - Fila " + contTipo_Isapre);
                                    }
                                    if (RepetidoExcelError == true)
                                    {
                                        ModelState.AddModelError("Error", "En este archivo de carga ya existe un registro que contiene el mismo contrato, para el mismo rut, en el mismo establecimiento - Fila " + contRepetidoExcel);
                                    }
                                    if (SumaJornadaError == true)
                                    {
                                        ModelState.AddModelError("Error", "Al incluir este registro en el archivo de carga, la jornada excede las 44 horas para el contrato proveniente de la Ley 19.738, para el mismo rut - Fila " + contSumaJornada);
                                    }

                                    if (SumaJornadaError == true || SumaJornadaBDError == true || RepetidoExcelError == true || RepetidoError == true || ServicioError == true || ComunaError == true || EstablecimientoError == true || AdministracionError == true || RUTError == true || DVError == true || AMaternoError == true || APaternoError == true || NombreError == true || SexoError == true || Fecha_NacimientoError == true || NacionalidadError == true || CategoriaError == true || ProfesionError == true || EspecialidadError == true || CargoError == true || LeyError == true || ContratoError == true || ChoferError == true || JornadaError == true || FechaIngresoError == true || AñosServicioError == true || BieniosError == true || Nivel_CarreraError == true || SBase_ComunalError == true || HaberesError == true || Tipo_PrevisionError == true || Tipo_IsapreError == true)
                                    {
                                        CargaError = true;
                                    }

                                    SumaJornadaBDError = false;
                                    SumaJornadaError = false;
                                    RepetidoExcelError = false;
                                    RepetidoError = false;
                                    ServicioError = false;
                                    ComunaError = false;
                                    EstablecimientoError = false;
                                    //EstablecimientoMadreError = false;
                                    //TipoEstablecimientoError = false;
                                    AdministracionError = false;
                                    RUTError = false;
                                    DVError = false;
                                    APaternoError = false;
                                    AMaternoError = false;
                                    NombreError = false;
                                    SexoError = false;
                                    Fecha_NacimientoError = false;
                                    NacionalidadError = false;
                                    CategoriaError = false;
                                    ContratoCategoriaError = false;
                                    ContratoProfesionError = false;
                                    ProfesionError = false;
                                    EspecialidadError = false;
                                    CargoError = false;
                                    LeyError = false;
                                    ContratoError = false;
                                    ChoferError = false;
                                    JornadaError = false;
                                    FechaIngresoError = false;
                                    AñosServicioError = false;
                                    BieniosError = false;
                                    Nivel_CarreraError = false;
                                    SBase_ComunalError = false;
                                    HaberesError = false;
                                    Tipo_PrevisionError = false;
                                    Tipo_IsapreError = false;
                                    RegistroEliminadoError = false;
                            }

                            //--Ejecuta la carga--//
                            foreach (DataRow row in resultado.Rows)
                            {
                                conn.Open();
                                //verifica si el registro está eliminado (activo = 1)//
                                string EliminadoID = "SELECT ISNULL((SELECT SUM([Activo]) FROM [dbo].[DOTACION_Registros] where [ID_Establecimiento] = (Select [CodigoNuevo] FROM [dbo].[DOTACION_Establecimientos] where Establecimiento = '" + row[3].ToString() + "' and [Id_Comuna1] = (SELECT [ID_Comuna1] FROM [dbo].[DOTACION_Comunas] where [Comuna] ='" + row[2].ToString() + "')) and [ID_Servicio] = (SELECT [ID_Servicio] FROM [dbo].[DOTACION_Servicios] where Servicio = '" + row[1].ToString() + "') and [Id_Comuna] = (SELECT [ID_Comuna1] FROM [dbo].[DOTACION_Comunas] where [Comuna] ='" + row[2].ToString() + "') and [Administracion] = '" + row[4].ToString() + "' and [Rut] = '" + row[5].ToString() + "' and [DV] = '" + row[6].ToString() + "' and [Apellido_Paterno] = upper('" + row[7].ToString() + "') and [Apellido_Materno] = upper('" + row[8].ToString() + "') and [Nombre] = upper('" + row[9].ToString() + "') and [Sexo] = '" + row[10].ToString() + "' and [Fecha_Nacimiento] ='" + row[11].ToString() + "' and [Nacionalidad] = '" + row[12].ToString() + "' and [Categoria] = '" + row[13].ToString() + "' and [Profesion] = '" + row[14].ToString() + "' and [Especialidad] = '" + row[15].ToString() + "' and [Cargo] = '" + row[16].ToString() + "' and [Funciones_Chofer] = '" + row[17].ToString() + "' and [Ley] = '" + row[18].ToString() + "' and [Tipo_contrato] = '" + row[19].ToString() + "' and [Jornada] = '" + row[20].ToString() + "' and [Fecha_Ingreso] = '" + row[21].ToString() + "' and [Anos_Servicio] = '" + row[22].ToString() + "' and [Bienios] = '" + row[23].ToString() + "' and [Nivel_Carrera] = '" + row[24].ToString() + "' and [Tipo_Prevision] = '" + row[25].ToString() + "' and [Tipo_Isapre] = '" + row[26].ToString() + "' and [SBase_Comunal] = '" + row[27].ToString() + "' and [Haberes] = '" + row[28].ToString() + "' and [Activo] = 1),0)";
                                SqlCommand cmdEliminado = new SqlCommand(EliminadoID, conn);
                                var RegistroEliminado = cmdEliminado.ExecuteScalar();

                                //--Registro repetido en la base de datos--//
                                if (Int32.Parse(RegistroEliminado.ToString())!= 0)
                                {
                                    RegistroEliminadoError = true;
                                }

                                if (CargaError == false && RegistroEliminadoError == false)
                                {

                                string query = "INSERT INTO [dbo].[DOTACION_Registros]  ([ID_Servicio] ,[ID_Comuna] ,[ID_Establecimiento] ,[Administracion] ,[Rut] ,[DV] ,[Apellido_Paterno] ,[Apellido_Materno] ,[Nombre] ,[Sexo] ,[Fecha_Nacimiento] ,[Nacionalidad] ,[Categoria] ,[Profesion] ,[Especialidad] ,[Cargo] ,[Funciones_Chofer] ,[Ley] ,[Tipo_contrato] ,[Jornada] ,[Fecha_Ingreso] ,[Anos_Servicio] ,[Bienios] ,[Nivel_Carrera] ,[Tipo_Prevision] ,[Tipo_Isapre] ,[SBase_Comunal] ,[Haberes] ,[Fecha_Carga] ,[usuario],Validado, Activo, Ingreso_registro, Revisado) VALUES((SELECT [ID_Servicio] FROM [dbo].[DOTACION_Servicios] where Servicio = '" + row[1].ToString() + "'),(SELECT [ID_Comuna1] FROM [dbo].[DOTACION_Comunas] WHERE Comuna ='" + row[2].ToString() + "'),(SELECT [CodigoNuevo] FROM [dbo].[DOTACION_Establecimientos] where [Establecimiento] = '" + row[3].ToString() + "'),'" + row[4].ToString() + "','" + row[5].ToString() + "','" + row[6].ToString() + "',upper('" + row[7].ToString() + "'),upper('" + row[8].ToString() + "'),upper('" + row[9].ToString() + "'),'" + row[10].ToString() + "','" + row[11].ToString() + "','" + row[12].ToString() + "','" + row[13].ToString() + "','" + row[14].ToString() + "','" + row[15].ToString() + "','" + row[16].ToString() + "','" + row[17].ToString() + "','" + row[18].ToString() + "','" + row[19].ToString() + "','" + row[20].ToString() + "','" + row[21].ToString() + "','" + row[22].ToString() + "','" + row[23].ToString() + "','" + row[24].ToString() + "','" + row[25].ToString() + "','" + row[26].ToString() + "','" + row[27].ToString() + "','" + row[28].ToString() + "', getdate(),@Usuario, 0,0,'Carga Masiva',0)";
                                SqlCommand cmd = new SqlCommand(query, conn);
                                cmd.Parameters.AddWithValue("@Usuario", User.Identity.Name);
                                cmd.ExecuteReader();
                                cont++;
                                }
                                else
                                {

                                }

                                RegistroEliminadoError = false;
                                conn.Close();

                            }
                        }
                    }
                }
            }

            ModelState.AddModelError("Error", "Se ha ingresado " + cont + " Registro/s");
            return View();
        }
    }
}

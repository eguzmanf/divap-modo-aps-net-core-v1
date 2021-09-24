using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotacionWEBCore.Helpers.Perfil;
using DotacionWEBCore.Helpers.String.StringCase;
using DotacionWEBCore.Models;
using DotacionWEBCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using ClosedXML;
using Humanizer;
using System.Globalization;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;

namespace DotacionWEBCore.Controllers
{
    public class EditaRegistrosController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IFlashMessage _flashMessage;
        private readonly IPerfilUsuario _perfil;
        private readonly StringCase _titleCase;
        private readonly string _perfilNombre;
        private readonly int _perfilId;

        private readonly IConfiguration configuration;

        public EditaRegistrosController(DatabaseContext context, IFlashMessage flashMessage, IPerfilUsuario perfil, StringCase stringCase, IConfiguration config)
        {
            _context = context;
            _flashMessage = flashMessage;
            _perfil = perfil;
            _titleCase = stringCase;

            _perfilNombre = _perfil.PerfilNombre();
            _perfilId = _perfil.PerfilId();

            configuration = config;
        }

        [HttpGet]
        public IActionResult EditaRegistros(string ID)
        {
            ViewBag.Rol = _perfilNombre;
            ViewBag.RolId = _perfilId.ToString();

            var idRegistro = _context.DOTACION_Registros_resultados.Where(w => w.ID == ID).Select(s => s.ID_Registro).First();

            var registro = _context.DOTACION_Registros_resultados.Where(w => w.ID_Registro == idRegistro).First();

            EditaRegistrosViewModel vm = new EditaRegistrosViewModel();
            // Datos del Establecimiento
            //
            vm.ID_Registro = registro.ID_Registro;
            vm.ID = registro.ID;
            vm.ID_Servicio = registro.ID_Servicio;
            vm.ServiciosList = ObtenerServicioPorId(registro.ID_Servicio);
            vm.ID_Comuna = registro.ID_Comuna;
            vm.ComunasList = ObtenerComunaPorId(registro.ID_Comuna, registro.ID_Servicio);
            vm.ID_Establecimiento = registro.ID_Establecimiento;
            vm.EstablecimientoList = ObtenerEstablecimientosPorComunaId(registro.ID_Comuna);
            vm.Administracion = registro.Administracion;
            vm.AdministracionList = ObtenerAdministracion();
            // Datos del funcionario
            //
            vm.Rut = registro.Rut.Trim();
            vm.DV = registro.DV;
            vm.DigitoVerificadorList = ObtenerDigitoVerificador();
            vm.Apellido_Paterno = registro.Apellido_Paterno.Trim();
            vm.Apellido_Materno = registro.Apellido_Materno.Trim();
            vm.Nombre = registro.Nombre.Trim();
            vm.Sexo = registro.Sexo;
            vm.GeneroList = ObtenerGenero();
            vm.Fecha_Nacimiento = registro.Fecha_Nacimiento;
            vm.Nacionalidad = registro.Nacionalidad;
            vm.NacionalidadList = ObtenerNacionalidad();
            // Datos del funcionario
            //
            vm.Ley = registro.Ley;
            vm.LeyList = ObtenerLey();
            vm.Tipo_contrato = registro.Tipo_contrato;
            vm.TipoContratoList = ObtenerTipoContratoPorLey(registro.Ley, registro.Tipo_contrato);
            vm.Categoria = registro.Categoria;
            vm.CategoriaList = ObtenerCategoriaPorLey(registro.Ley, registro.Categoria);
            vm.Nivel_Carrera = registro.Nivel_Carrera;
            vm.NivelCarreraList = ObtenerNivelCarreraPorLey(registro.Ley, registro.Nivel_Carrera);
            vm.Profesion = registro.Profesion;
            vm.ProfesionList = ObtenerProfesionPorLey(registro.Ley, registro.Categoria, registro.Profesion);
            vm.Especialidad = registro.Especialidad;
            vm.EspecialidadList = ObtenerEspecialidadSiEsMedico(registro.Profesion);
            vm.Cargo = registro.Cargo;
            vm.CargoList = ObtenerCargo();
            vm.Funciones_Chofer = registro.Funciones_Chofer;
            vm.FuncionesChoferList = ObtenerFuncionesChofer(registro.Cargo);
            //
            vm.Jornada = registro.Jornada;
            vm.Anos_Servicio = registro.Anos_Servicio.Trim();
            vm.Fecha_Ingreso = registro.Fecha_Ingreso;
            vm.Bienios = registro.Bienios;
            vm.BieniosList = ObtenerBienios();
            vm.Tipo_Prevision = registro.Tipo_Prevision;
            vm.TipoPrevisionList = ObtenerTipoPrevision();
            vm.Tipo_Isapre = registro.Tipo_Isapre;
            vm.TipoIsapreList = ObtenerTipoIsapre();
            vm.SBase_Comunal = registro.SBase_Comunal.Trim();
            vm.Haberes = registro.Haberes.Trim();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditaRegistros(string ID_Registro, string ID, string DV, [Bind("ID_Registro, ID, ID_Servicio, ID_Comuna, ID_Establecimiento, Administracion, Rut, DV, Apellido_Paterno, Apellido_Materno, Nombre, Sexo, Fecha_Nacimiento, Nacionalidad, Ley, Tipo_contrato, Categoria, Nivel_Carrera, Profesion, Especialidad, Cargo, Funciones_Chofer, Jornada, Anos_Servicio, Fecha_Ingreso, Bienios, Tipo_Prevision, Tipo_Isapre, SBase_Comunal, Haberes")] EditaRegistrosViewModel vm)
        {
            var idRegistro = Int32.Parse(ID_Registro);

            vm.SBase_Comunal = vm.SBase_Comunal.Replace(".", "");
            vm.Haberes = vm.Haberes.Replace(".", "");

            if(idRegistro != vm.ID_Registro)
            {
                _flashMessage.Danger("Error: El número identificador del registro enviado no coincide: " + idRegistro + " vs " + vm.ID_Registro);
                return RedirectToAction(nameof(EditaRegistros));
            }

            if(ID != vm.ID)
            {
                _flashMessage.Danger("Error: El número ID enviado no coincide: " + ID + " vs " + vm.ID);
                return RedirectToAction(nameof(EditaRegistros));
            }

            if (ModelState.IsValid)
            {
                var jornadaMayor44Ley = _context.DOTACION_Registros_resultados.Where(w => w.Rut == vm.Rut && (w.Ley == "Ley 19.378" || w.Ley == "Código del Trabajo") && w.Activo == 0).Select(s => s.Jornada).Sum();
                jornadaMayor44Ley = jornadaMayor44Ley != null ? jornadaMayor44Ley : 0;

                var jornadaMayor44Edit = _context.DOTACION_Registros_resultados.Where(w => w.Rut == vm.Rut && w.ID == ID && w.Activo == 0).Sum(s => s.Jornada);
                jornadaMayor44Edit = jornadaMayor44Edit != null ? jornadaMayor44Edit : 0;

                var jornadaMayor44Resta = jornadaMayor44Ley - jornadaMayor44Edit;

                var sumaJornadas = jornadaMayor44Resta + vm.Jornada;

                if(vm.Ley == "Ley 19.378" || vm.Ley == "Código del Trabajo")
                {
                    if (sumaJornadas > 44) { 
                        //
                        _flashMessage.Danger("Error: Los contratos Indefinidos, a plazo fijo, de reemplazo y/o por código del trabajo del Run ingresado, exceden las 44 horas de jornada laboral (se toman en cuenta todos los contratos en el país)");
                        _flashMessage.Warning("No se ha actualizado el Registro");
                        return RedirectToAction(nameof(EditaRegistros));
                    }
                }

                /* ########################################################################################## */
                string Connstr3 = configuration.GetConnectionString("DefaultConnection");
                SqlConnection conn3 = new SqlConnection(Connstr3);
                conn3.Open();

                string query3 = "INSERT INTO.[dbo].[DOTACION_Registros_cambios]([ID_Servicio],[ID_Comuna],[ID_Establecimiento],[ID_Establecimiento_Madre],[Tipo_Establecimiento],[Administracion],[Rut],[DV],[Apellido_Paterno],[Apellido_Materno],[Nombre],[Sexo],[Fecha_Nacimiento],[Nacionalidad],[Categoria],[Profesion],[Especialidad],[Cargo],[Funciones_Chofer],[Ley],[Tipo_contrato],[Jornada],[Fecha_Ingreso],[Anos_Servicio],[Bienios],[Nivel_Carrera],[SBase_Comunal],[Haberes],[Tipo_Prevision],[Tipo_Isapre],[Fecha_Carga],[usuario],[Validado],[Activo],[Fecha_Validacion],[usuario_validador],[Ingreso_registro],[Revisado],[Fecha_Revision],[usuario_revisor],[ID_Registro_Original]) (select[ID_Servicio],[ID_Comuna],[ID_Establecimiento],[ID_Establecimiento_Madre],[Tipo_Establecimiento],[Administracion],[Rut],[DV],[Apellido_Paterno],[Apellido_Materno],[Nombre],[Sexo],[Fecha_Nacimiento],[Nacionalidad],[Categoria],[Profesion],[Especialidad],[Cargo],[Funciones_Chofer],[Ley],[Tipo_contrato],[Jornada],[Fecha_Ingreso],[Anos_Servicio],[Bienios],[Nivel_Carrera],[SBase_Comunal],[Haberes],[Tipo_Prevision],[Tipo_Isapre],[Fecha_Carga],[usuario],[Validado],[Activo],[Fecha_Validacion],[usuario_validador],[Ingreso_registro],[Revisado],[Fecha_Revision],[usuario_revisor],[ID_Registro] from.[dbo].[DOTACION_Registros] WHERE ID_Registro = " + idRegistro + ")";
                SqlCommand cmd3 = new SqlCommand(query3, conn3);
                cmd3.ExecuteReader();
                conn3.Close();
                /* ########################################################################################## */

                try
                {
                    var registro = _context.DOTACION_Registros.Find(vm.ID_Registro);

                    if (registro != null) 
                    {
                        registro.ID_Servicio = vm.ID_Servicio;
                        registro.ID_Comuna = vm.ID_Comuna;
                        registro.ID_Establecimiento = vm.ID_Establecimiento;
                        registro.Administracion = vm.Administracion;
                        //
                        registro.Rut = vm.Rut.Trim();
                        registro.DV = vm.DV.ToUpper().Trim();
                        registro.Apellido_Paterno = vm.Apellido_Paterno.ToUpper().Trim();
                        registro.Apellido_Materno = vm.Apellido_Materno.ToUpper().Trim();
                        registro.Nombre = vm.Nombre.ToUpper().Trim();
                        registro.Sexo = vm.Sexo;
                        registro.Fecha_Nacimiento = vm.Fecha_Nacimiento;
                        registro.Nacionalidad = vm.Nacionalidad;
                        //
                        registro.Ley = vm.Ley;
                        registro.Tipo_contrato = vm.Tipo_contrato;
                        registro.Categoria = vm.Categoria;
                        registro.Nivel_Carrera = vm.Nivel_Carrera;
                        registro.Profesion = vm.Profesion;
                        registro.Especialidad = vm.Especialidad;
                        registro.Cargo = vm.Cargo;
                        registro.Funciones_Chofer = vm.Funciones_Chofer;
                        registro.Jornada = vm.Jornada;
                        registro.Anos_Servicio = vm.Anos_Servicio.Trim();
                        registro.Fecha_Ingreso = vm.Fecha_Ingreso;
                        registro.Bienios = vm.Bienios;
                        registro.Tipo_Prevision = vm.Tipo_Prevision;
                        registro.Tipo_Isapre = vm.Tipo_Isapre;
                        registro.SBase_Comunal = vm.SBase_Comunal.Trim();
                        registro.Haberes = vm.Haberes.Trim();
                        registro.usuario = User.Identity.Name;

                        _context.Update(registro);
                        _context.SaveChanges();

                        _flashMessage.Confirmation("Se ha editado al funcionario " + vm.Nombre + " " + vm.Apellido_Paterno + " " + vm.Apellido_Materno + " [" + vm.Rut + "-" + vm.DV + "] exitosamente!");
                    }
                    else
                    {
                        _flashMessage.Danger("Error: Los registros asociados al funcionario no existen en la base de datos!");
                    }
                }
                catch(DbUpdateConcurrencyException) {
                    //
                    _flashMessage.Danger("Error: Ha ocurrido una excepción del tipo DbUpdateConcurrencyException, al parecer debido a que los registros asociados al funcionario no existe en la base de datos!");
                    return RedirectToAction(nameof(EditaRegistros));
                }

                return RedirectToAction(nameof(EditaRegistros));
            }

            _flashMessage.Danger("Favor informar de este problema para ser analizado y corregido!");

            return View(vm);
        }

        /* ############################################################################################################################################################# */

        private List<SelectListItem> ObtenerListaServicios()
        {
            return _context.DOTACION_Servicios.OrderBy(u => u.Servicio)
                                              .Select(u => new SelectListItem
                                              {
                                                  Value = u.ID_Servicio.ToString(),
                                                  Text = u.Servicio
                                              }).ToList();
        }

        private List<SelectListItem> ObtenerServicioPorId(int id)
        {
            var servicio = new List<SelectListItem>();

            if (_perfilId == 1 || _perfilId == 2)
            {
                servicio = _context.DOTACION_Servicios.Where(w => w.ID_Servicio == id).OrderBy(o => o.Servicio).Select(s => new SelectListItem 
                { 
                    Value = s.ID_Servicio.ToString(),
                    Text = s.Servicio

                }).ToList();

            } 
            else if(_perfilId == 3)
            {
                servicio = _context.DOTACION_Servicios.Where(w => w.ID_Servicio != 25).OrderBy(u => u.Servicio)     // menos Aisén
                                              .Select(u => new SelectListItem
                                              {
                                                  Value = u.ID_Servicio.ToString(),
                                                  Text = u.Servicio

                                              }).ToList();
            }
            
            return servicio;
        }

        private List<SelectListItem> ObtenerComunaPorId(int idComuna, int idServicio)
        {
            var comuna = new List<SelectListItem>();

            if(_perfilId == 2)
            {
                comuna = _context.DOTACION_Comunas.Where(w => w.ID_Comuna1 == idComuna).Select(s => new SelectListItem { 
            
                    Value = s.ID_Comuna1.ToString(),
                    Text = s.Comuna

                }).OrderBy(o => o.Text).ToList();
            } 
            else if (_perfilId == 1 || _perfilId == 3)
            {
                comuna = _context.DOTACION_Comunas.Where(w => w.ID_Servicio == idServicio).Select(s => new SelectListItem
                {

                    Value = s.ID_Comuna1.ToString(),
                    Text = s.Comuna

                }).OrderBy(o => o.Text).ToList();
            }
            

            return comuna;
        }

        private List<SelectListItem> ObtenerEstablecimientosPorComunaId(int id)
        {
            var establecimientos = new List<SelectListItem>();

            establecimientos = _context.DOTACION_Establecimientos.Where(w => w.ID_Comuna1 == id).Select(s => new SelectListItem
            {
                Value = s.CodigoNuevo.ToString(),
                Text = s.Establecimiento

            }).OrderBy(o => o.Text).ToList();

            return establecimientos;
        }

        private List<SelectListItem> ObtenerAdministracion()
        {
            var admin = new List<SelectListItem>();

            admin = _context.DOTACION_Administracion.Select(s => new SelectListItem
            {
                Value = s.Administracion,
                Text = s.Administracion

            }).OrderBy(o => o.Text).ToList();

            return admin;
        }

        private List<SelectListItem> ObtenerDigitoVerificador()
        {
            var dv = new List<SelectListItem>();

            dv = _context.DOTACION_DV.Select(s => new SelectListItem
            {
                Value = s.DV,
                Text = s.DV

            }).OrderBy(o => o.Text).ToList();

            return dv;
        }

        private List<SelectListItem> ObtenerGenero()
        {
            return _context.DOTACION_Sexo.OrderBy(o => o.Sexo).Select(s => new SelectListItem
            {
                Value = s.Sexo,
                Text = s.Sexo

            }).ToList();
        }

        private List<SelectListItem> ObtenerNacionalidad()
        {
            return _context.DOTACION_Nacionalidad.OrderBy(o => o.Nacionalidad).Select(s => new SelectListItem
            {
                Value = s.Nacionalidad,
                Text = s.Nacionalidad
            }).ToList();
        }

        private List<SelectListItem> ObtenerLey()
        {
            return _context.DOTACION_Ley.OrderBy(o => o.Ley).Select(s => new SelectListItem
            {
                Value = s.Ley,
                Text = s.Ley
            }).ToList();
        }

        private List<SelectListItem> ObtenerTipoContratoPorLey(String ley, String tipoContrato)
        {
            var contrato = new List<SelectListItem>();

            if(ley == "Ley 19.378")
            {
                contrato = _context.DOTACION_Contrato.Where(w => w.Ley == ley).Select(s => new SelectListItem
                {
                    Value = s.Tipo_contrato,
                    Text = s.Tipo_contrato

                }).OrderBy(o => o.Text).ToList();
            } 
            else
            {
                contrato = _context.DOTACION_Contrato.Where(w => w.Ley == ley && w.Tipo_contrato == tipoContrato).Select(s => new SelectListItem
                {
                    Value = s.Tipo_contrato,
                    Text = s.Tipo_contrato

                }).OrderBy(o => o.Text).ToList();
            }
            

            return contrato;
        }

        private List<SelectListItem> ObtenerCategoriaPorLey(String ley, String cat)
        {
            var categoria = new List<SelectListItem>();

            if (ley == "Ley 19.378")
            {
                categoria = _context.DOTACION_Categoria.Where(w => w.IdCategoria != 7 && w.Categoria != "N/A").Select(s => new SelectListItem
                {
                    Value = s.Categoria,
                    Text = s.Categoria

                }).OrderBy(o => o.Text).ToList();
            }
            else
            {
                categoria = _context.DOTACION_Categoria.Where(w => w.Categoria == cat).Select(s => new SelectListItem
                {
                    Value = s.Categoria,
                    Text = s.Categoria

                }).OrderBy(o => o.Text).ToList();
            }

            return categoria;
        }

        private List<SelectListItem> ObtenerNivelCarreraPorLey(String ley, String nivelCarrerra)
        {
            var carrera = new List<SelectListItem>();

            if (ley == "Ley 19.378")
            {
                carrera = _context.DOTACION_Nivel_Carrera.Where(w => w.IdNivel_Carrera != 16 && w.Nivel_Carrera != "0").Select(s => new SelectListItem
                {
                    Value = s.Nivel_Carrera,
                    Text = s.Nivel_Carrera

                }).OrderBy(o => o.Text.Length).ThenBy(t => t.Text).ToList();
            }
            else
            {
                carrera = _context.DOTACION_Nivel_Carrera.Where(w => w.Nivel_Carrera == "0").Select(s => new SelectListItem
                {
                    Value = "N/A",
                    Text = "N/A"

                }).OrderBy(o => o.Text.Length).ThenBy(t => t.Text).ToList();
            }

            return carrera;
        }

        private List<SelectListItem> ObtenerProfesionPorLey(String ley, String categoria, String prof)
        {
            var profesion = new List<SelectListItem>();

            if (ley == "Ley 19.378")
            {
                profesion = _context.DOTACION_Profesion.Where(w => w.Categoria == categoria).Select(s => new SelectListItem
                {
                    Value = s.Profesion,
                    Text = s.Profesion

                }).OrderBy(o => o.Text).ToList();
            }
            else
            {
                profesion = _context.DOTACION_Profesion.Where(w => w.Categoria == "N/A").Select(s => new SelectListItem
                {
                    Value = s.Profesion,
                    Text = s.Profesion

                }).OrderBy(o => o.Text).ToList();
            }

            return profesion;
        }

        private List<SelectListItem> ObtenerEspecialidadSiEsMedico(String profesion)
        {
            var especialidad = new List<SelectListItem>();

            if (profesion == "MEDICO")
            {
                // especialidad = _context.DOTACION_Especialidad.Where(w => w.Especialidad != "N/A" && w.Profesion == "MEDICO").Select(s => new SelectListItem
                especialidad = _context.DOTACION_Especialidad.Where(w => w.Profesion == "MEDICO").Select(s => new SelectListItem
                {
                    Value = s.Especialidad,
                    Text = s.Especialidad

                }).OrderBy(o => o.Text).ToList();
            }
            else
            {
                especialidad = _context.DOTACION_Especialidad.Where(w => w.Especialidad == "N/A" && w.Profesion == profesion).Select(s => new SelectListItem
                {
                    Value = s.Especialidad,
                    Text = s.Especialidad

                }).OrderBy(o => o.Text).ToList();
            }

            return especialidad;
        }

        private List<SelectListItem> ObtenerCargo()
        {
            var cargo = new List<SelectListItem>();

            cargo = _context.DOTACION_Cargo.Select(s => new SelectListItem
            {
                Value = s.Cargo,
                Text = s.Cargo

            }).OrderBy(o => o.Text).ToList();

            return cargo;
        }

        private List<SelectListItem> ObtenerFuncionesChofer( String cargo)
        {
            var funcionesChofer = new List<SelectListItem>();

            if(cargo == "CHOFER")
            {
                funcionesChofer = _context.DOTACION_Chofer.Where( w => w.Cargo == "CHOFER" ).Select(s => new SelectListItem
                {
                    Value = s.Funciones_Chofer,
                    Text = s.Funciones_Chofer

                }).OrderBy(o => o.Text).ToList();
            } 
            else
            {
                funcionesChofer = _context.DOTACION_Chofer.Where(w => w.Cargo != "CHOFER" && w.Cargo == cargo && w.Funciones_Chofer == "N/A").Select(s => new SelectListItem
                {
                    Value = s.Funciones_Chofer,
                    Text = s.Funciones_Chofer

                }).OrderBy(o => o.Text).ToList();
            }
            

            return funcionesChofer;
        }

        private List<SelectListItem> ObtenerBienios()
        {
            var bienios = new List<SelectListItem>();

            bienios = _context.DOTACION_Bienios.Select(s => new SelectListItem
            {
                Value = s.Bienios,
                Text = s.Bienios

            }).OrderBy(o => o.Text.Length).ThenBy(t => t.Text).ToList();

            return bienios;
        }

        private List<SelectListItem> ObtenerTipoPrevision()
        {
            var prevision = new List<SelectListItem>();

            prevision = _context.DOTACION_Tipo_prevision.Select(s => new SelectListItem
            {
                Value = s.Tipo_Prevision,
                Text = s.Tipo_Prevision

            }).OrderBy(o => o.Text).ToList();

            return prevision;
        }

        private List<SelectListItem> ObtenerTipoIsapre()
        {
            var isapre = new List<SelectListItem>();

            isapre = _context.DOTACION_Tipo_Isapre.Select(s => new SelectListItem
            {
                Value = s.Tipo_Isapre,
                Text = s.Tipo_Isapre

            }).OrderBy(o => o.Text).ToList();

            return isapre;
        }

        /* ##################################################################################################################################################################### */

        [HttpPost]
        public JsonResult GetComunasByServicioChange([FromBody] FetchServicioComunaCargaSimpleViewModel model)
        {
            var idServicio = Convert.ToInt32(model.idServicio);

            var comuna = _context.DOTACION_Comunas.Where(w => w.ID_Servicio == idServicio).Select(s => new SelectListItem
            {
                Value = s.ID_Comuna1.ToString(),
                Text = s.Comuna
            }).OrderBy(o => o.Text).ToList();

            return Json(comuna);
        }

        [HttpPost]
        public JsonResult GetEstablecimientosByComunasChange([FromBody] FetchComunaEstablecimientoCargaSimpleViewModel model)
        {
            var idComuna = int.Parse(model.idComuna);

            var establecimiento = _context.DOTACION_Establecimientos.Where(w => w.ID_Comuna1 == idComuna).Select(u => new SelectListItem
            {
                Value = u.CodigoNuevo.ToString(),
                Text = u.Establecimiento
            }).OrderBy(o => o.Text).ToList();

            return Json(establecimiento);
        }

        [HttpPost]
        public JsonResult GetTipoContrato([FromBody] FetchContratoCargaSimpleViewModel model)
        {
            var Ley = model.Ley;
            var contrato = _context.DOTACION_Contrato.Where(c => c.Ley == Ley).Select(u => new SelectListItem
            {
                Value = u.Tipo_contrato,
                Text = u.Tipo_contrato
            }).OrderBy(o => o.Text).ToList();

            return Json(contrato);
        }

        [HttpPost]
        public JsonResult GetTipoCategorias([FromBody] FetchCategoriaCargaSimpleViewModel model)
        {
            var Ley = model.Ley;
            var categoria = new List<SelectListItem>();

            if (Ley == "Ley 19.378")
            {
                categoria = _context.DOTACION_Categoria.Where(w => w.Categoria != "N/A").Select(s => new SelectListItem
                {
                    Value = s.Categoria,
                    Text = s.Categoria
                }).OrderBy(o => o.Text).ToList();

            }
            else
            {
                categoria = _context.DOTACION_Categoria.Where(w => w.Categoria == "N/A").Select(s => new SelectListItem
                {
                    Value = s.Categoria,
                    Text = s.Categoria
                }).OrderBy(o => o.Text).ToList();
            }


            return Json(categoria);
        }

        [HttpPost]
        public JsonResult GetNivelCarrera([FromBody] FetchNivelCarreraCargaSimpleViewModel model)
        {
            var Ley = model.Ley;
            var niveCarrera = new List<SelectListItem>();

            if (Ley == "Ley 19.378")
            {
                niveCarrera = _context.DOTACION_Nivel_Carrera.Where(w => w.Nivel_Carrera != "0").Select(s => new SelectListItem
                {
                    Value = s.Nivel_Carrera,
                    Text = s.Nivel_Carrera

                }).OrderBy(o => o.Text.Length).ThenBy(t => t.Text).ToList();

            }
            else
            {
                niveCarrera = _context.DOTACION_Nivel_Carrera.Where(w => w.Nivel_Carrera == "0").Select(s => new SelectListItem
                {
                    Value = s.Nivel_Carrera,
                    Text = s.Nivel_Carrera

                }).OrderBy(o => o.Text.Length).ThenBy(t => t.Text).ToList();
            }

            return Json(niveCarrera);
        }

        [HttpPost]
        public JsonResult GetProfesion([FromBody] FetchCategoriaProfesionCargaSimpleViewModel model)
        {
            var Ley = model.Ley;
            var idCategoria = model.idCategoria;
            var profesion = new List<SelectListItem>();

            if (Ley == "Ley 19.378")
            {
                profesion = _context.DOTACION_Profesion.Where(w => w.Categoria != "N/A" && w.Categoria == idCategoria).Select(s => new SelectListItem
                {
                    Value = s.Profesion,
                    Text = s.Profesion
                }).OrderBy(o => o.Text).ToList();

            }
            else
            {
                profesion = _context.DOTACION_Profesion.Where(w => w.Categoria == "N/A" && w.IdCategoria == 7).Select(s => new SelectListItem
                {
                    Value = s.Profesion,
                    Text = s.Profesion
                }).OrderBy(o => o.Text).ToList();
            }

            return Json(profesion);
        }

        [HttpPost]
        public JsonResult GetTipoCategoriaChange([FromBody] FetchCategoriaProfesionCargaSimpleViewModel model)
        {
            var Ley = model.Ley;
            var idCategoria = model.idCategoria;
            // var profesion = new List<SelectListItem>();

            var profesion = _context.DOTACION_Profesion.Where(w => w.Categoria == model.idCategoria).Select(u => new SelectListItem
            {
                Value = u.Profesion,
                Text = u.Profesion
            }).OrderBy(o => o.Text).ToList();

            return Json(profesion);
        }

        [HttpPost]
        public JsonResult GetTipoEspecialidadProfesionChange([FromBody] FetchProfesionEspecialidadCargaSimpleViewModel model)
        {
            var especialidad = new List<SelectListItem>();

            if (model.idProfesion == "MEDICO")
            {
                especialidad = _context.DOTACION_Especialidad.Where(w => w.Profesion == "MEDICO").Select(u => new SelectListItem
                {
                    Value = u.Especialidad,
                    Text = u.Especialidad
                }).OrderBy(o => o.Text).ToList();

            }
            else if (model.idProfesion == "PROFESION")
            {
                especialidad = _context.DOTACION_Profesion.Where(w => w.Profesion == "PROFESION").Select(u => new SelectListItem
                {
                    Value = u.Categoria,
                    Text = u.Categoria
                }).OrderBy(o => o.Text).ToList();

            }
            else
            {
                especialidad = _context.DOTACION_Especialidad.Where(w => w.Profesion != "MEDICO" && w.Profesion == model.idProfesion).Select(u => new SelectListItem
                {
                    Value = u.Especialidad,
                    Text = u.Especialidad
                }).OrderBy(o => o.Text).ToList();
            }

            return Json(especialidad);
        }

        [HttpPost]
        public JsonResult GetAsignacionChoferAmbulancia([FromBody] FetchCargoAsignacionChoferCargaSimpleViewModel model)
        {
            var chofer = new List<SelectListItem>();

            if (model.idCargo == "CHOFER")
            {
                chofer = _context.DOTACION_Chofer.Where(w => w.Cargo == "CHOFER").Select(u => new SelectListItem
                {
                    Value = u.Funciones_Chofer,
                    Text = u.Funciones_Chofer
                }).OrderByDescending(o => o.Text).ToList();

            }
            else
            {
                chofer = _context.DOTACION_Chofer.Where(w => w.Cargo != "CHOFER" && w.Cargo == model.idCargo).Select(u => new SelectListItem
                {
                    Value = u.Funciones_Chofer,
                    Text = u.Funciones_Chofer
                }).OrderBy(o => o.Text).ToList();
            }

            return Json(chofer);
        }

        /* ##################################################################################################################################################################### */
    }
}
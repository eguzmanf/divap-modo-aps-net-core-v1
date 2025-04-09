using DotacionWEBCore.Helpers.Rut.V1;
using DotacionWEBCore.Helpers.Rut.V2;
using DotacionWEBCore.Models;
using DotacionWEBCore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using Vereyon.Web;

namespace DotacionWEBCore.Controllers
{
    public class CargasimpleController : Controller
    {

        // ------- definiendo conexion a base de datos ------- //
        private readonly IConfiguration configuration;
        private readonly DatabaseContext _context;
        private static Boolean Jornada44Error; Boolean ServicioError; Boolean ComunaError; Boolean EstablecimientoError; Boolean AdministracionError; Boolean RUTError; Boolean DVError; Boolean APaternoError; Boolean AMaternoError; Boolean NombreError; Boolean SexoError; Boolean Fecha_NacimientoError; Boolean NacionalidadError; Boolean CategoriaError; Boolean ContratoCategoriaError; Boolean ContratoProfesionError; Boolean ProfesionError; Boolean EspecialidadError; Boolean CargoError; Boolean LeyError; Boolean ContratoError; Boolean ChoferError; Boolean JornadaError; Boolean FechaIngresoError; Boolean AñosServicioError; Boolean BieniosError; Boolean Nivel_CarreraError; Boolean SBase_ComunalError; Boolean HaberesError; Boolean Tipo_PrevisionError; Boolean Tipo_IsapreError; Boolean CargaError; Boolean RegistroEliminadoError;
        private int Servicio_usuario_lista; int Comuna_usuario_lista; int Perfil_U;

        private readonly IFlashMessage _flashMessage;

        private readonly IVerificaRutHelpersV1 _rutv1;
        private readonly IVerificaRutHelpersV2 _rutv2;

        // ------- definiendo conexion a base de datos ------- //
        public CargasimpleController(DatabaseContext context, IConfiguration config, IFlashMessage flashMessage, IVerificaRutHelpersV1 verificaRutHelpersV1, IVerificaRutHelpersV2 verificaRutHelpersV2)
        {
            _context = context;
            configuration = config;
            _flashMessage = flashMessage;
            _rutv1 = verificaRutHelpersV1;
            _rutv2 = verificaRutHelpersV2;
        }

        public IActionResult Cargasimple()
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            if (ViewBag.OpcionesServicios != null)
            {

            }
            else
            {

                //--conecta a SQL--//
                string Connstr = configuration.GetConnectionString("DefaultConnection");
                SqlConnection conn = new SqlConnection(Connstr);
                conn.Open();

                //Obtiene id del servicio//
                string Servicio_usuario = "SELECT TOP 1 [IdServicio] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
                SqlCommand cmdServicio = new SqlCommand(Servicio_usuario, conn);
                var SQLServicio = cmdServicio.ExecuteScalar();
                Servicio_usuario_lista = Int32.Parse(SQLServicio.ToString());

                //// ------- Setting Data back to ViewBag after Posting Form ------- //
                List<ListaServicios> ListadoServicios = new List<ListaServicios>();
                if (Perfil_U == 1 || Perfil_U == 2)
                {
                    //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
                    ListadoServicios = (from Servicios in _context.DOTACION_Servicios
                                        where Servicios.ID_Servicio == Servicio_usuario_lista
                                        orderby Servicios.IdOrdenServicio
                                        select Servicios).ToList();
                }  else if(Perfil_U == 3)
                {
                    ListadoServicios = (from Servicios in _context.DOTACION_Servicios
                                        where Servicios.ID_Servicio != 25       // menos Aisén
                                        orderby Servicios.IdOrdenServicio
                                        select Servicios).OrderBy(e => e.Servicio).ToList();
                }
                

                //Obtiene ID_Comuna del registro//
                string Comuna_usuario = "SELECT TOP 1 [ID_Comuna_U] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
                SqlCommand cmdComuna = new SqlCommand(Comuna_usuario, conn);
                var SQLComuna = cmdComuna.ExecuteScalar();
                Comuna_usuario_lista = Int32.Parse(SQLComuna.ToString());

                conn.Close();

                /*
                // Carga comunas según perfil //
                if (Perfil_U == 1)
                {
                    List<ListaComunas> ListadoComunas = new List<ListaComunas>();

                    //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
                    ListadoComunas = (from ComunasBD in _context.DOTACION_Comunas
                                      where ComunasBD.ID_Servicio == Servicio_usuario_lista
                                      orderby ComunasBD.Comuna
                                      select ComunasBD).ToList();

                    //--------------Insertando select item en la lista --------------
                    ListadoComunas.Insert(0, new ListaComunas { ID_Comuna1 = 0, Comuna = "" });

                    //--------------Asignando Categorylist a viewBag.ListofCategory --------------

                    ViewBag.OpcionesComunas = ListadoComunas.Distinct();
                    // return Json(new SelectList(ListadoComunas, "ID_Comuna1", "Comuna"));


                }
                else
                {
                    List<ListaComunas> ListadoComunas = new List<ListaComunas>();

                    //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
                    ListadoComunas = (from ComunasBD in _context.DOTACION_Comunas
                                  where ComunasBD.ID_Comuna1 == Comuna_usuario_lista
                                  orderby ComunasBD.Comuna
                                  select ComunasBD).ToList();
                    ViewBag.OpcionesComunas = ListadoComunas.Distinct();


                }
                */

                List<ListaEstablecimientos> EstablecimientoList = new List<ListaEstablecimientos>();
                EstablecimientoList = (from EstablecimientosBD in _context.DOTACION_Establecimientos
                                       select EstablecimientosBD).ToList();
                EstablecimientoList.Insert(0, new ListaEstablecimientos { Establecimiento = "" });


                List<ListaAdministracion> listadoAdministracion = new List<ListaAdministracion>();
                listadoAdministracion = (from Administracion in _context.DOTACION_Administracion
                                         select Administracion).ToList();
                listadoAdministracion.Insert(0, new ListaAdministracion { Administracion = "" });


                List<ListaDV> ListadoDV = new List<ListaDV>();
                ListadoDV = (from DV in _context.DOTACION_DV
                             select DV).ToList();
                ListadoDV.Insert(0, new ListaDV { DV = "" });


                List<ListaSexo> ListadoSexo = new List<ListaSexo>();
                ListadoSexo = (from Sexo in _context.DOTACION_Sexo
                               select Sexo).ToList();
                ListadoSexo.Insert(0, new ListaSexo { Sexo = "" });


                List<ListaNacionalidad> ListadoNacionalidad = new List<ListaNacionalidad>();
                ListadoNacionalidad = (from Nacionalidad in _context.DOTACION_Nacionalidad
                                       select Nacionalidad).ToList();
                ListadoNacionalidad.Insert(0, new ListaNacionalidad { Nacionalidad = "" });


                List<ListaCategorias> listadoCategorias = new List<ListaCategorias>();
                listadoCategorias = (from Categoria in _context.DOTACION_Categoria
                                     select Categoria).ToList();
                listadoCategorias.Insert(0, new ListaCategorias { Categoria = "" });


                List<ListaCargo> ListadoCargo = new List<ListaCargo>();
                ListadoCargo = (from Cargo in _context.DOTACION_Cargo
                                orderby Cargo.Cargo
                                select Cargo).ToList();
                ListadoCargo.Insert(0, new ListaCargo { Cargo = "" });


                List<ListaLey> ListadoLey = new List<ListaLey>();
                ListadoLey = (from Ley in _context.DOTACION_Ley
                              select Ley).ToList();
                ListadoLey.Insert(0, new ListaLey { Ley = "" });


                List<ListaAñosServicio> ListadoAnosServicio = new List<ListaAñosServicio>();
                ListadoAnosServicio = (from Anos_Servicio in _context.DOTACION_Anos_Servicio
                                       select Anos_Servicio).ToList();
                ListadoAnosServicio.Insert(0, new ListaAñosServicio { Anos_Servicio = "" });


                List<ListaBienios> ListadoBienios = new List<ListaBienios>();
                ListadoBienios = (from Bienios in _context.DOTACION_Bienios
                                  select Bienios).ToList();
                ListadoBienios.Insert(0, new ListaBienios { Bienios = "" });


                List<ListaNivelCarrera> ListadoNivelCarrera = new List<ListaNivelCarrera>();
                ListadoNivelCarrera = (from Nivel_Carrera in _context.DOTACION_Nivel_Carrera
                                       select Nivel_Carrera).ToList();
                ListadoNivelCarrera.Insert(0, new ListaNivelCarrera { Nivel_Carrera = "" });


                List<ListaTipoPrevision> ListadoTipoPrevision = new List<ListaTipoPrevision>();
                ListadoTipoPrevision = (from Tipo_Prevision in _context.DOTACION_Tipo_prevision
                                        select Tipo_Prevision).ToList();
                ListadoTipoPrevision.Insert(0, new ListaTipoPrevision { Tipo_Prevision = "" });


                List<ListaTipoIsapre> ListadoTipoIsapre = new List<ListaTipoIsapre>();
                ListadoTipoIsapre = (from Tipo_Isapre in _context.DOTACION_Tipo_Isapre
                                     select Tipo_Isapre).ToList();
                ListadoTipoIsapre.Insert(0, new ListaTipoIsapre { Tipo_Isapre = "" });

                //--------------Asignando Categorylist a viewBag.ListofCategory --------------

                ViewBag.OpcionesServicios = ListadoServicios.Distinct();

                //ViewBag.OpcionesTipo_Establecimiento = listadoTipoEstablecimiento.Distinct();
                ViewBag.OpcionesAdministracion = listadoAdministracion.Distinct();
                ViewBag.OpcionesDV = ListadoDV.Distinct();
                ViewBag.OpcionesSexo = ListadoSexo.Distinct();
                ViewBag.OpcionesNacionalidad = ListadoNacionalidad.Distinct();
                ViewBag.OpcionesCategorias = listadoCategorias.Distinct();
                ViewBag.OpcionesCargo = ListadoCargo.Distinct();
                ViewBag.OpcionesLey = ListadoLey.Distinct();
                ViewBag.OpcionesAnosServicio = ListadoAnosServicio.Distinct();
                ViewBag.OpcionesBienios = ListadoBienios.Distinct();
                ViewBag.OpcionesNivelCarrera = ListadoNivelCarrera.Distinct();
                ViewBag.OpcionesTipoIsapre = ListadoTipoIsapre.Distinct();
                ViewBag.OpcionesTipoPrevision = ListadoTipoPrevision.Distinct();

                var stringArray = new string[21] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                ViewBag.ArrayRequest = stringArray;
                ViewBag.TieneError = 0;
            }

            return View();
        }

        /*
            public JsonResult GetComunas(int ID_Servicio)
            {
                //--conecta a SQL--//
                string Connstr = configuration.GetConnectionString("DefaultConnection");
                SqlConnection conn = new SqlConnection(Connstr);
                conn.Open();

                //Obtiene perfil del usuario//
                string Perfil = "SELECT TOP 1 [ID_Perfil] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
                SqlCommand cmdPerfil = new SqlCommand(Perfil, conn);
                var SQLPerfil = cmdPerfil.ExecuteScalar();
                Perfil_U = Int32.Parse(SQLPerfil.ToString());

                //Obtiene id del servicio//
                string Servicio_usuario = "SELECT TOP 1 [IdServicio] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
                SqlCommand cmdServicio = new SqlCommand(Servicio_usuario, conn);
                var SQLServicio = cmdServicio.ExecuteScalar();
                Servicio_usuario_lista = Int32.Parse(SQLServicio.ToString());

                //Obtiene id de la comuna//
                string Comuna_usuario = "SELECT TOP 1 [ID_Comuna_U] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
                SqlCommand cmdComuna = new SqlCommand(Comuna_usuario, conn);
                var SQLComuna = cmdComuna.ExecuteScalar();
                Comuna_usuario_lista = Int32.Parse(SQLComuna.ToString());

                conn.Close();

                // Carga comunas según perfil //
                if (Perfil_U == 1)
                {
                    List<ListaComunas> ListadoComunas = new List<ListaComunas>();

                    //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
                    ListadoComunas = (from ComunasBD in _context.DOTACION_Comunas
                                  where ComunasBD.ID_Servicio == Servicio_usuario_lista
                                  orderby ComunasBD.Comuna
                                  select ComunasBD).ToList();

                    //--------------Insertando select item en la lista --------------
                    ListadoComunas.Insert(0, new ListaComunas { ID_Comuna1 = 0, Comuna = "" });

                    //--------------Asignando Categorylist a viewBag.ListofCategory --------------
                    return Json(new SelectList(ListadoComunas, "ID_Comuna1", "Comuna"));
                }
                else
                {
                    List<ListaComunas> ListadoComunas = new List<ListaComunas>();

                    //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
                    ListadoComunas = (from ComunasBD in _context.DOTACION_Comunas
                                  where ComunasBD.ID_Comuna1 == Comuna_usuario_lista
                                  orderby ComunasBD.Comuna
                                  select ComunasBD).ToList();

                    //--------------Asignando Categorylist a viewBag.ListofCategory --------------
                    return Json(new SelectList(ListadoComunas, "ID_Comuna1", "Comuna"));
                }

            }
        */

        /*
            public JsonResult GetEstablecimientos(int ID_Comuna)
            {
                List<ListaEstablecimientos> EstablecimientoList = new List<ListaEstablecimientos>();

                //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
                EstablecimientoList = (from EstablecimientosBD in _context.DOTACION_Establecimientos
                                       where EstablecimientosBD.ID_Comuna1 == ID_Comuna
                                       orderby EstablecimientosBD.Establecimiento
                                       select EstablecimientosBD).ToList();

                //--------------Insertando select item en la lista --------------
                EstablecimientoList.Insert(0, new ListaEstablecimientos { CodigoNuevo = 0, Establecimiento = "" });

                //--------------Asignando Categorylist a viewBag.ListofCategory --------------
                return Json(new SelectList(EstablecimientoList, "CodigoNuevo", "Establecimiento"));
            }
        */

        /*    
            public JsonResult GetEstablecimientosMadres(int ID_Comuna, int Codigonuevo)
            {
                List<ListaEstablecimientosMadres> EstablecimientoMadreList = new List<ListaEstablecimientosMadres>();

                //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
                EstablecimientoMadreList = (from EstablecimientosBD in _context.DOTACION_EstablecimientosMadre
                                       where EstablecimientosBD.ID_Comuna == ID_Comuna && EstablecimientosBD.CodigoNuevoMadre != Codigonuevo
                                            orderby EstablecimientosBD.EstablecimientoMadre
                                       select EstablecimientosBD).ToList();

                //--------------Insertando select item en la lista --------------
                EstablecimientoMadreList.Insert(0, new ListaEstablecimientosMadres { CodigoNuevoMadre = 0, EstablecimientoMadre = "No Aplica" });

                //--------------Asignando Categorylist a viewBag.ListofCategory --------------
                return Json(new SelectList(EstablecimientoMadreList, "CodigoNuevoMadre", "EstablecimientoMadre"));
            }
        */

        /*
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
        */

        /*
            public JsonResult GetAsignacionChofer(String Cargo)
            {
                List<ListaChofer> ListadoChofer = new List<ListaChofer>();
                ListadoChofer = (from Chofer in _context.DOTACION_Chofer
                                 where Chofer.Cargo == Cargo
                                 select Chofer).ToList();

                ListadoChofer.Insert(0, new ListaChofer { Funciones_Chofer = "" });

                return Json(new SelectList(ListadoChofer, "Funciones_Chofer", "Funciones_Chofer").Distinct());
            }
        */

        /*
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
        */

        /*
        public JsonResult GetContrato(String Ley)
        {
            List<ListaContratos> ContratosList = new List<ListaContratos>();

            if (Ley == "Ley 19.378") {
                //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
                ContratosList = (from ContratosBD in _context.DOTACION_Contrato
                                 where ContratosBD.Ley == Ley
                                 orderby ContratosBD.Tipo_contrato
                                 select ContratosBD).ToList();

                //--------------Insertando select item en la lista --------------
                ContratosList.Insert(0, new ListaContratos { Tipo_contrato = "Seleccionar..." });
            } else
            {
                ContratosList = (from ContratosBD in _context.DOTACION_Contrato
                                 where ContratosBD.Ley == Ley
                                 orderby ContratosBD.Tipo_contrato
                                 select ContratosBD).ToList();
            }
            //--------------Asignando Categorylist a viewBag.ListofCategory --------------
            return Json(new SelectList(ContratosList, "Tipo_contrato", "Tipo_contrato").Distinct());
        }
        */

        // POST: Cargasimple/GetComunasByServicioChange
        [HttpPost]
        public JsonResult GetComunasByServicioChange([FromBody] FetchServicioComunaCargaSimpleViewModel model)
        {
            var idServicio = Convert.ToInt32(model.idServicio);

            var comuna = _context.DOTACION_Comunas.Where(w => w.ID_Servicio == idServicio).Select(s => new SelectListItem { 
            
                Value = s.ID_Comuna1.ToString(),
                Text = s.Comuna
            }).OrderBy(o => o.Text).ToList();

            return Json(comuna);
        }

        // POST: Cargasimple/GetComunasDelServicio
        [HttpPost]
        public JsonResult GetComunasDelServicio([FromBody] FetchServicioComunaCargaSimpleViewModel model)
        {

            /*
             * var id = int.Parse(model.idServicio);
                var comunaDelServicio = _context.DOTACION_servicio_comuna.Where(w => w.IdServicio == id).Select(u => new SelectListItem
                {
                    Value = u.IdComuna.ToString(),
                    Text = u.Comuna
                }).OrderBy(o => o.Text).ToList();
            */
            var comunaDelServicio = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => new SelectListItem
            {
                Value = b.ID_Comuna_U.ToString(),
                Text = _context.DOTACION_Comunas.Where(w => w.ID_Comuna1 == b.ID_Comuna_U).Select(s => s.Comuna).First()

            }).OrderBy(o => o.Text).ToList();

            return Json(comunaDelServicio);
        }

        // POST: Cargasimple/GetComunasListSelect
        [HttpPost]
        public JsonResult GetComunasListSelect ([FromBody] FetchServicioComunaCargaSimpleViewModel model)
        {
            var idPerfilUsuario = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            var idServicioUsuario = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.IdServicio).First();
            var idComunaUsuario = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Comuna_U).First();
            var comunas= new List<SelectListItem>();

            if (idPerfilUsuario == 1)
            {
                comunas = _context.DOTACION_Comunas.Where(w => w.ID_Servicio == idServicioUsuario).Select(u => new SelectListItem
                {
                    Value = u.ID_Comuna1.ToString(),
                    Text = u.Comuna
                }).OrderBy(o => o.Text).ToList();

            }
            else if (idPerfilUsuario == 2)
            {
                comunas = _context.DOTACION_Comunas.Where(w => w.ID_Comuna1 == idComunaUsuario).Select(u => new SelectListItem
                {
                    Value = u.ID_Comuna1.ToString(),
                    Text = u.Comuna
                }).OrderBy(o => o.Text).ToList();

            } 
            else if(idPerfilUsuario == 3) 
            {
                comunas = _context.DOTACION_Comunas.Where(w => w.ID_Servicio == int.Parse(model.idServicio)).Select(u => new SelectListItem
                {
                    Value = u.ID_Comuna1.ToString(),
                    Text = u.Comuna
                }).OrderBy(o => o.Text).ToList();
            }

            return Json(comunas);
        }

        // POST: Cargasimple/GetEstablecimientosListSelect
        [HttpPost]
        public JsonResult GetEstablecimientosListSelect([FromBody] FetchComunaEstablecimientoCargaSimpleViewModel model)
        {
            var establecimiento = _context.DOTACION_Establecimientos.Where(w => w.ID_Comuna1 == int.Parse(model.idComuna)).Select(u => new SelectListItem { 
                Value = u.CodigoNuevo.ToString(),
                Text = u.Establecimiento
            }).OrderBy(o => o.Text).ToList();

            return Json(establecimiento);
        }

        // POST: Cargasimple/GetTipoCategoriaChange
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

        // POST: Cargasimple/GetTipoEspecialidadProfesionChange
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

            } else if(model.idProfesion == "PROFESION")
            {
                especialidad = _context.DOTACION_Profesion.Where(w => w.Profesion == "PROFESION").Select(u => new SelectListItem
                {
                    Value = u.Categoria,
                    Text = u.Categoria
                }).OrderBy(o => o.Text).ToList();

            } else
            {
                especialidad = _context.DOTACION_Especialidad.Where(w => w.Profesion != "MEDICO" && w.Profesion == model.idProfesion).Select(u => new SelectListItem
                {
                    Value = u.Especialidad,
                    Text = u.Especialidad
                }).OrderBy(o => o.Text).ToList();
            }

            return Json(especialidad);
        }

        // POST: Cargasimple/GetAsignacionChoferAmbulancia
        [HttpPost]
        public JsonResult GetAsignacionChoferAmbulancia([FromBody] FetchCargoAsignacionChoferCargaSimpleViewModel model)
        {
            var chofer = new List<SelectListItem>();

            if(model.idCargo == "CHOFER")
            {
                chofer = _context.DOTACION_Chofer.Where(w => w.Cargo == "CHOFER").Select(u => new SelectListItem
                {
                    Value = u.Funciones_Chofer,
                    Text = u.Funciones_Chofer
                }).OrderByDescending(o => o.Text).ToList();

            } else
            {
                chofer = _context.DOTACION_Chofer.Where(w => w.Cargo != "CHOFER" && w.Cargo == model.idCargo).Select(u => new SelectListItem
                {
                    Value = u.Funciones_Chofer,
                    Text = u.Funciones_Chofer
                }).OrderBy(o => o.Text).ToList();
            }

            return Json(chofer);
        }

        // POST: Cargasimple/GetTipoContrato
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

        // POST: Cargasimple/GetTipoCategorias
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

            } else
            {
                categoria = _context.DOTACION_Categoria.Where(w => w.Categoria == "N/A").Select(s => new SelectListItem
                {
                    Value = s.Categoria,
                    Text = s.Categoria
                }).OrderBy(o => o.Text).ToList();
            }
            

            return Json(categoria);
        }

        // POST: Cargasimple/GetNivelCarrera
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
            
            } else
            {
                niveCarrera = _context.DOTACION_Nivel_Carrera.Where(w => w.Nivel_Carrera == "0").Select(s => new SelectListItem
                {
                    Value = s.Nivel_Carrera,
                    Text = s.Nivel_Carrera

                }).OrderBy(o => o.Text.Length).ThenBy(t => t.Text).ToList();
            }

            return Json(niveCarrera);
        }

        // POST Cargasimple/GetProfesion
        [HttpPost]
        public JsonResult GetProfesion([FromBody] FetchCategoriaProfesionCargaSimpleViewModel model)
        {
            var Ley = model.Ley;
            var idCategoria = model.idCategoria;
            var profesion = new List<SelectListItem>();

            if(Ley == "Ley 19.378")
            {
                profesion = _context.DOTACION_Profesion.Where(w => w.Categoria != "N/A" && w.Categoria == idCategoria).Select(s => new SelectListItem
                {
                    Value = s.Profesion,
                    Text = s.Profesion
                }).OrderBy(o => o.Text).ToList();

            } else
            {
                profesion = _context.DOTACION_Profesion.Where(w => w.Categoria == "N/A" && w.IdCategoria == 7 && w.IdProfesion != 22).Select(s => new SelectListItem
                {
                    Value = s.Profesion,
                    Text = s.Profesion
                }).OrderBy(o => o.Text).ToList();
            }

            return Json(profesion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cargasimple(Registros registros)
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            Jornada44Error = false;
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
            NacionalidadError = false;
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

            if (ViewBag.OpcionesServicios != null)

            {

            }

            else
            {

                //--conecta a SQL--//
                string Connstr = configuration.GetConnectionString("DefaultConnection");
                SqlConnection conn = new SqlConnection(Connstr);
                conn.Open();

                //Obtiene id del servicio//
                string Servicio_usuario = "SELECT TOP 1 [IdServicio] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
                SqlCommand cmdServicio = new SqlCommand(Servicio_usuario, conn);
                var SQLServicio = cmdServicio.ExecuteScalar();
                Servicio_usuario_lista = Int32.Parse(SQLServicio.ToString());

                /*
                //// ------- Setting Data back to ViewBag after Posting Form ------- //
                List<ListaServicios> ListadoServicios = new List<ListaServicios>();
                //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
                ListadoServicios = (from Servicios in _context.DOTACION_Servicios
                                    where Servicios.ID_Servicio == Servicio_usuario_lista
                                    orderby Servicios.IdOrdenServicio
                                    select Servicios).ToList();
                */

                //// ------- Setting Data back to ViewBag after Posting Form ------- //
                List<ListaServicios> ListadoServicios = new List<ListaServicios>();
                if (Perfil_U == 1 || Perfil_U == 2)
                {
                    //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
                    ListadoServicios = (from Servicios in _context.DOTACION_Servicios
                                        where Servicios.ID_Servicio == Servicio_usuario_lista
                                        orderby Servicios.IdOrdenServicio
                                        select Servicios).ToList();
                }
                else if (Perfil_U == 3)
                {
                    ListadoServicios = (from Servicios in _context.DOTACION_Servicios
                                        orderby Servicios.IdOrdenServicio
                                        select Servicios).ToList();
                }

                //Obtiene ID_Comuna del registro//
                string Comuna_usuario = "SELECT TOP 1 [ID_Comuna_U] FROM .[dbo].[AspNetUsers] WHERE [UserName] = '" + User.Identity.Name + "'";
                SqlCommand cmdComuna = new SqlCommand(Comuna_usuario, conn);
                var SQLComuna = cmdComuna.ExecuteScalar();
                Comuna_usuario_lista = Int32.Parse(SQLComuna.ToString());

                conn.Close();

                List<ListaComunas> ListadoComunas = new List<ListaComunas>();
                //--------------obteniendo datos desde la base de datos usando entity/frameworkCore --------------
                ListadoComunas = (from ComunasBD in _context.DOTACION_Comunas
                                  where ComunasBD.ID_Comuna1 == Comuna_usuario_lista
                                  orderby ComunasBD.Comuna
                                  select ComunasBD).ToList();


                /*    List<ListaTipoEstablecimiento> listadoTipoEstablecimiento = new List<ListaTipoEstablecimiento>();
                    listadoTipoEstablecimiento = (from Tipo_Establecimiento in _context.DOTACION_Tipo_Establecimiento
                                                  select Tipo_Establecimiento).ToList();
                    listadoTipoEstablecimiento.Insert(0, new ListaTipoEstablecimiento { Tipo_Establecimiento = "" });
                */

                List<ListaAdministracion> listadoAdministracion = new List<ListaAdministracion>();
                listadoAdministracion = (from Administracion in _context.DOTACION_Administracion
                                         select Administracion).ToList();
                listadoAdministracion.Insert(0, new ListaAdministracion { Administracion = "" });

               
                List<ListaDV> ListadoDV = new List<ListaDV>();
                ListadoDV = (from DV in _context.DOTACION_DV
                             select DV).ToList();
                ListadoDV.Insert(0, new ListaDV { DV = "" });


                List<ListaSexo> ListadoSexo = new List<ListaSexo>();
                ListadoSexo = (from Sexo in _context.DOTACION_Sexo
                               select Sexo).ToList();
                ListadoSexo.Insert(0, new ListaSexo { Sexo = "" });


                List<ListaNacionalidad> ListadoNacionalidad = new List<ListaNacionalidad>();
                ListadoNacionalidad = (from Nacionalidad in _context.DOTACION_Nacionalidad
                                       select Nacionalidad).ToList();
                ListadoNacionalidad.Insert(0, new ListaNacionalidad { Nacionalidad = "" });


                List<ListaCategorias> listadoCategorias = new List<ListaCategorias>();
                listadoCategorias = (from Categoria in _context.DOTACION_Categoria
                                     select Categoria).ToList();
                listadoCategorias.Insert(0, new ListaCategorias { Categoria = "" });


                List<ListaCargo> ListadoCargo = new List<ListaCargo>();
                ListadoCargo = (from Cargo in _context.DOTACION_Cargo
                                select Cargo).ToList();
                ListadoCargo.Insert(0, new ListaCargo { Cargo = "" });


                List<ListaLey> ListadoLey = new List<ListaLey>();
                ListadoLey = (from Ley in _context.DOTACION_Ley
                              select Ley).ToList();
                ListadoLey.Insert(0, new ListaLey { Ley = "" });


                List<ListaContratos> ListadoContratos = new List<ListaContratos>();
                ListadoContratos = (from Contratos in _context.DOTACION_Contrato
                                    select Contratos).ToList();
                ListadoContratos.Insert(0, new ListaContratos { Tipo_contrato = "" });


                List<ListaChofer> ListadoChofer = new List<ListaChofer>();
                ListadoChofer = (from Chofer in _context.DOTACION_Chofer
                                 select Chofer).ToList();
                ListadoChofer.Insert(0, new ListaChofer { Funciones_Chofer = "" });


                List<ListaAñosServicio> ListadoAnosServicio = new List<ListaAñosServicio>();
                ListadoAnosServicio = (from Anos_Servicio in _context.DOTACION_Anos_Servicio
                                       select Anos_Servicio).ToList();
                ListadoAnosServicio.Insert(0, new ListaAñosServicio { Anos_Servicio = "" });


                List<ListaBienios> ListadoBienios = new List<ListaBienios>();
                ListadoBienios = (from Bienios in _context.DOTACION_Bienios
                                       select Bienios).ToList();
                ListadoBienios.Insert(0, new ListaBienios { Bienios = "" });


                List<ListaNivelCarrera> ListadoNivelCarrera = new List<ListaNivelCarrera>();
                ListadoNivelCarrera = (from Nivel_Carrera in _context.DOTACION_Nivel_Carrera
                                       select Nivel_Carrera).ToList();
                ListadoNivelCarrera.Insert(0, new ListaNivelCarrera { Nivel_Carrera = "" });


                List<ListaTipoPrevision> ListadoTipoPrevision = new List<ListaTipoPrevision>();
                ListadoTipoPrevision = (from Tipo_Prevision in _context.DOTACION_Tipo_prevision
                                        select Tipo_Prevision).ToList();
                ListadoTipoPrevision.Insert(0, new ListaTipoPrevision { Tipo_Prevision = "" });


                List<ListaTipoIsapre> ListadoTipoIsapre = new List<ListaTipoIsapre>();
                ListadoTipoIsapre = (from Tipo_Isapre in _context.DOTACION_Tipo_Isapre
                                     select Tipo_Isapre).ToList();
                ListadoTipoIsapre.Insert(0, new ListaTipoIsapre { Tipo_Isapre = "" });

                //--------------Asignando Categorylist a viewBag.ListofCategory --------------

                ViewBag.OpcionesServicios = ListadoServicios.Distinct();
                ViewBag.OpcionesComunas = ListadoComunas.Distinct();
                //ViewBag.OpcionesTipo_Establecimiento = listadoTipoEstablecimiento.Distinct();
                ViewBag.OpcionesAdministracion = listadoAdministracion.Distinct();
                ViewBag.OpcionesDV = ListadoDV.Distinct();
                ViewBag.OpcionesSexo = ListadoSexo.Distinct();
                ViewBag.OpcionesNacionalidad = ListadoNacionalidad.Distinct();
                ViewBag.OpcionesCategorias = listadoCategorias.Distinct();
                ViewBag.OpcionesCargo = ListadoCargo.Distinct();
                ViewBag.OpcionesLey = ListadoLey.Distinct();
                ViewBag.OpcionesAnosServicio = ListadoAnosServicio.Distinct();
                ViewBag.OpcionesBienios = ListadoBienios.Distinct();
                ViewBag.OpcionesNivelCarrera = ListadoNivelCarrera.Distinct();
                ViewBag.OpcionesTipoIsapre = ListadoTipoIsapre.Distinct();
                ViewBag.OpcionesTipoPrevision = ListadoTipoPrevision.Distinct();

                var stringArray = new string[21] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                ViewBag.ArrayRequest = stringArray;
                ViewBag.TieneError = 0;
            }



            //// ------- Validation ------- //

                if (HttpContext.Request.Form["ID_Servicio"].ToString() == "" && HttpContext.Request.Form["ID_Comuna"].ToString() == "" && HttpContext.Request.Form["CodigoNuevo"].ToString() == "" && HttpContext.Request.Form["Ley"].ToString() == "" && HttpContext.Request.Form["Categoria"].ToString() == "")
                {
                    ModelState.AddModelError("0", "Error");
                }
                if (HttpContext.Request.Form["ID_Servicio"].ToString() == "")
                {
                    ServicioError = true;
                    ModelState.AddModelError("0", "Seleccione un servicio");
                }


                if (HttpContext.Request.Form["ID_Comuna"].ToString() == "")
                {
                    ComunaError = true;
                    ModelState.AddModelError("0", "Seleccione una comuna");
                }


                if (HttpContext.Request.Form["CodigoNuevo"].ToString() == "")
                {
                    EstablecimientoError = true;
                    ModelState.AddModelError("0", "Seleccione un establecimiento");
                }

                /*           if (HttpContext.Request.Form["CodigoNuevoMadre"].ToString() == "")
                           {
                               EstablecimientoMadreError = true;
                               ModelState.AddModelError("0", "Seleccione un establecimiento Madre");
                           }

                           if (HttpContext.Request.Form["Tipo_Establecimiento"].ToString() == "")
                           {
                               TipoEstablecimientoError = true;
                               ModelState.AddModelError("0", "Seleccione un tipo de establecimiento");
                           }
                */

                if (HttpContext.Request.Form["Administracion"].ToString() == "")
                {
                    AdministracionError = true;
                    ModelState.AddModelError("", "Ingrese Administracion");
                }


                if (HttpContext.Request.Form["Rut"].ToString() == "")
                {
                    RUTError = true;
                    ModelState.AddModelError("", "Ingrese un Rut");
                }


                if (HttpContext.Request.Form["DV"].ToString() == "")
                {
                    DVError = true;
                    ModelState.AddModelError("", "Ingrese un Dv");
                }

                var rutSinDv = HttpContext.Request.Form["Rut"].ToString();
                rutSinDv = Regex.Replace(rutSinDv, @"[^0-9]+", "");
                var dv = HttpContext.Request.Form["DV"].ToString();
                var rutCompleto = rutSinDv + "-" + dv;
                Regex regex = new Regex(@"[0-9]{1,2}[0-9]{3}[0-9]{3}[-][0-9Kk]{1}");
                Match match = regex.Match(rutCompleto);
                if (!match.Success) {
                    DVError = true;
                    ModelState.AddModelError("", "Run con formato inválido");
                }

                var isValidRutV1b = _rutv1.verificaRut(rutCompleto);
                if (!isValidRutV1b)
                {
                    DVError = true;
                    ModelState.AddModelError("", "El Run ingresado es inválido");
                }


                if (HttpContext.Request.Form["Apellido_Paterno"].ToString() == "")
                {
                    APaternoError = true;
                    ModelState.AddModelError("", "Ingrese Apellido Paterno");
                }


                if (HttpContext.Request.Form["Apellido_Materno"].ToString() == "")
                {
                    AMaternoError = true;
                    ModelState.AddModelError("", "Ingrese Apellido Materno");
                }


                if (HttpContext.Request.Form["Nombres"].ToString() == "")
                {
                    NombreError = true;
                    ModelState.AddModelError("", "Ingrese Nombres");
                }


                if (HttpContext.Request.Form["Sexo"].ToString() == "")
                {
                    SexoError = true;
                    ModelState.AddModelError("0", "Seleccione Sexo");
                }

                string fechaNacimientoString = HttpContext.Request.Form["Fecha_Nacimiento"].ToString();
                DateTime fechaNacimientoDate = Convert.ToDateTime(fechaNacimientoString);
                DateTime fechaActualDate = DateTime.Now;

                int now = int.Parse(fechaActualDate.ToString("yyyyMMdd"));
                int dob = int.Parse(fechaNacimientoDate.ToString("yyyyMMdd"));
                int age = (now - dob) / 10000;

                if (age < 18 || age > 100) {
                    Fecha_NacimientoError = true;
                    ModelState.AddModelError("0", "La fecha de nacimiento debe corresponder a una edad mayor a 17 años o menor a 101 años, edad: " + age);
                }

                if (HttpContext.Request.Form["Fecha_Nacimiento"].ToString() == "")
                {
                    Fecha_NacimientoError = true;
                    ModelState.AddModelError("0", "Ingrese fecha de nacimiento");
                }


                if (HttpContext.Request.Form["Nacionalidad"].ToString() == "")
                {
                    NacionalidadError = true;
                    ModelState.AddModelError("0", "Seleccione Nacionalidad");
                }


                if (HttpContext.Request.Form["Categoria"].ToString() == "")
                {
                    CategoriaError = true;
                    ModelState.AddModelError("", "Ingrese una Categoria");
                }


                if (HttpContext.Request.Form["Profesion"].ToString() == "")
                {
                    ProfesionError = true;
                    ModelState.AddModelError("", "Seleccione una Profesion");
                }


                if (HttpContext.Request.Form["Especialidad"].ToString() == "")
                {
                    EspecialidadError = true;
                    ModelState.AddModelError("", "Seleccione Especialidad");
                }


                if (HttpContext.Request.Form["Cargo"].ToString() == "")
                {
                    CargoError = true;
                    ModelState.AddModelError("", "Seleccione Cargo");
                }


                if (HttpContext.Request.Form["Ley"].ToString() == "")
                {
                    LeyError = true;
                    ModelState.AddModelError("", "Ingrese Ley");
                }


                if (HttpContext.Request.Form["Ley"].ToString() != "Ley 19.378" && HttpContext.Request.Form["Categoria"].ToString() != "N/A")
                {
                    ContratoCategoriaError = true;
                    ModelState.AddModelError("", "Al seleccionar un contrato por Código del trabajo u honorarios debe seleccionar la opción N/A en la categoria");
                }

                if (HttpContext.Request.Form["Tipo_Contrato"].ToString() == "")
                {
                    ContratoError = true;
                    ModelState.AddModelError("", "Ingrese un tipo de contrato");
                }


                if (HttpContext.Request.Form["Funciones_Chofer"].ToString() == "")
                {
                    ChoferError = true;
                    ModelState.AddModelError("", "Indique si el chofer de ambulancia cumple asignación de funciones con resolución");
                }


                if (HttpContext.Request.Form["Jornada"].ToString() == "")
                {
                    JornadaError = true;
                    ModelState.AddModelError("", "Ingrese horas semanales de jornada laboral");
                }
                else if (Convert.ToInt16(HttpContext.Request.Form["Jornada"].ToString()) > 44 && HttpContext.Request.Form["Ley"].ToString() != "Honorario" || Convert.ToInt16(HttpContext.Request.Form["Jornada"].ToString()) < 1 && HttpContext.Request.Form["Ley"].ToString() != "Honorario")
                {
                    JornadaError = true;
                    ModelState.AddModelError("", "La jornada laboral no puede ser menor a 1 hora o mayor a 44 horas");
                }


                if (HttpContext.Request.Form["Fecha_Ingreso"].ToString() == "")
                {
                    FechaIngresoError = true;
                    ModelState.AddModelError("", "Asigne una fecha de ingreso a la dotación comunal");
                }


                if (HttpContext.Request.Form["Anos_Servicio"].ToString() == "")
                {
                    AñosServicioError = true;
                    ModelState.AddModelError("", "Ingrese la cantidad de años de servicio");
                }
                else if (Convert.ToInt16(HttpContext.Request.Form["Anos_Servicio"].ToString()) > 99 || Convert.ToInt16(HttpContext.Request.Form["Anos_Servicio"].ToString()) < 0)
                {
                    JornadaError = true;
                    ModelState.AddModelError("", "Los años de servicio no pueden ser menos de 0 o mas de 99 años");
                }


                if (HttpContext.Request.Form["Bienios"].ToString() == "")
                {
                    BieniosError = true;
                    ModelState.AddModelError("", "Ingrese la cantidad de bienios");
                }


                if (HttpContext.Request.Form["Nivel_Carrera"].ToString() == "")
                {
                    Nivel_CarreraError = true;
                    ModelState.AddModelError("", "Ingrese el nivel de la carrera");
                }


                if (HttpContext.Request.Form["SBase_Comunal"].ToString() == "")
                {
                    SBase_ComunalError = true;
                    ModelState.AddModelError("", "Ingrese la base comunal a julio de este año");
                }


                if (HttpContext.Request.Form["Haberes"].ToString() == "")
                {
                    HaberesError = true;
                    ModelState.AddModelError("", "Ingrese el total de haberes");
                }


                if (HttpContext.Request.Form["Tipo_Prevision"].ToString() == "")
                {
                    Tipo_PrevisionError = true;
                    ModelState.AddModelError("", "Ingrese un tipo de previsión");
                }


                if (HttpContext.Request.Form["Tipo_Isapre"].ToString() == "")
                {
                    Tipo_IsapreError = true;
                    ModelState.AddModelError("", "Ingrese un seguro de salud");
                }
            

            if (ServicioError == true || ComunaError == true || EstablecimientoError == true || AdministracionError == true || RUTError == true || DVError == true || AMaternoError == true || APaternoError == true || NombreError == true || SexoError == true || Fecha_NacimientoError == true || NacionalidadError == true || CategoriaError == true || ContratoCategoriaError == true || ContratoProfesionError == true || ProfesionError == true || EspecialidadError == true || CargoError == true || LeyError == true || ContratoError == true || ChoferError == true || JornadaError == true || FechaIngresoError == true || AñosServicioError == true || BieniosError == true || Nivel_CarreraError == true || SBase_ComunalError == true || HaberesError == true || Tipo_PrevisionError == true || Tipo_IsapreError == true)
            {
                CargaError = true;
            }


            if (CargaError == false)
            {
                var sueldoBaseComunal = HttpContext.Request.Form["SBase_Comunal"].ToString().Replace(".", "");
                var totalHaberes = HttpContext.Request.Form["Haberes"].ToString().Replace(".", "");

                string Connstr = configuration.GetConnectionString("DefaultConnection");
                SqlConnection conn = new SqlConnection(Connstr);
                conn.Open();
                //verifica si el registro está eliminado (activo = 1)//
                string EliminadoID = "SELECT ISNULL((SELECT SUM([Activo]) FROM [dbo].[DOTACION_Registros] where ID_Servicio = @ID_Servicio and [ID_Comuna] = @ID_Comuna and [ID_Establecimiento] = @IdEstablecimiento and [Administracion] = @Administracion and [Rut] = @RUT and [DV] = @DV and [Apellido_Paterno] = @APaterno and [Apellido_Materno] = @AMaterno and [Nombre] = @Nombre and [Sexo] = @Sexo and [Fecha_Nacimiento] = @Fecha_Nacimiento and [Nacionalidad] = @Nacionalidad and [Categoria] = @Categoria and [Profesion] = @Profesion and [Especialidad] = @Especialidad and [Cargo] = @Cargo and [Funciones_Chofer] = @Chofer and [Ley] = @Ley and [Tipo_contrato] = @Contrato and [Jornada] = @Jornada and [Fecha_Ingreso] = @FechaIngreso and [Anos_Servicio] = @AñosServicio and [Bienios] = @Bienios and [Nivel_Carrera] = @Nivel_Carrera and [Tipo_Prevision] = @Tipo_Prevision and [Tipo_Isapre] = @Tipo_Isapre and [SBase_Comunal] = @SBase_Comunal and [Haberes] = @Haberes and [Activo] = 1),0)";
                SqlCommand cmdEliminado = new SqlCommand(EliminadoID, conn);
                cmdEliminado.Parameters.AddWithValue("@ID_Servicio", HttpContext.Request.Form["ID_Servicio"].ToString());
                cmdEliminado.Parameters.AddWithValue("@ID_Comuna", HttpContext.Request.Form["ID_Comuna"].ToString());
                cmdEliminado.Parameters.AddWithValue("@IdEstablecimiento", HttpContext.Request.Form["CodigoNuevo"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Administracion", HttpContext.Request.Form["Administracion"].ToString());
                cmdEliminado.Parameters.AddWithValue("@RUT", HttpContext.Request.Form["RUT"].ToString());
                cmdEliminado.Parameters.AddWithValue("@DV", HttpContext.Request.Form["DV"].ToString());
                cmdEliminado.Parameters.AddWithValue("@APaterno", HttpContext.Request.Form["Apellido_Paterno"].ToString());
                cmdEliminado.Parameters.AddWithValue("@AMaterno", HttpContext.Request.Form["Apellido_Materno"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Nombre", HttpContext.Request.Form["Nombres"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Sexo", HttpContext.Request.Form["Sexo"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Fecha_Nacimiento", HttpContext.Request.Form["Fecha_Nacimiento"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Nacionalidad", HttpContext.Request.Form["Nacionalidad"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Categoria", HttpContext.Request.Form["Categoria"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Profesion", HttpContext.Request.Form["Profesion"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Especialidad", HttpContext.Request.Form["Especialidad"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Cargo", HttpContext.Request.Form["Cargo"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Ley", HttpContext.Request.Form["Ley"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Contrato", HttpContext.Request.Form["Tipo_contrato"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Chofer", HttpContext.Request.Form["Funciones_Chofer"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Jornada", HttpContext.Request.Form["Jornada"].ToString());
                cmdEliminado.Parameters.AddWithValue("@FechaIngreso", HttpContext.Request.Form["Fecha_Ingreso"].ToString());
                cmdEliminado.Parameters.AddWithValue("@AñosServicio", HttpContext.Request.Form["Anos_Servicio"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Bienios", HttpContext.Request.Form["Bienios"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Nivel_Carrera", HttpContext.Request.Form["Nivel_Carrera"].ToString());
                cmdEliminado.Parameters.AddWithValue("@SBase_Comunal", sueldoBaseComunal);
                cmdEliminado.Parameters.AddWithValue("@Haberes", totalHaberes);
                cmdEliminado.Parameters.AddWithValue("@Tipo_Prevision", HttpContext.Request.Form["Tipo_Prevision"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Tipo_Isapre", HttpContext.Request.Form["Tipo_Isapre"].ToString());
                cmdEliminado.Parameters.AddWithValue("@Usuario", User.Identity.Name);
                var RegistroEliminado = cmdEliminado.ExecuteScalar();

                string RegistroRepetido = "SELECT [ID_Servicio],[ID_Comuna], [ID_Establecimiento],[Rut],[Tipo_Contrato] FROM .[dbo].[DOTACION_Registros] where [ID_Servicio] = @ID_Servicio and [ID_Comuna] = @ID_Comuna and [ID_Establecimiento] = @IdEstablecimiento and [Rut] = @rut and [Ley] = @Ley and [Tipo_Contrato] = @Contrato and [Activo] = 0";
                string JornadaMayor44 = "SELECT ISNULL((SELECT sum([Jornada]) FROM .[dbo].[DOTACION_Registros_resultados] where [Rut] = @Rut and ([Ley] = 'Ley 19.378' or [Ley] = 'Código del Trabajo') and [Activo] = 0),0)";
                SqlCommand cmdRepetido = new SqlCommand(RegistroRepetido, conn);
                SqlCommand cmdJornadaMayor44 = new SqlCommand(JornadaMayor44, conn);
                cmdRepetido.Parameters.AddWithValue("@ID_Servicio", HttpContext.Request.Form["ID_Servicio"].ToString());
                cmdRepetido.Parameters.AddWithValue("@ID_Comuna", HttpContext.Request.Form["ID_Comuna"].ToString());
                cmdRepetido.Parameters.AddWithValue("@IdEstablecimiento", HttpContext.Request.Form["CodigoNuevo"].ToString());
                cmdRepetido.Parameters.AddWithValue("@RUT", HttpContext.Request.Form["RUT"].ToString());
                cmdRepetido.Parameters.AddWithValue("@Ley", HttpContext.Request.Form["Ley"].ToString());
                cmdRepetido.Parameters.AddWithValue("@Contrato", HttpContext.Request.Form["Tipo_contrato"].ToString());
                cmdJornadaMayor44.Parameters.AddWithValue("@IdEstablecimiento", HttpContext.Request.Form["CodigoNuevo"].ToString());
                cmdJornadaMayor44.Parameters.AddWithValue("@RUT", HttpContext.Request.Form["RUT"].ToString());
                cmdJornadaMayor44.Parameters.AddWithValue("@Ley", HttpContext.Request.Form["Ley"].ToString());
                cmdJornadaMayor44.Parameters.AddWithValue("@jornada", HttpContext.Request.Form["Jornada"].ToString());


                var Repetido = cmdRepetido.ExecuteScalar();
                var VarJornadaMayor44 = cmdJornadaMayor44.ExecuteScalar();
                int jornada = Int32.Parse(HttpContext.Request.Form["Jornada"].ToString());


                //--Registro repetido en la base de datos--//
                if (Int32.Parse(RegistroEliminado.ToString()) != 0)
                {
                    RegistroEliminadoError = true;
                    ModelState.AddModelError("Error", "Entre los registros inactivos ya existe un registro que contiene la misma información");
                    ModelState.AddModelError("Error", "No se ha ingresado el Registro");

                    var stringArray = new string[21] {
                            HttpContext.Request.Form["Rut"].ToString(),
                            HttpContext.Request.Form["Apellido_Paterno"].ToString(),
                            HttpContext.Request.Form["Apellido_Materno"].ToString(),
                            HttpContext.Request.Form["Nombres"].ToString(),
                            HttpContext.Request.Form["Fecha_Nacimiento"].ToString(),
                            HttpContext.Request.Form["Jornada"].ToString(),
                            HttpContext.Request.Form["Anos_Servicio"].ToString(),
                            HttpContext.Request.Form["Fecha_Ingreso"].ToString(),
                            HttpContext.Request.Form["SBase_Comunal"].ToString(),
                            HttpContext.Request.Form["Haberes"].ToString(),
                            HttpContext.Request.Form["CodigoNuevo"].ToString(),
                            HttpContext.Request.Form["ID_Comuna"].ToString(),
                            HttpContext.Request.Form["ID_Servicio"].ToString(),
                            HttpContext.Request.Form["Ley"].ToString(),
                            HttpContext.Request.Form["Tipo_Contrato"].ToString(),
                            HttpContext.Request.Form["Categoria"].ToString(),
                            HttpContext.Request.Form["Nivel_Carrera"].ToString(),
                            HttpContext.Request.Form["Profesion"].ToString(),
                            HttpContext.Request.Form["Especialidad"].ToString(),
                            HttpContext.Request.Form["Cargo"].ToString(),
                            HttpContext.Request.Form["Funciones_Chofer"].ToString()
                        };
                    ViewBag.ArrayRequest = stringArray;
                    ViewBag.TieneError = 1;
                }

                if (HttpContext.Request.Form["Ley"].ToString() == "Ley 19.378" || HttpContext.Request.Form["Ley"].ToString() == "Código del Trabajo")
                {

                    if ((int.Parse(VarJornadaMayor44.ToString()) + jornada) > 44)

                    {
                        ModelState.AddModelError("Error", "Los contratos Indefinidos, a plazo fijo, de reemplazo y/o por código del trabajo del rut ingresado, exceden las 44 horas de jornada laboral (se toman en cuenta todos los contratos en el país)");
                        ModelState.AddModelError("Error", "No se ha actualizado el Registro");

                        var stringArray = new string[21] {
                            HttpContext.Request.Form["Rut"].ToString(),
                            HttpContext.Request.Form["Apellido_Paterno"].ToString(),
                            HttpContext.Request.Form["Apellido_Materno"].ToString(),
                            HttpContext.Request.Form["Nombres"].ToString(),
                            HttpContext.Request.Form["Fecha_Nacimiento"].ToString(),
                            HttpContext.Request.Form["Jornada"].ToString(),
                            HttpContext.Request.Form["Anos_Servicio"].ToString(),
                            HttpContext.Request.Form["Fecha_Ingreso"].ToString(),
                            HttpContext.Request.Form["SBase_Comunal"].ToString(),
                            HttpContext.Request.Form["Haberes"].ToString(),
                            HttpContext.Request.Form["CodigoNuevo"].ToString(),
                            HttpContext.Request.Form["ID_Comuna"].ToString(),
                            HttpContext.Request.Form["ID_Servicio"].ToString(),
                            HttpContext.Request.Form["Ley"].ToString(),
                            HttpContext.Request.Form["Tipo_Contrato"].ToString(),
                            HttpContext.Request.Form["Categoria"].ToString(),
                            HttpContext.Request.Form["Nivel_Carrera"].ToString(),
                            HttpContext.Request.Form["Profesion"].ToString(),
                            HttpContext.Request.Form["Especialidad"].ToString(),
                            HttpContext.Request.Form["Cargo"].ToString(),
                            HttpContext.Request.Form["Funciones_Chofer"].ToString()
                        };
                        ViewBag.ArrayRequest = stringArray;
                        ViewBag.TieneError = 1;

                        Jornada44Error = true;
                    }

                }

                if (Jornada44Error == false)
                {
                    if (Repetido != null)
                    {
                        ModelState.AddModelError("Error", "Ya se ha ingresado un registro conteniendo el mismo rut, establecimiento y contrato");
                        
                        var stringArray = new string[21] {
                            HttpContext.Request.Form["Rut"].ToString(),
                            HttpContext.Request.Form["Apellido_Paterno"].ToString(),
                            HttpContext.Request.Form["Apellido_Materno"].ToString(),
                            HttpContext.Request.Form["Nombres"].ToString(),
                            HttpContext.Request.Form["Fecha_Nacimiento"].ToString(),
                            HttpContext.Request.Form["Jornada"].ToString(),
                            HttpContext.Request.Form["Anos_Servicio"].ToString(),
                            HttpContext.Request.Form["Fecha_Ingreso"].ToString(),
                            HttpContext.Request.Form["SBase_Comunal"].ToString(),
                            HttpContext.Request.Form["Haberes"].ToString(),
                            HttpContext.Request.Form["CodigoNuevo"].ToString(),
                            HttpContext.Request.Form["ID_Comuna"].ToString(),
                            HttpContext.Request.Form["ID_Servicio"].ToString(),
                            HttpContext.Request.Form["Ley"].ToString(),
                            HttpContext.Request.Form["Tipo_Contrato"].ToString(),
                            HttpContext.Request.Form["Categoria"].ToString(),
                            HttpContext.Request.Form["Nivel_Carrera"].ToString(),
                            HttpContext.Request.Form["Profesion"].ToString(),
                            HttpContext.Request.Form["Especialidad"].ToString(),
                            HttpContext.Request.Form["Cargo"].ToString(),
                            HttpContext.Request.Form["Funciones_Chofer"].ToString()
                        };
                        ViewBag.ArrayRequest = stringArray;
                        ViewBag.TieneError = 1;

                        ModelState.AddModelError("Error", "No se ha ingresado el Registro");
                    }
                    else
                    {

                        if (CargaError == false && RegistroEliminadoError == false)
                        {

                            string query = "INSERT INTO [dbo].[DOTACION_Registros] ([ID_Servicio],[ID_Comuna],[ID_Establecimiento],[Administracion],[Rut],[DV],[Apellido_Paterno],[Apellido_Materno],[Nombre],[Sexo],[Fecha_Nacimiento],[Nacionalidad],[Categoria],[Profesion],[Especialidad],[Cargo],[Ley],[Tipo_contrato],[Funciones_Chofer],[Jornada],[Fecha_Ingreso],[Anos_Servicio],[Bienios],[Nivel_Carrera],[SBase_Comunal],[Haberes],[Tipo_Prevision],[Tipo_Isapre],[Fecha_Carga],[usuario],Validado, Activo, Ingreso_registro, Revisado) VALUES(@ID_Servicio,@ID_Comuna,@IdEstablecimiento,@Administracion,@RUT,@DV,@APaterno,@AMaterno,@Nombre,@Sexo,@Fecha_Nacimiento,@Nacionalidad,@Categoria,@Profesion,@Especialidad,@Cargo,@Ley,@Contrato,@Chofer,@Jornada,@FechaIngreso,@AñosServicio,@Bienios,@Nivel_Carrera,@SBase_Comunal,@Haberes,@Tipo_Prevision,@Tipo_Isapre, getdate(), @Usuario,0,0,'Carga Simple',0)";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@ID_Servicio", HttpContext.Request.Form["ID_Servicio"].ToString());
                            cmd.Parameters.AddWithValue("@ID_Comuna", HttpContext.Request.Form["ID_Comuna"].ToString());
                            cmd.Parameters.AddWithValue("@IdEstablecimiento", HttpContext.Request.Form["CodigoNuevo"].ToString());
                            cmd.Parameters.AddWithValue("@Administracion", HttpContext.Request.Form["Administracion"].ToString());
                            cmd.Parameters.AddWithValue("@RUT", HttpContext.Request.Form["RUT"].ToString());
                            cmd.Parameters.AddWithValue("@DV", HttpContext.Request.Form["DV"].ToString());
                            cmd.Parameters.AddWithValue("@APaterno", HttpContext.Request.Form["Apellido_Paterno"].ToString());
                            cmd.Parameters.AddWithValue("@AMaterno", HttpContext.Request.Form["Apellido_Materno"].ToString());
                            cmd.Parameters.AddWithValue("@Nombre", HttpContext.Request.Form["Nombres"].ToString());
                            cmd.Parameters.AddWithValue("@Sexo", HttpContext.Request.Form["Sexo"].ToString());
                            cmd.Parameters.AddWithValue("@Fecha_Nacimiento", HttpContext.Request.Form["Fecha_Nacimiento"].ToString());
                            cmd.Parameters.AddWithValue("@Nacionalidad", HttpContext.Request.Form["Nacionalidad"].ToString());
                            cmd.Parameters.AddWithValue("@Categoria", HttpContext.Request.Form["Categoria"].ToString());
                            cmd.Parameters.AddWithValue("@Profesion", HttpContext.Request.Form["Profesion"].ToString());
                            cmd.Parameters.AddWithValue("@Especialidad", HttpContext.Request.Form["Especialidad"].ToString());
                            cmd.Parameters.AddWithValue("@Cargo", HttpContext.Request.Form["Cargo"].ToString());
                            cmd.Parameters.AddWithValue("@Ley", HttpContext.Request.Form["Ley"].ToString());
                            cmd.Parameters.AddWithValue("@Contrato", HttpContext.Request.Form["Tipo_contrato"].ToString());
                            cmd.Parameters.AddWithValue("@Chofer", HttpContext.Request.Form["Funciones_Chofer"].ToString());
                            cmd.Parameters.AddWithValue("@Jornada", HttpContext.Request.Form["Jornada"].ToString());
                            cmd.Parameters.AddWithValue("@FechaIngreso", HttpContext.Request.Form["Fecha_Ingreso"].ToString());
                            cmd.Parameters.AddWithValue("@AñosServicio", HttpContext.Request.Form["Anos_Servicio"].ToString());
                            cmd.Parameters.AddWithValue("@Bienios", HttpContext.Request.Form["Bienios"].ToString());
                            cmd.Parameters.AddWithValue("@Nivel_Carrera", HttpContext.Request.Form["Nivel_Carrera"].ToString());
                            cmd.Parameters.AddWithValue("@SBase_Comunal", sueldoBaseComunal);
                            cmd.Parameters.AddWithValue("@Haberes", totalHaberes);
                            cmd.Parameters.AddWithValue("@Tipo_Prevision", HttpContext.Request.Form["Tipo_Prevision"].ToString());
                            cmd.Parameters.AddWithValue("@Tipo_Isapre", HttpContext.Request.Form["Tipo_Isapre"].ToString());
                            cmd.Parameters.AddWithValue("@Usuario", User.Identity.Name);

                            cmd.ExecuteReader();
                            conn.Close();

                            _flashMessage.Confirmation("Se ha ingresado al funcionario " + HttpContext.Request.Form["Nombres"].ToString() + " " + HttpContext.Request.Form["Apellido_Paterno"].ToString() + " " + HttpContext.Request.Form["Apellido_Materno"].ToString() + " [" + HttpContext.Request.Form["RUT"].ToString() + "-" + HttpContext.Request.Form["DV"].ToString() + "] exitosamente!");
                            ModelState.AddModelError("Error", "Se ha ingresado el Registro");
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("Error", "No se ha ingresado el Registro");

                var stringArray = new string[21] {
                    HttpContext.Request.Form["Rut"].ToString(),
                    HttpContext.Request.Form["Apellido_Paterno"].ToString(),
                    HttpContext.Request.Form["Apellido_Materno"].ToString(),
                    HttpContext.Request.Form["Nombres"].ToString(),
                    HttpContext.Request.Form["Fecha_Nacimiento"].ToString(),
                    HttpContext.Request.Form["Jornada"].ToString(),
                    HttpContext.Request.Form["Anos_Servicio"].ToString(),
                    HttpContext.Request.Form["Fecha_Ingreso"].ToString(),
                    HttpContext.Request.Form["SBase_Comunal"].ToString(),
                    HttpContext.Request.Form["Haberes"].ToString(),
                    HttpContext.Request.Form["CodigoNuevo"].ToString(),
                    HttpContext.Request.Form["ID_Comuna"].ToString(),
                    HttpContext.Request.Form["ID_Servicio"].ToString(),
                    HttpContext.Request.Form["Ley"].ToString(),
                    HttpContext.Request.Form["Tipo_Contrato"].ToString(),
                    HttpContext.Request.Form["Categoria"].ToString(),
                    HttpContext.Request.Form["Nivel_Carrera"].ToString(),
                    HttpContext.Request.Form["Profesion"].ToString(),
                    HttpContext.Request.Form["Especialidad"].ToString(),
                    HttpContext.Request.Form["Cargo"].ToString(),
                    HttpContext.Request.Form["Funciones_Chofer"].ToString()
                };
                ViewBag.ArrayRequest = stringArray;
                ViewBag.TieneError = 1;

            }

            return View();
        }

       
    }

}

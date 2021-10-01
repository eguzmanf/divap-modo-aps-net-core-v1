using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DotacionWEBCore.Areas.Identity.Data;
using DotacionWEBCore.Helpers.Rut.V1;
using DotacionWEBCore.Helpers.Rut.V2;
using DotacionWEBCore.Helpers.String.StringCase;
using DotacionWEBCore.Models;
using DotacionWEBCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Vereyon.Web;
using X.PagedList;

namespace DotacionWEBCore.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IFlashMessage _flashMessage;
        private readonly UserManager<ListaUsuarios> _userManager;
        private readonly ISession _session;
        private readonly IVerificaRutHelpersV1 _rutv1;
        private readonly IVerificaRutHelpersV2 _rutv2;
        private readonly StringCase _strCase;

        public UsuarioController(DatabaseContext context, IFlashMessage flashMessage, UserManager<ListaUsuarios> userManager, IHttpContextAccessor httpContextAccessor, IVerificaRutHelpersV1 verificaRutHelpersV1, IVerificaRutHelpersV2 verificaRutHelpersV2, StringCase stringCase)
        {
            _context = context;
            _flashMessage = flashMessage;
            _userManager = userManager;
            _session = httpContextAccessor.HttpContext.Session;
            _rutv1 = verificaRutHelpersV1;
            _rutv2 = verificaRutHelpersV2;
            _strCase = stringCase;
        }

        // GET: Usuario
        public async Task<IActionResult> Index(ListadoViewModel<UsuarioFull> modelo)
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            if (Perfil_U == 3)
            {
                var usuarios = _context.DOTACION_Usuarios_Full.Select(c => c);

                if (modelo.TerminoBusqueda == "N/A")
                {
                    usuarios = usuarios.Where(u => ((u.ServicioName == null && u.IdServicio == 9001) || (u.ComunaName == null && u.ID_Comuna_U == 90101)) && (u.RoleName == "MINSAL"));

                } else if(modelo.TerminoBusqueda == "Habilitado")
                {
                    usuarios = usuarios.Where(u => u.LockoutEnabled == true && (u.LockoutEnd == null || u.LockoutEnd < DateTime.Now));

                } else if(modelo.TerminoBusqueda == "Inhabilitado")
                {
                    usuarios = usuarios.Where(u => u.LockoutEnabled == true && u.LockoutEnd > DateTime.Now);

                } else if(modelo.TerminoBusqueda == "Sin Registros")
                {
                    usuarios = usuarios.Where(u => u.UsuarioNombre == null || u.ApellidoPaterno == null || u.ApellidoMaterno == null || u.FechaNacimiento == null || u.Sexo == null || u.Nacionalidad == null);
                }
                else if (!String.IsNullOrEmpty(modelo.TerminoBusqueda))
                {
                    usuarios = usuarios.Where(u => u.UserName.Contains(modelo.TerminoBusqueda) || u.Email.Contains(modelo.TerminoBusqueda) || u.PhoneNumber.Contains(modelo.TerminoBusqueda) || u.ComunaName.Contains(modelo.TerminoBusqueda) || u.ServicioName.Contains(modelo.TerminoBusqueda) || 
                                                   u.RoleName.Contains(modelo.TerminoBusqueda) || u.UsuarioNombre.Contains(modelo.TerminoBusqueda) || u.ApellidoPaterno.Contains(modelo.TerminoBusqueda) || u.ApellidoMaterno.Contains(modelo.TerminoBusqueda) || u.FechaNacimiento.ToString().Contains(modelo.TerminoBusqueda) || 
                                                   u.Nacionalidad.Contains(modelo.TerminoBusqueda) || u.Sexo.Contains(modelo.TerminoBusqueda)
                                             );
                }

                var numeroPagina = modelo.Pagina ?? 1;
                var registros = await usuarios.ToPagedListAsync(numeroPagina, 10);
                modelo.Registros = registros;

                return View(modelo);

            } else
            {
                _flashMessage.Danger("Acceso denegado, Usted no tiene los permisos necesarios para acceder a este Recurso!");
                return RedirectToAction("Index", "Home", null);
            }

        }

        // POST: Usuario/ExportUserExcel
        [HttpPost]
        public IActionResult ExportUserExcel(ListadoViewModel<UsuarioFull> modelo)
        {
            var terminoBusqueda = modelo.TerminoBusqueda;
            var usuarios = new List<UsuarioFull>();

            if (terminoBusqueda == null)
            {
                usuarios = _context.DOTACION_Usuarios_Full.Select(s => s).ToList();

            } else if(terminoBusqueda == "N/A")
            {
                usuarios = _context.DOTACION_Usuarios_Full.Select(s => s).Where(u => ((u.ServicioName == null && u.IdServicio == 9001) || (u.ComunaName == null && u.ID_Comuna_U == 90101)) && (u.RoleName == "MINSAL")).ToList();

            } else if(terminoBusqueda == "Habilitado")
            {
                usuarios = _context.DOTACION_Usuarios_Full.Select(s => s).Where(u => u.LockoutEnabled == true && (u.LockoutEnd == null || u.LockoutEnd < DateTime.Now)).ToList();

            } else if(terminoBusqueda == "Inhabilitado")
            {
                usuarios = _context.DOTACION_Usuarios_Full.Select(s => s).Where(u => u.LockoutEnabled == true && u.LockoutEnd > DateTime.Now).ToList();

            } else if(terminoBusqueda == "Sin Registros")
            {
                usuarios = _context.DOTACION_Usuarios_Full.Select(s => s).Where(u => u.UsuarioNombre == null || u.ApellidoPaterno == null || u.ApellidoMaterno == null || u.FechaNacimiento == null || u.Sexo == null || u.Nacionalidad == null).ToList();
            }
            else
            {
                usuarios = _context.DOTACION_Usuarios_Full.Select(s => s).Where(u => u.UserName.Contains(modelo.TerminoBusqueda) ||
                                                                                         u.Email.Contains(modelo.TerminoBusqueda) || u.PhoneNumber.Contains(modelo.TerminoBusqueda) || u.ComunaName.Contains(modelo.TerminoBusqueda) || u.ServicioName.Contains(modelo.TerminoBusqueda) ||
                                                                                         u.RoleName.Contains(modelo.TerminoBusqueda) || u.UsuarioNombre.Contains(modelo.TerminoBusqueda) || u.ApellidoPaterno.Contains(modelo.TerminoBusqueda) || u.ApellidoMaterno.Contains(modelo.TerminoBusqueda) ||
                                                                                         u.FechaNacimiento.ToString().Contains(modelo.TerminoBusqueda) || u.Nacionalidad.Contains(modelo.TerminoBusqueda) || u.Sexo.Contains(modelo.TerminoBusqueda)
                                                                                ).ToList();
            }

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            DateTime fechaActual = DateTime.Now;
            var timeStamp = String.Format("{0:yyyy-MM-dd_HH-mm-ss}", fechaActual);
            string fileName = "usuarios_" + timeStamp + ".xlsx";

            try
            {
                using (var workbook = new XLWorkbook()) 
                {
                    IXLWorksheet worksheet = workbook.Worksheets.Add("Usuarios");

                    // var imagePath = @"E:\trabajo\ModoAPS\dotacion-webcore\DotacionWEBCore\wwwroot\images\logo-180x44.png";
                    
                    // var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/logo-180x44.png");
                    // var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")).Scale(1);

                    // worksheet.Range(worksheet.Cell(1, 1).Address, worksheet.Cell(1, 14).Address).Merge();
                    // worksheet.Range(worksheet.Cell(2, 1).Address, worksheet.Cell(2, 14).Address).Merge();
                    
                    // worksheet.Range("A1:N2").Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick);
                    // worksheet.Range("A1:N2").Style.Border.SetInsideBorder(XLBorderStyleValues.Thick);
                    // worksheet.Range("A1:N2").Style.Fill.BackgroundColor = XLColor.LightGray;

                    // worksheet.Cell(1, 1).Value = "Id";
                    // worksheet.Cell(1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell(1, 1).Value = "Nombres";
                    worksheet.Cell(1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell(1, 2).Value = "Apellido Paterno";
                    worksheet.Cell(1, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell(1, 3).Value = "Apellido Materno";
                    worksheet.Cell(1, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell(1, 4).Value = "Usuario";
                    worksheet.Cell(1, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell(1, 5).Value = "Fecha Nacimiento";
                    worksheet.Cell(1, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell(1, 6).Value = "Género";
                    worksheet.Cell(1, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell(1, 7).Value = "Nacionalidad";
                    worksheet.Cell(1, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell(1, 8).Value = "Correo";
                    worksheet.Cell(1, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell(1, 9).Value = "Teléfono";
                    worksheet.Cell(1, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell(1, 10).Value = "Servicio de Salud";
                    worksheet.Cell(1, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell(1, 11).Value = "Comuna";
                    worksheet.Cell(1, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell(1, 12).Value = "Perfil";
                    worksheet.Cell(1, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell(1, 13).Value = "Estado";
                    worksheet.Cell(1, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.RangeUsed().SetAutoFilter();

                    var rangeHeader = worksheet.Range("A1:M1");
                    rangeHeader.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick);   // Generamos las lineas exteriores
                    rangeHeader.Style.Border.SetInsideBorder(XLBorderStyleValues.Medium);   //Generamos las lineas interiores
                    rangeHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; //Alineamos horizontalmente
                    rangeHeader.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;  //Alineamos verticalmente
                    // rangeHeader.Style.Font.SetFontName("Courier New"); // Utilizo una fuente monoespacio: Times New Roman, Arial
                    rangeHeader.Style.Font.FontName = "Times New Roman";         // Utilizo una fuente monoespacio
                    rangeHeader.Style.Font.FontSize = 14; //Indicamos el tamaño de la fuente
                    rangeHeader.Style.Font.FontColor = XLColor.White;
                    rangeHeader.Style.Fill.BackgroundColor = XLColor.MidnightBlue; //Indicamos el color de background
                    // rangeHeader.Style.Font.Bold = true;
                    rangeHeader.Style.Font.SetBold();

                    for (int index = 1; index <= usuarios.Count; index++)
                    {
                        /*
                        worksheet.Cell(index + 1, 1).Value = usuarios[index - 1].Id;
                        worksheet.Column(1).AdjustToContents();
                        worksheet.Row(index + 1).AdjustToContents();
                        worksheet.Cell(index + 1, 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        worksheet.Cell(index + 1, 1).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        */

                        if (usuarios[index - 1].UsuarioNombre == null)
                        {
                            worksheet.Cell(index + 1, 1).Value = "Sin Registros";
                            worksheet.Column(1).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 1).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        } else
                        {
                            worksheet.Cell(index + 1, 1).Value = usuarios[index - 1].UsuarioNombre;
                            worksheet.Column(1).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 1).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        }

                        if (usuarios[index - 1].ApellidoPaterno == null)
                        {
                            worksheet.Cell(index + 1, 2).Value = "Sin Registros";
                            worksheet.Column(2).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 2).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        } else
                        {
                            worksheet.Cell(index + 1, 2).Value = usuarios[index - 1].ApellidoPaterno;
                            worksheet.Column(2).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 2).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        }

                        if (usuarios[index - 1].ApellidoMaterno == null)
                        {
                            worksheet.Cell(index + 1, 3).Value = "Sin Registros";
                            worksheet.Column(3).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 3).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        } else
                        {
                            worksheet.Cell(index + 1, 3).Value = usuarios[index - 1].ApellidoMaterno;
                            worksheet.Column(3).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 3).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        }
                     
                        worksheet.Cell(index + 1, 4).Value = usuarios[index - 1].UserName;
                        worksheet.Column(4).AdjustToContents();
                        worksheet.Row(index + 1).AdjustToContents();
                        worksheet.Cell(index + 1, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        worksheet.Cell(index + 1, 4).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                        if (usuarios[index - 1].FechaNacimiento == null)
                        {
                            worksheet.Cell(index + 1, 5).Value = "Sin Registros";
                            worksheet.Column(5).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 5).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        } else
                        {
                            worksheet.Cell(index + 1, 5).Value = usuarios[index - 1].FechaNacimiento;
                            worksheet.Column(5).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 5).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        }

                        if (usuarios[index - 1].Sexo == null)
                        {
                            worksheet.Cell(index + 1, 6).Value = "Sin Registros";
                            worksheet.Column(6).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 6).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        } else
                        {
                            worksheet.Cell(index + 1, 6).Value = usuarios[index - 1].Sexo;
                            worksheet.Column(6).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 6).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        }

                        if (usuarios[index - 1].Nacionalidad == null) 
                        {
                            worksheet.Cell(index + 1, 7).Value = "Sin Registros";
                            worksheet.Column(7).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 7).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        } else
                        {
                            worksheet.Cell(index + 1, 7).Value = usuarios[index - 1].Nacionalidad;
                            worksheet.Column(7).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 7).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        }
                        
                        worksheet.Cell(index + 1, 8).Value = usuarios[index - 1].Email;
                        worksheet.Column(8).AdjustToContents();
                        worksheet.Row(index + 1).AdjustToContents();
                        worksheet.Cell(index + 1, 8).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        worksheet.Cell(index + 1, 8).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                        worksheet.Cell(index + 1, 9).Value = usuarios[index - 1].PhoneNumber;
                        worksheet.Column(9).AdjustToContents();
                        worksheet.Row(index + 1).AdjustToContents();
                        worksheet.Cell(index + 1, 9).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        worksheet.Cell(index + 1, 9).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                        if (usuarios[index - 1].ID_Perfil == 3 && usuarios[index - 1].ServicioName == null)
                        {
                            worksheet.Cell(index + 1, 10).Value = "N/A";
                            worksheet.Column(10).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 10).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 10).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                        } else
                        {
                            worksheet.Cell(index + 1, 10).Value = usuarios[index - 1].ServicioName;
                            worksheet.Column(10).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 10).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 10).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        }
                        
                        if (usuarios[index - 1].ID_Perfil == 3 && usuarios[index - 1].ComunaName == null)
                        {
                            worksheet.Cell(index + 1, 11).Value = "N/A";
                            worksheet.Column(11).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 11).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 11).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                        } else
                        {
                            worksheet.Cell(index + 1, 11).Value = usuarios[index - 1].ComunaName;
                            worksheet.Column(11).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 11).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 11).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        }
                        
                        worksheet.Cell(index + 1, 12).Value = usuarios[index - 1].RoleName;
                        worksheet.Column(12).AdjustToContents();
                        worksheet.Row(index + 1).AdjustToContents();
                        worksheet.Cell(index + 1, 12).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        worksheet.Cell(index + 1, 12).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                        if (usuarios[index - 1].LockoutEnabled == true && usuarios[index - 1].LockoutEnd > DateTime.Now)
                        {
                            worksheet.Cell(index + 1, 13).Value = "Inhabilitado";
                            worksheet.Column(13).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 13).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 13).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        }
                        else 
                        {
                            worksheet.Cell(index + 1, 13).Value = "Habilitado";
                            worksheet.Column(13).AdjustToContents();
                            worksheet.Row(index + 1).AdjustToContents();
                            worksheet.Cell(index + 1, 13).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            worksheet.Cell(index + 1, 13).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        }
                         
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                _flashMessage.Danger("Ha ocurrido un error al exportar!");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET Usuario/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            if (Perfil_U == 3)
            {
                if (id == null)
                {
                    // return NotFound();
                    _flashMessage.Danger("El usuario no existe registrado en el sistema!");
                    return RedirectToAction(nameof(Index));
                }

                var usuario = await _context.AspNetUsers.FirstOrDefaultAsync(m => m.Id == id);

                var usuarioExtendido = await _context.AspNetUsers_Extended.Where(w => w.Rut == usuario.UserName).FirstOrDefaultAsync();

                if (usuario == null)
                {
                    // return NotFound();
                    _flashMessage.Danger("El usuario no existe registrado en el sistema!");
                    return RedirectToAction(nameof(Index));
                }

                var servicioName = "";
                var comunaName = "";

                if (usuario.ID_Perfil == 3)
                {
                    servicioName = "N/A";
                    comunaName = "N/A";
                } else
                {
                    var servicio = ObtenerServicioPorId(usuario.IdServicio);
                    var comuna = ObtenerComunaPorId(usuario.ID_Comuna_U);
                    servicioName = servicio.Servicio;
                    comunaName = comuna.Comuna;
                }

                var rol = ObtenerRolePorId(usuario.ID_Perfil);

                UsuariosExtendidoDetailsViewModel vm = new UsuariosExtendidoDetailsViewModel();
                vm.Detalle.Id = usuario.Id;
                vm.Detalle.UserName = usuario.UserName;
                vm.Detalle.Email = usuario.Email;
                vm.Detalle.PhoneNumber = usuario.PhoneNumber;
                vm.Detalle.ServicioName = servicioName;
                vm.Detalle.ComunaName = comunaName;
                vm.Detalle.RoleName = rol.Nombre;
                vm.Detalle.LockoutEnd = usuario.LockoutEnd;
                vm.Detalle.LockoutEnabled = usuario.LockoutEnabled;

                if(usuarioExtendido != null)
                {
                    var genero = ObtenerGeneroPorId(usuarioExtendido.Genero);
                    var nacionalidad = ObtenerNacionalidadPorId(usuarioExtendido.NacionalidadId);

                    vm.Extended.Nombres = usuarioExtendido.Nombres;
                    vm.Extended.ApellidoPaterno = usuarioExtendido.ApellidoPaterno;
                    vm.Extended.ApellidoMaterno = usuarioExtendido.ApellidoMaterno;
                    vm.Extended.Rut = usuarioExtendido.Rut;
                    vm.Extended.FechaNacimiento = usuarioExtendido.FechaNacimiento;
                    vm.Extended.GeneroName = genero.Sexo;
                    vm.Extended.NacionalidadName = nacionalidad.Nacionalidad;

                } else
                {
                    var fechaZeros = new DateTime(1000, 01, 01);

                    vm.Extended.Nombres = "N/A";
                    vm.Extended.ApellidoPaterno = "N/A";
                    vm.Extended.ApellidoMaterno = "N/A";
                    vm.Extended.Rut = usuario.UserName.ToLower().Replace(".", "").Trim();
                    vm.Extended.FechaNacimiento = fechaZeros;
                    vm.Extended.GeneroName = "N/A";
                    vm.Extended.NacionalidadName = "N/A";

                    _flashMessage.Warning("Por favor completar los registros del Usuario!");
                }

                return View(vm);

            } else
            {
                _flashMessage.Danger("Acceso denegado, Usted no tiene los permisos necesarios para acceder a este Recurso!");
                return RedirectToAction("Index", "Home", null);
            }
        }

        // GET: Usuario/Create
        public IActionResult Create()
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            if (Perfil_U == 3)
            {
                UsuariosExtendidoViewModel vm = new UsuariosExtendidoViewModel();
                // Console.WriteLine("Testing Console");
                vm.Create.Roles = ObtenerListaRoles();
                vm.Create.Servicios = ObtenerListaServicios();
                vm.Create.Comunas = ObtenerComunasServicioSaludMetropolitanoSurOriente();
                vm.Extended.ListaNacionalidad = ObtenerNacionalidad();
                vm.Extended.ListaSexo = ObtenerGenero();

                return View(vm);

            } else
            {
                _flashMessage.Danger("Acceso denegado, Usted no tiene los permisos necesarios para acceder a este Recurso!");
                return RedirectToAction("Index", "Home", null);
            }
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        /* public async Task<IActionResult> Create([Bind("Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, ConfirmPassword, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, IdServicio, ID_Perfil, Perfil, ID_Comuna_U, IdExtended, Nombres, ApellidoPaterno, ApellidoMaterno, Rut, FechaNacimiento, NacionalidadId")] UsuariosExtendidoViewModel vm)   */
        public async Task<IActionResult> Create(UsuariosExtendidoViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var rutDuplicado = await _context.AspNetUsers.Where(w => w.UserName == vm.Create.UserName).FirstOrDefaultAsync();
                if(rutDuplicado != null)
                {
                    _flashMessage.Danger("El usuario " +  vm.Create.UserName + " ya se encuentra registrado en el sistema, no es posible crearlo nuevamente!");
                    return RedirectToAction(nameof(Index));
                }

                var isValidRutV1b = _rutv1.verificaRut(vm.Create.UserName);
                if(!isValidRutV1b)
                {
                    _flashMessage.Danger("El Run ingresado " + vm.Create.UserName + " es inválido [" + isValidRutV1b + "]!");
                    return RedirectToAction(nameof(Index));
                }

                var isValidRutV2a = _rutv2.ValidaRut(vm.Create.UserName);
                if (!isValidRutV2a)
                {
                    _flashMessage.Danger("El Run ingresado " + vm.Create.UserName + " es inválido [" + isValidRutV2a + "]!");
                    return RedirectToAction(nameof(Index));
                }

                if (vm.Create.ID_Perfil == 1) {

                    var siUsuarioServicioRepetido = await _context.AspNetUsers.Where(d => d.IdServicio == vm.Create.IdServicio && d.ID_Perfil == 1 && d.LockoutEnabled == false).FirstOrDefaultAsync();

                    if(siUsuarioServicioRepetido != null)
                    {
                        var servicioSelect = ObtenerServicioPorId(vm.Create.IdServicio);
                        var servicioNameSelect = servicioSelect.Servicio;
                        _flashMessage.Danger("No es posible crear más de un usuario para un mismo Servicio de Salud (" + servicioNameSelect.ToString() + ")!");
                        return RedirectToAction(nameof(Index));
                    }
                }

                if (vm.Create.ID_Perfil == 2)
                {
                    var siUsuarioComunaRepetido = await _context.AspNetUsers.Where(c => c.ID_Comuna_U == vm.Create.ID_Comuna_U && c.ID_Perfil == 2 && c.LockoutEnabled == false).FirstOrDefaultAsync();

                    if(siUsuarioComunaRepetido != null)
                    {
                        var comunaSelect = ObtenerComunaPorId(vm.Create.ID_Comuna_U);
                        var comunaNameSelect = comunaSelect.Comuna;
                        _flashMessage.Danger("No es posible crear más de un usuario para la misma comuna (" + comunaNameSelect.ToString() + ")!");
                        return RedirectToAction(nameof(Index));
                    }
                }

                var ec = 0;
                bool ecb = Convert.ToBoolean(ec);

                var passwordHasher = new PasswordHasher<Usuario>();
                var user = new Usuario();
                var hashedPassword = passwordHasher.HashPassword(user, vm.Create.PasswordHash);

                var securityStamp = Guid.NewGuid().ToString();

                var concurrencyStamp = RandomString(32);

                var pnc = 0;
                bool pncb = Convert.ToBoolean(pnc);

                var tfe = 0;
                bool tfeb = Convert.ToBoolean(tfe);

                var lockoutEnabled = 0;
                bool lockoutEnabledBool = Convert.ToBoolean(lockoutEnabled);

                var accessFailedCountInt = 0;

                var rol = ObtenerRolePorId(vm.Create.ID_Perfil);

                if(vm.Create.ID_Perfil == 3 && vm.Create.IdServicio == 0 && vm.Create.ID_Comuna_U == 0)
                {
                    vm.Create.IdServicio = 9001;
                    vm.Create.ID_Comuna_U = 90101;
                }

                ListaUsuarios usuario = new ListaUsuarios
                {
                    UserName = vm.Create.UserName.ToLower().Replace(".", "").Trim(),
                    NormalizedUserName = vm.Create.UserName.ToUpper().Replace(".", "").Trim(),
                    Email = vm.Create.Email.ToLower().Trim(),
                    NormalizedEmail = vm.Create.Email.ToUpper().Trim(),
                    EmailConfirmed = ecb,
                    PasswordHash = hashedPassword,
                    SecurityStamp = securityStamp,
                    ConcurrencyStamp = concurrencyStamp,
                    PhoneNumber = vm.Create.PhoneNumber.Trim(),
                    PhoneNumberConfirmed = pncb,
                    TwoFactorEnabled = tfeb,
                    LockoutEnd = null,
                    LockoutEnabled = lockoutEnabledBool,
                    AccessFailedCount = accessFailedCountInt,
                    IdServicio = vm.Create.IdServicio,
                    ID_Perfil = vm.Create.ID_Perfil,
                    Perfil = rol.Nombre.Trim(),
                    ID_Comuna_U = vm.Create.ID_Comuna_U
                };

                _context.Add(usuario);
                await _context.SaveChangesAsync();
              
                AspNetUsersExtended usuarioExtended = new AspNetUsersExtended
                {
                    Nombres = _strCase.StringCaseFormat(vm.Extended.Nombres),
                    ApellidoPaterno = _strCase.StringCaseFormat(vm.Extended.ApellidoPaterno),
                    ApellidoMaterno = _strCase.StringCaseFormat(vm.Extended.ApellidoMaterno),
                    Rut = vm.Create.UserName.ToLower().Replace(".", "").Trim(),
                    FechaNacimiento = vm.Extended.FechaNacimiento,
                    Genero = vm.Extended.Genero,
                    NacionalidadId = vm.Extended.NacionalidadId,
                };
                
                _context.Add(usuarioExtended);
                await _context.SaveChangesAsync();

                _flashMessage.Confirmation("Se ha creado al Usuario " + usuarioExtended.Nombres +" "+ usuarioExtended.ApellidoPaterno +" "+ usuarioExtended.ApellidoMaterno +" ["+ usuario.UserName + "] exitosamente!");
               
                return RedirectToAction(nameof(Index));
            }

            vm.Create.Roles = ObtenerListaRoles();
            vm.Create.Servicios = ObtenerListaServicios();
            vm.Create.Comunas = ObtenerComunasServicioSaludMetropolitanoSurOriente();
            vm.Extended.ListaNacionalidad = ObtenerNacionalidad();
            vm.Extended.ListaSexo = ObtenerGenero();

            _flashMessage.Warning("Formulario con errores, favor corregir");

            return View(vm);
        }

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            if (Perfil_U == 3)
            {

                if (id == null)
                {
                    // return NotFound();
                    _flashMessage.Danger("El usuario no existe registrado en la base de datos!");
                    return RedirectToAction(nameof(Index));
                }

                // var usuario = await _context.AspNetUsers.FirstOrDefaultAsync(m => m.Id == id);
                /*
                var usuario = (from u in _context.AspNetUsers
                               join e in _context.AspNetUsers_Extended on u.UserName equals e.Rut into leftJoin
                               from result in leftJoin.DefaultIfEmpty()
                               where u.Id == id
                               select new
                               {
                                   Id = u.Id,
                                   UserName = u.UserName,
                                   Email = u.Email,
                                   PhoneNumber = u.PhoneNumber,
                                   IdServicio = u.IdServicio,
                                   ID_Comuna_U = u.ID_Comuna_U,
                                   ID_Perfil = u.ID_Perfil,
                                   LockoutEnd = u.LockoutEnd,
                                   LockoutEnabled = u.LockoutEnabled,
                                   IdExtended = result.Id,
                                   Nombres = result.Nombres,
                                   ApellidoPaterno = result.ApellidoPaterno,
                                   ApellidoMaterno = result.ApellidoMaterno,
                                   Rut = result.Rut,
                                   FechaNacimiento = (DateTime?) result.FechaNacimiento,
                                   Genero = (int?) result.Genero,
                                   NacionalidadId = (int?) result.NacionalidadId
                               }).ToList().First();
                */
                
                var usuario = (from u in _context.AspNetUsers
                              where u.Id == id
                              select u).ToList().FirstOrDefault();

                var usuarioExtended = (from e in _context.AspNetUsers_Extended
                                       where e.Rut == usuario.UserName
                                       select e).ToList().FirstOrDefault();

                if (usuario == null)
                {
                    return NotFound();
                }

                UsuariosExtendidoEditViewModel vm = new UsuariosExtendidoEditViewModel();
                vm.Edit.Id = usuario.Id;
                vm.Edit.UserName = usuario.UserName;
                vm.Edit.Email = usuario.Email;
                vm.Edit.PhoneNumber = usuario.PhoneNumber;
                vm.Edit.IdServicio = usuario.IdServicio;
                vm.Edit.ID_Perfil = usuario.ID_Perfil;
                vm.Edit.ID_Comuna_U = usuario.ID_Comuna_U;
                vm.Edit.LockoutEnd = usuario.LockoutEnd;
                vm.Edit.LockoutEnabled = usuario.LockoutEnabled;

                if (usuarioExtended != null)
                {
                    vm.Extended.Id = usuarioExtended.Id;
                    vm.Extended.Nombres = usuarioExtended.Nombres;
                    vm.Extended.ApellidoPaterno = usuarioExtended.ApellidoPaterno;
                    vm.Extended.ApellidoMaterno = usuarioExtended.ApellidoMaterno;
                    vm.Extended.Rut = usuarioExtended.Rut;
                    vm.Extended.FechaNacimiento = usuarioExtended.FechaNacimiento;
                    vm.Extended.Genero = usuarioExtended.Genero;
                    vm.Extended.NacionalidadId = usuarioExtended.NacionalidadId;
                } 
                else
                {
                    vm.Extended.Nombres = "";
                    vm.Extended.ApellidoPaterno = "";
                    vm.Extended.ApellidoMaterno = "";
                    vm.Extended.Rut = "";
                    vm.Extended.FechaNacimiento = (DateTime?)null;
                    vm.Extended.Genero = 0;
                    vm.Extended.NacionalidadId = 0;
                }

                vm.Edit.Servicios = ObtenerListaServicios();
                vm.Edit.Roles = ObtenerListaRoles();
                vm.Edit.Comunas = ObtenerComunasServicioSaludMetropolitanoSurOriente();
                vm.Extended.ListaNacionalidad = ObtenerNacionalidad();
                vm.Extended.ListaSexo = ObtenerGenero();

                return View(vm);

            } else
            {
                _flashMessage.Danger("Acceso denegado, Usted no tiene los permisos necesarios para acceder a este Recurso!");
                return RedirectToAction("Index", "Home", null);
            }
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(string id, [Bind("Id, UserName, Email, PhoneNumber, LockoutEnd, LockoutEnabled, AccessFailedCount, IdServicio, ID_Perfil, Perfil, ID_Comuna_U")] UsuarioEditViewModel vm)
        public async Task<IActionResult> Edit(string id, UsuariosExtendidoEditViewModel vm)
        {
            if (id != vm.Edit.Id)
            {
                _flashMessage.Danger("Error: El número identificador del usuario enviado no coincide: " + id + " vs " + vm.Edit.Id);
                return RedirectToAction(nameof(Index));
            }

            var isValidRutV1b = _rutv1.verificaRut(vm.Edit.UserName);
            if (!isValidRutV1b)
            {
                _flashMessage.Danger("El Rut del usuario " + vm.Edit.UserName + " es inválido [" + isValidRutV1b + "]!");
                return RedirectToAction(nameof(Index));
            }

            var isValidRutV2a = _rutv2.ValidaRut(vm.Edit.UserName);
            if (!isValidRutV2a)
            {
                _flashMessage.Danger("El Rut del usuario " + vm.Edit.UserName + " es inválido [" + isValidRutV2a + "]!");
                return RedirectToAction(nameof(Index));
            }

            if (vm.Edit.ID_Perfil == 1)
            {

                var siUsuarioServicioRepetido = await _context.AspNetUsers.Where(d => d.IdServicio == vm.Edit.IdServicio && d.ID_Perfil == 1 && d.UserName != vm.Edit.UserName && d.LockoutEnabled == false).FirstOrDefaultAsync();

                if (siUsuarioServicioRepetido != null)
                {
                    var servicioSelect = ObtenerServicioPorId(vm.Edit.IdServicio);
                    var servicioNameSelect = servicioSelect.Servicio;
                    _flashMessage.Danger("No es posible contar con más de un usuario para un mismo Servicio de Salud (" + servicioNameSelect.ToString() + ")!");
                    return RedirectToAction(nameof(Index));
                }
            }

            if (vm.Edit.ID_Perfil == 2)
            {
                var siUsuarioComunaRepetido = await _context.AspNetUsers.Where(c => c.ID_Comuna_U == vm.Edit.ID_Comuna_U && c.ID_Perfil == 2 && c.UserName != vm.Edit.UserName && c.LockoutEnabled == false).FirstOrDefaultAsync();

                if (siUsuarioComunaRepetido != null)
                {
                    var comunaSelect = ObtenerComunaPorId(vm.Edit.ID_Comuna_U);
                    var comunaNameSelect = comunaSelect.Comuna;
                    _flashMessage.Danger("No es posible contar con más de un usuario para la misma comuna (" + comunaNameSelect.ToString() + ")!");
                    return RedirectToAction(nameof(Index));
                }
            }

            var rol = ObtenerRolePorId(vm.Edit.ID_Perfil);

            var estado = false;
            Nullable<DateTimeOffset> dateLockoutEnd = null;
            var accessFailedCount = 0;

            if (vm.Edit.LockoutEnabled)
            {
                estado = vm.Edit.LockoutEnabled;
                dateLockoutEnd = new DateTime(4999, 12, 30);
                accessFailedCount = 0;
            } else
            {
                estado = false;
                dateLockoutEnd = null;
                accessFailedCount = 0;
            }

            if (vm.Edit.ID_Perfil == 3 && vm.Edit.IdServicio == 0 && vm.Edit.ID_Comuna_U == 0)
            {
                vm.Edit.IdServicio = 9001;
                vm.Edit.ID_Comuna_U = 90101;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioBd = await _context.AspNetUsers.FindAsync(vm.Edit.Id);
                    usuarioBd.UserName = vm.Edit.UserName;
                    usuarioBd.NormalizedUserName = vm.Edit.UserName.ToUpper();
                    usuarioBd.Email = vm.Edit.Email;
                    usuarioBd.NormalizedEmail = vm.Edit.Email.ToUpper();
                    usuarioBd.PhoneNumber = vm.Edit.PhoneNumber;
                    usuarioBd.IdServicio = vm.Edit.IdServicio;
                    usuarioBd.ID_Perfil = vm.Edit.ID_Perfil;
                    usuarioBd.Perfil = rol.Nombre;      // perfilRole.Nombre;
                    usuarioBd.ID_Comuna_U = vm.Edit.ID_Comuna_U;

                    usuarioBd.LockoutEnabled = estado;
                    usuarioBd.LockoutEnd = dateLockoutEnd;
                    usuarioBd.AccessFailedCount = accessFailedCount;

                    _context.Update(usuarioBd);
                    await _context.SaveChangesAsync();

                    var usuarioExtendBd = await _context.AspNetUsers_Extended.FindAsync(vm.Extended.Id);
                    if(usuarioExtendBd != null)
                    {
                        usuarioExtendBd.Nombres = _strCase.StringCaseFormat(vm.Extended.Nombres);
                        usuarioExtendBd.ApellidoPaterno = _strCase.StringCaseFormat(vm.Extended.ApellidoPaterno);
                        usuarioExtendBd.ApellidoMaterno = _strCase.StringCaseFormat(vm.Extended.ApellidoMaterno);
                        usuarioExtendBd.Rut = vm.Edit.UserName.ToLower().Replace(".", "").Trim();
                        usuarioExtendBd.FechaNacimiento = vm.Extended.FechaNacimiento;
                        usuarioExtendBd.Genero = vm.Extended.Genero;
                        usuarioExtendBd.NacionalidadId = vm.Extended.NacionalidadId;

                        _context.Update(usuarioExtendBd);
                        await _context.SaveChangesAsync();
                        _flashMessage.Confirmation("Se ha editado al Usuario " + usuarioExtendBd.Nombres + " " + usuarioExtendBd.ApellidoPaterno + " " + usuarioExtendBd.ApellidoMaterno + " [" + usuarioBd.UserName + "] exitosamente!");
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(vm.Edit.Id))
                    {
                        _flashMessage.Danger("Ha ocurrido una excepción del tipo DbUpdateConcurrencyException, al parecer debido a que el usuario no existe en la base de datos!");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }

                var usuarioExtendBdNew = await _context.AspNetUsers_Extended.FindAsync(vm.Extended.Id);
                if (usuarioExtendBdNew == null) {

                    AspNetUsersExtended usuarioExtended = new AspNetUsersExtended
                    {
                        Nombres = _strCase.StringCaseFormat(vm.Extended.Nombres),
                        ApellidoPaterno = _strCase.StringCaseFormat(vm.Extended.ApellidoPaterno),
                        ApellidoMaterno = _strCase.StringCaseFormat(vm.Extended.ApellidoMaterno),
                        Rut = vm.Edit.UserName.ToLower().Replace(".", "").Trim(),
                        FechaNacimiento = vm.Extended.FechaNacimiento,
                        Genero = vm.Extended.Genero,
                        NacionalidadId = vm.Extended.NacionalidadId,
                    };

                    _context.Add(usuarioExtended);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Se ha editado al Usuario ***creado*** " + usuarioExtended.Nombres + " " + usuarioExtended.ApellidoPaterno + " " + usuarioExtended.ApellidoMaterno + " [" + usuarioExtended.Rut + "] exitosamente!");

                }

                    return RedirectToAction(nameof(Index));
            }

            vm.Edit.Servicios = ObtenerListaServicios();
            vm.Edit.Roles = ObtenerListaRoles();
            vm.Edit.Comunas = ObtenerComunasServicioSaludMetropolitanoSurOriente();
            vm.Extended.ListaNacionalidad = ObtenerNacionalidad();
            vm.Extended.ListaSexo = ObtenerGenero();

            return View(vm);
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            if (Perfil_U != 1 && Perfil_U != 2 && Perfil_U != 3 && Perfil_U != 4 && Perfil_U != 5 && Perfil_U != 6 && Perfil_U != 7 && Perfil_U != 8 && Perfil_U != 9 && Perfil_U != 10)
            {

                if (id == null)
                {
                    return NotFound();
                }

                var usuario = await _context.AspNetUsers.FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null)
                {
                    return NotFound();
                }

                UsuarioDeleteViewModel vm = new UsuarioDeleteViewModel();
                vm.Id = usuario.Id;
                vm.UserName = usuario.UserName;

                return View(vm);
            
            } else
            {
                _flashMessage.Danger("Acceso denegado, Usted no tiene los permisos necesarios para acceder a este Recurso!");
                return RedirectToAction("Index", "Home", null);
            }
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var usuario = await _context.AspNetUsers.FindAsync(id);

            _context.AspNetUsers.Remove(usuario);
            await _context.SaveChangesAsync();

            _flashMessage.Danger("Se ha eliminado al Usuario " + usuario.UserName + " exitosamente!");

            return RedirectToAction(nameof(Index));
        }

        // GET: Usuario/ChangePassword/5
        public IActionResult ChangePassword(string id)
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            if (Perfil_U == 3)
            {
                UsuarioChangePasswordViewModel vm = new UsuarioChangePasswordViewModel();
                vm.Id = id;
                return View(vm);

            } else
            {
                _flashMessage.Danger("Acceso denegado, Usted no tiene los permisos necesarios para acceder a este Recurso!");
                return RedirectToAction("Index", "Home", null);
            }
        }

        // POST: Usuario/ChangePassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> ChangePassword(UsuarioChangePasswordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var idUsuario = vm.Id;
                    var usuarioBd = await _context.AspNetUsers.FirstOrDefaultAsync(c => c.Id == idUsuario);

                    var passwordHasher = new PasswordHasher<Usuario>();
                    var usuario = new Usuario();
                    var hashedPassword = passwordHasher.HashPassword(usuario, vm.PasswordHash);

                    var securityStamp = Guid.NewGuid().ToString();
                    var concurrencyStamp = RandomString(32);

                    usuarioBd.PasswordHash = hashedPassword;
                    usuarioBd.SecurityStamp = securityStamp;
                    usuarioBd.ConcurrencyStamp = concurrencyStamp;

                    _context.Update(usuarioBd);
                    await _context.SaveChangesAsync();

                    _flashMessage.Confirmation("Se ha cambiado la contraseña del Usuario " + usuarioBd.UserName + " exitosamente!");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(vm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            _flashMessage.Warning("Formulario con errores, favor corregir");

            return View(vm);

        }

        // GET: Usuario/ChangeOwnPasswordByTheUser
        public IActionResult ChangeOwnPasswordByTheUser() {
            //
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            var idUsuario = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).Select(s => s.Id).First();

            UsuarioChangePasswordViewModel vm = new UsuarioChangePasswordViewModel();
            vm.Id = idUsuario;

            return View(vm);
        }

        // POST: Usuario/ChangeOwnPasswordByTheUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeOwnPasswordByTheUser([Bind("Id, PasswordHash, ConfirmPassword, SecurityStamp, ConcurrencyStamp")] UsuarioChangePasswordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var idUsuario = vm.Id;
                    var usuarioBd = await _context.AspNetUsers.FirstOrDefaultAsync(c => c.Id == idUsuario);

                    var passwordHasher = new PasswordHasher<Usuario>();
                    var usuario = new Usuario();
                    var hashedPassword = passwordHasher.HashPassword(usuario, vm.PasswordHash);

                    var securityStamp = Guid.NewGuid().ToString();
                    var concurrencyStamp = RandomString(32);

                    usuarioBd.PasswordHash = hashedPassword;
                    usuarioBd.SecurityStamp = securityStamp;
                    usuarioBd.ConcurrencyStamp = concurrencyStamp;

                    _context.Update(usuarioBd);
                    await _context.SaveChangesAsync();

                    _flashMessage.Confirmation("Se ha cambiado su contraseña exitosamente!");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(vm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "Home", null);
            }

            _flashMessage.Warning("Formulario con errores, favor corregir");

            return View(vm);
        }

        // GET Usuario/PerfilUsuario/5
        public async Task<IActionResult> PerfilUsuario()
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            var usuario = await _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();

            var usuarioExtendido = await _context.AspNetUsers_Extended.Where(w => w.Rut == usuario.UserName).FirstOrDefaultAsync();

            if (usuario == null)
            {
                _flashMessage.Confirmation("El usuario no existe registrado en el sistema!");
                return RedirectToAction("Index", "Home", null);
            }

            var servicioName = "";
            var comunaName = "";

            if (usuario.ID_Perfil == 3)
            {
                servicioName = "N/A";
                comunaName = "N/A";
            }
            else
            {
                var servicio = ObtenerServicioPorId(usuario.IdServicio);
                var comuna = ObtenerComunaPorId(usuario.ID_Comuna_U);
                servicioName = servicio.Servicio;
                comunaName = comuna.Comuna;
            }

            var rol = ObtenerRolePorId(usuario.ID_Perfil);

            UsuariosExtendidoDetailsViewModel vm = new UsuariosExtendidoDetailsViewModel();
            vm.Detalle.Id = usuario.Id;
            vm.Detalle.UserName = usuario.UserName;
            vm.Detalle.Email = usuario.Email;
            vm.Detalle.PhoneNumber = usuario.PhoneNumber;
            vm.Detalle.ServicioName = servicioName;
            vm.Detalle.ComunaName = comunaName;
            vm.Detalle.RoleName = rol.Nombre;
            vm.Detalle.LockoutEnd = usuario.LockoutEnd;
            vm.Detalle.LockoutEnabled = usuario.LockoutEnabled;

            if (usuarioExtendido != null)
            {
                var genero = ObtenerGeneroPorId(usuarioExtendido.Genero);
                var nacionalidad = ObtenerNacionalidadPorId(usuarioExtendido.NacionalidadId);

                vm.Extended.Nombres = usuarioExtendido.Nombres;
                vm.Extended.ApellidoPaterno = usuarioExtendido.ApellidoPaterno;
                vm.Extended.ApellidoMaterno = usuarioExtendido.ApellidoMaterno;
                vm.Extended.Rut = usuarioExtendido.Rut;
                vm.Extended.FechaNacimiento = usuarioExtendido.FechaNacimiento;
                vm.Extended.GeneroName = genero.Sexo;
                vm.Extended.NacionalidadName = nacionalidad.Nacionalidad;

            }
            else
            {
                var fechaZeros = new DateTime(1000, 01, 01);

                vm.Extended.Nombres = "N/A";
                vm.Extended.ApellidoPaterno = "N/A";
                vm.Extended.ApellidoMaterno = "N/A";
                vm.Extended.Rut = usuario.UserName.ToLower().Replace(".", "").Trim();
                vm.Extended.FechaNacimiento = fechaZeros;
                vm.Extended.GeneroName = "N/A";
                vm.Extended.NacionalidadName = "N/A";

                _flashMessage.Warning("Por favor completar los registros del Usuario!");
            }

            return View(vm);
        }

        private List<SelectListItem> ObtenerListaRoles()
        {
            return _context.DOTACION_Roles.OrderBy(u => u.Id)
                                        .Select(u => new SelectListItem
                                        {
                                            Value = u.Id.ToString(),
                                            Text = u.Nombre
                                        }).ToList();
        }

        private List<SelectListItem> ObtenerListaServicios()
        {
            return _context.DOTACION_Servicios.Where(w => w.ID_Servicio != 25).OrderBy(u => u.Servicio)     // menos Aisén
                                              .Select(u => new SelectListItem
                                              {
                                                  Value = u.ID_Servicio.ToString(),
                                                  Text = u.Servicio
                                              }).ToList();
        }

        private List<SelectListItem> ObtenerNacionalidad()
        {
            return _context.DOTACION_Nacionalidad.OrderBy(o => o.Nacionalidad).Select(s => new SelectListItem
            {
                Value = s.IdNacionalidad.ToString(),
                Text = s.Nacionalidad
            }).ToList();
        }

        private List<SelectListItem> ObtenerGenero()
        {
            return _context.DOTACION_Sexo.OrderBy(o => o.Sexo).Select(s => new SelectListItem
            {
                Value = s.IdSexo.ToString(),
                Text = s.Sexo
            }).ToList();
        }

        public List<SelectListItem> ObtenerComunasServicioSaludMetropolitanoSurOriente()
        {
            List<SelectListItem> comunas = new List<SelectListItem>();

            var arrayComunas = new[]
            {
                new SelectListItem{Value="13110", Text="La Florida"},
                new SelectListItem{Value="13111", Text="La Granja"},
                new SelectListItem{Value="13112", Text="La Pintana"},
                new SelectListItem{Value="13131", Text="San Ramón"},
                new SelectListItem{Value="13201", Text="Puente Alto"},
                new SelectListItem{Value="13202", Text="Pirque"},
                new SelectListItem{Value="13203", Text="San José de Maipo"},
            };

            comunas = arrayComunas.ToList();

            return comunas;
        }

        private bool UsuarioExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }

        private ListaServicios ObtenerServicioPorId(int id)
        {
            var servicio = _context.DOTACION_Servicios.FirstOrDefault(m => m.ID_Servicio == id);

            return servicio;
        }

        private ListaComunas ObtenerComunaPorId(int id)
        {
            var comuna = _context.DOTACION_Comunas.FirstOrDefault(c => c.ID_Comuna1 == id);

            return comuna;
        }

        private Role ObtenerRolePorId(int id)
        {
            var rol = _context.DOTACION_Roles.FirstOrDefault(r => r.Id == id);

            return rol;
        }

        private ListaSexo ObtenerGeneroPorId(int? id)
        {
            var genero = _context.DOTACION_Sexo.FirstOrDefault(g => g.IdSexo == id);

            return genero;
        }

        private ListaNacionalidad ObtenerNacionalidadPorId(int? id)
        {
            var nacionalidad = _context.DOTACION_Nacionalidad.FirstOrDefault(n => n.IdNacionalidad == id);

            return nacionalidad;
        }

        private static string NewSecurityStamp()
        {
            // create a stronger hash code using RNGCryptoServiceProvider
            byte[] random = new byte[25];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            // populate with random bytes
            rng.GetBytes(random);

            // RandomNumberGenerator.Fill(random);

            // convert random bytes to string
            string randomBase64 = Convert.ToBase64String(random);

            return randomBase64.ToUpper().Replace("=", string.Empty).Replace("/", string.Empty).Replace("+", string.Empty);
        }

        public string GuidNewGuidToString()
        {
            var guid = Guid.NewGuid().ToString().ToUpper().Replace("=", string.Empty).Replace("/", string.Empty).Replace("+", string.Empty).Replace("-", string.Empty);

            return guid;
        }

        static string RandomString(int length, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        // Divide the byte into allowedCharSet-sized groups. If the
                        // random value falls into the last group and the last group is
                        // too small to choose from the entire allowedCharSet, ignore
                        // the value in order to avoid biasing the result.
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString().ToUpper();
            }
        }

        // POST: Usuario/GetComunas
        [HttpPost]
        public JsonResult GetComunas([FromBody] UsuarioSelectServicioComunaViewModel model)
        {
            if (model.perfilCom == "2")
            {
                var id = int.Parse(model.idservicio);
                var comuna = _context.DOTACION_Comunas.Where(c => c.ID_Servicio == id).Select(u => new SelectListItem
                {
                    Value = u.ID_Comuna1.ToString(),
                    Text = u.Comuna
                }).OrderBy(o => o.Text).ToList();

                return Json(comuna);
            }

            return Json("Perfil Inválido");
        }

        // POST: Usuario/GetJsonUsuario
        [HttpPost]
        public JsonResult GetJsonUsuario([FromBody] UsuarioObtenerPorIdViewModel model)
        {
            var usuario = _context.AspNetUsers.FirstOrDefault(c => c.Id == model.idUsuario);

            return Json(usuario);
        }

        // POST: Usuario/GetComunaDelServicio
        [HttpPost]
        public JsonResult GetComunaDelServicio([FromBody] UsuarioSelectComunaDelServicioViewModel model)
        {
            if(model.perfilComServ == "1")
            {
                var id = int.Parse(model.idServicio);

                var comuna = _context.DOTACION_servicio_comuna.Where(c => c.IdServicio == id).Select(u => new SelectListItem
                {
                    Value = u.IdComuna.ToString(),
                    Text = u.Comuna
                }).OrderBy(o => o.Text).ToList();

                return Json(comuna);
            }

            return Json("Perfil Inválido");
        }

        // POST: Usuario/GetComunasEdit
        [HttpPost]
        public JsonResult GetComunasEdit([FromBody] UsuarioSelectServicioComunaViewModel model)
        {
            var id = int.Parse(model.idservicio);
            var comuna = _context.DOTACION_Comunas.Where(c => c.ID_Servicio == id).Select(u => new SelectListItem
            {
                Value = u.ID_Comuna1.ToString(),
                Text = u.Comuna
            }).OrderBy(o => o.Text).ToList();

            return Json(comuna);
        }

        // POST: Usuario/GetComunaDelServicioEdit
        [HttpPost]
        public JsonResult GetComunaDelServicioEdit([FromBody] UsuarioSelectComunaDelServicioViewModel model)
        {
            var id = int.Parse(model.idServicio);

            var comuna = _context.DOTACION_servicio_comuna.Where(c => c.IdServicio == id).Select(u => new SelectListItem
            {
                Value = u.IdComuna.ToString(),
                Text = u.Comuna
            }).OrderBy(o => o.Text).ToList();

            return Json(comuna);
            
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DotacionWEBCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;
using Microsoft.AspNetCore.Authorization;

namespace DotacionWEBCore.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class RolesController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IFlashMessage _flashMessage;

        public RolesController(DatabaseContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            if (Perfil_U == 3) {

                List<Role> listaRoles = await _context.DOTACION_Roles.ToListAsync();

                return View(listaRoles);

            } else
            {
                _flashMessage.Danger("Acceso denegado, Usted no tiene los permisos necesarios para acceder a este Recurso!");
                return RedirectToAction("Index", "Home", null);
            }
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            if (Perfil_U == 3)
            {

                if (id == null)
                {
                    return NotFound();
                }

                var role = await _context.DOTACION_Roles.FirstOrDefaultAsync(m => m.Id == id);

                if (role == null)
                {
                    return NotFound();
                }

                return View(role);

            } else
            {
                _flashMessage.Danger("Acceso denegado, Usted no tiene los permisos necesarios para acceder a este Recurso!");
                return RedirectToAction("Index", "Home", null);
            }
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            if (Perfil_U == 3)
            {
                return View();

            } else
            {
                _flashMessage.Danger("Acceso denegado, Usted no tiene los permisos necesarios para acceder a este Recurso!");
                return RedirectToAction("Index", "Home", null);
            }
        }

        // POST: Roles/Create
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Nombre")] Role role)
        {
            if (ModelState.IsValid)
            {
                _flashMessage.Confirmation("Se ha creado el Rol " + role.Nombre + " exitosamente!");
                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var perfilName = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == User.Identity.Name).Select(b => b.RoleName).First();
            ViewBag.Rol = perfilName;

            var Perfil_U = _context.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(b => b.ID_Perfil).First();
            ViewBag.RolId = Perfil_U.ToString();

            if (Perfil_U == 3)
            {

                if (id == null)
                {
                    return NotFound();
                }

                var role = await _context.DOTACION_Roles.FindAsync(id);

                if (role == null)
                {
                    return NotFound();
                }

                return View(role);

            } else
            {
                _flashMessage.Danger("Acceso denegado, Usted no tiene los permisos necesarios para acceder a este Recurso!");
                return RedirectToAction("Index", "Home", null);
            }
        }

        // POST: Roles/Edit/5
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Nombre")] Role role)
        {
            if(id != role.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _flashMessage.Info("Se ha editado el Rol " + role.Nombre + " exitosamente!");
                    _context.Update(role);
                    await _context.SaveChangesAsync();

                } catch(DbUpdateConcurrencyException)
                {
                    if(!RoleExists(role.Id))
                    {
                        return NotFound();
                    } else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

                var role = await _context.DOTACION_Roles.FirstOrDefaultAsync(m => m.Id == id);

                if (role == null)
                {
                    return NotFound();
                }

                return View(role);

            } else
            {
                _flashMessage.Danger("Acceso denegado, Usted no tiene los permisos necesarios para acceder a este Recurso!");
                return RedirectToAction("Index", "Home", null);
            }
        }

        // POST: Roles/Delete/5
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.DOTACION_Roles.FindAsync(id);

            _flashMessage.Danger("Se ha eliminado el Rol " + role.Nombre + " exitosamente!");

            _context.DOTACION_Roles.Remove(role);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
            return _context.DOTACION_Roles.Any(e => e.Id == id);
        }

    }
}
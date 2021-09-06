using DotacionWEBCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using DotacionWEBCore.Areas.Identity.Data;

namespace DotacionWEBCore.Helpers.Perfil
{
    public class PerfilUsuario : IPerfilUsuario
    {
        private readonly DatabaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<DotacionWEBCoreUser> _userManager;

        private readonly string _userName;

        public PerfilUsuario(DatabaseContext context, IHttpContextAccessor httpContextAccessor, UserManager<DotacionWEBCoreUser> userManager) {

            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;

            _userName = _httpContextAccessor.HttpContext.User.Identity.Name;
        }

        public string PerfilNombre()
        {
            var perfilNombre = _context.DOTACION_Usuarios_Full.Where(s => s.UserName == _userName).Select(b => b.RoleName).First();

            return perfilNombre;
        }

        public int PerfilId()
        {
            var perfilId = _context.AspNetUsers.Where(s => s.UserName == _userName).Select(b => b.ID_Perfil).First();

            return perfilId;
        }

    }
}

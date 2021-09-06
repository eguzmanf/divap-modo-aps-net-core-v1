using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.ViewModels
{
    public class UsuarioDetailsViewModel : IdentityUser
    {
        [Display(Name = "Correo")]
        public override string Email { get; set; }

        [Display(Name = "Usuario")]
        public override string UserName { get; set; }

        [Display(Name = "Teléfono")]
        public override string PhoneNumber { get; set; }

        [Display(Name = "Servicio de Salud")]
        public string ServicioName { get; set; }

        [Display(Name = "Comuna")]
        public string ComunaName { get; set; }

        [Display(Name = "Perfil")]
        public string RoleName { get; set; }

        [Display(Name = "Estado")]
        public override bool LockoutEnabled { get; set; }

    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.ViewModels
{
    public class UsuarioDeleteViewModel : IdentityUser
    {
        [Display(Name = "Nombre de Usuario (Rut)")]
        public override string UserName { get; set; }
    }
}

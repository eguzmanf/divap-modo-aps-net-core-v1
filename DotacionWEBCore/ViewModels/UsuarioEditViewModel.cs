using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.ViewModels
{
    public class UsuarioEditViewModel : IdentityUser
    {
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "El Correo es requerido.")]
        [EmailAddress(ErrorMessage = "El Correo debe tener un formato válido.")]
        public override string Email { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El Usuario es requerido.")]
        [StringLength(10, ErrorMessage = "El {0} debe tener entre {2} y {1} dígitos.", MinimumLength = 9)]
        public override string UserName { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El Teléfono es requerido.")]
        [StringLength(9, ErrorMessage = "El {0} debe tener {2} dígito.", MinimumLength = 9)]
        [RegularExpression(@"^([2-9]{1})(\d{8})", ErrorMessage = "Por favor ingrese un número telefónico válido.")]
        public override string PhoneNumber { get; set; }

        [Display(Name = "Servicio de Salud")]
        [Required(ErrorMessage = "El Servicio de Salud es requerido.")]
        public int IdServicio { get; set; }

        [Display(Name = "Perfil")]
        [Required(ErrorMessage = "El Perfil es requerido.")]
        public int ID_Perfil { get; set; }

        public string Perfil { get; set; }

        [Display(Name = "Comuna")]
        [Required(ErrorMessage = "La Comuna es requerida.")]
        public int ID_Comuna_U { get; set; }

        [Display(Name = "Estado")]
        public override bool LockoutEnabled { get; set;}

        public List<SelectListItem> Roles { get; set; }
        public List<SelectListItem> Servicios { get; set; }
        public List<SelectListItem> Comunas { get; set; }
    }
}

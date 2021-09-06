using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.ViewModels
{
    public class UsuarioChangePasswordViewModel : IdentityUser
    {
        public override string Id { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La Contraseña es requerida.")]
        [StringLength(12, ErrorMessage = "La {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 8)]
        [RegularExpression("^[a-zA-ZñÑ0-9]*$", ErrorMessage = "Solo se permiten letras y/o números.")]
        [DataType(DataType.Password)]
        public override string PasswordHash { get; set; }

        [Display(Name = "Confirmar Contraseña")]
        [Required(ErrorMessage = "La confirmación de la Contraseña es requerida.")]
        [StringLength(12, ErrorMessage = "La {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 8)]
        [Compare("PasswordHash", ErrorMessage = "La Contraseña y Confirmar Contraseña deben ser iguales.")]
        public string ConfirmPassword { get; set; }
    }
}

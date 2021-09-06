using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Display(Name = "Perfil")]
        [Required(ErrorMessage = "El Perfil es requerido.")]
        public string Nombre { get; set; }
    }
}

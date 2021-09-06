using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.Models
{
    public class UsuarioFull : IdentityUser
    {
        [Display(Name = "Usuario")]
        public override string UserName { get; set; }

        [Display(Name = "Correo")]
        public override string Email { get; set; }

        [Display(Name = "Teléfono")]
        public override string PhoneNumber { get; set; }

        public int IdServicio { get; set; }
        public int ID_Perfil { get; set; }
        public string Perfil { get; set; }
        public int ID_Comuna_U { get; set; }

        [Display(Name = "Estado")]
        public override bool LockoutEnabled { get; set; }

        [Display(Name = "Servicio de Salud")]
        public string ServicioName { get; set; }

        [Display(Name = "Comuna")]
        public string ComunaName { get; set; }

        [Display(Name = "Perfil")]
        public string RoleName { get; set; }

        [Display(Name = "Nombres")]
        public string UsuarioNombre { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; }

        public string Rut { get; set; }

        [Display(Name = "Fecha Nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> FechaNacimiento { get; set; }

        public int? Genero { get; set; }

        [Display(Name = "Género")]
        public string Sexo { get; set; }

        public Nullable<int> NacionalidadId { get; set; }

        [Display(Name = "Nacionalidad")]
        public string Nacionalidad { get; set; }

    }
}

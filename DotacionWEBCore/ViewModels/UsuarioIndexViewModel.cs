using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.ViewModels
{
    public class UsuarioIndexViewModel : IdentityUser
    {
        public override string UserName { get; set; }

        public override string Email { get; set; }

        public override string PhoneNumber { get; set; }

        public int IdServicio { get; set; }

        public string ServicioName { get; set; }

        public int ID_Comuna_U { get; set; }

        public string ComunaName { get; set; }

        public int ID_Perfil { get; set; }

        public string Perfil { get; set; }

        public string RoleName { get; set; }

        public string UsuarioNombre { get; set; }

        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }

        public string Rut { get; set; }

        public Nullable<DateTime> FechaNacimiento { get; set; }

        public int? Genero { get; set; }

        public string Sexo { get; set; }

        public Nullable<int> NacionalidadId { get; set; }

        public string Nacionalidad { get; set; }
    }
}

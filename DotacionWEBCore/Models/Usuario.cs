using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.Models
{
    public class Usuario : IdentityUser
    {
        public int IdServicio { get; set; }
        public int ID_Perfil { get; set; }
        public string Perfil { get; set; }
        public int ID_Comuna_U { get; set; }
        
    }
}

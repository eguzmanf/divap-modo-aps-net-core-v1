using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DotacionWEBCore.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the DotacionWEBCoreUser class
    public class DotacionWEBCoreUser : IdentityUser
    {
        public int IdServicio { get; set; }
        public int ID_Perfil { get; set; }
        public string Perfil { get; set; }
        public int ID_Comuna_U { get; set; }


    }

    public class ApplicationUser : IdentityUser
    {
        public int IdServicio { get; set; }
        public int ID_Perfil { get; set; }
        public string Perfil { get; set; }
        public int ID_Comuna_U { get; set; }
    }
}

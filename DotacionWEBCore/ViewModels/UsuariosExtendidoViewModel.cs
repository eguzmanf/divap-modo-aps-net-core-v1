using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.ViewModels
{
    public class UsuariosExtendidoViewModel
    {
        public UsuariosExtendidoViewModel()
        {
            this.Create = new UsuarioCreateViewModel();
            this.Extended = new AspNetUsersExtendedViewModel();
        }

        public UsuarioCreateViewModel Create { get; set; }

        public AspNetUsersExtendedViewModel Extended { get; set; }
    }
}

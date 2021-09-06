using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.ViewModels
{
    public class UsuariosExtendidoDetailsViewModel
    {
        public UsuariosExtendidoDetailsViewModel()
        {
            this.Detalle = new UsuarioDetailsViewModel();
            this.Extended = new AspNetUsersExtendedViewModel();
        }

        public UsuarioDetailsViewModel Detalle { get; set; }

        public AspNetUsersExtendedViewModel Extended { get; set; }
    }
}

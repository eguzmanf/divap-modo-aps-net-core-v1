using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.ViewModels
{
    public class UsuariosExtendidoEditViewModel
    {
        public UsuariosExtendidoEditViewModel()
        {
            this.Edit = new UsuarioEditViewModel();
            this.Extended = new AspNetUsersExtendedViewModel();
        }

        public UsuarioEditViewModel Edit { get; set; }

        public AspNetUsersExtendedViewModel Extended { get; set; }
    }
}

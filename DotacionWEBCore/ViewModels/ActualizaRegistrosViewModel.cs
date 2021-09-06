using DotacionWEBCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.ViewModels
{
    public class ActualizaRegistrosViewModel
    {
        public ActualizaRegistrosViewModel()
        {
            this.Registros = new ListaRegistros();
            this.ListadoX = new ListadoViewModel<DotacionWEBCore.Models.ListaRegistros>();
        }

        public ListaRegistros Registros { get; set; }

        public ListadoViewModel<DotacionWEBCore.Models.ListaRegistros> ListadoX { get; set; }
    }
}

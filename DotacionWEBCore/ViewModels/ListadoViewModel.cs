using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace DotacionWEBCore.ViewModels
{
    public class ListadoViewModel<T>
    {
        public string TerminoBusqueda { get; set; }

        public int? Pagina { get; set; }

        public IPagedList<T> Registros { get; set; }

    }
}

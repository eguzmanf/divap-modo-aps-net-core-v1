using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.Models
{
    public class ComunaDelServicio
    {
        [Key]
        public int Id { get; set; }
        public int IdServicio { get; set; }
        public string Servicio { get; set; }
        public int IdComuna { get; set; }
        public string Comuna { get; set; }
        public int ID_Perfil { get; set; }
        public string Perfil { get; set; }
    }
}

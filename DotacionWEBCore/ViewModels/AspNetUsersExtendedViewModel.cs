using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.ViewModels
{
    public class AspNetUsersExtendedViewModel
    {
        // [Key]
        public int Id { get; set; }

        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "Los Nombres son requeridos.")]
        [StringLength(80, ErrorMessage = "Los {0} deben tener entre {2} y {1} caracteres.", MinimumLength = 2)]
        [RegularExpression(@"^([a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]+\s)*[a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]+$", ErrorMessage = "Solo se permiten letras y un espacio entre palabras.")]
        public string Nombres { get; set; }

        [Display(Name = "Apellido Paterno")]
        [Required(ErrorMessage = "El Apellido Paterno es requerido.")]
        [StringLength(80, ErrorMessage = "El {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 2)]
        [RegularExpression(@"^([a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]+\s)*[a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]+$", ErrorMessage = "Solo se permiten letras y un espacio entre palabras.")]
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        [Required(ErrorMessage = "El Apellido Materno es requerido.")]
        [StringLength(80, ErrorMessage = "El {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 2)]
        [RegularExpression(@"^([a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]+\s)*[a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]+$", ErrorMessage = "Solo se permiten letras y un espacio entre palabras.")]
        public string ApellidoMaterno { get; set; }

        [Display(Name = "Run")]
        public string Rut { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [Required(ErrorMessage = "La Fecha de Nacimiento es requerida.")]
        //  [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4} (2[0-3]|[0-1][0-9]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "La Fecha de Nacimiento debe tener un formato válido (DD/MM/YYYY).")]
        //  [RegularExpression(@"^\s*(3[01]|[12][0-9]|0[0-9])\/(1[012]|0[0-9])\/([0-9]{4}) [012]{0,1}[0-3]:[0-5][0-9]:[0-5][0-9]\s*$", ErrorMessage = "La Fecha de Nacimiento debe tener un formato válido (DD/MM/YYYY).")]
        //  [RegularExpression(@"^([0][0-9]|[1|2][0-9]|[3][0|1])[\/]([0][0-9]|[1][0-2])[\/]([0-9]{4}) (2[0-3]|[0-1][0-9]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "La Fecha de Nacimiento debe tener un formato válido (DD/MM/YYYY).")]
        //  [RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[0-9]|1[0-2])\/([0-9]{4})) [012]{0,1}[0-3]:[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "La Fecha de Nacimiento debe tener un formato válido (DD/MM/YYYY).")]
        [DataType(DataType.Date)]
        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaNacimiento { get; set; }

        [Display(Name = "Género")]
        [Required(ErrorMessage = "El Género es requerido.")]
        public int? Genero { get; set; }

        [Display(Name = "Nacionalidad")]
        [Required(ErrorMessage = "La Nacionalidad es requerida.")]
        public int? NacionalidadId { get; set; }

        [Display(Name = "Género")]
        public string GeneroName { get; set; }

        [Display(Name = "Nacionalidad")]
        public string NacionalidadName { get; set; }

        public List<SelectListItem> ListaSexo { get; set; }
        public List<SelectListItem> ListaNacionalidad { get; set; }
    }
}

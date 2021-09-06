using DotacionWEBCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.ViewModels
{
    public class EditaRegistrosViewModel
    {
        public int ID_Registro { get; set; }

        [Display(Name = "Servicio de Salud:")]
        [Required(ErrorMessage = "El Servicio de Salud es requerido.")]
        public int ID_Servicio { get; set; }

        public string Servicio { get; set; }

        [Display(Name = "Comuna:")]
        [Required(ErrorMessage = "La Comuna es requerida.")]
        public int ID_Comuna { get; set; }

        public string Comuna { get; set; }

        [Display(Name = "Establecimiento:")]
        [Required(ErrorMessage = "El Establecimiento es requerido.")]
        public int ID_Establecimiento { get; set; }

        public string Establecimiento { get; set; }

        [Display(Name = "Administración Salud:")]
        [Required(ErrorMessage = "La Administración es requerida.")]
        public string Administracion { get; set; }

        [Display(Name = "RUN (Sin puntos):")]
        [Required(ErrorMessage = "El RUN es requerido.")]
        [StringLength(8, ErrorMessage = "El {0} no debe tener mas de {1} caracteres.")]
        public string Rut { get; set; }

        [Display(Name = "-")]
        [Required(ErrorMessage = "El Dígito Verificador es requerido.")]
        public string DV { get; set; }

        [Display(Name = "Apellido Paterno:")]
        [Required(ErrorMessage = "El Apellido Paterno es requerido.")]
        [StringLength(40, ErrorMessage = "El {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 2)]
        [RegularExpression(@"^([a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]+\s)*[a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]+$", ErrorMessage = "Solo se permiten letras y un espacio entre palabras.")]
        public string Apellido_Paterno { get; set; }

        [Display(Name = "Apellido Materno:")]
        [Required(ErrorMessage = "El Apellido Materno es requerido.")]
        [StringLength(40, ErrorMessage = "El {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 2)]
        [RegularExpression(@"^([a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]+\s)*[a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]+$", ErrorMessage = "Solo se permiten letras y un espacio entre palabras.")]
        public string Apellido_Materno { get; set; }

        [Display(Name = "Nombres:")]
        [Required(ErrorMessage = "Los Nombres son requeridos.")]
        [StringLength(40, ErrorMessage = "Los {0} deben tener entre {2} y {1} caracteres.", MinimumLength = 2)]
        [RegularExpression(@"^([a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]+\s)*[a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]+$", ErrorMessage = "Solo se permiten letras y un espacio entre palabras.")]
        public string Nombre { get; set; }

        [Display(Name = "Sexo:")]
        [Required(ErrorMessage = "El Sexo es requerido.")]
        public string Sexo { get; set; }

        [Display(Name = "Fecha de Nacimiento:")]
        [Required(ErrorMessage = "La Fecha de Nacimiento es requerida.")]
        [DataType(DataType.Date)]
        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Fecha_Nacimiento { get; set; }

        [Display(Name = "Nacionalidad:")]
        [Required(ErrorMessage = "La Nacionalidad es requerida.")]
        public string Nacionalidad { get; set; }

        [Display(Name = "Categoría:")]
        [Required(ErrorMessage = "La Categoría es requerida.")]
        public string Categoria { get; set; }

        [Display(Name = "Profesión:")]
        [Required(ErrorMessage = "La Profesión es requerida.")]
        public string Profesion { get; set; }

        [Display(Name = "Especialidad:")]
        [Required(ErrorMessage = "La Especialidad es requerida.")]
        public string Especialidad { get; set; }

        [Display(Name = "Cargo:")]
        [Required(ErrorMessage = "El Cargo es requerido.")]
        public string Cargo { get; set; }

        [Display(Name = "Asignación Chofer:")]
        [Required(ErrorMessage = "La Asignación Chofer es requerida.")]
        public string Funciones_Chofer { get; set; }

        [Display(Name = "Ley:")]
        [Required(ErrorMessage = "La Ley es requerida.")]
        public string Ley { get; set; }

        [Display(Name = "Tipo Contrato:")]
        [Required(ErrorMessage = "El Tipo Contrato es requerido.")]
        public string Tipo_contrato { get; set; }

        [Display(Name = "Jornada Laboral:")]
        [Required(ErrorMessage = "La Jornada Laboral es requerida.")]
        [Range(1, 44, ErrorMessage = "La {0} debe tener un valor entre {1} y {2} horas.")]
        [RegularExpression("([0-9]{1,2})", ErrorMessage = "La Jornada Laboral no debe tener mas de 2 caracteres.")]
        // [StringLength(2, ErrorMessage = "La {0} no debe tener mas de {1} caracteres.")]
        public decimal Jornada { get; set; }

        [Display(Name = "Fecha Ingreso dotación comunal:")]
        [Required(ErrorMessage = "La Fecha de Ingreso es requerida.")]
        [DataType(DataType.Date)]
        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Fecha_Ingreso { get; set; }

        [Display(Name = "Años de Servicio:")]
        [Required(ErrorMessage = "Los Años de Servicio son requeridos.")]
        [Range(1, 60, ErrorMessage = "Los {0} deben tener un valor entre {1} y {2} años.")]
        [RegularExpression("([0-9]{1,2})", ErrorMessage = "Los Años de Servicio no deben tener mas de 2 caracteres.")]
        // [StringLength(2, ErrorMessage = "Los {0} no deben tener mas de {1} caracteres.")]
        public string Anos_Servicio { get; set; }

        [Display(Name = "Bienios:")]
        [Required(ErrorMessage = "Los Bienios son requeridos.")]
        public string Bienios { get; set; }

        [Display(Name = "Nivel Carrera:")]
        [Required(ErrorMessage = "El Nivel Carrera es requerido.")]
        public string Nivel_Carrera { get; set; }

        [Display(Name = "Sueldo Base Comunal:")]
        [Required(ErrorMessage = "El Sueldo Base Comunal es requerido.")]
        // [RegularExpression("[0-9]{1,2}(.[0-9]{3})(.[0-9]{3})", ErrorMessage = "El Total Haberes no debe tener mas de 10 caracteres.")]
        // [Range(0, 20000000, ErrorMessage = "El {0} debe tener un valor máximo de {2}.")]
        // [RegularExpression("([0-9]{1,11})", ErrorMessage = "El Sueldo Base Comunal no debe tener mas de 10 caracteres.")]
        // [StringLength(8, ErrorMessage = "El {0} no debe tener mas de {1} caracteres.")]
        public string SBase_Comunal { get; set; }

        [Display(Name = "Total Haberes (Sueldo Bruto):")]
        [Required(ErrorMessage = "El Total Haberes es requerido.")]
        // [RegularExpression("[0-9]{1,2}(.[0-9]{3})(.[0-9]{3})", ErrorMessage = "El Total Haberes no debe tener mas de 10 caracteres.")]
        // [Range(0, 20000000, ErrorMessage = "El {0} debe tener un valor máximo de {2}.")]
        // [RegularExpression("([0-9]{1,11})", ErrorMessage = "El Total Haberes no debe tener mas de 10 caracteres.")]
        // [StringLength(8, ErrorMessage = "El {0} no debe tener mas de {1} caracteres.")]
        public string Haberes { get; set; }

        [Display(Name = "Previsión:")]
        [Required(ErrorMessage = "La Previsión es requerida.")]
        public string Tipo_Prevision { get; set; }

        [Display(Name = "Isapre:")]
        [Required(ErrorMessage = "La Isapre es requerida.")]
        public string Tipo_Isapre { get; set; }
        
        public DateTime Fecha_Carga { get; set; }
        public string usuario { get; set; }
        public int Validado { get; set; }
        public string ValidadoTexto { get; set; }
        public int Activo { get; set; }
        public string ID { get; set; }

        // [Display(Name = "Fecha de Nacimiento Texto:")]
        // [Required(ErrorMessage = "La Fecha de Nacimiento es requerida.")]
        // [DataType(DataType.Text)]
        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public string Fecha_Nacimiento_Texto { get; set; }

        public string Fecha_Ingreso_Texto { get; set; }
        public int Revisado { get; set; }
        public string RevisadoTexto { get; set; }

        // public ICollection<Registros> ListaModelo { get; internal set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
        // public DbSet<ListaRegistros> DOTACION_Registros_resultados { get; set; }

        public List<SelectListItem> ServiciosList { get; set; }
        public List<SelectListItem> ComunasList { get; set; }
        public List<SelectListItem> EstablecimientoList { get; set; }
        public List<SelectListItem> AdministracionList { get; set; }
        public List<SelectListItem> DigitoVerificadorList { get; set; }
        public List<SelectListItem> GeneroList { get; set; }
        public List<SelectListItem> NacionalidadList { get; set; }
        public List<SelectListItem> LeyList { get; set; }
        public List<SelectListItem> TipoContratoList { get; set; }
        public List<SelectListItem> CategoriaList { get; set; }
        public List<SelectListItem> NivelCarreraList { get; set; }
        public List<SelectListItem> ProfesionList { get; set; }
        public List<SelectListItem> EspecialidadList { get; set; }
        public List<SelectListItem> CargoList { get; set; }
        public List<SelectListItem> FuncionesChoferList { get; set; }
        public List<SelectListItem> BieniosList { get; set; }
        public List<SelectListItem> TipoPrevisionList { get; set; }
        public List<SelectListItem> TipoIsapreList { get; set; }
    }
}

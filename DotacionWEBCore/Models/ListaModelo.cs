
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DotacionWEBCore.Models
{

    public class ListaUsuarios : IdentityUser
    {
        //[Key]
        //public int Id { get; set; }
        public override string UserName { get; set; }
        public override string Email { get; set; }
        public override string PhoneNumber { get; set; }
        //public string NormalizedUserName { get; set; }
        //public string NormalizedEmail { get; set; }
        //public int IdServicio { get; set; }
        //public int ID_Comuna_U { get; set; }
        //public int ID_Perfil { get; set; }
        public int IdServicio { get; set; }
        public int ID_Perfil { get; set; }
        public string Perfil { get; set; }
        public int ID_Comuna_U { get; set; }
        // public virtual ICollection<Registros> AspNetUsers { get; set; }

        
    }


    public class ListaModelo
    {
        public int IdOrdenServicio { get; set; }
        [Key]
        public int ID_Servicio { get; set; }
        public string Servicio { get; set; }
        public int ID_Comuna { get; set; }
        public string Comuna { get; set; }
        public string IdServicioIdComuna { get; set; }
        public int CodigoNuevo { get; set; }
        public int CodigoNuevoMadre { get; set; }
        public string ID_Establecimiento { get; set; }
        public string Establecimiento { get; set; }
        public int ID_EstablecimientoMadre { get; set; }
        public string EstablecimientoMadre { get; set; }
        public int IdTipoEstablecimiento { get; set; }
        public string Tipo_Establecimiento { get; set; }
        public string DV { get; set; }
        public string Administracion { get; set; }
        public string Sexo { get; set; }
        public string Nacionalidad { get; set; }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        public int IdProfesion { get; set; }
        public string Profesion { get; set; }
        public string Especialidad { get; set; }
        public string Ley { get; set; }
        public int IdContrato { get; set; }
        public string Tipo_contrato { get; set; }
        public string Funciones_Chofer { get; set; }
        public string InscripcionSS { get; set; }
        public string Cargo { get; set; }
        public string Anos_Servicio { get; set; }
        public string Bienios { get; set; }
        public string Nivel_Carrera { get; set; }
        public string Tipo_Prevision { get; set; }
        public string Tipo_Isapre { get; set; }
        public string Fecha_Nacimiento_Texto { get; set; }
        public string Fecha_Ingreso_Texto { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaServicios
    {
        [Key]
        public int IdOrdenServicio { get; set; }
        public int ID_Servicio { get; set; }
        public string Servicio { get; set; }
        [NotMapped]
        public int ID_Comuna { get; set; }
        [NotMapped]
        public int CodigoNuevo { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaComunas
    {
        [Key]
        public int ID_Comuna { get; set; }
        public int ID_Servicio { get; set; }
        public string Comuna { get; set; }
        public int ID_Comuna1 { get; set; }
        public string Servicio { get; set; }
        public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaEstablecimientos
    {
        [Key]
        public int CodigoNuevo { get; set; }
        public string Establecimiento { get; set; }
        public int ID_Servicio { get; set; }
        public int ID_Comuna { get; set; }
        public int ID_Comuna1 { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaEstablecimientosMadres
    {
        [Key]
        public int CodigoNuevoMadre { get; set; }
        public string EstablecimientoMadre { get; set; }
        public int IdServicio { get; set; }
        public int IdComuna { get; set; }
        public int IdComuna1 { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaTipoEstablecimiento
    {
        [Key]
        public int IdTipoEstablecimiento { get; set; }
        public string Tipo_Establecimiento { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaAdministracion
    {
        [Key]
        public int IdAdministracion { get; set; }
        public string Administracion { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaSexo
    {
        [Key]
        public int IdSexo { get; set; }
        public string Sexo { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaNacionalidad
    {
        [Key]
        public int IdNacionalidad { get; set; }
        public string Nacionalidad { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }


    public class ListaCategorias
    {
        [Key]
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaProfesiones
    {
        [Key]
        public int IdProfesion { get; set; }
        public string Profesion { get; set; }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaEspecialidad
    {
        [Key]
        public int IdEspecialidad { get; set; }
        public string Especialidad { get; set; }
        public int IdProfesion { get; set; }
        public string Profesion { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaLey
    {
        [Key]
        public int IdLey { get; set; }
        public string Ley { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaContratos
    {
        [Key]
        public int IdContrato { get; set; }
        public string Tipo_contrato { get; set; }
        public int IdLey { get; set; }
        public string Ley { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaChofer
    {
        [Key]
        public int IdFunciones_Chofer { get; set; }
        public string Funciones_Chofer { get; set; }
        public string Cargo { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaInscripcionSS
    {
        [Key]
        public int IdInscripcionSS { get; set; }
        public string InscripcionSS { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaDV
    {
        [Key]
        public int IdDV { get; set; }
        public string DV { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaCargo
    {
        [Key]
        public int IdCargo { get; set; }
        public string Cargo { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }


    public class ListaAñosServicio
    {
        [Key]
        public int IdAnos_Servicio { get; set; }
        public string Anos_Servicio { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }


    public class ListaBienios
    {
        [Key]
        public int IdBienios { get; set; }
        public string Bienios { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }


    public class ListaNivelCarrera
    {
        [Key]
        public int IdNivel_Carrera { get; set; }
        public string Nivel_Carrera { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaTipoPrevision
    {
        [Key]
        public int IdTipo_Prevision { get; set; }
        public string Tipo_Prevision { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaTipoIsapre
    {
        [Key]
        public int IdTipo_Isapre { get; set; }
        public string Tipo_Isapre { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaRegistros
    {
        [Key]
        public int ID_Registro { get; set; }
        public int ID_Servicio { get; set; }
        public string Servicio { get; set; }
        public int ID_Comuna { get; set; }
        public string Comuna { get; set; }
        public int ID_Establecimiento { get; set; }
        public string Establecimiento { get; set; }
        public string Administracion { get; set; }
        public string Rut { get; set; }
        public string DV { get; set; } 
        public string Apellido_Paterno { get; set; }
        public string Apellido_Materno { get; set; }
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        // [NotMapped]
        [DataType(DataType.Date)]
        public DateTime Fecha_Nacimiento { get; set; }
        public string Nacionalidad { get; set; }
        public string Categoria { get; set; }
        public string Profesion { get; set; }
        public string Especialidad { get; set; }
        public string Cargo { get; set; }
        public string Funciones_Chofer { get; set; }
        public string Ley { get; set; }
        public string Tipo_contrato { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal Jornada { get; set; }
        
        // [NotMapped]
        [DataType(DataType.Date)]
        public DateTime Fecha_Ingreso { get; set; }
        public string Anos_Servicio { get; set; }
        public string Bienios { get; set; }
        public string Nivel_Carrera { get; set; }

        // [DisplayFormat(DataFormatString = "{0:n}", ApplyFormatInEditMode = true)]
        public string SBase_Comunal { get; set; }

        public string Haberes { get; set; }
        public string Tipo_Prevision { get; set; }
        public string Tipo_Isapre { get; set; }
        public DateTime Fecha_Carga { get; set; }
        public string usuario { get; set; }
        public int Validado { get; set; }
        public string ValidadoTexto { get; set; }
        public int Activo { get; set; }
        public string ID { get; set; }
        public string Fecha_Nacimiento_Texto { get; set; }
        public string Fecha_Ingreso_Texto { get; set; }
        public int Revisado { get; set; }
        public string RevisadoTexto { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaRegistrosIR
    {
        [Key]
        public int ID_Registro { get; set; }
        public int ID_Servicio { get; set; }
        public string Servicio { get; set; }
        public int ID_Comuna { get; set; }
        public string Comuna { get; set; }
        public int ID_Establecimiento { get; set; }
        public string Establecimiento { get; set; }
        public string Administracion { get; set; }
        public string Rut { get; set; }
        public string DV { get; set; }
        public string Apellido_Paterno { get; set; }
        public string Apellido_Materno { get; set; }
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        [NotMapped]
        public Date Fecha_Nacimiento { get; set; }
        public string Nacionalidad { get; set; }
        public string Categoria { get; set; }
        public string Profesion { get; set; }
        public string Especialidad { get; set; }
        public string Cargo { get; set; }
        public string Funciones_Chofer { get; set; }
        public string Ley { get; set; }
        public string Tipo_contrato { get; set; }
        public decimal Jornada { get; set; }
        [NotMapped]
        public Date Fecha_Ingreso { get; set; }
        public string Anos_Servicio { get; set; }
        public string Bienios { get; set; }
        public string Nivel_Carrera { get; set; }
        public string SBase_Comunal { get; set; }
        public string Haberes { get; set; }
        public string Tipo_Prevision { get; set; }
        public string Tipo_Isapre { get; set; }
        public DateTime Fecha_Carga { get; set; }
        public string usuario { get; set; }
        public int Validado { get; set; }
        public string ValidadoTexto { get; set; }
        public int Activo { get; set; }
        public string ID { get; set; }
        public string Fecha_Nacimiento_Texto { get; set; }
        public string Fecha_Ingreso_Texto { get; set; }
        public int Revisado { get; set; }
        public string RevisadoTexto { get; set; }
        public int PostulaRetiro { get; set; }
        public string PostulaRetiroTexto { get; set; }
        // public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }

    public class ListaRegistrosSS
    {
        [Key]
        public int ID_Registro { get; set; }

        public string Servicio { get; set; }

        public virtual ICollection<Registros> DOTACION_Registros { get; set; }
    }


}
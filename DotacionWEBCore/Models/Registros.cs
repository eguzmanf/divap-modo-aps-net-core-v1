using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotacionWEBCore.Models
{
    public class Registros
    {
        public int ID_Registro { get; set; }
        public int ID_Servicio { get; set; }
        public int ID_Comuna { get; set; }
        public int ID_Establecimiento { get; set; }
        public string ID_Establecimiento_Madre { get; set; }
        public string Tipo_Establecimiento { get; set; }
        public string Administracion { get; set; }
        public string Rut { get; set; }
        public string DV { get; set; }
        public string Apellido_Paterno { get; set; }
        public string Apellido_Materno { get; set; }
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        // [NotMapped]
        [DataType(DataType.Date)]
        public DateTime? Fecha_Nacimiento { get; set; }
        public string Nacionalidad { get; set; }
        public string Categoria { get; set; }
        public string Profesion { get; set; }
        public string Especialidad { get; set; }
        public string Cargo { get; set; }
        public string Funciones_Chofer { get; set; }
        public string Ley { get; set; }
        public string Tipo_contrato { get; set; }
        public decimal Jornada { get; set; }
        // public string Id_Cont { get; set; }
        // [NotMapped]
        [DataType(DataType.Date)]
        public DateTime? Fecha_Ingreso { get; set; }
        public string Anos_Servicio { get; set; }
        public string Bienios { get; set; }
        public string Nivel_Carrera { get; set; }
        public string SBase_Comunal { get; set; }
        public string Haberes { get; set; }
        public string Tipo_Prevision { get; set; }
        public string Tipo_Isapre { get; set; }
        public DateTime? Fecha_Carga { get; set; }
        public string usuario { get; set; }
        public int? Validado { get; set; }
        public int? Activo { get; set; }
        public DateTime? Fecha_Validacion { get; set; }
        public string usuario_validador { get; set; }
        public string Ingreso_registro { get; set; }
        public int? Revisado { get; set; }
        public DateTime? Fecha_Revision { get; set; }
        public string usuario_revisor { get; set; }
        public int? PostulaRetiro { get; set; }
        public DateTime? Fecha_Postulacion { get; set; }
        public string usuario_postulador { get; set; }

        // public string Tipo_Contrato { get; set; }
        // public ICollection<Registros> ListaModelo { get; internal set; }
    }

    public class RegistrosVista
    {
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
        public virtual ICollection<Registros> ListaModelo { get; internal set; }

        public class Usuarios
        {
            public int ID { get; set; }
            public string UserName { get; set; }
            public string NormalizedUserName { get; set; }
            public string Email { get; set; }
            public string NormalizedEmail { get; set; }
            public string EmailConfirmed { get; set; }
            public int IdServicio { get; set; }
            public int ID_Perfil { get; set; }
            public string Perfil { get; set; }
            public int ID_Comuna_U { get; set; }
            public ICollection<Registros> ListaModelo { get; internal set; }
        }


    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.Models
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<ListaUsuarios> AspNetUsers { get; set; }
        public DbSet<UsuarioFull> DOTACION_Usuarios_Full { get; set; }
        public DbSet<ComunaDelServicio> DOTACION_servicio_comuna { get; set; }
        public DbSet<AspNetUsersExtended> AspNetUsers_Extended { get; set; }
        public DbSet<ListaServicios> DOTACION_Servicios { get; set; }
        public DbSet<ListaComunas> DOTACION_Comunas { get; set; }
        public DbSet<ListaEstablecimientos> DOTACION_Establecimientos { get; set; }
        public DbSet<ListaEstablecimientosMadres> DOTACION_EstablecimientosMadre { get; set; }
        public DbSet<ListaTipoEstablecimiento> DOTACION_Tipo_Establecimiento { get; set; }
        public DbSet<ListaAdministracion> DOTACION_Administracion { get; set; }
        public DbSet<ListaDV> DOTACION_DV { get; set; }
        public DbSet<ListaSexo> DOTACION_Sexo { get; set; }
        public DbSet<ListaNacionalidad> DOTACION_Nacionalidad { get; set; }
        public DbSet<ListaCategorias> DOTACION_Categoria { get; set; }
        public DbSet<ListaProfesiones> DOTACION_Profesion { get; set; }
        public DbSet<ListaEspecialidad> DOTACION_Especialidad { get; set; }
        public DbSet<ListaCargo> DOTACION_Cargo { get; set; }
        public DbSet<ListaLey> DOTACION_Ley { get; set; }
        public DbSet<ListaContratos> DOTACION_Contrato { get; set; }
        public DbSet<ListaChofer> DOTACION_Chofer { get; set; }
        public DbSet<ListaInscripcionSS> DOTACION_InscripcionSS { get; set; }
        public DbSet<ListaAñosServicio> DOTACION_Anos_Servicio { get; set; }
        public DbSet<ListaBienios> DOTACION_Bienios { get; set; }
        public DbSet<ListaNivelCarrera> DOTACION_Nivel_Carrera { get; set; }
        public DbSet<ListaTipoPrevision> DOTACION_Tipo_prevision { get; set; }
        public DbSet<ListaTipoIsapre> DOTACION_Tipo_Isapre { get; set; }
        public DbSet<ListaModelo> DOTACION_Modelo { get; set; }
        public DbSet<Registros> DOTACION_Registros { get; set; }
        public DbSet<ListaRegistros> DOTACION_Registros_resultados { get; set; }
        public DbSet<ListaRegistrosIR> DOTACION_Registros_resultados_IR { get; set; }

        /* ########################     ##############################################*/

        public DbSet<Role> DOTACION_Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");


            modelBuilder.Entity<Registros>(entity =>
            {
                entity.HasKey(e => e.ID_Registro);

                entity.ToTable("DOTACION_Registros");

                entity.Property(e => e.ID_Registro).HasColumnName("ID_Registro");

                entity.Property(e => e.Apellido_Materno)
                    .HasColumnName("Apellido_Materno")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Apellido_Paterno)
                    .HasColumnName("Apellido_Paterno")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Categoria)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.DV)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ID_Comuna).HasColumnName("ID_Comuna");

                // entity.Property(e => e.Id_Cont).HasColumnName("Id_Cont");

                entity.Property(e => e.ID_Establecimiento).HasColumnName("ID_Establecimiento");

                entity.Property(e => e.ID_Servicio).HasColumnName("ID_Servicio");

                entity.Property(e => e.Jornada).HasColumnType("numeric(2, 0)");


                entity.Property(e => e.Nombre)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Profesion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rut)
                    .HasMaxLength(8)
                    .IsUnicode(false);
                
            });
        }

    }
}

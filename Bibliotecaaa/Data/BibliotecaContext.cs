using System;
using System.Collections.Generic;
using Bibliotecaaa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bibliotecaaa.Data
{
    // Heredamos de IdentityDbContext para incluir las tablas de ASP.NET Identity
    public partial class BibliotecaContext : IdentityDbContext<IdentityUser>
    {
        public BibliotecaContext()
        {
        }

        public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
            : base(options)
        {
        }

        // Tu DbSet personalizado para Libros, Generos y Autores
        public virtual DbSet<Libro> Libros { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Autor> Autores { get; set; }

        // Método OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Llama al método base para configurar las tablas de ASP.NET Identity
            base.OnModelCreating(modelBuilder);

            // Configuración de la relación entre Libro y Genero
            modelBuilder.Entity<Libro>()
                .HasOne(l => l.GeneroNavigation) // Relación con la entidad Genero
                .WithMany() // Muchos libros pueden pertenecer a un genero
                .HasForeignKey(l => l.Genero) // La clave foránea en Libro es Genero
                .OnDelete(DeleteBehavior.Restrict); // Definir comportamiento de eliminación

            // Configuración de la relación entre Libro y Autor
            modelBuilder.Entity<Libro>()
                .HasOne(l => l.AutorNavigation) // Cambiar de 'Autor' a 'AutorNavigation' aquí
                .WithMany() // Un autor puede tener muchos libros
                .HasForeignKey(l => l.AutorRel) // Clave foránea en la tabla Libro
                .OnDelete(DeleteBehavior.Restrict); // Definir comportamiento de eliminación

            // Configuración para la collation (ordenación)
            modelBuilder.UseCollation("Modern_Spanish_CI_AS");

            // Configuración personalizada para la entidad Libro
            modelBuilder.Entity<Libro>(entity =>
            {
                entity.HasKey(e => e.IdLibro).HasName("PK__libros__EC09C24E6CECC7A8");

                entity.ToTable("libros");

                entity.Property(e => e.IdLibro).HasColumnName("id_libro");
                entity.Property(e => e.AutorRel) // Configura la propiedad de la clave foránea
                    .HasColumnName("IdAutor");
                entity.Property(e => e.Descripcion)
                    .HasColumnType("text")
                    .HasColumnName("descripcion");
                entity.Property(e => e.Disponible)
                    .HasDefaultValue(true)
                    .HasColumnName("disponible");
                entity.Property(e => e.FechaAgregado)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_agregado");
                entity.Property(e => e.FechaPublicacion).HasColumnName("fecha_publicacion");
                entity.Property(e => e.Genero) // Configura Genero como clave foránea
                    .HasColumnName("genero");
                entity.Property(e => e.Portada)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("portada");
                entity.Property(e => e.Titulo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("titulo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

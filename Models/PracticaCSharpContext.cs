using System;
using System.Collections.Generic;
using Ejercicio3;
using Ejercicio3.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ejercicio4.Models;

public partial class PracticaCSharpContext : DbContext
{
    public PracticaCSharpContext()
    {
    }

    public PracticaCSharpContext(DbContextOptions<PracticaCSharpContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumnos { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Reunion> Reunions { get; set; }
    
    public DbSet<AlumnoReunionView> AlumnoReunionViews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=127.0.0.1;uid=root;pwd=;database=practica_cSharp");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("PRIMARY");

            entity.ToTable("alumno");

            entity.Property(e => e.IdPersona)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id_persona");
            entity.Property(e => e.Especialidad)
                .HasMaxLength(64)
                .HasColumnName("especialidad");
            entity.Property(e => e.Matricula)
                .HasMaxLength(64)
                .HasColumnName("matricula");
            entity.Property(e => e.Semestre)
                .HasMaxLength(64)
                .HasColumnName("semestre");

            entity.HasOne(d => d.IdPersonaNavigation).WithOne(p => p.Alumno)
                .HasForeignKey<Alumno>(d => d.IdPersona)
                .HasConstraintName("alumno_ibfk_1");

            entity.HasMany(d => d.IdReunions).WithMany(p => p.IdAlumnos)
                .UsingEntity<Dictionary<string, object>>(
                    "AlumnoReunion",
                    r => r.HasOne<Reunion>().WithMany()
                        .HasForeignKey("IdReunion")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("alumno_reunion_ibfk_2"),
                    l => l.HasOne<Alumno>().WithMany()
                        .HasForeignKey("IdAlumno")
                        .HasConstraintName("alumno_reunion_ibfk_1"),
                    j =>
                    {
                        j.HasKey("IdAlumno", "IdReunion").HasName("PRIMARY");
                        j.ToTable("alumno_reunion");
                        j.HasIndex(new[] { "IdReunion" }, "id_reunion");
                        j.IndexerProperty<int>("IdAlumno")
                            .HasColumnType("int(10) unsigned")
                            .HasColumnName("id_alumno");
                        j.IndexerProperty<int>("IdReunion")
                            .HasColumnType("int(10) unsigned")
                            .HasColumnName("id_reunion");
                    });
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("PRIMARY");

            entity.ToTable("instructor");

            entity.Property(e => e.IdPersona)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id_persona");
            entity.Property(e => e.Folio)
                .HasMaxLength(64)
                .HasColumnName("folio");

            entity.HasOne(d => d.IdPersonaNavigation).WithOne(p => p.Instructor)
                .HasForeignKey<Instructor>(d => d.IdPersona)
                .HasConstraintName("instructor_ibfk_1");

            entity.HasMany(d => d.IdReunions).WithMany(p => p.IdInstructors)
                .UsingEntity<Dictionary<string, object>>(
                    "InstructorReunion",
                    r => r.HasOne<Reunion>().WithMany()
                        .HasForeignKey("IdReunion")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("instructor_reunion_ibfk_2"),
                    l => l.HasOne<Instructor>().WithMany()
                        .HasForeignKey("IdInstructor")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("instructor_reunion_ibfk_1"),
                    j =>
                    {
                        j.HasKey("IdInstructor", "IdReunion").HasName("PRIMARY");
                        j.ToTable("instructor_reunion");
                        j.HasIndex(new[] { "IdReunion" }, "id_reunion");
                        j.IndexerProperty<int>("IdInstructor")
                            .HasColumnType("int(10) unsigned")
                            .HasColumnName("id_instructor");
                        j.IndexerProperty<int>("IdReunion")
                            .HasColumnType("int(10) unsigned")
                            .HasColumnName("id_reunion");
                    });
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("PRIMARY");

            entity.ToTable("persona");

            entity.Property(e => e.IdPersona)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id_persona");
            entity.Property(e => e.ApellidoDos)
                .HasMaxLength(64)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("apellido_dos");
            entity.Property(e => e.ApellidoUno)
                .HasMaxLength(64)
                .HasColumnName("apellido_uno");
            entity.Property(e => e.DNacimiento)
                .HasColumnType("date")
                .HasColumnName("D_nacimiento");
            entity.Property(e => e.NombreDos)
                .HasMaxLength(64)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("nombre_dos");
            entity.Property(e => e.NombreUno)
                .HasMaxLength(64)
                .HasColumnName("nombre_uno");
            entity.Property(e => e.TipoRol)
                .HasMaxLength(30)
                .HasColumnName("tipo_rol");
        });

        modelBuilder.Entity<Reunion>(entity =>
        {
            entity.HasKey(e => e.IdReunion).HasName("PRIMARY");

            entity.ToTable("reunion");

            entity.Property(e => e.IdReunion)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id_reunion");
            entity.Property(e => e.Fecha)
                .HasColumnType("date")
                .HasColumnName("fecha");
            entity.Property(e => e.Hora)
                .HasColumnType("time")
                .HasColumnName("hora");
            entity.Property(e => e.Lugar)
                .HasMaxLength(100)
                .HasColumnName("lugar");
            entity.Property(e => e.Tema)
                .HasMaxLength(200)
                .HasColumnName("tema");
        });
        
        modelBuilder.Entity<AlumnoReunionView>().ToView("AlumnoReunionView").HasKey(v => v.id_persona);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

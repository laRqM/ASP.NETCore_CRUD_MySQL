using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ejercicio4.Models;

public partial class Alumno
{
    public int IdPersona { get; set; }

    public string Matricula { get; set; } = null!;
    public string? Carrera { get; set; } = null!;

    public string Semestre { get; set; } = null!;

    public string Especialidad { get; set; } = null!;

    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    public virtual ICollection<Reunion> IdReunions { get; set; } = new List<Reunion>();
}
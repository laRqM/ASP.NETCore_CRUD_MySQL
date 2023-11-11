using System;
using System.Collections.Generic;

namespace Ejercicio4.Models;

public partial class Reunion
{
    public int IdReunion { get; set; }

    public DateTime Fecha { get; set; }

    public TimeSpan Hora { get; set; }

    public string Lugar { get; set; } = null!;

    public string Tema { get; set; } = null!;

    public virtual ICollection<Alumno> IdAlumnos { get; set; } = new List<Alumno>();

    public virtual ICollection<Instructor> IdInstructors { get; set; } = new List<Instructor>();
}
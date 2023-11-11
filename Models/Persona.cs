using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ejercicio4.Models;

public partial class Persona
{
    public int IdPersona { get; set; }

    public string? NombreUno { get; set; } = null!;

    public string? NombreDos { get; set; }

    public string? ApellidoUno { get; set; } = null!;

    public string? ApellidoDos { get; set; }

    public DateTime? DNacimiento { get; set; }

    public string? TipoRol { get; set; } = null!;

    public virtual Alumno? Alumno { get; set; }

    public virtual Instructor? Instructor { get; set; }
}
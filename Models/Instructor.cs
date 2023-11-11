using System;
using System.Collections.Generic;

namespace Ejercicio4.Models;

public partial class Instructor
{
    public int IdPersona { get; set; }

    public string Folio { get; set; } = null!;

    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    public virtual ICollection<Reunion> IdReunions { get; set; } = new List<Reunion>();
}
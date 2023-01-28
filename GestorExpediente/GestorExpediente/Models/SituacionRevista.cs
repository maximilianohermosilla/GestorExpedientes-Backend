using System;
using System.Collections.Generic;

namespace GestorExpediente.Models;

public partial class SituacionRevista
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Expediente> Expediente { get; } = new List<Expediente>();
}

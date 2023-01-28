using System;
using System.Collections.Generic;

namespace GestorExpediente.Models;

public partial class Perfil
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuario { get; } = new List<Usuario>();
}

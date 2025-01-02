using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Role
{
    public long Idrole { get; set; }

    public string? RoleType { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

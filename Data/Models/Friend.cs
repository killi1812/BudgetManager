using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Friend
{
    public long Idfriend { get; set; }

    public Guid Guid { get; set; } = System.Guid.NewGuid();

    public long UserId { get; set; }

    public long FriendUserId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User FriendUser { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

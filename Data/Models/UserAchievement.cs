using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class UserAchievement
{
    public long IduserAchievement { get; set; }

    public Guid Guid { get; set; } = System.Guid.NewGuid();

    public long UserId { get; set; }

    public long AchievementId { get; set; }

    public DateTime? EarnedAt { get; set; }

    public virtual Achievement Achievement { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

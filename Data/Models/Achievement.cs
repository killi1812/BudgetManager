using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Achievement
{
    public long Idachievement { get; set; }

    public Guid Guid { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Icon { get; set; }

    public string Criteria { get; set; } = null!;

    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}

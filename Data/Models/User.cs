using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class User
{
    public long Iduser { get; set; }

    public Guid Guid { get; set; } = System.Guid.NewGuid();

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Jmbag { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string PassHash { get; set; } = null!;

    public string? ProfilePicture { get; set; }

    public string Username { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<BankAccountApi> BankAccountApis { get; set; } = new List<BankAccountApi>();

    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    public virtual ICollection<Expense> ExpensePayers { get; set; } = new List<Expense>();

    public virtual ICollection<Expense> ExpenseUsers { get; set; } = new List<Expense>();

    public virtual ICollection<Friend> FriendFriendUsers { get; set; } = new List<Friend>();

    public virtual ICollection<Friend> FriendUsers { get; set; } = new List<Friend>();

    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();

    public virtual ICollection<Saving> Savings { get; set; } = new List<Saving>();

    public virtual ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();

    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}

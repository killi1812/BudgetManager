using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public partial class BudgetManagerContext : DbContext
{
    public BudgetManagerContext()
    {
    }

    public BudgetManagerContext(DbContextOptions<BudgetManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<BankAccountApi> BankAccountApis { get; set; }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<Income> Incomes { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Saving> Savings { get; set; }

    public virtual DbSet<Statistic> Statistics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAchievement> UserAchievements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.Idachievement).HasName("PK__Achievem__B222C16F7F5FFAF3");

            entity.Property(e => e.Idachievement).HasColumnName("IDAchievement");
            entity.Property(e => e.Criteria).HasColumnType("text");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Icon).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<BankAccountApi>(entity =>
        {
            entity.HasKey(e => e.IdbankAccountApi).HasName("PK__BankAcco__96DB0D7C867B3945");

            entity.ToTable("BankAccountAPI");

            entity.Property(e => e.IdbankAccountApi).HasColumnName("IDBankAccountAPI");
            entity.Property(e => e.Apikey)
                .HasMaxLength(255)
                .HasColumnName("APIKey");
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasColumnName("URL");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.BankAccountApis)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BankAccou__UserI__44FF419A");
        });

        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.Idbudget).HasName("PK__Budget__25431717775EF84F");

            entity.ToTable("Budget");

            entity.Property(e => e.Idbudget).HasColumnName("IDBudget");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Sum).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Category).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Budget__Category__5629CD9C");

            entity.HasOne(d => d.User).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Budget__UserID__5535A963");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Idcategory).HasName("PK__Category__1AA1EC661DF2F510");

            entity.ToTable("Category");

            entity.Property(e => e.Idcategory).HasColumnName("IDCategory");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
            entity.Property(e => e.Color).HasMaxLength(20);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Idexpense).HasName("PK__Expense__58C884BFAD7C8502");

            entity.ToTable("Expense");

            entity.Property(e => e.Idexpense).HasColumnName("IDExpense");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.PayerId).HasColumnName("PayerID");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValueSql("('Unpaid')");
            entity.Property(e => e.Sum).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Category).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Expense__Categor__4F7CD00D");

            entity.HasOne(d => d.Payer).WithMany(p => p.ExpensePayers)
                .HasForeignKey(d => d.PayerId)
                .HasConstraintName("FK__Expense__PayerID__52593CB8");

            entity.HasOne(d => d.User).WithMany(p => p.ExpenseUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Expense__UserID__5070F446");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.Idfriend).HasName("PK__Friends__FB73C3900892B96D");

            entity.HasIndex(e => new { e.UserId, e.FriendUserId }, "UQ__Friends__11BD2B9DDCD5B013").IsUnique();

            entity.Property(e => e.Idfriend).HasColumnName("IDFriend");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.FriendUserId).HasColumnName("FriendUserID");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValueSql("('Pending')");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.FriendUser).WithMany(p => p.FriendFriendUsers)
                .HasForeignKey(d => d.FriendUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Friends__FriendU__3F466844");

            entity.HasOne(d => d.User).WithMany(p => p.FriendUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Friends__UserID__3E52440B");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.Idincome).HasName("PK__Income__6CA4E6394B9B37FB");

            entity.ToTable("Income");

            entity.Property(e => e.Idincome).HasColumnName("IDIncome");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Frequency).HasMaxLength(255);
            entity.Property(e => e.Source).HasMaxLength(255);
            entity.Property(e => e.Sum).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Income__UserID__4AB81AF0");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3213E83F32DB1AD5");

            entity.ToTable("Log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .HasColumnName("message");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Idrole).HasName("PK__Role__A1BA16C4BF53712A");

            entity.ToTable("Role");

            entity.Property(e => e.Idrole).HasColumnName("IDRole");
            entity.Property(e => e.RoleType).HasMaxLength(150);
        });

        modelBuilder.Entity<Saving>(entity =>
        {
            entity.HasKey(e => e.Idsavings).HasName("PK__Savings__F3FF684A7830C938");

            entity.Property(e => e.Idsavings).HasColumnName("IDSavings");
            entity.Property(e => e.Current).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Goal).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Savings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Savings__UserID__47DBAE45");
        });

        modelBuilder.Entity<Statistic>(entity =>
        {
            entity.HasKey(e => e.Idstatistics).HasName("PK__Statisti__9B81FB288198B972");

            entity.Property(e => e.Idstatistics).HasColumnName("IDStatistics");
            entity.Property(e => e.IncomePercent).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SpendingPercent).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalIncome).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalSpent).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Statistics)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Statistic__UserI__4222D4EF");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PK__User__EAE6D9DFDA753F8E");

            entity.ToTable("User");

            entity.Property(e => e.Iduser).HasColumnName("IDUser");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FirstName).HasMaxLength(150);
            entity.Property(e => e.Jmbag)
                .HasMaxLength(20)
                .HasColumnName("JMBAG");
            entity.Property(e => e.LastName).HasMaxLength(150);
            entity.Property(e => e.PassHash).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.ProfilePicture).HasColumnType("text");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<UserAchievement>(entity =>
        {
            entity.HasKey(e => e.IduserAchievement).HasName("PK__UserAchi__F4B681D1F8FA245C");

            entity.HasIndex(e => new { e.UserId, e.AchievementId }, "UQ__UserAchi__05FEFFA331779219").IsUnique();

            entity.Property(e => e.IduserAchievement).HasColumnName("IDUserAchievement");
            entity.Property(e => e.AchievementId).HasColumnName("AchievementID");
            entity.Property(e => e.EarnedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Achievement).WithMany(p => p.UserAchievements)
                .HasForeignKey(d => d.AchievementId)
                .HasConstraintName("FK__UserAchie__Achie__5DCAEF64");

            entity.HasOne(d => d.User).WithMany(p => p.UserAchievements)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserAchie__UserI__5CD6CB2B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

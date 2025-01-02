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

    public virtual DbSet<Income> Incomes { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Saving> Savings { get; set; }

    public virtual DbSet<Statistic> Statistics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAchievement> UserAchievements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=BudgetManager;User Id=sa;Password=password123!;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.Idachievement).HasName("PK__Achievem__B222C16FB6777B24");

            entity.Property(e => e.Idachievement).HasColumnName("IDAchievement");
            entity.Property(e => e.Criteria).HasColumnType("text");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Icon).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<BankAccountApi>(entity =>
        {
            entity.HasKey(e => e.IdbankAccountApi).HasName("PK__BankAcco__96DB0D7CF4647544");

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
                .HasConstraintName("FK__BankAccou__UserI__5812160E");
        });

        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.Idbudget).HasName("PK__Budget__25431717224C3BA5");

            entity.ToTable("Budget");

            entity.Property(e => e.Idbudget).HasColumnName("IDBudget");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Sum).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Category).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Budget__Category__6B24EA82");

            entity.HasOne(d => d.User).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Budget__UserID__6A30C649");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Idcategory).HasName("PK__Category__1AA1EC660879E772");

            entity.ToTable("Category");

            entity.Property(e => e.Idcategory).HasColumnName("IDCategory");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
            entity.Property(e => e.Color).HasMaxLength(20);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Idexpense).HasName("PK__Expense__58C884BFDF5E3C29");

            entity.ToTable("Expense");

            entity.Property(e => e.Idexpense).HasColumnName("IDExpense");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.PayerId).HasColumnName("PayerID");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValueSql("('Unpaid')");
            entity.Property(e => e.Sum).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Category).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Expense__Categor__6383C8BA");

            entity.HasOne(d => d.Payer).WithMany(p => p.ExpensePayers)
                .HasForeignKey(d => d.PayerId)
                .HasConstraintName("FK__Expense__PayerID__66603565");

            entity.HasOne(d => d.User).WithMany(p => p.ExpenseUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Expense__UserID__6477ECF3");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.Idincome).HasName("PK__Income__6CA4E639F5F36939");

            entity.ToTable("Income");

            entity.Property(e => e.Idincome).HasColumnName("IDIncome");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Frequency).HasMaxLength(255);
            entity.Property(e => e.Source).HasMaxLength(255);
            entity.Property(e => e.Sum).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Income__UserID__5DCAEF64");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3213E83F4CD17A4E");

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
            entity.HasKey(e => e.Idrole).HasName("PK__Role__A1BA16C48CAC0108");

            entity.ToTable("Role");

            entity.Property(e => e.Idrole).HasColumnName("IDRole");
            entity.Property(e => e.RoleType).HasMaxLength(150);
        });

        modelBuilder.Entity<Saving>(entity =>
        {
            entity.HasKey(e => e.Idsavings).HasName("PK__Savings__F3FF684AC53EA648");

            entity.Property(e => e.Idsavings).HasColumnName("IDSavings");
            entity.Property(e => e.Current).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Goal).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Savings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Savings__UserID__5AEE82B9");
        });

        modelBuilder.Entity<Statistic>(entity =>
        {
            entity.HasKey(e => e.Idstatistics).HasName("PK__Statisti__9B81FB2895D0D535");

            entity.Property(e => e.Idstatistics).HasColumnName("IDStatistics");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IncomePercent).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SpendingPercent).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalIncome).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalSpent).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Statistics)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Statistic__UserI__5535A963");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PK__User__EAE6D9DF19F6EB75");

            entity.ToTable("User");

            entity.Property(e => e.Iduser).HasColumnName("IDUser");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FirstName).HasMaxLength(150);
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Jmbag)
                .HasMaxLength(20)
                .HasColumnName("JMBAG");
            entity.Property(e => e.LastName).HasMaxLength(150);
            entity.Property(e => e.PassHash).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.ProfilePicture).HasColumnType("text");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__RoleID__49C3F6B7");
        });

        modelBuilder.Entity<UserAchievement>(entity =>
        {
            entity.HasKey(e => e.IduserAchievement).HasName("PK__UserAchi__F4B681D14AD5C359");

            entity.HasIndex(e => new { e.UserId, e.AchievementId }, "UQ__UserAchi__05FEFFA3B70EEB34").IsUnique();

            entity.Property(e => e.IduserAchievement).HasColumnName("IDUserAchievement");
            entity.Property(e => e.AchievementId).HasColumnName("AchievementID");
            entity.Property(e => e.EarnedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Achievement).WithMany(p => p.UserAchievements)
                .HasForeignKey(d => d.AchievementId)
                .HasConstraintName("FK__UserAchie__Achie__74AE54BC");

            entity.HasOne(d => d.User).WithMany(p => p.UserAchievements)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserAchie__UserI__73BA3083");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=BudgetManager;User=sa;Password=password123!;Encrypt=False;TrustServerCertificate=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccountApi>(entity =>
        {
            entity.HasKey(e => e.IdbankAccountApi).HasName("PK__BankAcco__96DB0D7C0C26E330");

            entity.ToTable("BankAccountAPI");

            entity.Property(e => e.IdbankAccountApi).HasColumnName("IDBankAccountAPI");
            entity.Property(e => e.Apikey).HasColumnName("APIKey");
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.Url).HasColumnName("URL");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.BankAccountApis)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BankAccou__UserI__3F466844");
        });

        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.Idbudget).HasName("PK__Budget__25431717152FA7C2");

            entity.ToTable("Budget");

            entity.Property(e => e.Idbudget).HasColumnName("IDBudget");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Sum).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Category).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Budget__Category__4E88ABD4");

            entity.HasOne(d => d.User).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Budget__UserID__4D94879B");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Idcategory).HasName("PK__Category__1AA1EC669BB66DE3");

            entity.ToTable("Category");

            entity.Property(e => e.Idcategory).HasColumnName("IDCategory");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Idexpense).HasName("PK__Expense__58C884BF2072C8AE");

            entity.ToTable("Expense");

            entity.Property(e => e.Idexpense).HasColumnName("IDExpense");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Sum).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Category).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Expense__Categor__49C3F6B7");

            entity.HasOne(d => d.User).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Expense__UserID__4AB81AF0");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.Idincome).HasName("PK__Income__6CA4E639DAA019B4");

            entity.ToTable("Income");

            entity.Property(e => e.Idincome).HasColumnName("IDIncome");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Frequency).HasMaxLength(1);
            entity.Property(e => e.Source).HasMaxLength(1);
            entity.Property(e => e.Sum).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Income__UserID__44FF419A");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3213E83FC29EE958");

            entity.ToTable("Log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Message).HasColumnName("message");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Idrole).HasName("PK__Role__A1BA16C4C2B7049B");

            entity.ToTable("Role");

            entity.Property(e => e.Idrole).HasColumnName("IDRole");
            entity.Property(e => e.RoleType).HasMaxLength(150);
        });

        modelBuilder.Entity<Saving>(entity =>
        {
            entity.HasKey(e => e.Idsavings).HasName("PK__Savings__F3FF684A4A6DD1AD");

            entity.Property(e => e.Idsavings).HasColumnName("IDSavings");
            entity.Property(e => e.Current).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Goal).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Savings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Savings__UserID__4222D4EF");
        });

        modelBuilder.Entity<Statistic>(entity =>
        {
            entity.HasKey(e => e.Idstatistics).HasName("PK__Statisti__9B81FB288B35FCB2");

            entity.Property(e => e.Idstatistics).HasColumnName("IDStatistics");
            entity.Property(e => e.IncomePercent).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SpendingPercent).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalIncome).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalSpent).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Statistics)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Statistic__UserI__3C69FB99");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PK__User__EAE6D9DFAAEE5431");

            entity.ToTable("User");

            entity.Property(e => e.Iduser).HasColumnName("IDUser");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FirstName).HasMaxLength(150);
            entity.Property(e => e.Jmbag)
                .HasMaxLength(20)
                .HasColumnName("JMBAG");
            entity.Property(e => e.LastName).HasMaxLength(150);
            entity.Property(e => e.PassHash).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__RoleID__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

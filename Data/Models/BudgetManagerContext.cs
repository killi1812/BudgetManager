﻿using System;
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

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Saving> Savings { get; set; }

    public virtual DbSet<Statistic> Statistics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccountApi>(entity =>
        {
            entity.HasKey(e => e.IdbankAccountApi).HasName("PK__BankAcco__96DB0D7CB663FB29");

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
                .HasConstraintName("FK__BankAccou__UserI__3F466844");
        });

        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.Idbudget).HasName("PK__Budget__254317171D334131");

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
            entity.HasKey(e => e.Idcategory).HasName("PK__Category__1AA1EC66F37720A3");

            entity.ToTable("Category");

            entity.Property(e => e.Idcategory).HasColumnName("IDCategory");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
            entity.Property(e => e.Color).HasMaxLength(20);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Idexpense).HasName("PK__Expense__58C884BF52D7882E");

            entity.ToTable("Expense");

            entity.Property(e => e.Idexpense).HasColumnName("IDExpense");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Description).HasMaxLength(500);
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
            entity.HasKey(e => e.Idincome).HasName("PK__Income__6CA4E63968B3EA75");

            entity.ToTable("Income");

            entity.Property(e => e.Idincome).HasColumnName("IDIncome");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Frequency).HasMaxLength(255);
            entity.Property(e => e.Source).HasMaxLength(255);
            entity.Property(e => e.Sum).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Income__UserID__44FF419A");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3213E83F6F301B4B");

            entity.ToTable("Log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .HasColumnName("message");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Idbudget).HasName("PK__Notifica__2543171761E68E7D");

            entity.ToTable("Notification");

            entity.Property(e => e.Idbudget).HasColumnName("IDBudget");
            entity.Property(e => e.Message).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notificat__UserI__534D60F1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Idrole).HasName("PK__Role__A1BA16C45DAD1C8C");

            entity.ToTable("Role");

            entity.Property(e => e.Idrole).HasColumnName("IDRole");
            entity.Property(e => e.RoleType).HasMaxLength(150);
        });

        modelBuilder.Entity<Saving>(entity =>
        {
            entity.HasKey(e => e.Idsavings).HasName("PK__Savings__F3FF684ACA5A4682");

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
            entity.HasKey(e => e.Idstatistics).HasName("PK__Statisti__9B81FB2832EF71DB");

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
            entity.HasKey(e => e.Iduser).HasName("PK__User__EAE6D9DF1BE8948E");

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

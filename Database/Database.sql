CREATE DATABASE BudgetManager
GO

USE BudgetManager
GO

CREATE TABLE [Role] 
(
    IDRole bigint primary key identity,
    RoleType nvarchar(150)
)
GO

CREATE TABLE [User] 
(
    IDUser bigint primary key identity,
    Guid     UNIQUEIDENTIFIER NOT NULL,
    FirstName nvarchar(150) NOT NULL,
    LastName nvarchar(150) NOT NULL,
    JMBAG nvarchar(20) NOT NULL,
    Email nvarchar(150) NOT NULL,
    PhoneNumber nvarchar(50),
    PassHash nvarchar(255) NOT NULL,
    RoleID bigint references [Role](IDRole)
)
GO

CREATE TABLE [Statistics]
(
	IDStatistics bigint primary key identity,
    Guid     UNIQUEIDENTIFIER NOT NULL,
	TotalSpent decimal,
	TotalIncome decimal,
	SpendingPercent decimal,
	IncomePercent decimal,
	UserID bigint references [User](IDUser)
)

CREATE TABLE BankAccountAPI
(
	IDBankAccountAPI bigint primary key identity,
    Guid     UNIQUEIDENTIFIER NOT NULL,
	BankName nvarchar(100),
	Balance decimal,
	[URL] nvarchar(255),
	APIKey nvarchar(255),
	UserID bigint references [User](IDUser)
)

CREATE TABLE Savings
(
	IDSavings bigint primary key identity,
    Guid     UNIQUEIDENTIFIER NOT NULL,
	Goal decimal,
	[Current] decimal,
	[Date] date,
	UserID bigint references [User](IDUser)
)

CREATE TABLE Income
(
	IDIncome bigint primary key identity,
    Guid     UNIQUEIDENTIFIER NOT NULL,
	[Sum] decimal,
	[Source] nvarchar(255),
	[Date] date,
	Frequency nvarchar(255),
	UserID bigint references [User](IDUser)
)

CREATE TABLE Category
(
	IDCategory bigint primary key identity,
    Guid     UNIQUEIDENTIFIER NOT NULL,
	CategoryName nvarchar(255),
	Color nvarchar(20)
)
GO

CREATE TABLE Expense 
(
	IDExpense bigint primary key identity,
    Guid     UNIQUEIDENTIFIER NOT NULL,
	[Sum] decimal,
	[Description] nvarchar(500),
	[Date] date,
	CategoryID bigint references Category(IDCategory),
	UserID bigint references [User](IDUser)
)

CREATE TABLE Budget
(
	IDBudget bigint primary key identity,
    Guid     UNIQUEIDENTIFIER NOT NULL,
	[Sum] decimal,
	UserID bigint references [User](IDUser),
	CategoryID bigint references Category(IDCategory)
)

CREATE TABLE Log
(
    id      int identity (1,1) primary key not null,
    date    datetime not null,
    message NVARCHAR(500)                  not null,
    Lvl     int      not null
);

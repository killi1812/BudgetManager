CREATE DATABASE BudgetingAndExpenseTracker
GO

USE BudgetingAndExpenseTracker
go

CREATE TABLE [Role] 
(
    IDRole bigint primary key identity,
    RoleType nvarchar(150)
)

CREATE TABLE [User] 
(
    IDUser bigint primary key identity,
    FirstName nvarchar(150),
    LastName nvarchar(150),
    JMBAG nvarchar(20),
    Email nvarchar(150),
    PhoneNumbeer nvarchar(50),
    PassHash nvarchar(max),
    PassSalt nvarchar(max),
    RoleID bigint references [Role](IDRole)
)

CREATE TABLE [Statistics]
(
	IDStatistics bigint primary key identity,
	TotalSpent decimal,
	TotalIncome decimal,
	SpendingPercent decimal,
	IncomePercent decimal,
	UserID bigint references [User](IDUser)
)

CREATE TABLE BankAccountAPI
(
	IDBankAccountAPI bigint primary key identity,
	BankName nvarchar(100),
	Balance decimal,
	[URL] nvarchar(max),
	APIKey nvarchar(max),
	UserID bigint references [User](IDUser)
)

CREATE TABLE Savings
(
	IDSavings bigint primary key identity,
	Goal decimal,
	[Current] decimal,
	[Date] date,
	UserID bigint references [User](IDUser)
)

CREATE TABLE Income
(
	IDIncome bigint primary key identity,
	[Sum] decimal,
	[Source] nvarchar,
	[Date] date,
	Frequency nvarchar,
	UserID bigint references [User](IDUser)
)

CREATE TABLE Category
(
	IDCategory bigint primary key identity,
	CategoryName nvarchar(max)
)

CREATE TABLE Expense 
(
	IDExpense bigint primary key identity,
	[Sum] decimal,
	[Description] nvarchar(max),
	[Date] date,
	CategoryID bigint references Category(IDCategory),
	UserID bigint references [User](IDUser)
)

CREATE TABLE Budget
(
	IDBudget bigint primary key identity,
	[Sum] decimal,
	UserID bigint references [User](IDUser),
	CategoryID bigint references Category(IDCategory)
)
<<<<<<< HEAD
CREATE DATABASE BudgetingAndExpenseTracker
GO

USE BudgetingAndExpenseTracker
go
=======
CREATE DATABASE BudgetManager
GO

USE BudgetManager
GO
>>>>>>> ac5b0cad220c3f6ade8f9084903c8a357f297280

CREATE TABLE [Role] 
(
    IDRole bigint primary key identity,
    RoleType nvarchar(150)
)
<<<<<<< HEAD
=======
GO
>>>>>>> ac5b0cad220c3f6ade8f9084903c8a357f297280

CREATE TABLE [User] 
(
    IDUser bigint primary key identity,
<<<<<<< HEAD
    FirstName nvarchar(150),
    LastName nvarchar(150),
    JMBAG nvarchar(20),
    Email nvarchar(150),
    PhoneNumbeer nvarchar(50),
    PassHash nvarchar(max),
    PassSalt nvarchar(max),
    RoleID bigint references [Role](IDRole)
)
=======
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
>>>>>>> ac5b0cad220c3f6ade8f9084903c8a357f297280

CREATE TABLE [Statistics]
(
	IDStatistics bigint primary key identity,
<<<<<<< HEAD
=======
    Guid     UNIQUEIDENTIFIER NOT NULL,
>>>>>>> ac5b0cad220c3f6ade8f9084903c8a357f297280
	TotalSpent decimal,
	TotalIncome decimal,
	SpendingPercent decimal,
	IncomePercent decimal,
	UserID bigint references [User](IDUser)
)

CREATE TABLE BankAccountAPI
(
	IDBankAccountAPI bigint primary key identity,
<<<<<<< HEAD
=======
    Guid     UNIQUEIDENTIFIER NOT NULL,
>>>>>>> ac5b0cad220c3f6ade8f9084903c8a357f297280
	BankName nvarchar(100),
	Balance decimal,
	[URL] nvarchar(max),
	APIKey nvarchar(max),
	UserID bigint references [User](IDUser)
)

CREATE TABLE Savings
(
	IDSavings bigint primary key identity,
<<<<<<< HEAD
=======
    Guid     UNIQUEIDENTIFIER NOT NULL,
>>>>>>> ac5b0cad220c3f6ade8f9084903c8a357f297280
	Goal decimal,
	[Current] decimal,
	[Date] date,
	UserID bigint references [User](IDUser)
)

CREATE TABLE Income
(
	IDIncome bigint primary key identity,
<<<<<<< HEAD
=======
    Guid     UNIQUEIDENTIFIER NOT NULL,
>>>>>>> ac5b0cad220c3f6ade8f9084903c8a357f297280
	[Sum] decimal,
	[Source] nvarchar,
	[Date] date,
	Frequency nvarchar,
	UserID bigint references [User](IDUser)
)

CREATE TABLE Category
(
	IDCategory bigint primary key identity,
<<<<<<< HEAD
	CategoryName nvarchar(max)
)
=======
    Guid     UNIQUEIDENTIFIER NOT NULL,
	CategoryName nvarchar(max)
)
GO
>>>>>>> ac5b0cad220c3f6ade8f9084903c8a357f297280

CREATE TABLE Expense 
(
	IDExpense bigint primary key identity,
<<<<<<< HEAD
=======
    Guid     UNIQUEIDENTIFIER NOT NULL,
>>>>>>> ac5b0cad220c3f6ade8f9084903c8a357f297280
	[Sum] decimal,
	[Description] nvarchar(max),
	[Date] date,
	CategoryID bigint references Category(IDCategory),
	UserID bigint references [User](IDUser)
)

CREATE TABLE Budget
(
	IDBudget bigint primary key identity,
<<<<<<< HEAD
	[Sum] decimal,
	UserID bigint references [User](IDUser),
	CategoryID bigint references Category(IDCategory)
)
=======
    Guid     UNIQUEIDENTIFIER NOT NULL,
	[Sum] decimal,
	UserID bigint references [User](IDUser),
	CategoryID bigint references Category(IDCategory)
)

CREATE TABLE Log
(
    id      int identity (1,1) primary key not null,
    date    datetime not null,
    message NVARCHAR(max)                  not null,
    Lvl     int      not null
);
>>>>>>> ac5b0cad220c3f6ade8f9084903c8a357f297280

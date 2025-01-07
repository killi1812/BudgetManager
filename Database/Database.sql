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

CREATE TABLE [User] (
    IDUser BIGINT PRIMARY KEY IDENTITY,
    Guid UNIQUEIDENTIFIER NOT NULL ,
    FirstName NVARCHAR(150) NOT NULL,
    LastName NVARCHAR(150) NOT NULL,
    JMBAG NVARCHAR(20) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    PhoneNumber NVARCHAR(50),
    PassHash NVARCHAR(255) NOT NULL,
    ProfilePicture TEXT,
    Username NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
)
GO

CREATE TABLE [Friends] (
    IDFriend BIGINT PRIMARY KEY IDENTITY,
    Guid UNIQUEIDENTIFIER NOT NULL ,
    UserID BIGINT NOT NULL,
    FriendUserID BIGINT NOT NULL,
    Status NVARCHAR(10) DEFAULT 'Pending',
    CreatedAt DATETIME,
    FOREIGN KEY (UserID) REFERENCES [User](IDUser),
    FOREIGN KEY (FriendUserID) REFERENCES [User](IDUser),
    UNIQUE(UserID, FriendUserID)
)
GO

CREATE TABLE [Statistics] (
    IDStatistics BIGINT PRIMARY KEY IDENTITY,
    Guid UNIQUEIDENTIFIER NOT NULL ,
    TotalSpent DECIMAL,
    TotalIncome DECIMAL,
    SpendingPercent DECIMAL,
    IncomePercent DECIMAL,
    UserID BIGINT REFERENCES [User](IDUser)
)
GO

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
GO

CREATE TABLE Savings
(
	IDSavings bigint primary key identity,
    	Guid     UNIQUEIDENTIFIER NOT NULL,
	Goal decimal,
	[Current] decimal,
	[Date] date,
	UserID bigint references [User](IDUser)
)
GO

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
GO

CREATE TABLE Category
(
	IDCategory bigint primary key identity,
    	Guid     UNIQUEIDENTIFIER NOT NULL,
	CategoryName nvarchar(255),
	Color nvarchar(20)
)
GO

CREATE TABLE [Expense] (
    IDExpense BIGINT PRIMARY KEY IDENTITY,
    Guid UNIQUEIDENTIFIER NOT NULL ,
    [Sum] DECIMAL,
    [Description] NVARCHAR(500),
    [Date] DATE,
    CategoryID BIGINT REFERENCES [Category](IDCategory),
    UserID BIGINT REFERENCES [User](IDUser),
    PayerID BIGINT,
    Status NVARCHAR(10) DEFAULT 'Unpaid',
    FOREIGN KEY (PayerID) REFERENCES [User](IDUser)
)
GO

CREATE TABLE [Budget] (
    IDBudget BIGINT PRIMARY KEY IDENTITY,
    Guid UNIQUEIDENTIFIER NOT NULL ,
    [Sum] DECIMAL,
    UserID BIGINT REFERENCES [User](IDUser),
    CategoryID BIGINT REFERENCES [Category](IDCategory)
)
GO

CREATE TABLE [Achievements] (
    IDAchievement BIGINT PRIMARY KEY IDENTITY,
    Guid UNIQUEIDENTIFIER NOT NULL ,
    Name NVARCHAR(100) NOT NULL,
    Description TEXT NOT NULL,
    Icon TEXT,
    Criteria TEXT NOT NULL
)
GO

CREATE TABLE [UserAchievements] (
    IDUserAchievement BIGINT PRIMARY KEY IDENTITY,
    Guid UNIQUEIDENTIFIER NOT NULL ,
    UserID BIGINT NOT NULL,
    AchievementID BIGINT NOT NULL,
    EarnedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES [User](IDUser) ON DELETE CASCADE,
    FOREIGN KEY (AchievementID) REFERENCES [Achievements](IDAchievement) ON DELETE CASCADE,
    UNIQUE(UserID, AchievementID)
)
GO

CREATE TABLE Log
(
    id      int identity (1,1) primary key not null,
    date    datetime not null,
    message NVARCHAR(500)                  not null,
    Lvl     int      not null
);

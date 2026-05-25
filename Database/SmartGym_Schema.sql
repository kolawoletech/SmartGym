/* =============================================================
   SmartGym Membership Management System
   Database Schema + Seed Data
   Target: Microsoft SQL Server (2016+)
   ============================================================= */

IF DB_ID('SmartGymDB') IS NOT NULL
BEGIN
    ALTER DATABASE SmartGymDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SmartGymDB;
END
GO

CREATE DATABASE SmartGymDB;
GO
USE SmartGymDB;
GO

/* -------------------------------------------------------------
   1. Members
   ------------------------------------------------------------- */
CREATE TABLE Members
(
    MemberId          INT IDENTITY(1,1) PRIMARY KEY,
    FullName          NVARCHAR(120)  NOT NULL,
    Email             NVARCHAR(150)  NOT NULL UNIQUE,
    PasswordHash      NVARCHAR(256)  NOT NULL,    -- store SHA256 hash
    PhoneNumber       NVARCHAR(20)   NULL,
    SecurityQuestion  NVARCHAR(200)  NOT NULL,
    SecurityAnswer    NVARCHAR(200)  NOT NULL,    -- store lower-cased hash
    Role              NVARCHAR(20)   NOT NULL DEFAULT 'Member', -- Member | Admin
    DateRegistered    DATETIME       NOT NULL DEFAULT GETDATE()
);
GO

/* -------------------------------------------------------------
   2. MembershipAccounts
   A member can hold multiple membership accounts (Standard, Family, etc.)
   ------------------------------------------------------------- */
CREATE TABLE MembershipAccounts
(
    AccountId       INT IDENTITY(1,1) PRIMARY KEY,
    MemberId        INT            NOT NULL,
    AccountName     NVARCHAR(100)  NOT NULL,
    MembershipType  NVARCHAR(50)   NOT NULL,  -- Standard | Premium | Family
    CreditBalance   DECIMAL(10,2)  NOT NULL DEFAULT 0,
    DateCreated     DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_MembershipAccounts_Members
        FOREIGN KEY (MemberId) REFERENCES Members(MemberId)
        ON DELETE CASCADE
);
GO

/* -------------------------------------------------------------
   3. FitnessClasses
   ------------------------------------------------------------- */
CREATE TABLE FitnessClasses
(
    ClassId        INT IDENTITY(1,1) PRIMARY KEY,
    ClassName      NVARCHAR(120)  NOT NULL,
    Instructor     NVARCHAR(120)  NOT NULL,
    Schedule       DATETIME       NOT NULL,
    Capacity       INT            NOT NULL,
    CreditCost     DECIMAL(10,2)  NOT NULL,
    Description    NVARCHAR(500)  NULL
);
GO

/* -------------------------------------------------------------
   4. Transactions
   Records both Bookings and Credit Top-Ups
   ------------------------------------------------------------- */
CREATE TABLE Transactions
(
    TransactionId   INT IDENTITY(1,1) PRIMARY KEY,
    AccountId       INT             NOT NULL,
    ClassId         INT             NULL,
    TransactionType NVARCHAR(20)    NOT NULL,   -- Booking | TopUp | Refund
    Amount          DECIMAL(10,2)   NOT NULL,
    BalanceAfter    DECIMAL(10,2)   NOT NULL,
    TransactionDate DATETIME        NOT NULL DEFAULT GETDATE(),
    Notes           NVARCHAR(250)   NULL,
    CONSTRAINT FK_Transactions_Accounts
        FOREIGN KEY (AccountId) REFERENCES MembershipAccounts(AccountId),
    CONSTRAINT FK_Transactions_Classes
        FOREIGN KEY (ClassId)   REFERENCES FitnessClasses(ClassId)
);
GO

/* -------------------------------------------------------------
   5. BookingLogs
   Audit trail of every booking action (mirrors File I/O log)
   ------------------------------------------------------------- */
CREATE TABLE BookingLogs
(
    LogId           INT IDENTITY(1,1) PRIMARY KEY,
    MemberId        INT             NOT NULL,
    ClassId         INT             NOT NULL,
    BookingDate     DATETIME        NOT NULL DEFAULT GETDATE(),
    CreditsDeducted DECIMAL(10,2)   NOT NULL,
    RemainingBalance DECIMAL(10,2)  NOT NULL,
    CONSTRAINT FK_BookingLogs_Members FOREIGN KEY (MemberId) REFERENCES Members(MemberId),
    CONSTRAINT FK_BookingLogs_Classes FOREIGN KEY (ClassId)  REFERENCES FitnessClasses(ClassId)
);
GO

/* =============================================================
   SEED DATA
   ============================================================= */

-- Admin password: Admin@123 (SHA256)
-- Member password: Password1 (SHA256)
INSERT INTO Members (FullName, Email, PasswordHash, PhoneNumber, SecurityQuestion, SecurityAnswer, Role)
VALUES
('System Administrator', 'admin@smartgym.local',
 '8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918', -- "admin" placeholder
 '0110000000', 'What is your favourite gym?', 'smartgym', 'Admin'),
('Jane Doe', 'jane@example.com',
 '70617A9C8DDB69E9166DEDD2FFA34A2A50F3E5DBA09EDA5F7F1D4D3E4F1A2B3C',
 '0821234567', 'What city were you born in?', 'cape town', 'Member'),
('John Smith', 'john@example.com',
 '70617A9C8DDB69E9166DEDD2FFA34A2A50F3E5DBA09EDA5F7F1D4D3E4F1A2B3C',
 '0839876543', 'Name of your first pet?', 'rex', 'Member');
GO

INSERT INTO MembershipAccounts (MemberId, AccountName, MembershipType, CreditBalance)
VALUES
(2, 'Jane Personal',  'Standard', 100.00),
(2, 'Jane Family',    'Family',   250.00),
(3, 'John Personal',  'Premium',  500.00);
GO

INSERT INTO FitnessClasses (ClassName, Instructor, Schedule, Capacity, CreditCost, Description)
VALUES
('Yoga Flow',        'Sarah Lee',      DATEADD(DAY, 1, GETDATE()), 20, 25.00, 'Vinyasa flow for all levels.'),
('HIIT Burn',        'Mike Johnson',   DATEADD(DAY, 2, GETDATE()), 15, 40.00, 'High intensity interval training.'),
('Spin Cycle',       'Thandi Mokoena', DATEADD(DAY, 3, GETDATE()), 25, 30.00, '45-minute indoor cycling.'),
('Pilates Core',     'Anna Botha',     DATEADD(DAY, 4, GETDATE()), 18, 35.00, 'Mat pilates focused on core strength.'),
('CrossFit WOD',     'David Naidoo',   DATEADD(DAY, 5, GETDATE()), 12, 50.00, 'Workout of the day - advanced.');
GO

INSERT INTO Transactions (AccountId, ClassId, TransactionType, Amount, BalanceAfter, Notes)
VALUES
(1, NULL, 'TopUp',  100.00, 100.00, 'Initial top-up'),
(1, 1,    'Booking', 25.00,  75.00, 'Booked Yoga Flow'),
(3, NULL, 'TopUp',  500.00, 500.00, 'Premium starter pack');
GO

PRINT 'SmartGymDB created successfully with seed data.';

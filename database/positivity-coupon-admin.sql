IF DB_ID('PositivityCouponAdmin') IS NULL
BEGIN
    CREATE DATABASE PositivityCouponAdmin;
END;
GO

USE PositivityCouponAdmin;
GO

IF OBJECT_ID('dbo.CouponRedemptions', 'U') IS NOT NULL DROP TABLE dbo.CouponRedemptions;
IF OBJECT_ID('dbo.Coupons', 'U') IS NOT NULL DROP TABLE dbo.Coupons;
IF OBJECT_ID('dbo.Companies', 'U') IS NOT NULL DROP TABLE dbo.Companies;
GO

CREATE TABLE dbo.Companies (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    HrEmail NVARCHAR(200) NOT NULL,
    CreatedAt DATETIME2 NOT NULL
);
GO

CREATE TABLE dbo.Coupons (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Code NVARCHAR(64) NOT NULL UNIQUE,
    Type INT NOT NULL,
    Status INT NOT NULL,
    DiscountValue DECIMAL(18,2) NULL,
    TotalPoolValue DECIMAL(18,2) NULL,
    RemainingPoolValue DECIMAL(18,2) NULL,
    ExpiryDate DATETIME2 NULL,
    ModuleName NVARCHAR(100) NOT NULL,
    AssignedToName NVARCHAR(200) NOT NULL,
    AssignedToEmail NVARCHAR(200) NOT NULL,
    MaxUsageCount INT NULL,
    UsedCount INT NOT NULL DEFAULT 0,
    MaxPerEmployeePerSession DECIMAL(18,2) NULL,
    MaxSessionsPerEmployee INT NULL,
    CreatedBy NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    CompanyId INT NULL FOREIGN KEY REFERENCES dbo.Companies(Id)
);
GO

CREATE TABLE dbo.CouponRedemptions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CouponId INT NOT NULL FOREIGN KEY REFERENCES dbo.Coupons(Id),
    EmployeeName NVARCHAR(200) NOT NULL,
    EmployeeEmail NVARCHAR(200) NOT NULL,
    SessionName NVARCHAR(200) NOT NULL,
    BookingId NVARCHAR(50) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    RedeemedAt DATETIME2 NOT NULL,
    Status NVARCHAR(50) NOT NULL
);
GO

INSERT INTO dbo.Companies (Name, HrEmail, CreatedAt)
VALUES
    ('Nestle India Pvt Ltd', 'hr@nestle.com', '2025-01-01'),
    ('Wipro HR', 'wellness@wipro.com', '2025-01-10'),
    ('TechCorp India', 'people@techcorp.com', '2025-02-05');
GO

INSERT INTO dbo.Coupons
    (Code, Type, Status, DiscountValue, TotalPoolValue, RemainingPoolValue, ExpiryDate, ModuleName, AssignedToName, AssignedToEmail, MaxUsageCount, UsedCount, MaxPerEmployeePerSession, MaxSessionsPerEmployee, CreatedBy, CreatedAt, CompanyId)
VALUES
    ('NESTLECARE25', 4, 1, NULL, 20000, 7500, '2025-12-31', '1:1 sessions', 'Nestle India', 'hr@nestle.com', 8, 5, 2500, 3, 'Admin', '2025-01-01', 1),
    ('WIPROFREE25', 3, 1, NULL, NULL, NULL, '2025-06-30', '1:1 sessions', 'Wipro HR', 'wellness@wipro.com', 50, 28, NULL, 1, 'Admin', '2025-01-10', 2),
    ('SAVE20', 1, 2, 20, NULL, NULL, '2025-04-15', 'Career counselling', 'Priya Ramesh', 'priya@gmail.com', 1, 1, NULL, NULL, 'Admin', '2025-02-20', NULL),
    ('GIFT500', 2, 3, 500, NULL, NULL, DATEADD(DAY, 2, CAST(GETUTCDATE() AS DATE)), 'All sessions', 'Suresh Kumar', 'suresh@yahoo.com', 1, 0, NULL, NULL, 'Admin', '2025-03-10', NULL),
    ('TECHCARE25', 4, 1, NULL, 50000, 42000, '2026-03-31', 'All sessions', 'TechCorp India', 'people@techcorp.com', 20, 3, 2500, 5, 'Admin', '2025-02-05', 3);
GO

INSERT INTO dbo.CouponRedemptions
    (CouponId, EmployeeName, EmployeeEmail, SessionName, BookingId, Amount, RedeemedAt, Status)
VALUES
    (1, 'Ananya Sharma', 'ananya@nestle.com', 'Career counselling 1:1', 'BKG-10091', 2500, '2025-03-28', 'Done'),
    (1, 'Rohit Verma', 'rohit@nestle.com', 'Leadership coaching', 'BKG-10088', 2500, '2025-03-25', 'Done'),
    (1, 'Priya Singh', 'priya.s@nestle.com', 'Mindfulness session', 'BKG-10065', 2500, '2025-03-15', 'Upcoming'),
    (1, 'Rohit Verma', 'rohit@nestle.com', 'Career counselling', 'BKG-10059', 2500, '2025-03-10', 'Done'),
    (3, 'Priya Ramesh', 'priya@gmail.com', 'Career counselling', 'BKG-09999', 0, DATEADD(MINUTE, -2, GETUTCDATE()), 'Done');
GO

-- 1. קודם כל מוחקים את הטבלאות הישנות (כדי שלא יהיו התנגשויות)
IF OBJECT_ID('[dbo].[Synergy]', 'U') IS NOT NULL DROP TABLE [dbo].[Synergy];
IF OBJECT_ID('[dbo].[SickDays]', 'U') IS NOT NULL DROP TABLE [dbo].[SickDays];
IF OBJECT_ID('[dbo].[Employees]', 'U') IS NOT NULL DROP TABLE [dbo].[Employees];

-- 2. יצירת טבלת עובדים מעודכנת (עם תעודת זהות)
CREATE TABLE [dbo].[Employees] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL, -- המזהה הפנימי למחשב (1,2,3)
    [RealID]    NVARCHAR (9)  NOT NULL,                 -- תעודת זהות אמיתית (למשל 305123456)
    [Name]      NVARCHAR (50) NOT NULL,
    [Role]      NVARCHAR (20) NOT NULL,
    [Seniority] INT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

-- 3. יצירת טבלת ימי מחלה (חדש!)
CREATE TABLE [dbo].[SickDays] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [EmployeeId] INT NOT NULL, -- מקושר ל-Id הפנימי של העובד
    [Day]        INT NOT NULL, -- יום בחודש (0 עד 29)
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

-- 4. יצירת טבלת סינרגיה (ללא שינוי)
CREATE TABLE [dbo].[Synergy] (
    [EmpId1] INT NOT NULL,
    [EmpId2] INT NOT NULL,
    [Score]  INT NOT NULL,
    PRIMARY KEY CLUSTERED ([EmpId1] ASC, [EmpId2] ASC)
);
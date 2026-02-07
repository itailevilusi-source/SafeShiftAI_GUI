-- =============================================
-- סקריפט אתחול נתונים מלא - SafeShift AI
-- =============================================

-- 1. ניקוי טבלאות קיימות (סדר המחיקה חשוב בגלל קשרי גומלין)
DELETE FROM Synergy;
DELETE FROM SickDays;
DELETE FROM Employees;

-- 2. איפוס המספור האוטומטי (Identity) כדי שנתחיל מ-ID 1
DBCC CHECKIDENT ('Employees', RESEED, 0);
DBCC CHECKIDENT ('SickDays', RESEED, 0);

-- =============================================
-- 3. הכנסת עובדים (30 עובדים: 10 מכל סוג)
-- =============================================

-- --- מנהלים (Managers) - IDs 1-10 ---
INSERT INTO Employees (RealID, Name, Role, Seniority) VALUES
('300123456', 'Avi Cohen', 'Manager', 10),
('300987654', 'Beni Levi', 'Manager', 5),
('204567891', 'Gadi Azaria', 'Manager', 12),
('312345678', 'Dana Ron', 'Manager', 3),
('054123456', 'Hila Shapira', 'Manager', 7),
('032165498', 'Yossi Golan', 'Manager', 20),
('201234567', 'Moti katz', 'Manager', 8),
('305647382', 'Noa Kirel', 'Manager', 4),
('209876543', 'Eli Yatzpan', 'Manager', 15),
('315642897', 'Sarah Netanyahu', 'Manager', 30);

-- --- רופאים (Doctors) - IDs 11-20 ---
INSERT INTO Employees (RealID, Name, Role, Seniority) VALUES
('023456789', 'Dr. House', 'Doctor', 15),
('301230987', 'Dr. Strange', 'Doctor', 8),
('206789123', 'Dr. Mike', 'Doctor', 3),
('314159265', 'Dr. Meredith', 'Doctor', 10),
('052345678', 'Dr. Shepherd', 'Doctor', 12),
('039876543', 'Dr. Cohen', 'Doctor', 25),
('207894561', 'Dr. Klein', 'Doctor', 5),
('308527419', 'Dr. Seuss', 'Doctor', 40),
('203698745', 'Dr. Phil', 'Doctor', 2),
('319632587', 'Dr. Watson', 'Doctor', 9);

-- --- נהגים (Drivers) - IDs 21-30 ---
INSERT INTO Employees (RealID, Name, Role, Seniority) VALUES
('050123456', 'Shlomi Shabat', 'Driver', 20),
('302587419', 'Ronen Tzur', 'Driver', 2),
('201478523', 'David Hamelech', 'Driver', 50),
('321654987', 'Moshe Ofnik', 'Driver', 5),
('054987654', 'Kipod Kipod', 'Driver', 1),
('036547891', 'Speedy Gonzalez', 'Driver', 10),
('209638527', 'Nahag Chadash', 'Driver', 0),
('307418529', 'Vin Diesel', 'Driver', 15),
('205896347', 'Han Solo', 'Driver', 30),
('318529637', 'Baby Driver', 'Driver', 3);

-- =============================================
-- 4. הכנסת ימי מחלה (Sick Days)
-- =============================================
-- נניח שחלק מהעובדים חולים בימים מסוימים (0 = היום הראשון בחודש)

INSERT INTO SickDays (EmployeeId, Day) VALUES
(1, 0), (1, 1),  -- אבי כהן (מנהל) חולה בימים 1 ו-2
(11, 5),         -- ד"ר האוס חולה ביום 6
(21, 10), (21, 11), (21, 12), -- שלומי (נהג) חולה 3 ימים באמצע החודש
(5, 29),         -- הילה (מנהלת) חולה ביום האחרון
(30, 15);        -- בייבי דרייבר חולה ביום 16

-- =============================================
-- 5. הכנסת ציוני התאמה/סינרגיה (Synergy)
-- =============================================

INSERT INTO Synergy (EmpId1, EmpId2, Score) VALUES
-- התאמות טובות (בונוס לציון)
(1, 11, 50),   -- אבי (מנהל) וד"ר האוס עובדים מעולה יחד
(1, 21, 30),   -- אבי (מנהל) ושלומי (נהג) מסתדרים טוב
(2, 12, 40),   -- בני (מנהל) וד"ר סטריינג' צוות חזק
(22, 12, 20),  -- נהג 22 ורופא 12 מסתדרים סבבה

-- התאמות גרועות (עונש לציון - האלגוריתם ינסה להפריד ביניהם)
(1, 2, -100),  -- שני המנהלים הראשונים שונאים אחד את השני (לא רלוונטי כי הם לא באותה משמרת, אבל טוב שיהיה)
(11, 21, -50), -- ד"ר האוס (11) ושלומי הנהג (21) רבים כל הזמן
(3, 13, -30),  -- גדי (מנהל) וד"ר מייק לא מסתדרים
(30, 20, -100); -- בייבי דרייבר (30) וד"ר ווטסון (20) - אסון

-- סיום
SELECT 'Database Populated Successfully!' AS Status;
-- 1. קודם כל מוחקים את ימי המחלה הקיימים כדי למנוע כפילויות
TRUNCATE TABLE SickDays;

-- 2. הכנסת ימי מחלה לפי שמות (הרבה יותר בטוח!)

-- אבי כהן (Avi Cohen) חולה בימים 0 ו-1
INSERT INTO SickDays (EmployeeId, Day)
SELECT Id, 0 FROM Employees WHERE Name = 'Avi Cohen'
UNION ALL
SELECT Id, 1 FROM Employees WHERE Name = 'Avi Cohen';

-- ד"ר האוס (Dr. House) חולה ביום 5
INSERT INTO SickDays (EmployeeId, Day)
SELECT Id, 5 FROM Employees WHERE Name = 'Dr. House';

-- שלומי שבת (Shlomi Shabat) חולה בימים 10, 11, 12
INSERT INTO SickDays (EmployeeId, Day)
SELECT Id, 10 FROM Employees WHERE Name = 'Shlomi Shabat'
UNION ALL
SELECT Id, 11 FROM Employees WHERE Name = 'Shlomi Shabat'
UNION ALL
SELECT Id, 12 FROM Employees WHERE Name = 'Shlomi Shabat';

-- הילה (Hila Shapira) חולה ביום 29 (סוף החודש)
INSERT INTO SickDays (EmployeeId, Day)
SELECT Id, 29 FROM Employees WHERE Name = 'Hila Shapira';

-- בייבי דרייבר (Baby Driver) חולה ביום 15
INSERT INTO SickDays (EmployeeId, Day)
SELECT Id, 15 FROM Employees WHERE Name = 'Baby Driver';

-- בדיקה: הצג את כל ימי המחלה עם שמות העובדים
SELECT E.Name, S.Day 
FROM SickDays S
JOIN Employees E ON S.EmployeeId = E.Id
ORDER BY E.Name, S.Day;
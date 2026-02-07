-- 1. הוספת עמודת ותק לטבלת העובדים הקיימת
ALTER TABLE Employees ADD Seniority INT DEFAULT 0 NOT NULL;

-- 2. יצירת טבלה חדשה לשמירת הקשרים בין העובדים (המטריצה)
CREATE TABLE Synergy (
    EmpId1 INT NOT NULL,
    EmpId2 INT NOT NULL,
    Score INT NOT NULL,
    PRIMARY KEY (EmpId1, EmpId2)
);
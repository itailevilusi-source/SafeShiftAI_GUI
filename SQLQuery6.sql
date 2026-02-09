-- =============================================
-- סקריפט מילוי אוטומטי למטריצת התאמה (Synergy)
-- =============================================

-- 1. ניקוי הטבלה מנתונים ישנים
TRUNCATE TABLE Synergy;

-- 2. הגדרת משתנים ללולאות
DECLARE @i INT = 1;       -- עובד ראשון
DECLARE @j INT = 1;       -- עובד שני
DECLARE @Score INT;       -- הציון שייבחר
DECLARE @RandomVal INT;   -- מספר אקראי להגרלה

-- 3. לולאה ראשית: רצה על כל העובדים מ-1 עד 30
WHILE @i <= 30
BEGIN
    -- לולאה פנימית: רצה תמיד מהעובד הבא (i+1) כדי למנוע כפילויות ולמנוע השוואה עצמית
    SET @j = @i + 1;

    WHILE @j <= 30
    BEGIN
        -- הגרלת מספר בין 0 ל-100
        SET @RandomVal = ABS(CHECKSUM(NEWID())) % 100;

        -- === לוגיקה לקביעת הציון ===
        
        -- אפשרות א': 60% סיכוי לניטרלי (0) - רוב האנשים אדישים אחד לשני
        IF @RandomVal < 60
        BEGIN
            SET @Score = 0;
        END
        
        -- אפשרות ב': 30% סיכוי לחיבור טוב (מספר חיובי בין 10 ל-40)
        ELSE IF @RandomVal < 90
        BEGIN
             -- מייצר מספר אקראי חיובי
            SET @Score = 10 + (ABS(CHECKSUM(NEWID())) % 31);
        END
        
        -- אפשרות ג': 10% סיכוי לחיבור רע (מספר שלילי בין -10 ל -30)
        ELSE
        BEGIN
            -- מייצר מספר אקראי שלילי
            SET @Score = -10 - (ABS(CHECKSUM(NEWID())) % 21);
        END

        -- הכנסה לטבלה
        INSERT INTO Synergy (EmpId1, EmpId2, Score)
        VALUES (@i, @j, @Score);

        -- מעבר לעובד הבא בזוג
        SET @j = @j + 1;
    END

    -- מעבר לעובד הבא
    SET @i = @i + 1;
END

-- הצגת הודעת סיום
SELECT 'Synergy Matrix populated successfully with randomized data!' AS Status;
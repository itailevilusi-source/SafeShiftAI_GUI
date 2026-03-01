using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeShiftAI_GUI
{
    internal class BacktrackingSolver
    {
        private Data_Layer data; // משתנה שיחזיק את כל הנתונים

        // רשימה של כל העובדים שיש 
        private List<Employee> allEmployees;

        // הלוח שלנו כרגע 
        private int[,,] currentSchedule;

        private int[,,] bestPartialSchedule; // הלוח הכי טוב - "צילום המסך" של השיא

        // כמה איטרציות עשינו
        public long IterationsCount { get; private set; }
        // כמה ימים הצלחנו להתקדם 
        public int MaxDayReached { get; private set; }

        // תנאי העצירה כדי שהמחשב לא ייתקע לנצח (2 מיליון ניסיונות)
        private readonly long MaxIterations = 2000000;

        // בנאי (Constructor) 
        public BacktrackingSolver(Data_Layer dataManager)
        {
            this.data = dataManager;
            this.allEmployees = data.Employees; // שולפים את העובדים מתוך הנתונים

            this.currentSchedule = new int[30, 3, 3];
            this.bestPartialSchedule = new int[30, 3, 3];

            // איפוס הלוח: ממלאים ב-(1-)
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        currentSchedule[i, j, k] = -1;
                        bestPartialSchedule[i, j, k] = -1;
                    }
                }
            }
        }

        // פונקציית ההפעלה עוטפת 
        public bool Solve()
        {
            IterationsCount = 0;
            MaxDayReached = -1;

            // מתחילים מיום 0, משמרת 0, תפקיד 0
            return SolveRecursive(0, 0, 0);
        }

        // האלגוריתם הרקורסיבי 
        private bool SolveRecursive(int currentDay, int currentShift, int currentRoleIndex)
        {
            // קידום המונה ועצירת חירום
            IterationsCount++;
            if (IterationsCount > MaxIterations)
                return false; // מפסיקים את החיפוש 

            // שמירת "תמונת מצב" אם שברנו שיא והגענו ליום חדש
            if (currentDay > MaxDayReached)
            {
                MaxDayReached = currentDay;
                SaveSnapshot();
            }

            // תנאי עצירה - מקרה טוב הגענו ליום השלושים וסיימנו את הלוח
            if (currentDay == 30)
            {
                return true;
            }

            //   חישוב המיקום הבא קודם מקדמים תפקיד, אז משמרת, אז יום מהפנים לחיצון
            int nextDay = currentDay;
            int nextShift = currentShift;
            int nextRoleIndex = currentRoleIndex + 1;

            if (nextRoleIndex >= 3) // סיימנו לשבץ את כל התפקידים במשמרת הזו
            {
                nextRoleIndex = 0;
                nextShift++; // עוברים למשמרת הבאה

                if (nextShift >= 3) // סיימנו את כל המשמרות להיום
                {
                    nextShift = 0;
                    nextDay++; // עוברים למחר
                }
            }

            // לולאת Backtracking על כל העובדים
            foreach (var emp in allEmployees)
            {
                if (IsValid(emp, currentDay, currentShift, currentRoleIndex))
                {
                    //  שיבוץ העובד בלוח
                    currentSchedule[currentDay, currentShift, currentRoleIndex] = emp.ID; //שיבוץ העובד אם תקין

                    //  קריאה רקורסיבית לשלב הבא
                    bool success = SolveRecursive(nextDay, nextShift, nextRoleIndex);

                    //  אם הצליח - מעבירים את ההצלחה 
                    if (success) return true;

                    //  חזרה לאחור (Backtracking) - מוחקים ומנסים מישהו אחר
                    currentSchedule[currentDay, currentShift, currentRoleIndex] = -1;
                }
            }

            // אם עברנו על כל העובדים ואף אחד לא התאים
            return false;
        }

        // --- בדיקת אילוצים ---
        private bool IsValid(Employee emp, int day, int shift, int roleIndex)
        {

            //  חולה
            if (emp.SickDays != null && emp.SickDays.Contains(day))
            {
                return false;
            }
            /////////////////////////////////////////////////////////////////////////////////
            //  אילוץ עומס חודשי (הקריסה הגדולה!): מקסימום 9 משמרות בחודש לעובד
            int monthlyShifts = 0;
            for (int d = 0; d < 30; d++)
            {
                for (int s = 0; s < 3; s++)
                {
                    for (int r = 0; r < 3; r++)
                    {
                        if (currentSchedule[d, s, r] == emp.ID) monthlyShifts++;
                    }
                }
            }
            if (monthlyShifts >= 9) return false; // עבר את המכסה החודשית - פסול!
            /////////////////////////////////////////////////////////////////////////////////////

            // תפקיד מתאים
            if (roleIndex == 0 && emp.Role != Employee.EmployeeRole.MGR) return false;
            if (roleIndex == 1 && emp.Role != Employee.EmployeeRole.MED) return false;
            if (roleIndex == 2 && emp.Role != Employee.EmployeeRole.DRV) return false;



            //  מניעת כפילות באותו יום 
            for (int s = 0; s < 3; s++)
            {
                for (int r = 0; r < 3; r++)
                {
                    if (currentSchedule[day, s, r] == emp.ID) return false;
                }
            }

            //  מניעת משמרת בוקר אחרי משמרת לילה
            // אם אנחנו משבצים עכשיו למשמרת בוקר (shift == 0) ויש יום קודם (day > 0)
            if (day > 0 && shift == 0)
            {
                for (int r = 0; r < 3; r++)
                {
                    // בודקים אם העובד עבד ביום הקודם במשמרת לילה (shift == 2)
                    if (currentSchedule[day - 1, 2, r] == emp.ID) return false;
                }
            }

            //  חוקים סינגריים  רק כשמשבצים נהג, כי אז הצוות שלם 
            if (roleIndex == 2)
            {
                int mngId = currentSchedule[day, shift, 0];
                int medId = currentSchedule[day, shift, 1];
                int drvId = emp.ID;

                // מוודאים שיש לנו מנהל ורופא 
                if (mngId != -1 && medId != -1)
                {
                    // שולפים את העובדים מהרשימה כדי לבדוק ותק
                    Employee mng = allEmployees.FirstOrDefault(e => e.ID == mngId);
                    Employee med = allEmployees.FirstOrDefault(e => e.ID == medId);

                    if (mng != null && med != null)
                    {
                        // בדיקה שהוותק של הצוות מעל 10
                        int sumSeniority = mng.Seniority + med.Seniority + emp.Seniority;
                        if (sumSeniority < 10) return false;

                        // אילוץ סינרגיה צוותית מעל 30
                        int synergySum = 0;
                        if (mngId < 1000 && medId < 1000) synergySum += data.SynergyMatrix[mngId, medId];
                        if (mngId < 1000 && drvId < 1000) synergySum += data.SynergyMatrix[mngId, drvId];
                        if (medId < 1000 && drvId < 1000) synergySum += data.SynergyMatrix[medId, drvId];

                        if (synergySum < 30) return false;
                    }
                }
            }

            // עברנו את כל המכשולים - השיבוץ חוקי
            return true; 
        }

        // פונקציית עזר לשמירת הלוח  
        private void SaveSnapshot()
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        bestPartialSchedule[i, j, k] = currentSchedule[i, j, k];
                    }
                }
            }
        }

        // פונקציה חיצונית לשליפת הלוח החלקי הטוב ביותר
        public int[,,] GetBestPartialSchedule()
        {
            return bestPartialSchedule;
        }
    }
}

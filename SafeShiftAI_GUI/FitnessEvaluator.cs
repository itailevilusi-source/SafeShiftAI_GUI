using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SafeShiftAI_GUI
{
    internal class FitnessEvaluator
    {
        private Data_Layer data;//Data_Layer כפרמטר חיבור הנתונים למחלקת פונקציית הכושר היא מקבלת את 


        //  חייבים מילון בגלל הsql 
        private Dictionary<int, Employee> _employeeMap;

        // בנאי שמקבל את מאגר הנתונים
        public FitnessEvaluator(Data_Layer dataManager)
        {
            this.data = dataManager;


            // --- אתחול המילון (חובה כדי למנוע קריסה) ---
            _employeeMap = new Dictionary<int, Employee>();
            foreach (var emp in data.Employees)//Employee emp in data.Employees=var
            {
                if (!_employeeMap.ContainsKey(emp.ID))
                {
                    _employeeMap.Add(emp.ID, emp);
                }
            }
        }

        // הפונקציה המרכזית שמקבלת לוח (כרומוזום) ומחזירה ציון
        public double CalculateFitness(int[,,] chromosome)
        {
            double score = 100000; // ציון בסיס לפי ההצעה

            // לולאה ראשונה: עוברת על כל יום - 30 ימים בחודש
            for (int day = 0; day < 30; day++)
            {
                HashSet<int> workersToday = new HashSet<int>();//רשימה של עובדים במשמרת

                //מניעת משמרת בוקר אחרי לילה: קנס של 100 נקודות על כל עובד המשובץ לבוקר מיד לאחר משמרת לילה

                if (day < 29) // כדי לא לחרוג מהמערך בחיפוש על מחר
                {

                    // רצים על 3 התפקידים מנהל, רופא, נהג
                    for (int role = 0; role < 3; role++)
                    {
                        // עובד במשמרת לילה של היום (day, shift=2)
                        int empIdNight = chromosome[day, 2, role];

                        // עובד במשמרת בוקר של מחר (day+1, shift=0)
                        int empIdMorning = chromosome[day + 1, 0, role];

                        //  בדיקה ששניהם לא אפס כדי למנוע באגים
                        if (empIdNight != 0 && empIdMorning != 0 && empIdNight == empIdMorning)
                        {
                            // אותו עובד שובץ לבוקר מיד אחרי לילה
                            score -= 100;
                        }
                    }
                   
                }


                // לולאה שנייה: עוברת על כל משמרת ביום - 3 משמרות: בוקר, ערב, לילה
                for (int shift = 0; shift < 3; shift++)
                {
                    // לולאה שלישית: עוברת על כל תפקיד במשמרת -3 תפקידים: מנהל, רופא, נהג
                    for (int role = 0; role < 3; role++)
                    {
                        // כאן נשלוף את ה-ID של העובד שנמצא במשבצת הספציפית
                        int employeeId = chromosome[day, shift, role];

                        //בדיקה כפילות עובד במשמרת , קנס של 10,000 נקודות עבור כל משמרת שבה עובד משובץ יותר מפעם אחת
                        if (employeeId != 0 && _employeeMap.ContainsKey(employeeId))
                     {  
                            Employee currentEmp = _employeeMap[employeeId];

                            if (workersToday.Contains(employeeId))
                        {
                            // העובד כבר שובץ היום במשמרת אחרת
                            score -= 10000;
                        }
                        else
                        {
                            workersToday.Add(employeeId);
                        }



                        // אם הוא חולה, נוריד 10,000 נקודות לפי האילוץ הקשיח בהצעה
                        if (data.Employees[employeeId].SickDays.Contains(day))
                        {
                           
                            score -= 10000;
                        }
                        //אם אין התאמה בין התפקיד למשבצת,קנס של 10,000 נקודות על כל משמרת שבה חסר אחד מהתפקידים הנדרשים
                        if (role==0&& data.Employees[employeeId].Role!= Employee.EmployeeRole.MGR)
                        {
                            // אם אין התאמה בין התפקיד מנהל למשבצת 
                            score -= 10000;
                        }

                        if (role == 1 && data.Employees[employeeId].Role != Employee.EmployeeRole.MED)
                        {
                            // אם אין התאמה בין התפקיד רופא למשבצת 
                            score -= 10000;
                        }

                        if (role ==2 && data.Employees[employeeId].Role != Employee.EmployeeRole.DRV)
                        {
                            // אם אין התאמה בין התפקיד נהג למשבצת 
                            score -= 10000;
                        }

                      }
                    }
                    
                    
                    int mngId = chromosome[day, shift, 0]; // מנהל
                    int medId = chromosome[day, shift, 1]; // רופא
                    int drvId = chromosome[day, shift, 2]; // נהג

                    // רק אם כל השלושה קיימים במילון - מחשבים
                    if (_employeeMap.ContainsKey(mngId) &&
                        _employeeMap.ContainsKey(medId) &&
                        _employeeMap.ContainsKey(drvId))
                    {
                        // העדפת ותק גבוה: קנס של 100 נקודות על צוותים שהותק המצטבר שלהם נמוך מ10-שנים אילוץ רצויי
                        int SumSeniority= _employeeMap[mngId].Seniority +_employeeMap[medId].Seniority +_employeeMap[drvId].Seniority;
                        if (SumSeniority<10)
                    {
                        score -= 100;
                    }

                    //מדד הסינרגיה מחושב כסכום כלל הציונים החיוביים והשליליים של כל הצוותים בלוח. הציון המצטבר מוכפל במשקל של 5

                    // שליפת הערכים מהמטריצה (3 זוגות אפשריים בצוות של 3)
                    int synergySum = 0;
                        // מוודאים שלא חורגים מגבולות המערך
                        if (mngId < 1000 && medId < 1000) synergySum += data.SynergyMatrix[mngId, medId];
                        if (mngId < 1000 && drvId < 1000) synergySum += data.SynergyMatrix[mngId, drvId];
                        if (medId < 1000 && drvId < 1000) synergySum += data.SynergyMatrix[medId, drvId];

                        //נוסחת בונוס הסינרגיה :בונוס הסינרגיה = 5 × סך ציון הסינרגיה המצטבר מכל הצוותים בלוח
                        score += (synergySum * 5);

                    // סינרגיה בין הצוות חייבת להיות מטווח מינימלי: קנס של 10,000 נקודות על כל צוות שהציון המצטבר שלו נמוך מ 30 

                    if (synergySum<30)
                    {
                        score -= 10000;
                    }
                    }
                }
            }
            // =========================================================
            // אילוץ עומס חודשי: מקסימום 9 משמרות בחודש לעובד (כמו ב-Backtracking)
            // =========================================================

            //  ניצור מילון שיספור כמה משמרות כל עובד קיבל בלוח הנוכחי
            Dictionary<int, int> monthlyShiftCount = new Dictionary<int, int>();
            for (int d = 0; d < 30; d++)
            {
                for (int s = 0; s < 3; s++)
                {
                    for (int r = 0; r < 3; r++)
                    {
                        int empId = chromosome[d, s, r];
                        if (empId != 0) // אם יש פה עובד 
                        {
                            if (!monthlyShiftCount.ContainsKey(empId))
                            {
                                monthlyShiftCount[empId] = 0;
                            }
                            monthlyShiftCount[empId]++;
                        }
                    }
                }
            }

            // 2. נעבור על כל העובדים, ומי שעבר 9 משמרות יחטוף קנס קשה!
            foreach (var kvp in monthlyShiftCount)
            {
                int shifts = kvp.Value;
                if (shifts > 9)
                {
                    // קנס של 10,000 נקודות על כל משמרת נוספת מעל 9
                    score -= (shifts - 9) * 10000;
                }
            }
            // =========================================================

            return score;
            // return Math.Max(0, score); //   לא מחזירים ציון שלילי,פתרון שהוא 0 נחשב לזבל והוא לא ימשיך באלגוריתם הגנטי
        }
    }
}

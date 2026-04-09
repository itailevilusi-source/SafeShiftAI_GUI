using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace SafeShiftAI_GUI
{
    public class DatabaseHelper
    {
        private string connectionString;//המשתנה הזה פרטי כדי שאף קובץ אחר יוכל לשנות לנו את כתובת הניווט ,משתנה זה הוא למעשה כתובת ניווט

        public DatabaseHelper()//ירוץ פעם אחת שנרשום בform1 שלנו new databaseHelper()
        {
            // חיבור דינמי ל-DB
            string dbFileName = "SafeShiftDB.mdf";//שמירת השם המדויק של מסד הנתונים במקרה שלנו הוא :SafeShiftDB.mdf
            string projectFolder = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;// טריק לשמירת נתיב של הפרוייקט המחשב בודק איפה התוכנה רצה  וחוזר שתי תיקיות אחורה
            string dbPath = Path.Combine(projectFolder, "SafeShiftAI_GUI", dbFileName);//Path.Combine היא פונקציה שמחברת מילים לנתיב תקני של Windows מוסיפה \ במקום המתאים יצרנו נתיב מלא לדאטה שלנו

            if (!File.Exists(dbPath))//בדיקה האם הקובץ נמצא בנתיב שחישבנו
            {
                dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbFileName);//אם לא נמצא תחפש את מסד הנתונים באותה תיקייה בדיוק איפה שנמצא קובץ ה-EXE של התוכנה
            }

            connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True";//בניית כתובת הניווט שלנו
            //Data Source=(LocalDB)\MSSQLLocalDB: אנחנו אומרים ל-C# שהמחסן שלנו הוא מנוע מקומי וחינמי של SQL שמותקן על המחשב
            //AttachDbFilename={dbPath}: אנחנו מחברים אליו את קובץ ה-.mdf הספציפי שלך שמצאנו הרגע
            //Integrated Security=True: אנחנו מבקשים להיכנס למסד הנתונים באמצעות המשתמש של Windows שפתוח כרגע, בלי לבקש שם משתמש וסיסמה מיוחדים ל-SQL
        }

        //  הוספת עובד, הפונקציה שנקראת  כשלוחצים על כפתור "הוסף עובד" במסך  
        public void AddEmployee(string realId, string name, string role, int seniority)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))//יוצרים קשר עם מסד הנתונים באמצעות connectionString 
                                                                            //using -מבטיח שברגע שסיימנו את מה שבתוך הסוגריים המסולסלים, מתנתק החיבור ונסגר זה מונע עומס על הזיכרון
            {
                // הפקודה בsql תכניס לתוך הטבלה Employees נתונים לעמודות של ת"ז, שם, תפקיד וותק
                //@ מטרתו כדי יתייחסו לטקסט כפרמטרים ושלא  יוכלו לכתוב קוד למשל קבלת הנתונים של אנשים במערכת שלנו
                string query = "INSERT INTO Employees (RealID, Name, Role, Seniority) VALUES (@RealID, @Name, @Role, @Seniority)";
                SqlCommand cmd = new SqlCommand(query, conn);//סוג של שליח יש לו את הפקודה query conn וחיבור למנווט שלנו 

                //אנחנו ממלאים את הפרמטרים שקיבלנו מהמשתמש בשטרודילים שציינו בשאילתה
                cmd.Parameters.AddWithValue("@RealID", realId); // הוספנו את הפרמטר הזה
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@Seniority", seniority);

                try
                {
                    conn.Open(); //ביצוע--פותחים את הקישור למסד הנתונים
                    cmd.ExecuteNonQuery(); //עכשיו השליח שלנו מבצע את המטלה NonQuery אומר תעשה אבל אל תביא לי טבלה חזרה כי אין טעם במקרה זה
                }
                catch (Exception ex) { MessageBox.Show("Error adding employee: " + ex.Message); }//תפיסת השיגאה אם הייתה ניסיון להכניס תעודת זהות כפולה ותקפוץ שגיאה
            }
        }

        //קבלת רשימת העובדים שיש לנו אנחנו מקבלים תשובה מסוג טבלה  
        public DataTable GetEmployees()
        {
            //השיטה שהשתמשתי בה פה חוסכת להישאר בקו פתוח עם מסד הנתונים אנחנו שואבים את הכל הנתונים מתרגמים וסוגרים את החיבור למסד הנתונים
            DataTable dt = new DataTable();//יצירת התשובה שנחזיר מסוג טבלה 
            using (SqlConnection conn = new SqlConnection(connectionString))//קישור למסד הנתונים
            {
                string query = "SELECT * FROM Employees";//השאילתה, * - תביא לי הכל את כל העמודות
                SqlCommand cmd = new SqlCommand(query, conn);//נותנים לשליח שלנו את הקישור והפעולה שאנו רוצים
                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);//המתרגם אנחנו מחכים שcmd יחזור עם תשובה 
                    adapter.Fill(dt);//adapter.Fill(dt): DataTable dt מסדר יפה ויוצר שורות ועמודות בטבלה שרצינו שאליה יתקבלו הנתונים 
                }
                catch (Exception ex) { MessageBox.Show("Error loading employees: " + ex.Message); }//בדיקה 
            }
            return dt;//החזרת הטבלה לform1 במקרה שלנו
        }

        //  ניהול ימי מחלה 
        public void AddSickDay(int empId, int day)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // מניעת כפילויות
                string checkQuery = "SELECT COUNT(*) FROM SickDays WHERE EmployeeId = @EmpId AND Day = @Day";//סופרת כמה שורות יש לעובד הספציפי הזה ביום ספיציפי :שאילתת בדיקה 
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@EmpId", empId);
                checkCmd.Parameters.AddWithValue("@Day", day);

                try
                {
                    conn.Open();
                    int exists = (int)checkCmd.ExecuteScalar();//ExecuteScalar:מחכה לערך של סכימה 1,2 וכו 
                    //ExecuteScalar() מביאה את השורה הראשונה של התוצאה זה יכול להיות אוביקט תאריך וכו אב ל בגלל שציינו int היא תחזיר את המספר הסכימה שלנו

                    if (exists == 0)//בדיקה שהעובד אכן לא סומן ביום הזה כחולה כבר
                                    // אם מצאנו שהסכום גדול מ0 אנחנו פשוט מדלגים
                    {
                        string query = "INSERT INTO SickDays (EmployeeId, Day) VALUES (@EmpId, @Day)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@EmpId", empId);
                        cmd.Parameters.AddWithValue("@Day", day);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error adding sick day: " + ex.Message); }//תפיסת השגיאה אם יש 
            }
        }

        //טעינת ימי המחלה 
        //אנחנו מחזירים רשימה שבנויה מתעודת זהות של העובד והיום בו הוא חולה
        //רצה פעם אחת כשהתוכנה עולה, ושואבת את כל ימי המחלה של כל העובדים במכה אחת, כדי לבנות את ה-Data_Layer
        public List<(int EmpId, int Day)> LoadSickDays()
        {
            var list = new List<(int, int)>();//יצית הרשימה אותה נחזיר ריקה כרגע
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT EmployeeId, Day FROM SickDays";
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();//יצירת קורא לנתונים 
                    //ExecuteReader:עובר על נתונים שורה שורה 
                    while (reader.Read())//כל עוד יש עוד שורה לקרוא 
                    {
                        list.Add(((int)reader["EmployeeId"], (int)reader["Day"]));//קח את המספר שבעמודת EmployeeId, קח את המספר שבעמודת Day ותוסיף אותם לרשימה כזוג ערכים
                    }
                }
                catch { }// התעלמות מכוונת- אם המסד נפל, נחזיר רשימה ריקה במקום שהתוכנה תקרוס למשתמש 
            }
            return list;//החזרת הרשימה שבנינו
        }

        // --- ניהול סינרגיה ---
        public void SaveSynergy(int id1, int id2, int score)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    IF EXISTS (SELECT 1 FROM Synergy WHERE EmpId1 = @Id1 AND EmpId2 = @Id2)
                        UPDATE Synergy SET Score = @Score WHERE EmpId1 = @Id1 AND EmpId2 = @Id2
                    ELSE
                        INSERT INTO Synergy (EmpId1, EmpId2, Score) VALUES (@Id1, @Id2, @Score)";

                //SELECT 1 סוג של תנאי בולאיני אם קיים תחזיר אחד 
                //IF EXISTS: "תבדוק בבקשה האם הזוג הזה (EmpId1 ו-EmpId2) כבר קיים בטבלה"
                // UPDATE: "אם הם קיימים, רק תעדכן (UPDATE) את הציון (Score) שלהם לציון החדש"
                //ELSE INSERT: "אם הם לא קיימים עדיין, תייצר להם שורה חדשה בטבלה (INSERT) עם הציון הזה"

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id1", id1);
                cmd.Parameters.AddWithValue("@Id2", id2);
                cmd.Parameters.AddWithValue("@Score", score);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();//ExecuteNonQuery נותנים הוראה לבצע אבל אנחנו לא מחכים לטבלה חזרה
                }
                catch { }//אם אם ציון אחד מתוך  המון הרצות לא יקלט זה יבלע ונמשיך בכל זאת  בכל מקרה הציון מאותחל ל0 וזה לא ישפיע על החישובים ערך ניטרלי
            }
        }

        //טעינת ציוני הסינגריה
        //באמצעות מילון המפתח הוא מחרוזת -זוג עובדים והערך הוא ציון ההתאמה
        public Dictionary<string, int> LoadSynergyData()
        {
            //הבחירה בהחזרת מילון שמבוסס על טבלת גיבוב היא מתאמי סיבכיות קבלת התוצאה בo1 
            var data = new Dictionary<string, int>();//יצירת מילון ריק אותו נמלא
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Synergy";// השאילתה טעינת כל הנתונים -*
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();//עוברים שורה שורה
                    while (reader.Read())//כל עוד יש שורה לקרוא
                    {
                        int id1 = (int)reader["EmpId1"];
                        int id2 = (int)reader["EmpId2"];
                        int score = (int)reader["Score"];
                        data[$"{id1}-{id2}"] = score; //הכנסת הנתונים למילון  למשל 
                        //5-11=8 המשמעות היא שההתאמה בין עובד 5 לעובד 11 היא 8
                    }
                }
                catch { }//בדיקה 
            }
            return data;//החזרת המילון שבנינו
        }
        // קבלת ימי המחלה של עובד ספציפי
        // רצה רק כשבמסך "ניהול עובדים" בוחרים שם של עובד ספציפי מהרשימה
        // מחזירה רשימה של ימים שהעובד הספציפי חולה בהם
        public List<int> GetSickDaysForEmployee(int empId)
        {
            List<int> days = new List<int>();//הרשימה אותה נחזיר

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Day FROM SickDays WHERE EmployeeId = @id";//השאילתה
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", empId);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())//כל עוד יש שורה לקרוא
                    {
                        days.Add((int)reader["Day"]);//מוסיפים את היום לרשימה
                    }
                }
                catch { } // הגנה מפני קריסות כמו בפונקציות האחרות
            }
            return days;// החזרת הימים של העובד הספציפי
        }

        //מחיקת עובד
        // "כדי למחוק עובד, לא יכולתי למחוק אותו ישירות מטבלת העובדים בגלל אילוצי Foreign Key של מסד הנתונים. לכן, הפונקציה שלי קודם כל מוחקת את ימי המחלה שלו מ-SickDays, לאחר מכן מוחקת את כל קשרי ההתאמה שלו מ-Synergy, ורק כשהוא מנותק משאר המערכת, היא מוחקת אותו סופית מ-Employees."
        public void DeleteEmployee(int empId)
        {

            //במקום לייצר 3 אובייקטים של שרת , יצרתי אובייקט אחד ופשוט עדכנתי את ה-CommandText שלו תוך כדי שאני ממחזר את אותו פרמטר,
            //המחיקה מתבצעת ב-3 שלבים מוקפדים כדי לא להפר אילוצי Foreign Key במסד הנתונים
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // יוצרים "שליח" אחד שמשמש את כל 3 המחיקות
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@id", empId); // מכניסים את הפרמטר רק פעם אחת
                

                try
                {
                    conn.Open();

                    // 1. מחיקת העובד ממטריצת ההתאמה
                    cmd.CommandText = "DELETE FROM Synergy WHERE EmpId1 = @id OR EmpId2 = @id";
                    cmd.ExecuteNonQuery();

                    // 2. מחיקת ימי המחלה של העובד
                    cmd.CommandText = "DELETE FROM SickDays WHERE EmployeeId = @id";
                    cmd.ExecuteNonQuery();

                    // 3. מחיקת העובד עצמו מטבלת העובדים
                    cmd.CommandText = "DELETE FROM Employees WHERE Id = @id";
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)//בדיקה שאף שלב לא נפל
                {
                    //  שגיאה אם משהו נכשל, כדי שהמשתמש לא יחשוב שהעובד נמחק
                    MessageBox.Show("Error deleting employee: " + ex.Message);
                }
            }
        }

    }
    }

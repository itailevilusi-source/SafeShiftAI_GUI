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
        private string connectionString;

        public DatabaseHelper()
        {
            // חיבור דינמי ל-DB
            string dbFileName = "SafeShiftDB.mdf";
            string projectFolder = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string dbPath = Path.Combine(projectFolder, "SafeShiftAI_GUI", dbFileName);

            if (!File.Exists(dbPath))
            {
                dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbFileName);
            }

            connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True";
        }

        // --- הוספת עובד (כולל תעודת זהות) ---
        public void AddEmployee(string realId, string name, string role, int seniority)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // הוספנו את RealID לשאילתה
                string query = "INSERT INTO Employees (RealID, Name, Role, Seniority) VALUES (@RealID, @Name, @Role, @Seniority)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@RealID", realId); // הוספנו את הפרמטר הזה
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@Seniority", seniority);

                try { conn.Open(); cmd.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("Error adding employee: " + ex.Message); }
            }
        }

        public DataTable GetEmployees()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees";
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                catch (Exception ex) { MessageBox.Show("Error loading employees: " + ex.Message); }
            }
            return dt;
        }

        // --- ניהול ימי מחלה (חדש!) ---
        public void AddSickDay(int empId, int day)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // מניעת כפילויות
                string checkQuery = "SELECT COUNT(*) FROM SickDays WHERE EmployeeId = @EmpId AND Day = @Day";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@EmpId", empId);
                checkCmd.Parameters.AddWithValue("@Day", day);

                try
                {
                    conn.Open();
                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists == 0)
                    {
                        string query = "INSERT INTO SickDays (EmployeeId, Day) VALUES (@EmpId, @Day)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@EmpId", empId);
                        cmd.Parameters.AddWithValue("@Day", day);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error adding sick day: " + ex.Message); }
            }
        }

        public List<(int EmpId, int Day)> LoadSickDays()
        {
            var list = new List<(int, int)>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT EmployeeId, Day FROM SickDays";
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(((int)reader["EmployeeId"], (int)reader["Day"]));
                    }
                }
                catch { }
            }
            return list;
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

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id1", id1);
                cmd.Parameters.AddWithValue("@Id2", id2);
                cmd.Parameters.AddWithValue("@Score", score);

                try { conn.Open(); cmd.ExecuteNonQuery(); } catch { }
            }
        }

        public Dictionary<string, int> LoadSynergyData()
        {
            var data = new Dictionary<string, int>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Synergy";
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int id1 = (int)reader["EmpId1"];
                        int id2 = (int)reader["EmpId2"];
                        int score = (int)reader["Score"];
                        data[$"{id1}-{id2}"] = score;
                    }
                }
                catch { }
            }
            return data;
        }

        public List<int> GetSickDaysForEmployee(int empId)
        {
            List<int> days = new List<int>();

            // שימוש באותו חיבור שיש לך במחלקה (נניח שקוראים לו connectionString)
            // אם השם אצלך שונה, שנה בהתאם
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\SafeShiftDB.mdf;Integrated Security=True"))
            {
                conn.Open();
                string query = "SELECT Day FROM SickDays WHERE EmployeeId = @id";
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", empId);
                    using (System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            days.Add((int)reader["Day"]);
                        }
                    }
                }
            }
            return days;
        }


        //"כדי למחוק עובד, לא יכולתי למחוק אותו ישירות מטבלת העובדים בגלל אילוצי Foreign Key של מסד הנתונים. לכן, הפונקציה שלי קודם כל מוחקת את ימי המחלה שלו מ-SickDays, לאחר מכן מוחקת את כל קשרי ההתאמה שלו מ-Synergy, ורק כשהוא מנותק משאר המערכת, היא מוחקת אותו סופית מ-Employees."
        public void DeleteEmployee(int empId)
        {
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\SafeShiftDB.mdf;Integrated Security=True"))
            {
                conn.Open();

                // 1. מחיקת העובד ממטריצת ההתאמה (גם כשהוא עובד 1 וגם כשהוא עובד 2)
                string querySynergy = "DELETE FROM Synergy WHERE EmpId1 = @id OR EmpId2 = @id";
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(querySynergy, conn))
                {
                    cmd.Parameters.AddWithValue("@id", empId);
                    cmd.ExecuteNonQuery();
                }

                // 2. מחיקת ימי המחלה של העובד
                string querySickDays = "DELETE FROM SickDays WHERE EmployeeId = @id";
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(querySickDays, conn))
                {
                    cmd.Parameters.AddWithValue("@id", empId);
                    cmd.ExecuteNonQuery();
                }

                // 3. מחיקת העובד עצמו מטבלת העובדים
                string queryEmployee = "DELETE FROM Employees WHERE Id = @id";
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(queryEmployee, conn))
                {
                    cmd.Parameters.AddWithValue("@id", empId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
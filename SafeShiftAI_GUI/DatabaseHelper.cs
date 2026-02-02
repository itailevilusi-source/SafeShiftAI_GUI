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
        // מחרוזת החיבור - משתמשת בנתיב יחסי כדי שזה יעבוד בכל מחשב
        private string connectionString;

        public DatabaseHelper()
        {
            // איתור הנתיב של הקובץ mdf באופן דינמי
            string dbFileName = "SafeShiftDB.mdf";
            string projectFolder = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string dbPath = Path.Combine(projectFolder, "SafeShiftAI_GUI", dbFileName);

            // אם לא מוצא שם, נסה בתיקייה הנוכחית (למקרה של ריצה רגילה)
            if (!File.Exists(dbPath))
            {
                dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbFileName);
            }

            connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True";
        }

        // פונקציה להוספת עובד חדש
        public void AddEmployee(string name, string role)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Employees (Name, Role) VALUES (@Name, @Role)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Role", role);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה בשמירה ל-SQL: " + ex.Message);
                }
            }
        }

        // פונקציה לקבלת כל העובדים
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
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה בקריאה מ-SQL: " + ex.Message);
                }
            }
            return dt;
        }
    }
}
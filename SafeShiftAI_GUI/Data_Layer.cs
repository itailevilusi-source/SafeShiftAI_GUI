//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Shift_Safe_Smart
//{
//    internal class Data_Layer
//    {
//        public List<Employee> Employees { get; private set; } = new List<Employee>();//רשימה של העובדים 10 מכל מקצוע 
//        public int[,] SynergyMatrix { get; private set; } = new int[30, 30];//מטריצת סינגריה 

//        public Data_Layer()
//        {
//            InitializeData();
//        }

//        //בדיקת האלוגירתם 
//        private void InitializeData()
//        {
//            // 1. יצירת 10 מנהלים (MGR)
//            for (int i = 0; i < 10; i++)
//            {
//                // ID יהיה בין 0 ל-9, ותק אקראי בין 5 ל-15
//                Employees.Add(new Employee(i, Employee.EmployeeRole.MGR, 5 + i));
//            }

//            // 2. יצירת 10 רופאים (MED)
//            for (int i = 10; i < 20; i++)
//            {
//                // ID יהיה בין 10 ל-19
//                Employees.Add(new Employee(i, Employee.EmployeeRole.MED, i - 5));
//            }

//            // 3. יצירת 10 נהגים (DRV)
//            for (int i = 20; i < 30; i++)
//            {
//                // ID יהיה בין 20 ל-29
//                Employees.Add(new Employee(i, Employee.EmployeeRole.DRV, i - 15));
//            }

//            // 4. מילוי מטריצת הסינרגיה בערכים אקראיים (למשל בין 1 ל-10)
//            Random rand = new Random();
//            for (int i = 0; i < 30; i++)
//            {
//                for (int j = 0; j < 30; j++)
//                {
//                    if (i == j) SynergyMatrix[i, j] = 0; // עובד עם עצמו - אין משמעות לסינרגיה
//                    else SynergyMatrix[i, j] = rand.Next(1, 11);
//                }
//            }
//        }



















//    }





//}using System;


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SafeShiftAI_GUI
{
    public class Data_Layer
    {
        public List<int> ManagerIDs = new List<int>();
        public List<int> DoctorIDs = new List<int>();
        public List<int> DriverIDs = new List<int>();

        public Dictionary<int, string> EmployeeNames = new Dictionary<int, string>();
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public int[,] SynergyMatrix { get; set; }

        private Random random = new Random();

        public Data_Layer()
        {
            DatabaseHelper db = new DatabaseHelper();
            DataTable dt = db.GetEmployees();
            var synergyData = db.LoadSynergyData();

            SynergyMatrix = new int[1000, 1000];

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int id = Convert.ToInt32(row["Id"]);
                    string name = row["Name"].ToString();

                    // --- קריאת תעודת זהות ---
                    // אנחנו בודקים אם זה NULL וממירים ל-String
                    string realId = row["RealID"] != DBNull.Value ? row["RealID"].ToString() : "";
                    // -----------------------

                    string roleStr = row["Role"].ToString().Trim().ToLower();

                    int seniority = 0;
                    if (row["Seniority"] != DBNull.Value)
                        seniority = Convert.ToInt32(row["Seniority"]);

                    Employee.EmployeeRole myRole = Employee.EmployeeRole.DRV;
                    if (roleStr == "manager") myRole = Employee.EmployeeRole.MGR;
                    else if (roleStr == "doctor") myRole = Employee.EmployeeRole.MED;
                    else if (roleStr == "driver") myRole = Employee.EmployeeRole.DRV;

                    // שימוש בבנאי החדש שלך (עם realId)
                    Employee emp = new Employee(id, realId, name, myRole, seniority);
                    Employees.Add(emp);

                    // מילוי רשימות עזר
                    if (!EmployeeNames.ContainsKey(id)) EmployeeNames.Add(id, name);

                    if (myRole == Employee.EmployeeRole.MGR) ManagerIDs.Add(id);
                    if (myRole == Employee.EmployeeRole.MED) DoctorIDs.Add(id);
                    if (myRole == Employee.EmployeeRole.DRV) DriverIDs.Add(id);
                }
            }

            // מילוי מטריצת סינרגיה
            if (synergyData != null)
            {
                foreach (var entry in synergyData)
                {
                    string[] parts = entry.Key.Split('-');
                    int id1 = int.Parse(parts[0]);
                    int id2 = int.Parse(parts[1]);
                    if (id1 < 1000 && id2 < 1000) SynergyMatrix[id1, id2] = entry.Value;
                }
            }

            // --- טעינת ימי מחלה ---
            var sickDaysList = db.LoadSickDays();
            foreach (var record in sickDaysList)
            {
                var emp = Employees.FirstOrDefault(e => e.ID == record.EmpId);
                if (emp != null)
                {
                    emp.SickDays.Add(record.Day);
                }
            }
        }

        public int GetRandomWorkerID(int roleType)
        {
            if (roleType == 0)
            {
                if (ManagerIDs.Count == 0) throw new Exception("Error: No Managers found.");
                return ManagerIDs[random.Next(ManagerIDs.Count)];
            }
            if (roleType == 1)
            {
                if (DoctorIDs.Count == 0) throw new Exception("Error: No Doctors found.");
                return DoctorIDs[random.Next(DoctorIDs.Count)];
            }
            if (roleType == 2)
            {
                if (DriverIDs.Count == 0) throw new Exception("Error: No Drivers found.");
                return DriverIDs[random.Next(DriverIDs.Count)];
            }
            return 0;
        }

        public string GetName(int id)
        {
            if (EmployeeNames.ContainsKey(id)) return EmployeeNames[id];
            return id.ToString();
        }

        // פונקציה שמחזירה שם + ת"ז לתצוגה יפה בטבלה הסופית
        public string GetEmployeeDetails(int id)
        {
            var emp = Employees.FirstOrDefault(e => e.ID == id);
            if (emp != null)
            {
                return $"{emp.Name} ({emp.RealID})";
            }
            return "Unknown";
        }
    }
}
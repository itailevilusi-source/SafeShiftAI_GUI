using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shift_Safe_Smart
{
    internal class Data_Layer
    {
        public List<Employee> Employees { get; private set; } = new List<Employee>();//רשימה של העובדים 10 מכל מקצוע 
        public int[,] SynergyMatrix { get; private set; } = new int[30, 30];//מטריצת סינגריה 

        public Data_Layer()
        {
            InitializeData();
        }

        //בדיקת האלוגירתם 
        private void InitializeData()
        {
            // 1. יצירת 10 מנהלים (MGR)
            for (int i = 0; i < 10; i++)
            {
                // ID יהיה בין 0 ל-9, ותק אקראי בין 5 ל-15
                Employees.Add(new Employee(i, Employee.EmployeeRole.MGR, 5 + i));
            }

            // 2. יצירת 10 רופאים (MED)
            for (int i = 10; i < 20; i++)
            {
                // ID יהיה בין 10 ל-19
                Employees.Add(new Employee(i, Employee.EmployeeRole.MED, i - 5));
            }

            // 3. יצירת 10 נהגים (DRV)
            for (int i = 20; i < 30; i++)
            {
                // ID יהיה בין 20 ל-29
                Employees.Add(new Employee(i, Employee.EmployeeRole.DRV, i - 15));
            }

            // 4. מילוי מטריצת הסינרגיה בערכים אקראיים (למשל בין 1 ל-10)
            Random rand = new Random();
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    if (i == j) SynergyMatrix[i, j] = 0; // עובד עם עצמו - אין משמעות לסינרגיה
                    else SynergyMatrix[i, j] = rand.Next(1, 11);
                }
            }
        }



















    }





}

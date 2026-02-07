using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeShiftAI_GUI
{
    public class Employee
    {
        public int ID { get; private set; } // מזהה ייחודי שיופיע במערך הכרומוזום
        public string RealID { get; private set; } // תעודת זהות (9 ספרות) לתצוגה
        public string Name { get; private set; }//שם של העובד 


        public enum EmployeeRole//enum תפקידים
        {
            MGR,//מנהל 
            MED,//רופא
            DRV//נהג
               //סהכ 30 עובדים 
        }

        public EmployeeRole Role { get; private set; }//תפקיד העובד 
        public int Seniority { get; private set; }//מדד וותק של העובד ,וותק - נקבע רק פעם אחת,וותק - נקבע בבנאי ולא ניתן לשינוי מבחוץ
        public HashSet<int> SickDays { get; } = new HashSet<int>();//ימי מחלה ,ימי מחלה - הרשימה עצמה קבועה (readonly), אבל ניתן להוסיף/להסיר ממנה ימים

        public Employee(int id, string realId, string name, EmployeeRole role, int seniority)
        {
            this.ID = id;
            this.RealID = realId;
            this.Name = name;
            this.Role = role;
            this.Seniority = seniority;
        }
    }
}

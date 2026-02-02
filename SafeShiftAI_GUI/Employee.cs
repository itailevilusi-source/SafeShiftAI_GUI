using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shift_Safe_Smart
{
    internal class Employee
    {
        public int ID { get; private set; } // מזהה ייחודי שיופיע במערך הכרומוזום

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
        
        public Employee(int id, EmployeeRole role, int seniority)//Employee בנאי 
        {
            this.ID = id;
            this.Role = role;
            this.Seniority = seniority;
        }
    }
}

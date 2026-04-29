using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeShiftAI_GUI
{
    // מחלקה שתייצג שורה בטבלה הגרפית
    public class ShiftDisplayModel
    {
        public string Day { get; set; }
        public string Shift { get; set; }
        public string ManagerID { get; set; }
        public string DoctorID { get; set; }
        public string DriverID { get; set; }
    }
}

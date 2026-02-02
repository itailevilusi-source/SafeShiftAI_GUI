using Shift_Safe_Smart; 
using System;
using System.Collections.Generic;
// הוספנו את זה בשביל ה-DataTable
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SafeShiftAI_GUI
{
    public partial class Form1 : Form
    {
        // משתנים למנוע הגנטי
        private GeneticEngine engine;
        private Data_Layer dl;

        // משתנה לחיבור ל-SQL
        private DatabaseHelper dbHelper;

        public Form1()
        {
            InitializeComponent();
            // אתחול החיבור ל-SQL
            dbHelper = new DatabaseHelper();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // טעינת רשימת העובדים במידה והטבלה קיימת
            // שמתי ב-Try למקרה שהטבלה עדיין לא מוכנה במסך
            try
            {
                LoadEmployeesList();
            }
            catch { }
        }

        // --- חלק 1: ניהול עובדים (SQL) ---

        private void LoadEmployeesList()
        {
            // בדיקה שהטבלה קיימת בחלון (למנוע קריסה אם מחקת אותה בטעות)
            if (dgvEmployees != null)
            {
                dgvEmployees.DataSource = dbHelper.GetEmployees();
            }
        }

        // פונקציה לכפתור "הוסף עובד"
        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            // וודא שהשמות של תיבות הטקסט תואמים למה שיצרת בעיצוב
            string name = txtName.Text;
            string role = cmbRole.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("נא למלא שם ותפקיד");
                return;
            }

            // שליחה ל-SQL
            dbHelper.AddEmployee(name, role);

            // רענון הטבלה
            LoadEmployeesList();

            // ניקוי
            txtName.Text = "";
            MessageBox.Show("העובד נוסף בהצלחה!");
        }


        // --- חלק 2: אלגוריתם גנטי (Run) ---

        private async void btnRun_Click(object sender, EventArgs e)
        {
            dl = new Data_Layer();
            engine = new GeneticEngine(dl);

            // ניקוי הגרף
            chartFitness.Series.Clear();
            chartFitness.Series.Add("Fitness");
            chartFitness.Series["Fitness"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            // אירוע עדכון בזמן אמת
            engine.OnGenerationImproved += (best, gen) => {
                if (this.IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)delegate {
                        lblFitness.Text = $"ניקוד נוכחי: {best.Fitness}";

                        int progress = (int)((gen / 3000.0) * 100);
                        pbProgress.Value = Math.Min(progress, 100);

                        chartFitness.Series["Fitness"].Points.AddXY(gen, best.Fitness);

                        if (gen % 500 == 0)
                        {
                            dgvSchedule.DataSource = engine.GetBestScheduleForUI();
                            ColorRows();
                        }
                    });
                }
            };

            pbProgress.Enabled = false;

            try
            {
                await Task.Run(() => engine.RunEvolution());

                dgvSchedule.DataSource = engine.GetBestScheduleForUI();
                ColorRows();
                MessageBox.Show($"האופטימיזציה הסתיימה!\nניקוד סופי: {engine.BestSolution.Fitness}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה במהלך ההרצה: {ex.Message}");
            }
            finally
            {
                pbProgress.Enabled = true;
            }
        }

        private void ColorRows()
        {
            for (int i = 0; i < dgvSchedule.Rows.Count; i++)
            {
                int dayIndex = i / 3;
                if (dayIndex % 2 != 0)
                {
                    dgvSchedule.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                }
                else
                {
                    dgvSchedule.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

    }
}
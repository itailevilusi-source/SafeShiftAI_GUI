using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks; // חובה עבור async/await
using System.Windows.Forms;

namespace SafeShiftAI_GUI
{
    public partial class Form1 : Form
    {
        private DatabaseHelper dbHelper;
        private GeneticEngine geneticEngine;

        public Form1()
        {
            InitializeComponent();
            ApplyModernDesign();
            dbHelper = new DatabaseHelper();

            // מילוי רשימת הימים (1-30) בתיבת הסימון
            PopulateSickDaysList();

            // טעינת רשימת העובדים לטבלה ול-ComboBox
            LoadEmployeesList();

            //טעינת מטריצת ההתאמה 
            LoadSynergyToGrid();
        }

        // --- פונקציות עזר לאתחול ---

        private void PopulateSickDaysList()
        {
            clbSickDays.Items.Clear();
            for (int i = 1; i <= 30; i++)
            {
                clbSickDays.Items.Add($"יום {i}");
            }
        }

        private void LoadEmployeesList()
        {
            DataTable dt = dbHelper.GetEmployees();

            // 1. עדכון הטבלה הגדולה במסך (אם יש לך כזו)
            if (dgvEmployees != null) dgvEmployees.DataSource = dt;

            // 2. עדכון רשימת העובדים בבחירת ימי מחלה
            // אנחנו מציגים את השם, אבל הערך שנשמר מאחורי הקלעים הוא ה-ID הפנימי
            if (cmbSickEmployee != null)
            {
                cmbSickEmployee.DataSource = dt;
                cmbSickEmployee.DisplayMember = "Name"; // מה רואים
                cmbSickEmployee.ValueMember = "Id";     // מה הערך (מזהה פנימי)
            }
        }

        // --- כפתור 1: הוספת עובד חדש (כולל תעודת זהות) ---
        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string realId = txtRealID.Text; // התיבה החדשה שיצרת
            string role = cmbRole.SelectedItem?.ToString(); // וודא שיש לך ComboBox לתפקידים בשם cmbRole

            // המרה בטוחה של הוותק
            if (!int.TryParse(txtSeniority.Text, out int seniority)) seniority = 0;

            // בדיקות תקינות
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(role) || string.IsNullOrEmpty(realId))
            {
                MessageBox.Show("נא למלא את כל השדות: שם, תעודת זהות ותפקיד.");
                return;
            }

            // שמירה ב-DB באמצעות הפונקציה המעודכנת
            dbHelper.AddEmployee(realId, name, role, seniority);

            MessageBox.Show("העובד נוסף בהצלחה!");

            // ניקוי שדות ורענון התצוגה
            txtName.Clear();
            txtRealID.Clear();
            txtSeniority.Value = 0;
            LoadEmployeesList(); // חשוב: מרענן גם את הרשימה בניהול ימי מחלה
        }

        // --- כפתור 2: שמירת ימי מחלה ---
        private void btnSaveSickDays_Click(object sender, EventArgs e)
        {
            // בדיקה שנבחר עובד
            if (cmbSickEmployee.SelectedValue == null)
            {
                MessageBox.Show("נא לבחור עובד מהרשימה");
                return;
            }

            // המרת הערך הנבחר ל-INT (זה ה-ID הפנימי)
            if (!int.TryParse(cmbSickEmployee.SelectedValue.ToString(), out int internalId)) return;

            int count = 0;

            // מעבר על כל הימים שסומנו ב-V
            foreach (var item in clbSickDays.CheckedItems)
            {
                // הטקסט הוא "יום 1", "יום 5". נחלץ את המספר.
                string dayText = item.ToString().Replace("יום ", "");
                int dayNum = int.Parse(dayText);

                // המרה לאינדקס מערך (0-29) במקום (1-30)
                int arrayDayIndex = dayNum - 1;

                // שמירה ב-DB
                dbHelper.AddSickDay(internalId, arrayDayIndex);
                count++;
            }

            if (count > 0)
            {
                MessageBox.Show($"נשמרו {count} ימי מחלה לעובד שנבחר!");

                // איפוס הסימונים לפעם הבאה
                for (int i = 0; i < clbSickDays.Items.Count; i++)
                    clbSickDays.SetItemChecked(i, false);
            }
            else
            {
                MessageBox.Show("לא נבחרו ימים לסימון.");
            }
        }

        // --- כפתור 3: הרצת האלגוריתם ---
        // --- עדכון 1: כפתור ההפעלה (מנקה את הגרף בהתחלה) ---
        private async void btnRunAlgorithm_Click(object sender, EventArgs e)
        {
            btnRunAlgorithm.Enabled = false;
            lblStatus.Text = "מתחיל תהליך...";

            // ניקוי טבלה
            dgvSchedule.DataSource = null;

            // === קוד חדש לגרף ===
            // וודא שיש סדרה בגרף
            if (chartFitness.Series.Count == 0)
            {
                chartFitness.Series.Add("Fitness");
                chartFitness.Series["Fitness"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chartFitness.Series["Fitness"].BorderWidth = 3;
            }
            // ניקוי נקודות ישנות
            chartFitness.Series[0].Points.Clear();
            // ====================

            try
            {
                // ... (כל קוד הטעינה והבדיקות שלך נשאר אותו דבר) ...
                Data_Layer currentData = new Data_Layer();
                if (currentData.Employees.Count == 0) throw new Exception("אין עובדים!");

                geneticEngine = new GeneticEngine(currentData);
                geneticEngine.OnGenerationImproved += GeneticEngine_OnGenerationImproved;

                lblStatus.Text = "מריץ אופטימיזציה...";

                Chromosome bestSolution = await Task.Run(() => geneticEngine.RunEvolution());

                var uiList = geneticEngine.GetBestScheduleForUI();
                DisplaySchedule(uiList);

                lblStatus.Text = $"סיום! ציון סופי: {bestSolution.Fitness:0.00}";
                MessageBox.Show($"התהליך הסתיים בהצלחה!\nציון סופי: {bestSolution.Fitness:0.00}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnRunAlgorithm.Enabled = true;
            }
        }

        // --- עדכון 2: האירוע בזמן אמת (מוסיף נקודות לגרף) ---
        private void GeneticEngine_OnGenerationImproved(Chromosome bestSoFar, int generation)
        {
            // משתמשים ב-Invoke כדי לעדכן את המסך מתהליך הרקע
            this.Invoke(new Action(() =>
            {
                // עדכון הטקסט
                lblStatus.Text = ($"דור: {generation} | ציון: {bestSoFar.Fitness:0.00}");

                // === עדכון הגרף (החלק החסר!) ===
                if (chartFitness.Series.Count > 0)
                {
                    // הוספת נקודה: ציר X = דור, ציר Y = ציון
                    chartFitness.Series[0].Points.AddXY(generation, bestSoFar.Fitness);

                    // גורם לגרף להתעדכן מיד
                    chartFitness.Update();
                }
                // ================================
            }));
        }

        private void LoadSynergyToGrid()
        {
            if (dgvSynergy == null) return;

            // 1. איפוס ועיצוב בסיסי
            dgvSynergy.DataSource = null;
            dgvSynergy.Rows.Clear();
            dgvSynergy.Columns.Clear();
            dgvSynergy.AllowUserToAddRows = false;

            // ביטול דחיסת העמודות - זה מה שגורם לבלאגן!
            dgvSynergy.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // עיצוב כיוון טקסט (חשוב לעברית)
            dgvSynergy.RightToLeft = RightToLeft.Yes;

            // 2. שליפת נתונים
            DataTable dtEmployees = dbHelper.GetEmployees();
            var synergyDict = dbHelper.LoadSynergyData();

            // 3. הוספת עמודה ראשונה קבועה (שמות העובדים)
            dgvSynergy.Columns.Add("MainColumn", "עובד");
            dgvSynergy.Columns["MainColumn"].ReadOnly = true;
            dgvSynergy.Columns["MainColumn"].Frozen = true; // הקפאת העמודה!
            dgvSynergy.Columns["MainColumn"].Width = 150;   // רוחב נדיב לשם
            dgvSynergy.Columns["MainColumn"].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(230, 230, 250); // צבע רקע שונה
            dgvSynergy.Columns["MainColumn"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);

            // 4. יצירת עמודות לכל עובד
            foreach (DataRow row in dtEmployees.Rows)
            {
                string empName = row["Name"].ToString();
                int empId = Convert.ToInt32(row["Id"]);

                // יצירת ראשי תיבות לשם העמודה כדי לחסוך מקום (למשל "Avi Cohen" -> "Avi C.")
                string shortName = empName;
                var parts = empName.Split(' ');
                if (parts.Length > 1) shortName = $"{parts[0]} {parts[1][0]}.";

                string colName = "col_" + empId;
                dgvSynergy.Columns.Add(colName, shortName);

                // הגדרות עיצוב לעמודות הנתונים
                dgvSynergy.Columns[colName].Width = 70; // רוחב קבוע ונוח
                dgvSynergy.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // 5. מילוי השורות
            foreach (DataRow rowA in dtEmployees.Rows)
            {
                int idA = Convert.ToInt32(rowA["Id"]);
                string nameA = rowA["Name"].ToString();

                int rowIndex = dgvSynergy.Rows.Add();
                dgvSynergy.Rows[rowIndex].Cells[0].Value = nameA;
                dgvSynergy.Rows[rowIndex].Tag = idA;
                dgvSynergy.Rows[rowIndex].Height = 35; // שורה גבוהה יותר

                for (int i = 0; i < dtEmployees.Rows.Count; i++)
                {
                    DataRow rowB = dtEmployees.Rows[i];
                    int idB = Convert.ToInt32(rowB["Id"]);
                    int colIndex = i + 1;

                    if (idA == idB)
                    {
                        dgvSynergy.Rows[rowIndex].Cells[colIndex].Style.BackColor = System.Drawing.Color.Gray;
                        dgvSynergy.Rows[rowIndex].Cells[colIndex].ReadOnly = true;
                    }
                    else
                    {
                        string key1 = $"{idA}-{idB}";
                        string key2 = $"{idB}-{idA}";
                        int score = 0;

                        if (synergyDict.ContainsKey(key1)) score = synergyDict[key1];
                        else if (synergyDict.ContainsKey(key2)) score = synergyDict[key2];

                        // צביעת תאים לפי הציון (ויזואליזציה יפה!)
                        dgvSynergy.Rows[rowIndex].Cells[colIndex].Value = score;
                        if (score > 0) dgvSynergy.Rows[rowIndex].Cells[colIndex].Style.BackColor = System.Drawing.Color.FromArgb(200, 255, 200); // ירוק בהיר
                        if (score < 0) dgvSynergy.Rows[rowIndex].Cells[colIndex].Style.BackColor = System.Drawing.Color.FromArgb(255, 200, 200); // אדום בהיר
                    }
                }
            }
        }
        // פונקציה למילוי ה-DataGridView של הלוח
        private void DisplaySchedule(List<ShiftDisplayModel> scheduleList)
        {
            dgvSchedule.DataSource = null;
            dgvSchedule.DataSource = scheduleList;

            // הגדרת כותרות בעברית
            if (dgvSchedule.Columns.Count > 0)
            {
                dgvSchedule.Columns["Day"].HeaderText = "יום";
                dgvSchedule.Columns["Shift"].HeaderText = "משמרת";
                dgvSchedule.Columns["ManagerID"].HeaderText = "מנהל (ת.ז)";
                dgvSchedule.Columns["DoctorID"].HeaderText = "רופא (ת.ז)";
                dgvSchedule.Columns["DriverID"].HeaderText = "נהג (ת.ז)";

                // עיצוב קטן
                dgvSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        // הוסף את זה בתוך Form1.cs

        private void ApplyModernDesign()
        {
            // 1. הגדרת גודל חלון התחלתי גדול
            // === שינוי: במקום מסך מלא, גודל קבוע ונוח ===
            this.WindowState = FormWindowState.Normal; // מצב רגיל (לא מקסימלי)
            this.Size = new System.Drawing.Size(1280, 800); // גודל רחב אבל לא תופס את כל המסך
            this.StartPosition = FormStartPosition.CenterScreen; // ייפתח באמצע המסך
            this.FormBorderStyle = FormBorderStyle.Sizable; // מאפשר לך להגדיל/להקטין ידנית אם תרצה

            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);

            // 2. עיצוב כפתורים
            StyleButton(btnAddEmployee);
            StyleButton(btnSaveSickDays);
            StyleButton(btnRunAlgorithm);

            Control[] matches = this.Controls.Find("btnSaveSynergy", true);
            if (matches.Length > 0 && matches[0] is Button) StyleButton((Button)matches[0]);

            // 3. עיצוב טבלאות
            StyleGrid(dgvSchedule);
            StyleGrid(dgvSynergy); // הטבלה הזו קיבלה טיפול מיוחד ב-LoadSynergyToGrid
            if (dgvEmployees != null) StyleGrid(dgvEmployees);

            // 4. עיצוב תווית סטטוס
            if (lblStatus != null)
            {
                lblStatus.ForeColor = System.Drawing.Color.FromArgb(51, 102, 255);
                lblStatus.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            }

            // 5. שיפור עיצוב הגרף (Spline = קו מעוגל)
            if (chartFitness != null)
            {
                chartFitness.BackColor = System.Drawing.Color.White;
                chartFitness.ChartAreas[0].BackColor = System.Drawing.Color.White;

                // ביטול קווי רשת צפופים
                chartFitness.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                chartFitness.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                chartFitness.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
                chartFitness.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;

                if (chartFitness.Series.Count > 0)
                {
                    chartFitness.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline; // קו מעוגל ויפה
                    chartFitness.Series[0].BorderWidth = 4;
                    chartFitness.Series[0].Color = System.Drawing.Color.FromArgb(51, 102, 255);
                }
            }
        }

        // פונקציית עזר לעיצוב כפתור
        private void StyleButton(Button btn)
        {
            if (btn == null) return;
            btn.BackColor = System.Drawing.Color.FromArgb(51, 102, 255); // כחול רויאל
            btn.ForeColor = System.Drawing.Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            btn.Height = 40; // כפתור גבוה ונוח יותר
        }

        // פונקציית עזר לעיצוב טבלה
        private void StyleGrid(DataGridView grid)
        {
            if (grid == null) return;

            grid.BackgroundColor = System.Drawing.Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.EnableHeadersVisualStyles = false;

            // כותרת הטבלה
            grid.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(51, 102, 255);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersHeight = 40;

            // שורות הטבלה
            grid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(200, 220, 255);
            grid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            grid.DefaultCellStyle.Padding = new Padding(5);
            grid.RowTemplate.Height = 35;
            grid.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void StyleLabel(Label lbl, bool isHeader)
        {
            if (lbl == null) return;
            if (isHeader)
            {
                lbl.ForeColor = System.Drawing.Color.FromArgb(51, 102, 255);
                lbl.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            }
        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtSeniority_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dgvSynergy_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveSynergy_Click(object sender, EventArgs e)
        {
            int count = 0;

            // רצים על כל השורות
            foreach (DataGridViewRow row in dgvSynergy.Rows)
            {
                if (row.Tag == null) continue;
                int id1 = (int)row.Tag; // שליפת ה-ID מהשורה

                // רצים על כל העמודות (מדלגים על עמודה 0 שהיא השמות)
                for (int i = 1; i < dgvSynergy.Columns.Count; i++)
                {
                    // שליפת ה-ID של העמודה (השם שלה הוא col_5 למשל)
                    string colName = dgvSynergy.Columns[i].Name;
                    int id2 = int.Parse(colName.Replace("col_", ""));

                    // אם זה האלכסון או אין ערך - מדלגים
                    var cellValue = row.Cells[i].Value;
                    if (cellValue == null || cellValue.ToString() == "X") continue;

                    if (int.TryParse(cellValue.ToString(), out int score))
                    {
                        // שמירה ב-DB (רק פעם אחת לכל זוג כדי לחסוך, או לדרוס הכל)
                        // כאן נשמור הכל וזה בסדר גמור
                        dbHelper.SaveSynergy(id1, id2, score);
                        count++;
                    }
                }
            }

            MessageBox.Show("הנתונים נשמרו בהצלחה!");
        }

        private void cmbSickEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            // הגנה מפני קריסה כשהטופס רק עולה
            if (cmbSickEmployee.SelectedValue == null) return;

            // ניסיון להמיר את הערך ל-ID
            if (int.TryParse(cmbSickEmployee.SelectedValue.ToString(), out int empId))
            {
                // 1. איפוס כל ה-V הקיימים
                for (int i = 0; i < clbSickDays.Items.Count; i++)
                {
                    clbSickDays.SetItemChecked(i, false);
                }

                // 2. הבאת הימים מה-SQL
                List<int> sickDays = dbHelper.GetSickDaysForEmployee(empId);

                // 3. סימון ה-V במקומות הנכונים
                foreach (int day in sickDays)
                {
                    // בדיקת גבולות (למנוע קריסה אם היום הוא 30 והמערך קצר יותר)
                    if (day >= 0 && day < clbSickDays.Items.Count)
                    {
                        clbSickDays.SetItemChecked(day, true);
                    }
                }
            }
        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
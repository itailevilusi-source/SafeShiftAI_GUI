using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks; //  עבור async/await מניעת קיפאון של המסך
using System.Windows.Forms;

namespace SafeShiftAI_GUI
{
    //partial-הform מחולק לשני חלקים המטרה להדגיש שמדובר במחלקה אחת גדולה
    public partial class Form1 : Form
    {
        private DatabaseHelper dbHelper;
        private GeneticEngine geneticEngine;

        public Form1()
        {
            InitializeComponent();//מציירת את הכפתורים והטבלאות על המסך לפני שרואים אותם
            ApplyModernDesign();//פונקצית עיצוב 
            dbHelper = new DatabaseHelper();

            // מילוי רשימת הימים (1-30) בתיבת הסימון
            PopulateSickDaysList();

            // טעינת רשימת העובדים לטבלה ול-ComboBox
            LoadEmployeesList();

            //טעינת מטריצת ההתאמה 
            LoadSynergyToGrid();
        }

        //  פונקציות עזר לאתחול 

        private void PopulateSickDaysList()
        {
            clbSickDays.Items.Clear();//ניקוי הרשימה לפני המילוי
            for (int i = 1; i <= 30; i++)
            {
                clbSickDays.Items.Add($"יום {i}");
            }
        }

        private void LoadEmployeesList()
        {
            DataTable dt = dbHelper.GetEmployees();//טבלת העובדים מsql

            //  עדכון הטבלה הגדולה במסך אם יש 
            if (dgvEmployees != null) dgvEmployees.DataSource = dt;

            //  עדכון רשימת העובדים-מעל ימי המחלה
            //הערך הוא הid הפנימי אך אנחנו מציגים את השם של העובד
            if (cmbSickEmployee != null)
            {
               
                cmbSickEmployee.DataSource = dt;//מסירת טבלת הנתונים כdata
                cmbSickEmployee.DisplayMember = "Name"; // מה רואים ובוחרים במסך
                cmbSickEmployee.ValueMember = "Id";     //-id כדי לשלוח לשרת מה הערך האמיתי מזהה פנימי זה הערך שמעניין אותנו והוא זה שנשמר
                                                        //זאת כדי להבטיח שאם לשני עובדים יש את אותו שם לכל אחד יהיה מזהה יחודי
            }
        }

        // כפתור 1: הוספת עובד חדש  
        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            //TextBox:אם המשתמש לא הקליד כלום אז יהיה ""
            //ComboBox:אם המשתמש פתח ולא בחר כלום יהיה null

            //לוקחים את הנתונים מהדף שלנו 
            string name = txtName.Text;
            string realId = txtRealID.Text; 
            string role = cmbRole.SelectedItem?.ToString(); //מוודאים שאכן המשתמש בחר תפקיד מהתיבת בחירה
                                                            //  ואל תנסה להמיר אותו למחרוזת כי אז התוכנה תקרוס אלה פשוט תשאר הסימן שאלה אומר שאם יש null תשאיר אותו 

            // המרה בטוחה של הוותק מנסים להמיר לint אם לא מצליחים אז וותק=0
            if (!int.TryParse(txtSeniority.Text, out int seniority)) seniority = 0;

            // בדיקות תקינות
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(role) || string.IsNullOrEmpty(realId))
            {
                MessageBox.Show("נא למלא את כל השדות: שם, תעודת זהות ותפקיד");
                return;
            }

            // שמירה העובד במסד הנתונים  
            dbHelper.AddEmployee(realId, name, role, seniority);

            MessageBox.Show("העובד נוסף בהצלחה");//הודעה קופצת למסך

            // ניקוי שדות ורענון התצוגה
            txtName.Clear();
            txtRealID.Clear();
            txtSeniority.Value = 0;
            LoadEmployeesList(); // חשוב: מרענן גם את הרשימה בניהול ימי מחלה
             //  רענון גם של המטריצה כדי שהעובד החדש יופיע 
            LoadSynergyToGrid();
        }

        // כפתור 2: שמירת ימי מחלה 
        private void btnSaveSickDays_Click(object sender, EventArgs e)
        {
            // בדיקה שנבחר עובד
            if (cmbSickEmployee.SelectedValue == null)
            {
                MessageBox.Show("נא לבחור עובד מהרשימה");
                return;
            }

            // מנסים להמיר את את העובד שבחרו בשם שלמעשה מדובר בid האישי שלו לint
            //הגדרנו בsql את הid int אבל SelectedValue מחזיר אובייקט ולכן צריך להמיר
            if (!int.TryParse(cmbSickEmployee.SelectedValue.ToString(), out int internalId)) return;

            dbHelper.ClearSickDaysForEmployee(internalId);//מחיקת הימים של העובד

            int count = 0;//כמות הימים שנבחרו

            // מעבר על כל הימים שסומנו ב-V
            foreach (var item in clbSickDays.CheckedItems)
            {
                //  הטקסט הוא "יום 1" למשל ניקח רק את המספר אנחנו מחליפים את יום ב ""
                string dayText = item.ToString().Replace("יום ", "");
                int dayNum = int.Parse(dayText);

                //צריך לחסר באחד מכיוון שהמערך הוא מ0 ולא מ1 כמו שמוצג למשתמש
                int arrayDayIndex = dayNum - 1;

                // שמירה בבסיס הנתונים
                dbHelper.AddSickDay(internalId, arrayDayIndex);
                count++;
            }

            if (count > 0)
            {
                MessageBox.Show($"נשמרו {count} ימי מחלה לעובד שנבחר");

                // איפוס הסימונים לפעם הבאה -ניקוי
                for (int i = 0; i < clbSickDays.Items.Count; i++)
                    clbSickDays.SetItemChecked(i, false);
            }
            else
            {
                MessageBox.Show("לא נבחרו ימים שסומנו");
            }
        }

        // כפתור 3: הרצת האלגוריתם 
        //async:לעשה הפונקציה הזו הולכת לחכות לפעולה  באמצעות שימוש ב await
        private async void btnRunAlgorithm_Click(object sender, EventArgs e)
        {
            btnRunAlgorithm.Enabled = false;// נועלים את הכפתור שהמשתמש לא יריץ כמה חישוביים גנטיים במקביל
            btnRunBacktracking.Enabled = false;
            lblStatus.Text = "מתחיל תהליך";

            // ניקוי טבלה בהתחלה
            dgvSchedule.DataSource = null;

            
            // מוודאים שאין קו
            if (chartFitness.Series.Count == 0)
            {
                chartFitness.Series.Add("Fitness");//שם הסדרה
                chartFitness.Series["Fitness"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;//סוג קו רציף
                chartFitness.Series["Fitness"].BorderWidth = 3;//עובי 3 פיקסלים
            }
            // ניקוי נקודות ישנות
            chartFitness.Series[0].Points.Clear();
            

            try
            {
               //טעינה של הדאטה 
                Data_Layer currentData = new Data_Layer();
                if (currentData.Employees.Count == 0) throw new Exception("אין עובדים!");

                geneticEngine = new GeneticEngine(currentData);
                geneticEngine.OnGenerationImproved += GeneticEngine_OnGenerationImproved;//בכל פעם שמוצאים דור יותר טוב מציירים נקודה בגרף באמצעות הפונקציה

                lblStatus.Text = "מריץ אופטימיזציה";

                Chromosome bestSolution = await Task.Run(() => geneticEngine.RunEvolution());//תהליך רקע  שלא עובד עלThread הראשי 

                //var uiList = geneticEngine.GetBestScheduleForUI();
                //DisplaySchedule(uiList);

                //lblStatus.Text = $"סיום ציון סופי: {bestSolution.Fitness:0.00}";
                //MessageBox.Show($"התהליך הסתיים בהצלחה!\nציון סופי: {bestSolution.Fitness:0.00}");

                //  קודם כל מציגים את התוצאה של האלגוריתם הגנטי בטבלה ביניים לפני שדרוג
                var uiList = geneticEngine.GetBestScheduleForUI();
                DisplaySchedule(uiList);
                lblStatus.Text = $"סיום שלב גנטי ציון ביניים: {bestSolution.Fitness:0.00}";
                lblStatus.ForeColor = System.Drawing.Color.Blue;

                //  הקפצת חלון  ששואל את המשתמש אם להפעיל את האלגוריתם ההיברידי שלנו
                DialogResult dialogResult = MessageBox.Show(
                    $"האלגוריתם הגנטי סיים בהצלחה\nציון הלוח כרגע: {bestSolution.Fitness:0.00}\n\nהאם תרצה להפעיל אלגוריתם חיפוש מקומי (Steepest-Ascent Hill Climbing) לשיפור וליטוש סופי של הלוח?",
                    "אופטימיזציה סופית (מערכת היברידית)",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                // אם נלחץ כן
                if (dialogResult == DialogResult.Yes)
                {
                    lblStatus.Text = "מבצע ליטוש סופי (Local Search)... מחפש את השיפור המקסימלי";
                    lblStatus.ForeColor = System.Drawing.Color.Orange; //  צבע רקע שהתהליך קורה 

                    // שומרים את הציון לפני הליטוש כדי שנוכל להשוות
                    double scoreBeforeLocalSearch = bestSolution.Fitness;

                    //שוב כדי שלא יתקע המסך משתמשים בתהליכון רקע
                    await Task.Run(() => geneticEngine.ExecuteLocalSearch(bestSolution));

                    // עדכון הטבלה מחדש עם הלוח גם אם היה שינוי וגם אם לא
                    uiList = geneticEngine.GetBestScheduleForUI();
                    DisplaySchedule(uiList);

                    // בודקים האם הציון באמת השתפר
                    if (bestSolution.Fitness > scoreBeforeLocalSearch)
                    {
                        // מקרה 1: היה שיפור
                        lblStatus.Text = $"סיום היברידי שופר מ-{scoreBeforeLocalSearch:0.00} ל-{bestSolution.Fitness:0.00}";
                        lblStatus.ForeColor = System.Drawing.Color.Green;

                        // עדכון הגרף עם הקפיצה
                        if (chartFitness.Series[0].Points.Count > 0)
                        {
                            chartFitness.Series[0].Points.AddXY(3000, bestSolution.Fitness);
                            chartFitness.Update();
                        }

                        MessageBox.Show(
                            $"הליטוש הסתיים בהצלחה\nהחיפוש המקומי סרק את כל האפשרויות ומצא שיפור.\n\nציון קודם: {scoreBeforeLocalSearch:0.00}\nציון סופי ומשופר: {bestSolution.Fitness:0.00}",
                            "סיום תהליך היברידי (נמצא שיפור)",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    else
                    {
                        // מקרה 2: הלוח היה מושלםLocal Optimom
                        lblStatus.Text = $"הלוח אופטימלי הציון נשאר: {bestSolution.Fitness:0.00}";
                        lblStatus.ForeColor = System.Drawing.Color.Green;

                        MessageBox.Show(
                            $"הסריקה הסתיימה!\nהחיפוש המקומי בדק עשרות אלפי אפשרויות והוכיח שהלוח כבר נמצא באופטימום מקומי (הציון הגבוה ביותר האפשרי בסביבה זו).\n\nהציון נשאר מקסימלי: {bestSolution.Fitness:0.00}",
                            "סיום תהליך היברידי (ללא שינוי)",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                
                }
                else
                {
                    //  אם המשתמש לחץ לא והסתפק בפתרון של הגנטי נאפשר לו
                    lblStatus.Text = $"סיום ציון סופי: {bestSolution.Fitness:0.00}";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    MessageBox.Show("השיבוץ הושלם ונשמר ללא ליטוש נוסף.", "סיום", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                
            }
            //תפיסת השגיאות
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // שחרור כפתורים להרצה חוזרת 
                btnRunAlgorithm.Enabled = true;
                btnRunBacktracking.Enabled = true;

            }
        }

        // עדכון האירוע בזמן אמת-מוסיף נקודות לגרף : += GeneticEngine_OnGenerationImproved
        private void GeneticEngine_OnGenerationImproved(Chromosome bestSoFar, int generation)
        {
            //רק תהליך UI Thread רשאי לשנות את המסך
            // משתמשים ב-Invoke כדי לעדכן את המסך מתהליך הרקע
            if (this.IsDisposed || !this.IsHandleCreated) return;//בדיקה שהטופס לא נסגר או הולך להיסגר
            
            //האם הקוד הגיע מתהליך רקע
            if (this.InvokeRequired)
            {
                //האחריות עוזבת את תהליך הרקע ועוברת ל UI
                this.BeginInvoke(new Action(() => GeneticEngine_OnGenerationImproved(bestSoFar, generation)));
                return; 
            }

            // עדכון הטקסט מה הציון והדור איך גבוהים שהגענו
            lblStatus.Text = ($"דור: {generation} | ציון: {bestSoFar.Fitness:0.00}");

                // עדכון הגרף  
                //מוודאים שיש כבר קו על המסך שיש להמשיך
                if (chartFitness.Series.Count > 0)
                {
                    // מוסיפים נקודה 
                    //ציר x: דור
                    //ציר y: ציון הלוח
                    //Chart יודע אוטומטית לחבר את הנקודה החדשה לנקודה הקודמת עם קו כי הוא מוגדר כקו רציף מעוגל
                    chartFitness.Series[0].Points.AddXY(generation, bestSoFar.Fitness);

                    // גורם לגרף להתעדכן מיד ולא לחכות כדי שהגרף יראה חי על המסך
                    chartFitness.Update();
                }
               
           
        }

        //טבלת הסינגריה של העובדים
        private void LoadSynergyToGrid()
        {
            if (dgvSynergy == null) return;

            //  איפוס ועיצוב בסיסי תמיד מתחילים מלוח ריק כדי שלא יהיה את העובד פעמיים שיהיה אחיד
            dgvSynergy.DataSource = null;
            dgvSynergy.Rows.Clear();//ניקוי שורות
            dgvSynergy.Columns.Clear();//ניקוי עמודות
            dgvSynergy.AllowUserToAddRows = false;//מונע מהמשתמש להוסיף שורה מתחת לטבלה

            // ביטול דחיסת העמודות זה גרם לי לבלאגן כי המחשב מנסה לדחוף את כל העמודות לגודל המסך ואז זה לא קריא
            dgvSynergy.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            //  כיוון טקסט לעברית
            dgvSynergy.RightToLeft = RightToLeft.Yes;

            // שליפת נתונים
            DataTable dtEmployees = dbHelper.GetEmployees();
            var synergyDict = dbHelper.LoadSynergyData();// DatabaseHelperמקבלים את הנתונים בתור מילון כמו שבניתי 
                                                         //Dictionary<string, int> "1-2",ציון

            // הוספת עמודה ראשונה קבועה שמות העובדים
            dgvSynergy.Columns.Add("MainColumn", "עובד");
            dgvSynergy.Columns["MainColumn"].ReadOnly = true;
            dgvSynergy.Columns["MainColumn"].Frozen = true; // הקפאת העמודה גם אם גוללים עדין נראה את העמודה
            //צבע ורוחב בולטים לעמודה זו
            dgvSynergy.Columns["MainColumn"].Width = 150;   // רוחב גדול לשם
            dgvSynergy.Columns["MainColumn"].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(230, 230, 250); // צבע רקע שונה
            dgvSynergy.Columns["MainColumn"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);

            // יצירת עמודות לכל עובד
            foreach (DataRow row in dtEmployees.Rows)
            {
                string empName = row["Name"].ToString();
                int empId = Convert.ToInt32(row["Id"]);

                // יצירת ראשי תיבות לשם העמודה כדי לחסוך מקום היה לי מאוד צפוף בעין "Avi Cohen" ל "Avi C."
                string shortName = empName;
                var parts = empName.Split(' ');
                if (parts.Length > 1) shortName = $"{parts[0]} {parts[1][0]}.";

                //מזהה פנימי לעמודה "col_6"
                string colName = "col_" + empId;
                dgvSynergy.Columns.Add(colName, shortName);

                // הגדרות עיצוב לעמודות הנתונים
                dgvSynergy.Columns[colName].Width = 70; // רוחב קבוע ונוח
                dgvSynergy.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//סנטר
            }

            // מילוי השורות
            foreach (DataRow rowA in dtEmployees.Rows)
            {
                int idA = Convert.ToInt32(rowA["Id"]);
                string nameA = rowA["Name"].ToString();


                int rowIndex = dgvSynergy.Rows.Add();
                dgvSynergy.Rows[rowIndex].Cells[0].Value = nameA;//השם 
                dgvSynergy.Rows[rowIndex].Tag = idA;//מזהה מוחבא בtag
                dgvSynergy.Rows[rowIndex].Height = 35; 

                //עובד מול עובד ציון שלהם
                for (int i = 0; i < dtEmployees.Rows.Count; i++)
                {
                    DataRow rowB = dtEmployees.Rows[i];
                    int idB = Convert.ToInt32(rowB["Id"]);
                    int colIndex = i + 1;//+1 MainColumn

                    //אותו אדם
                    if (idA == idB)
                    {
                        dgvSynergy.Rows[rowIndex].Cells[colIndex].Style.BackColor = System.Drawing.Color.Gray;
                        dgvSynergy.Rows[rowIndex].Cells[colIndex].ReadOnly = true;
                    }
                    else
                    {
                        //הכנת המפתח למילון
                        string key1 = $"{idA}-{idB}";
                        string key2 = $"{idB}-{idA}";
                        int score = 0;

                        //האם המפתח קיים אם כן score מקבל אותו
                        if (synergyDict.ContainsKey(key1)) score = synergyDict[key1];
                        else if (synergyDict.ContainsKey(key2)) score = synergyDict[key2];

                        //צביעה
                        dgvSynergy.Rows[rowIndex].Cells[colIndex].Value = score;
                        if (score > 0) dgvSynergy.Rows[rowIndex].Cells[colIndex].Style.BackColor = System.Drawing.Color.FromArgb(200, 255, 200); // ירוק בהיר
                        if (score < 0) dgvSynergy.Rows[rowIndex].Cells[colIndex].Style.BackColor = System.Drawing.Color.FromArgb(255, 200, 200); // אדום בהיר
                    }
                }
            }
            //מניעת מיון במסך
            foreach (DataGridViewColumn column in dgvSynergy.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        // DataGridView מילוי
        private void DisplaySchedule(List<ShiftDisplayModel> scheduleList)
        {
            dgvSchedule.DataSource = null;//מחיקה למען עדכון הנתונים
            dgvSchedule.DataSource = scheduleList;

            //  כותרות בעברית
            if (dgvSchedule.Columns.Count > 0)
            {
                dgvSchedule.Columns["Day"].HeaderText = "יום";
                dgvSchedule.Columns["Shift"].HeaderText = "משמרת";
                dgvSchedule.Columns["ManagerID"].HeaderText = "מנהל ת.ז";
                dgvSchedule.Columns["DoctorID"].HeaderText = "רופא ת.ז";
                dgvSchedule.Columns["DriverID"].HeaderText = "נהג ת.ז";

                // עיצוב 
                dgvSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        
        //עיצוב של החלון
        private void ApplyModernDesign()
        {
            //גודל קבוע ונוח
            this.WindowState = FormWindowState.Normal;
            this.Size = new System.Drawing.Size(1280, 800); 
            this.StartPosition = FormStartPosition.CenterScreen; 
            this.FormBorderStyle = FormBorderStyle.Sizable; 

            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);

            // עיצוב כפתורים
            StyleButton(btnAddEmployee);
            StyleButton(btnSaveSickDays);
            StyleButton(btnRunAlgorithm);

            Control[] matches = this.Controls.Find("btnSaveSynergy", true);//חיפוש הכפתור באמצעות רקורסיה
            if (matches.Length > 0 && matches[0] is Button) StyleButton((Button)matches[0]);

            // עיצוב טבלאות
            StyleGrid(dgvSchedule);
            StyleGrid(dgvSynergy); 
            if (dgvEmployees != null) StyleGrid(dgvEmployees);

            // עיצוב תווית סטטוס
            if (lblStatus != null)
            {
                lblStatus.ForeColor = System.Drawing.Color.FromArgb(51, 102, 255);
                lblStatus.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            }

            // שיפור עיצוב הגרף של התקדמות האלוגריטם הגנטי
            if (chartFitness != null)
            {
                chartFitness.BackColor = System.Drawing.Color.White;
                chartFitness.ChartAreas[0].BackColor = System.Drawing.Color.White;

                // שינוי הקווים של הטבלה
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
            btn.BackColor = System.Drawing.Color.FromArgb(51, 102, 255); 
            btn.ForeColor = System.Drawing.Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            btn.Height = 40; 
        }

        // פונקציית עזר לעיצוב טבלה
        private void StyleGrid(DataGridView grid)
        {
            if (grid == null) return;

            grid.BackgroundColor = System.Drawing.Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.EnableHeadersVisualStyles = false;//כדי לאפשר עיצוב של הטבלה

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


        //כפתור שמירה סינגריה 
        private void btnSaveSynergy_Click(object sender, EventArgs e)
        {
            int count = 0;//כמות שמירות

            // רצים על כל השורות
            foreach (DataGridViewRow row in dgvSynergy.Rows)
            {
               
                if (row.Tag != null)
                {
                    int id1 = (int)row.Tag; //שורה ID 

                    // עוברים על כל העמודות
                    for (int i = 1; i < dgvSynergy.Columns.Count; i++)
                    {
                       
                        string colName = dgvSynergy.Columns[i].Name; //col_5 למשל
                        int id2 = int.Parse(colName.Replace("col_", ""));

                        var cellValue = row.Cells[i].Value;//הציון

                        if (cellValue != null && cellValue.ToString() != "X")
                        {
                            // המרה
                            if (int.TryParse(cellValue.ToString(), out int score))
                            {
                                // שמירה במסד הנתונים 
                                dbHelper.SaveSynergy(id1, id2, score);
                                count++;
                            }
                        }
                    }
                }
            }

            MessageBox.Show("הנתונים נשמרו בהצלחה");
        }

        //פונקציה סינכרון ימי מחלה
        private void cmbSickEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            // מניעת קריסה כשהטופס רק עולה
            if (cmbSickEmployee.SelectedValue == null) return;

            
            if (int.TryParse(cmbSickEmployee.SelectedValue.ToString(), out int empId))
            {
                // V איפוס
                for (int i = 0; i < clbSickDays.Items.Count; i++)
                {
                    clbSickDays.SetItemChecked(i, false);
                }

                //  טעינה של הימים של העובד לפי מסד הנתונים
                List<int> sickDays = dbHelper.GetSickDaysForEmployee(empId);

                
                foreach (int day in sickDays)
                {
                  
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

        //כפתור מחיקת העובד
        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            //  האם המשתמש בחר שורה בטבלת העובדים
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                //id
                int selectedEmpId = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["Id"].Value);
                string empName = dgvEmployees.SelectedRows[0].Cells["Name"].Value.ToString();

                // הודעת אזהרה
                DialogResult dialogResult = MessageBox.Show(
                    $"האם אתה בטוח שברצונך למחוק את העובד '{empName}'?\nפעולה זו תמחק גם את ימי המחלה ונתוני ההתאמה שלו.",
                    "אישור מחיקה",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                //כן
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        // פונקציית המחיקה
                        dbHelper.DeleteEmployee(selectedEmpId);

                        // רענון טבלת העובדים במסך
                        LoadEmployeesList();
                        

                        // רענון מטריצת ההתאמה 
                        LoadSynergyToGrid();

                        MessageBox.Show("העובד נמחק בהצלחה", "נמחק", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("שגיאה במחיקת העובד: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("נא לבחור עובד מהטבלה כדי למחוק אותו", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Backtracking כפתור תהליך רקע
        private async void btnRunBacktracking_Click(object sender, EventArgs e)
        {
            btnRunBacktracking.Enabled = false;
            btnRunAlgorithm.Enabled = false;
            lblStatusBacktracking.Text = "מריץ אלגוריתם נאיבי... נא להמתין זה ייקח כמה שניות";
            lblStatusBacktracking.ForeColor = System.Drawing.Color.Orange;
            dgvBacktrackingSchedule.DataSource = null; // ניקוי הטבלה

            try
            {
                // טעינת הנתונים    
                Data_Layer currentData = new Data_Layer();
                if (currentData.Employees.Count == 0) throw new Exception("אין עובדים במערכת");

                //  יצירת הפותר 
                BacktrackingSolver solver = new BacktrackingSolver(currentData);

                //  הרצה ברקע
                bool success = await Task.Run(() => solver.Solve());

                
                if (success)
                {
                    lblStatusBacktracking.Text = $"הצליח (מצב נדיר) איטרציות: {solver.IterationsCount:N0}";
                    lblStatusBacktracking.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblStatusBacktracking.Text = $"האלגוריתם נתקע ונעצר הגיע עד יום: {solver.MaxDayReached + 1} | איטרציות: {solver.IterationsCount:N0}";
                    lblStatusBacktracking.ForeColor = System.Drawing.Color.Red;
                }

                // המרה לטבלה
                int[,,] bestMatrix = solver.GetBestPartialSchedule();
                DisplayBacktrackingMatrix(bestMatrix, currentData.Employees);
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה: " + ex.Message);
            }
            finally
            {
                btnRunBacktracking.Enabled = true;
                btnRunAlgorithm.Enabled = true;
            }
        }

        // פוקציה להצגת הטבלה של הפתרון Backtracking
        private void DisplayBacktrackingMatrix(int[,,] matrix, List<Employee> employees)
        {
            List<ShiftDisplayModel> listForGrid = new List<ShiftDisplayModel>();
            string[] shiftNames = { "Morning", "Evening", "Night" };//תרגום אינדקס

            for (int day = 0; day < 30; day++)
            {
                for (int shift = 0; shift < 3; shift++)
                {
                    ShiftDisplayModel row = new ShiftDisplayModel();

                    
                    row.Day = (day + 1).ToString();
                    row.Shift = shiftNames[shift];

                    
                    //??-אם הid נמצא במערך אבל העובד לא קיים ברשימה
                    // MGR
                    int mgrId = matrix[day, shift, 0];
                    row.ManagerID = mgrId != -1 ? employees.FirstOrDefault(e => e.ID == mgrId)?.Name ?? "ריק" : "ריק";

                    // MED
                    int medId = matrix[day, shift, 1];
                    row.DoctorID = medId != -1 ? employees.FirstOrDefault(e => e.ID == medId)?.Name ?? "ריק" : "ריק";

                    // DRV
                    int drvId = matrix[day, shift, 2];
                    row.DriverID = drvId != -1 ? employees.FirstOrDefault(e => e.ID == drvId)?.Name ?? "ריק" : "ריק";

                    listForGrid.Add(row);
                }
            }

            dgvBacktrackingSchedule.DataSource = listForGrid;

           
            if (dgvBacktrackingSchedule.Columns.Count > 0)
            {
                if (dgvBacktrackingSchedule.Columns.Contains("Day")) dgvBacktrackingSchedule.Columns["Day"].HeaderText = "יום";
                if (dgvBacktrackingSchedule.Columns.Contains("Shift")) dgvBacktrackingSchedule.Columns["Shift"].HeaderText = "משמרת";
                if (dgvBacktrackingSchedule.Columns.Contains("ManagerID")) dgvBacktrackingSchedule.Columns["ManagerID"].HeaderText = "מנהל";
                if (dgvBacktrackingSchedule.Columns.Contains("DoctorID")) dgvBacktrackingSchedule.Columns["DoctorID"].HeaderText = "רופא";
                if (dgvBacktrackingSchedule.Columns.Contains("DriverID")) dgvBacktrackingSchedule.Columns["DriverID"].HeaderText = "נהג";

                //פונקציית העיצוב
                StyleGrid(dgvBacktrackingSchedule);
            }
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }
    }
}
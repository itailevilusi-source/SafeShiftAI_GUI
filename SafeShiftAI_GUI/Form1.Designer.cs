namespace SafeShiftAI_GUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblFitness = new System.Windows.Forms.Label();
            this.chartFitness = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pbStatus = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvSchedule = new System.Windows.Forms.DataGridView();
            this.btnRunAlgorithm = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnDeleteEmployee = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSaveSickDays = new System.Windows.Forms.Button();
            this.clbSickDays = new System.Windows.Forms.CheckedListBox();
            this.cmbSickEmployee = new System.Windows.Forms.ComboBox();
            this.txtRealID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSeniority = new System.Windows.Forms.NumericUpDown();
            this.dgvEmployees = new System.Windows.Forms.DataGridView();
            this.btnAddEmployee = new System.Windows.Forms.Button();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnSaveSynergy = new System.Windows.Forms.Button();
            this.dgvSynergy = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dgvBacktrackingSchedule = new System.Windows.Forms.DataGridView();
            this.lblStatusBacktracking = new System.Windows.Forms.Label();
            this.btnRunBacktracking = new System.Windows.Forms.Button();
            this.tabPageStatistics = new System.Windows.Forms.TabPage();
            this.numRuns = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.btnRunBenchmark = new System.Windows.Forms.Button();
            this.dgvAblationResults = new System.Windows.Forms.DataGridView();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartFitness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedule)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSeniority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmployees)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSynergy)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBacktrackingSchedule)).BeginInit();
            this.tabPageStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRuns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAblationResults)).BeginInit();
            this.SuspendLayout();
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblStatus);
            this.tabPage2.Controls.Add(this.lblFitness);
            this.tabPage2.Controls.Add(this.chartFitness);
            this.tabPage2.Controls.Add(this.pbStatus);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.dgvSchedule);
            this.tabPage2.Controls.Add(this.btnRunAlgorithm);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(1130, 715);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "תוצאות ושיבוץ";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(566, 23);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.Click += new System.EventHandler(this.lblStatus_Click);
            // 
            // lblFitness
            // 
            this.lblFitness.AutoSize = true;
            this.lblFitness.Location = new System.Drawing.Point(903, 32);
            this.lblFitness.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFitness.Name = "lblFitness";
            this.lblFitness.Size = new System.Drawing.Size(36, 13);
            this.lblFitness.TabIndex = 10;
            this.lblFitness.Text = "ניקוד";
            // 
            // chartFitness
            // 
            chartArea3.Name = "ChartArea1";
            this.chartFitness.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartFitness.Legends.Add(legend3);
            this.chartFitness.Location = new System.Drawing.Point(733, 84);
            this.chartFitness.Margin = new System.Windows.Forms.Padding(2);
            this.chartFitness.Name = "chartFitness";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartFitness.Series.Add(series3);
            this.chartFitness.Size = new System.Drawing.Size(310, 420);
            this.chartFitness.TabIndex = 9;
            this.chartFitness.Text = "chart1";
            // 
            // pbStatus
            // 
            this.pbStatus.Location = new System.Drawing.Point(281, 38);
            this.pbStatus.Margin = new System.Windows.Forms.Padding(2);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(442, 41);
            this.pbStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbStatus.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-5, -38);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "label1";
            // 
            // dgvSchedule
            // 
            this.dgvSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSchedule.Location = new System.Drawing.Point(4, 84);
            this.dgvSchedule.Margin = new System.Windows.Forms.Padding(2);
            this.dgvSchedule.Name = "dgvSchedule";
            this.dgvSchedule.ReadOnly = true;
            this.dgvSchedule.RowHeadersWidth = 51;
            this.dgvSchedule.RowTemplate.Height = 24;
            this.dgvSchedule.Size = new System.Drawing.Size(720, 408);
            this.dgvSchedule.TabIndex = 6;
            // 
            // btnRunAlgorithm
            // 
            this.btnRunAlgorithm.Location = new System.Drawing.Point(27, 10);
            this.btnRunAlgorithm.Margin = new System.Windows.Forms.Padding(2);
            this.btnRunAlgorithm.Name = "btnRunAlgorithm";
            this.btnRunAlgorithm.Size = new System.Drawing.Size(234, 58);
            this.btnRunAlgorithm.TabIndex = 5;
            this.btnRunAlgorithm.Text = "הפעל אופטימיזציה";
            this.btnRunAlgorithm.UseVisualStyleBackColor = true;
            this.btnRunAlgorithm.Click += new System.EventHandler(this.btnRunAlgorithm_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnDeleteEmployee);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.txtRealID);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtSeniority);
            this.tabPage1.Controls.Add(this.dgvEmployees);
            this.tabPage1.Controls.Add(this.btnAddEmployee);
            this.tabPage1.Controls.Add(this.cmbRole);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.txtName);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(1130, 715);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "ניהול עובדים";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // btnDeleteEmployee
            // 
            this.btnDeleteEmployee.BackColor = System.Drawing.Color.IndianRed;
            this.btnDeleteEmployee.Location = new System.Drawing.Point(82, 463);
            this.btnDeleteEmployee.Margin = new System.Windows.Forms.Padding(2);
            this.btnDeleteEmployee.Name = "btnDeleteEmployee";
            this.btnDeleteEmployee.Size = new System.Drawing.Size(226, 71);
            this.btnDeleteEmployee.TabIndex = 11;
            this.btnDeleteEmployee.Text = "מחק עובד נבחר";
            this.btnDeleteEmployee.UseVisualStyleBackColor = false;
            this.btnDeleteEmployee.Click += new System.EventHandler(this.btnDeleteEmployee_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSaveSickDays);
            this.groupBox1.Controls.Add(this.clbSickDays);
            this.groupBox1.Controls.Add(this.cmbSickEmployee);
            this.groupBox1.Location = new System.Drawing.Point(357, 25);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(231, 444);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ניהול ימי מחלה";
            // 
            // btnSaveSickDays
            // 
            this.btnSaveSickDays.Location = new System.Drawing.Point(46, 304);
            this.btnSaveSickDays.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveSickDays.Name = "btnSaveSickDays";
            this.btnSaveSickDays.Size = new System.Drawing.Size(136, 60);
            this.btnSaveSickDays.TabIndex = 0;
            this.btnSaveSickDays.Text = "שמור ימי מחלה";
            this.btnSaveSickDays.UseVisualStyleBackColor = true;
            this.btnSaveSickDays.Click += new System.EventHandler(this.btnSaveSickDays_Click);
            // 
            // clbSickDays
            // 
            this.clbSickDays.FormattingEnabled = true;
            this.clbSickDays.Location = new System.Drawing.Point(28, 41);
            this.clbSickDays.Margin = new System.Windows.Forms.Padding(2);
            this.clbSickDays.Name = "clbSickDays";
            this.clbSickDays.Size = new System.Drawing.Size(163, 229);
            this.clbSickDays.TabIndex = 1;
            // 
            // cmbSickEmployee
            // 
            this.cmbSickEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSickEmployee.FormattingEnabled = true;
            this.cmbSickEmployee.Location = new System.Drawing.Point(28, 17);
            this.cmbSickEmployee.Margin = new System.Windows.Forms.Padding(2);
            this.cmbSickEmployee.Name = "cmbSickEmployee";
            this.cmbSickEmployee.Size = new System.Drawing.Size(188, 21);
            this.cmbSickEmployee.TabIndex = 0;
            this.cmbSickEmployee.SelectedIndexChanged += new System.EventHandler(this.cmbSickEmployee_SelectedIndexChanged);
            // 
            // txtRealID
            // 
            this.txtRealID.Location = new System.Drawing.Point(111, 274);
            this.txtRealID.Margin = new System.Windows.Forms.Padding(2);
            this.txtRealID.MaxLength = 9;
            this.txtRealID.Name = "txtRealID";
            this.txtRealID.Size = new System.Drawing.Size(162, 20);
            this.txtRealID.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(289, 274);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "תעודת זהות";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(302, 242);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "ותק";
            // 
            // txtSeniority
            // 
            this.txtSeniority.Location = new System.Drawing.Point(111, 237);
            this.txtSeniority.Margin = new System.Windows.Forms.Padding(2);
            this.txtSeniority.Name = "txtSeniority";
            this.txtSeniority.Size = new System.Drawing.Size(161, 20);
            this.txtSeniority.TabIndex = 6;
            // 
            // dgvEmployees
            // 
            this.dgvEmployees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEmployees.Location = new System.Drawing.Point(589, 42);
            this.dgvEmployees.Margin = new System.Windows.Forms.Padding(2);
            this.dgvEmployees.Name = "dgvEmployees";
            this.dgvEmployees.ReadOnly = true;
            this.dgvEmployees.RowHeadersWidth = 51;
            this.dgvEmployees.RowTemplate.Height = 24;
            this.dgvEmployees.Size = new System.Drawing.Size(441, 566);
            this.dgvEmployees.TabIndex = 5;
            // 
            // btnAddEmployee
            // 
            this.btnAddEmployee.Location = new System.Drawing.Point(82, 339);
            this.btnAddEmployee.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddEmployee.Name = "btnAddEmployee";
            this.btnAddEmployee.Size = new System.Drawing.Size(226, 67);
            this.btnAddEmployee.TabIndex = 4;
            this.btnAddEmployee.Text = "הוסף עובד";
            this.btnAddEmployee.UseVisualStyleBackColor = true;
            this.btnAddEmployee.Click += new System.EventHandler(this.btnAddEmployee_Click);
            // 
            // cmbRole
            // 
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Items.AddRange(new object[] {
            "Manager",
            "Doctor",
            "Driver"});
            this.cmbRole.Location = new System.Drawing.Point(111, 189);
            this.cmbRole.Margin = new System.Windows.Forms.Padding(2);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(162, 21);
            this.cmbRole.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(302, 192);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "תפקיד";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(111, 152);
            this.txtName.Margin = new System.Windows.Forms.Padding(2);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(162, 20);
            this.txtName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(286, 152);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "שם העובד";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPageStatistics);
            this.tabControl1.Location = new System.Drawing.Point(0, 10);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1138, 741);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnSaveSynergy);
            this.tabPage3.Controls.Add(this.dgvSynergy);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage3.Size = new System.Drawing.Size(1130, 715);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "מטריצת התאמה";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnSaveSynergy
            // 
            this.btnSaveSynergy.Location = new System.Drawing.Point(298, 592);
            this.btnSaveSynergy.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveSynergy.Name = "btnSaveSynergy";
            this.btnSaveSynergy.Size = new System.Drawing.Size(496, 41);
            this.btnSaveSynergy.TabIndex = 2;
            this.btnSaveSynergy.Text = "שמור נתוני התאמה";
            this.btnSaveSynergy.UseVisualStyleBackColor = true;
            this.btnSaveSynergy.Click += new System.EventHandler(this.btnSaveSynergy_Click);
            // 
            // dgvSynergy
            // 
            this.dgvSynergy.AllowUserToAddRows = false;
            this.dgvSynergy.AllowUserToDeleteRows = false;
            this.dgvSynergy.AllowUserToResizeColumns = false;
            this.dgvSynergy.AllowUserToResizeRows = false;
            this.dgvSynergy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSynergy.Location = new System.Drawing.Point(4, 5);
            this.dgvSynergy.Margin = new System.Windows.Forms.Padding(2);
            this.dgvSynergy.Name = "dgvSynergy";
            this.dgvSynergy.RowHeadersWidth = 51;
            this.dgvSynergy.RowTemplate.Height = 24;
            this.dgvSynergy.Size = new System.Drawing.Size(1065, 583);
            this.dgvSynergy.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dgvBacktrackingSchedule);
            this.tabPage4.Controls.Add(this.lblStatusBacktracking);
            this.tabPage4.Controls.Add(this.btnRunBacktracking);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1130, 715);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "השוואת Backtracking";
            this.tabPage4.UseVisualStyleBackColor = true;
            this.tabPage4.Click += new System.EventHandler(this.tabPage4_Click);
            // 
            // dgvBacktrackingSchedule
            // 
            this.dgvBacktrackingSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBacktrackingSchedule.Location = new System.Drawing.Point(544, 20);
            this.dgvBacktrackingSchedule.Margin = new System.Windows.Forms.Padding(2);
            this.dgvBacktrackingSchedule.Name = "dgvBacktrackingSchedule";
            this.dgvBacktrackingSchedule.ReadOnly = true;
            this.dgvBacktrackingSchedule.RowHeadersWidth = 51;
            this.dgvBacktrackingSchedule.RowTemplate.Height = 24;
            this.dgvBacktrackingSchedule.Size = new System.Drawing.Size(519, 479);
            this.dgvBacktrackingSchedule.TabIndex = 2;
            // 
            // lblStatusBacktracking
            // 
            this.lblStatusBacktracking.AutoSize = true;
            this.lblStatusBacktracking.Location = new System.Drawing.Point(239, 95);
            this.lblStatusBacktracking.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatusBacktracking.Name = "lblStatusBacktracking";
            this.lblStatusBacktracking.Size = new System.Drawing.Size(68, 13);
            this.lblStatusBacktracking.TabIndex = 1;
            this.lblStatusBacktracking.Text = "מוכן להרצה";
            // 
            // btnRunBacktracking
            // 
            this.btnRunBacktracking.Location = new System.Drawing.Point(146, 171);
            this.btnRunBacktracking.Margin = new System.Windows.Forms.Padding(2);
            this.btnRunBacktracking.Name = "btnRunBacktracking";
            this.btnRunBacktracking.Size = new System.Drawing.Size(273, 115);
            this.btnRunBacktracking.TabIndex = 0;
            this.btnRunBacktracking.Text = "החל הרצה";
            this.btnRunBacktracking.UseVisualStyleBackColor = true;
            this.btnRunBacktracking.Click += new System.EventHandler(this.btnRunBacktracking_Click);
            // 
            // tabPageStatistics
            // 
            this.tabPageStatistics.Controls.Add(this.dgvAblationResults);
            this.tabPageStatistics.Controls.Add(this.btnRunBenchmark);
            this.tabPageStatistics.Controls.Add(this.label6);
            this.tabPageStatistics.Controls.Add(this.numRuns);
            this.tabPageStatistics.Location = new System.Drawing.Point(4, 22);
            this.tabPageStatistics.Name = "tabPageStatistics";
            this.tabPageStatistics.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStatistics.Size = new System.Drawing.Size(1130, 715);
            this.tabPageStatistics.TabIndex = 4;
            this.tabPageStatistics.Text = "סטטיסטיקה ומחקר";
            this.tabPageStatistics.UseVisualStyleBackColor = true;
            this.tabPageStatistics.Click += new System.EventHandler(this.tabPageStatistics_Click);
            // 
            // numRuns
            // 
            this.numRuns.Location = new System.Drawing.Point(38, 40);
            this.numRuns.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numRuns.Name = "numRuns";
            this.numRuns.Size = new System.Drawing.Size(189, 20);
            this.numRuns.TabIndex = 0;
            this.numRuns.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(297, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "מספר הרצות לכל מודל";
            // 
            // btnRunBenchmark
            // 
            this.btnRunBenchmark.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnRunBenchmark.Location = new System.Drawing.Point(606, 40);
            this.btnRunBenchmark.Name = "btnRunBenchmark";
            this.btnRunBenchmark.Size = new System.Drawing.Size(286, 72);
            this.btnRunBenchmark.TabIndex = 2;
            this.btnRunBenchmark.Text = "הרץ בנצ\'מרק ומחקר";
            this.btnRunBenchmark.UseVisualStyleBackColor = false;
            this.btnRunBenchmark.Click += new System.EventHandler(this.btnRunBenchmark_Click);
            // 
            // dgvAblationResults
            // 
            this.dgvAblationResults.AllowUserToAddRows = false;
            this.dgvAblationResults.AllowUserToDeleteRows = false;
            this.dgvAblationResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAblationResults.Location = new System.Drawing.Point(38, 140);
            this.dgvAblationResults.Name = "dgvAblationResults";
            this.dgvAblationResults.ReadOnly = true;
            this.dgvAblationResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAblationResults.Size = new System.Drawing.Size(1046, 553);
            this.dgvAblationResults.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1216, 737);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartFitness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedule)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSeniority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmployees)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSynergy)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBacktrackingSchedule)).EndInit();
            this.tabPageStatistics.ResumeLayout(false);
            this.tabPageStatistics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRuns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAblationResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartFitness;
        private System.Windows.Forms.ProgressBar pbStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvSchedule;
        private System.Windows.Forms.Button btnRunAlgorithm;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnAddEmployee;
        private System.Windows.Forms.Label lblFitness;
        private System.Windows.Forms.DataGridView dgvEmployees;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown txtSeniority;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgvSynergy;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtRealID;
        private System.Windows.Forms.CheckedListBox clbSickDays;
        private System.Windows.Forms.ComboBox cmbSickEmployee;
        private System.Windows.Forms.Button btnSaveSickDays;
        private System.Windows.Forms.Button btnSaveSynergy;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnDeleteEmployee;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label lblStatusBacktracking;
        private System.Windows.Forms.Button btnRunBacktracking;
        private System.Windows.Forms.DataGridView dgvBacktrackingSchedule;
        private System.Windows.Forms.TabPage tabPageStatistics;
        private System.Windows.Forms.NumericUpDown numRuns;
        private System.Windows.Forms.DataGridView dgvAblationResults;
        private System.Windows.Forms.Button btnRunBenchmark;
        private System.Windows.Forms.Label label6;
    }
}


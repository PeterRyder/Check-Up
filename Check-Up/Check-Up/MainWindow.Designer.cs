namespace Check_Up {
    partial class MainWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutCheckUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button_gatherData = new System.Windows.Forms.Button();
            this.button_analyzeProcesses = new System.Windows.Forms.Button();
            this.button_resetChart = new System.Windows.Forms.Button();
            this.button_monitorStop = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label_percentage = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chartControls = new System.Windows.Forms.GroupBox();
            this.groupBox_warnings = new System.Windows.Forms.GroupBox();
            this.listView_warnings = new System.Windows.Forms.ListView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.chartControls.SuspendLayout();
            this.groupBox_warnings.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1089, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutCheckUpToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutCheckUpToolStripMenuItem
            // 
            this.aboutCheckUpToolStripMenuItem.Name = "aboutCheckUpToolStripMenuItem";
            this.aboutCheckUpToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.aboutCheckUpToolStripMenuItem.Text = "About Check Up";
            this.aboutCheckUpToolStripMenuItem.Click += new System.EventHandler(this.aboutCheckUpToolStripMenuItem_Click);
            // 
            // button_gatherData
            // 
            this.button_gatherData.AutoSize = true;
            this.button_gatherData.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_gatherData.BackColor = System.Drawing.SystemColors.Control;
            this.button_gatherData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_gatherData.Location = new System.Drawing.Point(6, 19);
            this.button_gatherData.Name = "button_gatherData";
            this.button_gatherData.Size = new System.Drawing.Size(75, 23);
            this.button_gatherData.TabIndex = 1;
            this.button_gatherData.Text = "Gather Data";
            this.button_gatherData.UseVisualStyleBackColor = false;
            this.button_gatherData.Click += new System.EventHandler(this.button_gatherData_Click);
            // 
            // button_analyzeProcesses
            // 
            this.button_analyzeProcesses.AutoSize = true;
            this.button_analyzeProcesses.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_analyzeProcesses.Location = new System.Drawing.Point(6, 106);
            this.button_analyzeProcesses.Name = "button_analyzeProcesses";
            this.button_analyzeProcesses.Size = new System.Drawing.Size(106, 23);
            this.button_analyzeProcesses.TabIndex = 3;
            this.button_analyzeProcesses.Text = "Analyze Processes";
            this.button_analyzeProcesses.UseVisualStyleBackColor = true;
            this.button_analyzeProcesses.Click += new System.EventHandler(this.analyzeProcesses_Click);
            // 
            // button_resetChart
            // 
            this.button_resetChart.AutoSize = true;
            this.button_resetChart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_resetChart.Location = new System.Drawing.Point(9, 77);
            this.button_resetChart.Name = "button_resetChart";
            this.button_resetChart.Size = new System.Drawing.Size(73, 23);
            this.button_resetChart.TabIndex = 8;
            this.button_resetChart.Text = "Reset Chart";
            this.button_resetChart.UseVisualStyleBackColor = true;
            this.button_resetChart.Click += new System.EventHandler(this.button_resetChart_Click);
            // 
            // button_monitorStop
            // 
            this.button_monitorStop.AutoSize = true;
            this.button_monitorStop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_monitorStop.BackColor = System.Drawing.SystemColors.Control;
            this.button_monitorStop.Cursor = System.Windows.Forms.Cursors.Default;
            this.button_monitorStop.Enabled = false;
            this.button_monitorStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_monitorStop.Location = new System.Drawing.Point(6, 48);
            this.button_monitorStop.Name = "button_monitorStop";
            this.button_monitorStop.Size = new System.Drawing.Size(91, 23);
            this.button_monitorStop.TabIndex = 7;
            this.button_monitorStop.Text = "Stop Monitoring";
            this.button_monitorStop.UseVisualStyleBackColor = false;
            this.button_monitorStop.Click += new System.EventHandler(this.monitorStop_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Location = new System.Drawing.Point(116, 103);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(0, 0);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted_1);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(3, 31);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(887, 23);
            this.progressBar1.TabIndex = 8;
            // 
            // label_percentage
            // 
            this.label_percentage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_percentage.AutoSize = true;
            this.label_percentage.Location = new System.Drawing.Point(896, 36);
            this.label_percentage.Name = "label_percentage";
            this.label_percentage.Size = new System.Drawing.Size(21, 13);
            this.label_percentage.TabIndex = 9;
            this.label_percentage.Text = "0%";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.splitContainer1);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 24);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(142, 653);
            this.flowLayoutPanel2.TabIndex = 11;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chartControls);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox_warnings);
            this.splitContainer1.Size = new System.Drawing.Size(139, 647);
            this.splitContainer1.SplitterDistance = 323;
            this.splitContainer1.TabIndex = 11;
            // 
            // chartControls
            // 
            this.chartControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.chartControls.Controls.Add(this.button_analyzeProcesses);
            this.chartControls.Controls.Add(this.button_resetChart);
            this.chartControls.Controls.Add(this.button_monitorStop);
            this.chartControls.Controls.Add(this.button_gatherData);
            this.chartControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControls.Location = new System.Drawing.Point(0, 0);
            this.chartControls.Name = "chartControls";
            this.chartControls.Size = new System.Drawing.Size(139, 323);
            this.chartControls.TabIndex = 9;
            this.chartControls.TabStop = false;
            this.chartControls.Text = "Chart Controls";
            // 
            // groupBox_warnings
            // 
            this.groupBox_warnings.Controls.Add(this.listView_warnings);
            this.groupBox_warnings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_warnings.Location = new System.Drawing.Point(0, 0);
            this.groupBox_warnings.Name = "groupBox_warnings";
            this.groupBox_warnings.Size = new System.Drawing.Size(139, 320);
            this.groupBox_warnings.TabIndex = 10;
            this.groupBox_warnings.TabStop = false;
            this.groupBox_warnings.Text = "Warnings";
            // 
            // listView_warnings
            // 
            this.listView_warnings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_warnings.FullRowSelect = true;
            this.listView_warnings.GridLines = true;
            this.listView_warnings.Location = new System.Drawing.Point(3, 16);
            this.listView_warnings.MultiSelect = false;
            this.listView_warnings.Name = "listView_warnings";
            this.listView_warnings.Size = new System.Drawing.Size(133, 301);
            this.listView_warnings.TabIndex = 0;
            this.listView_warnings.UseCompatibleStateImageBehavior = false;
            this.listView_warnings.View = System.Windows.Forms.View.Details;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.chart, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(142, 24);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.02151F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.97849F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(947, 653);
            this.tableLayoutPanel2.TabIndex = 13;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 95F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel3.Controls.Add(this.progressBar1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label_percentage, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 564);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(941, 86);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // chart
            // 
            this.chart.BorderSkin.BackColor = System.Drawing.Color.Black;
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(3, 3);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(941, 555);
            this.chart.TabIndex = 1;
            this.chart.Text = "chart";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1089, 677);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(700, 311);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Check Up";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.chartControls.ResumeLayout(false);
            this.chartControls.PerformLayout();
            this.groupBox_warnings.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutCheckUpToolStripMenuItem;
        private System.Windows.Forms.Button button_gatherData;
        private System.Windows.Forms.Button button_analyzeProcesses;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button_monitorStop;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label_percentage;
        private System.Windows.Forms.Button button_resetChart;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox chartControls;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.GroupBox groupBox_warnings;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listView_warnings;



    }
}


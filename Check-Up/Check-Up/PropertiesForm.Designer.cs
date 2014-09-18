namespace Check_Up {
    partial class PropertiesForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertiesForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox_cpu = new System.Windows.Forms.CheckBox();
            this.checkBox_memory = new System.Windows.Forms.CheckBox();
            this.checkBox_network = new System.Windows.Forms.CheckBox();
            this.checkBox_diskio = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_ignoreTime = new System.Windows.Forms.CheckBox();
            this.visiblePoints = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 133);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button2.Location = new System.Drawing.Point(84, 133);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox_cpu
            // 
            this.checkBox_cpu.AutoSize = true;
            this.checkBox_cpu.Location = new System.Drawing.Point(6, 19);
            this.checkBox_cpu.Name = "checkBox_cpu";
            this.checkBox_cpu.Size = new System.Drawing.Size(48, 17);
            this.checkBox_cpu.TabIndex = 7;
            this.checkBox_cpu.Text = "CPU";
            this.checkBox_cpu.UseVisualStyleBackColor = true;
            // 
            // checkBox_memory
            // 
            this.checkBox_memory.AutoSize = true;
            this.checkBox_memory.Location = new System.Drawing.Point(6, 42);
            this.checkBox_memory.Name = "checkBox_memory";
            this.checkBox_memory.Size = new System.Drawing.Size(63, 17);
            this.checkBox_memory.TabIndex = 8;
            this.checkBox_memory.Text = "Memory";
            this.checkBox_memory.UseVisualStyleBackColor = true;
            // 
            // checkBox_network
            // 
            this.checkBox_network.AutoSize = true;
            this.checkBox_network.Location = new System.Drawing.Point(6, 65);
            this.checkBox_network.Name = "checkBox_network";
            this.checkBox_network.Size = new System.Drawing.Size(66, 17);
            this.checkBox_network.TabIndex = 9;
            this.checkBox_network.Text = "Network";
            this.checkBox_network.UseVisualStyleBackColor = true;
            // 
            // checkBox_diskio
            // 
            this.checkBox_diskio.AutoSize = true;
            this.checkBox_diskio.Location = new System.Drawing.Point(6, 88);
            this.checkBox_diskio.Name = "checkBox_diskio";
            this.checkBox_diskio.Size = new System.Drawing.Size(61, 17);
            this.checkBox_diskio.TabIndex = 10;
            this.checkBox_diskio.Text = "Disk IO";
            this.checkBox_diskio.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(137, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Data Polling Time (sec)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Data Polling Interval (sec)";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(137, 29);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.checkBox_cpu);
            this.groupBox1.Controls.Add(this.checkBox_memory);
            this.groupBox1.Controls.Add(this.checkBox_network);
            this.groupBox1.Controls.Add(this.checkBox_diskio);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(78, 124);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(87, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(240, 124);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel1);
            this.flowLayoutPanel1.Controls.Add(this.groupBox2);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(483, 215);
            this.flowLayoutPanel1.TabIndex = 17;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.visiblePoints);
            this.groupBox2.Controls.Add(this.checkBox_ignoreTime);
            this.groupBox2.Location = new System.Drawing.Point(333, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(129, 124);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Global Options";
            // 
            // checkBox_ignoreTime
            // 
            this.checkBox_ignoreTime.AutoSize = true;
            this.checkBox_ignoreTime.Location = new System.Drawing.Point(10, 19);
            this.checkBox_ignoreTime.Name = "checkBox_ignoreTime";
            this.checkBox_ignoreTime.Size = new System.Drawing.Size(82, 17);
            this.checkBox_ignoreTime.TabIndex = 1;
            this.checkBox_ignoreTime.Text = "Ignore Time";
            this.checkBox_ignoreTime.UseVisualStyleBackColor = true;
            // 
            // visiblePoints
            // 
            this.visiblePoints.Location = new System.Drawing.Point(7, 98);
            this.visiblePoints.Name = "visiblePoints";
            this.visiblePoints.Size = new System.Drawing.Size(100, 20);
            this.visiblePoints.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Amount of Visible Points";
            // 
            // PropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 215);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PropertiesForm";
            this.Text = "Properties";
            this.Load += new System.EventHandler(this.properties_form_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox_cpu;
        private System.Windows.Forms.CheckBox checkBox_memory;
        private System.Windows.Forms.CheckBox checkBox_network;
        private System.Windows.Forms.CheckBox checkBox_diskio;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_ignoreTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox visiblePoints;
    }
}
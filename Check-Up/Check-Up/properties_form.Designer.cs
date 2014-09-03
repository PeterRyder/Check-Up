namespace Check_Up {
    partial class properties_form {
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
            this.label1 = new System.Windows.Forms.Label();
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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data to Gather";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(251, 295);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(348, 295);
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
            this.checkBox_cpu.Location = new System.Drawing.Point(38, 73);
            this.checkBox_cpu.Name = "checkBox_cpu";
            this.checkBox_cpu.Size = new System.Drawing.Size(48, 17);
            this.checkBox_cpu.TabIndex = 7;
            this.checkBox_cpu.Text = "CPU";
            this.checkBox_cpu.UseVisualStyleBackColor = true;
            // 
            // checkBox_memory
            // 
            this.checkBox_memory.AutoSize = true;
            this.checkBox_memory.Location = new System.Drawing.Point(38, 97);
            this.checkBox_memory.Name = "checkBox_memory";
            this.checkBox_memory.Size = new System.Drawing.Size(63, 17);
            this.checkBox_memory.TabIndex = 8;
            this.checkBox_memory.Text = "Memory";
            this.checkBox_memory.UseVisualStyleBackColor = true;
            // 
            // checkBox_network
            // 
            this.checkBox_network.AutoSize = true;
            this.checkBox_network.Location = new System.Drawing.Point(38, 121);
            this.checkBox_network.Name = "checkBox_network";
            this.checkBox_network.Size = new System.Drawing.Size(66, 17);
            this.checkBox_network.TabIndex = 9;
            this.checkBox_network.Text = "Network";
            this.checkBox_network.UseVisualStyleBackColor = true;
            // 
            // checkBox_diskio
            // 
            this.checkBox_diskio.AutoSize = true;
            this.checkBox_diskio.Location = new System.Drawing.Point(38, 145);
            this.checkBox_diskio.Name = "checkBox_diskio";
            this.checkBox_diskio.Size = new System.Drawing.Size(61, 17);
            this.checkBox_diskio.TabIndex = 10;
            this.checkBox_diskio.Text = "Disk IO";
            this.checkBox_diskio.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(294, 70);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(172, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Data Polling Time (sec)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(160, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Data Polling Interval (sec)";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(294, 97);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 14;
            // 
            // properties_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 343);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkBox_diskio);
            this.Controls.Add(this.checkBox_network);
            this.Controls.Add(this.checkBox_memory);
            this.Controls.Add(this.checkBox_cpu);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "properties_form";
            this.Text = "properties_form";
            this.Load += new System.EventHandler(this.properties_form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
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
    }
}
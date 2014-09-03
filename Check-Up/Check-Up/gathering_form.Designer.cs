namespace Check_Up {
    partial class gathering_form {
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label_cpu = new System.Windows.Forms.Label();
            this.label_memory = new System.Windows.Forms.Label();
            this.label_network = new System.Windows.Forms.Label();
            this.label_diskio = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(139, 123);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(332, 23);
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(171, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(259, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Gathering Data - Please wait";
            // 
            // label_cpu
            // 
            this.label_cpu.AutoSize = true;
            this.label_cpu.Location = new System.Drawing.Point(12, 9);
            this.label_cpu.Name = "label_cpu";
            this.label_cpu.Size = new System.Drawing.Size(29, 13);
            this.label_cpu.TabIndex = 3;
            this.label_cpu.Text = "CPU";
            this.label_cpu.Click += new System.EventHandler(this.label1_Click);
            // 
            // label_memory
            // 
            this.label_memory.AutoSize = true;
            this.label_memory.Location = new System.Drawing.Point(10, 31);
            this.label_memory.Name = "label_memory";
            this.label_memory.Size = new System.Drawing.Size(44, 13);
            this.label_memory.TabIndex = 4;
            this.label_memory.Text = "Memory";
            // 
            // label_network
            // 
            this.label_network.AutoSize = true;
            this.label_network.Location = new System.Drawing.Point(12, 59);
            this.label_network.Name = "label_network";
            this.label_network.Size = new System.Drawing.Size(47, 13);
            this.label_network.TabIndex = 5;
            this.label_network.Text = "Network";
            // 
            // label_diskio
            // 
            this.label_diskio.AutoSize = true;
            this.label_diskio.Location = new System.Drawing.Point(12, 89);
            this.label_diskio.Name = "label_diskio";
            this.label_diskio.Size = new System.Drawing.Size(42, 13);
            this.label_diskio.TabIndex = 6;
            this.label_diskio.Text = "Disk IO";
            // 
            // gathering_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 262);
            this.Controls.Add(this.label_diskio);
            this.Controls.Add(this.label_network);
            this.Controls.Add(this.label_memory);
            this.Controls.Add(this.label_cpu);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBar1);
            this.Name = "gathering_form";
            this.Text = "gathering_form";
            this.Load += new System.EventHandler(this.gathering_form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_cpu;
        private System.Windows.Forms.Label label_memory;
        private System.Windows.Forms.Label label_network;
        private System.Windows.Forms.Label label_diskio;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}
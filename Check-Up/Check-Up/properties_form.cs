using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Check_Up {
    public partial class properties_form : Form {
        public properties_form() {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e) {

        }


        private void properties_form_Load(object sender, EventArgs e) {
            checkBox_cpu.Checked = Properties.Settings.Default.CPU;
            checkBox_memory.Checked = Properties.Settings.Default.Memory;
            checkBox_network.Checked = Properties.Settings.Default.Network;
            checkBox_diskio.Checked = Properties.Settings.Default.DiskIO;

            textBox1.Text = "" + Properties.Settings.Default.PollingTime;
            textBox2.Text = "" + Properties.Settings.Default.PollingInterval;

        }

        private void button1_Click(object sender, EventArgs e) {

            Properties.Settings.Default.CPU = checkBox_cpu.Checked;
            Properties.Settings.Default.Memory = checkBox_memory.Checked;
            Properties.Settings.Default.Network = checkBox_network.Checked;
            Properties.Settings.Default.DiskIO = checkBox_diskio.Checked;

            try {
                int pollingTime = Convert.ToInt32(textBox1.Text);
                Properties.Settings.Default.PollingTime = pollingTime;
            }
            catch {
                Console.WriteLine("Couldn't convert {0} to int", textBox1.Text);
            }

            try {
                int pollingInterval = Convert.ToInt32(textBox2.Text);
                Properties.Settings.Default.PollingInterval = pollingInterval;
            }
            catch {
                Console.WriteLine("Couldn't convert {0} to int", textBox2.Text);
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }


    }
}

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

namespace ReadWriteCsv {
    public partial class PropertiesForm : Form {
        public PropertiesForm() {
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
                Console.Error.WriteLine("Couldn't convert {0} to int", textBox1.Text);
            }

            try {
                int pollingInterval = Convert.ToInt32(textBox2.Text);
                Properties.Settings.Default.PollingInterval = pollingInterval;
            }
            catch {
                Console.Error.WriteLine("Couldn't convert {0} to int", textBox2.Text);
            }

            #region Properties debug output
#if DEBUG
            Console.WriteLine("Properties window closed using OK - settings should have saved");

            Console.WriteLine("CPU checkbox: " + checkBox_cpu.Checked);
            Console.WriteLine("Memory checkbox: " + checkBox_memory.Checked);
            Console.WriteLine("Network checkbox: " + checkBox_network.Checked);
            Console.WriteLine("DiskIO checkbox: " + checkBox_diskio.Checked);

            Console.WriteLine("CPU property: " + Properties.Settings.Default.CPU);
            Console.WriteLine("Memory property: " + Properties.Settings.Default.Memory);
            Console.WriteLine("Network property: " + Properties.Settings.Default.Network);
            Console.WriteLine("DiskIO property: " + Properties.Settings.Default.DiskIO);
#endif
            #endregion

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            #region Properties debug output
#if DEBUG
            Console.WriteLine("Properties window closed using CANCEL - settings should not have saved");

            Console.WriteLine("CPU checkbox: " + checkBox_cpu.Checked);
            Console.WriteLine("Memory checkbox: " + checkBox_memory.Checked);
            Console.WriteLine("Network checkbox: " + checkBox_network.Checked);
            Console.WriteLine("DiskIO checkbox: " + checkBox_diskio.Checked);


            Console.WriteLine("CPU property: " + Properties.Settings.Default.CPU);
            Console.WriteLine("Memory property: " + Properties.Settings.Default.Memory);
            Console.WriteLine("Network property: " + Properties.Settings.Default.Network);
            Console.WriteLine("DiskIO property: " + Properties.Settings.Default.DiskIO);
#endif
            #endregion
            this.Close();
        }


    }
}

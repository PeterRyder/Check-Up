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
    public partial class PropertiesForm : Form {
        public PropertiesForm() {
            InitializeComponent();
        }

        /// <summary>
        /// When the Properties form loads, load all the default settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void properties_form_Load(object sender, EventArgs e) {
            checkBox_cpu.Checked = Properties.Settings.Default.CPU;
            checkBox_memory.Checked = Properties.Settings.Default.Memory;
            checkBox_network.Checked = Properties.Settings.Default.Network;
            checkBox_diskio.Checked = Properties.Settings.Default.DiskIO;
            checkBox_ignoreTime.Checked = Properties.Settings.Default.IgnoreTime;

            textBox_dataPollingTime.Text = "" + Properties.Settings.Default.PollingTime;
            textBox_dataPollingInterval.Text = "" + Properties.Settings.Default.PollingInterval;
            visiblePoints.Text = "" + Properties.Settings.Default.VisiblePoints;

        }

        /// <summary>
        /// When the user confirms the settings, save them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {

            double pollingTime = Convert.ToDouble(textBox_dataPollingTime.Text);
            double pollingInterval = Convert.ToDouble(textBox_dataPollingInterval.Text);

            if (pollingTime < pollingInterval) {
#if DEBUG
                Console.WriteLine("Polling time greater than polling interval");
#endif
                error1.Visible = true;
                return;
            }


            Properties.Settings.Default.CPU = checkBox_cpu.Checked;
            Properties.Settings.Default.Memory = checkBox_memory.Checked;
            Properties.Settings.Default.Network = checkBox_network.Checked;
            Properties.Settings.Default.DiskIO = checkBox_diskio.Checked;
            Properties.Settings.Default.IgnoreTime = checkBox_ignoreTime.Checked;

            try {
                
                Properties.Settings.Default.PollingTime = pollingTime;
            }
            catch {
                Console.Error.WriteLine("Couldn't convert {0} to double", textBox_dataPollingTime.Text);
            }

            try {
                
                Properties.Settings.Default.PollingInterval = pollingInterval;
            }
            catch {
                Console.Error.WriteLine("Couldn't convert {0} to double", textBox_dataPollingInterval.Text);
            }

            try {
                int VisiblePoints = Convert.ToInt32(visiblePoints.Text);
                Properties.Settings.Default.VisiblePoints = VisiblePoints;
            }
            catch {
                Console.Error.WriteLine("Couldn't convert {0} to int", visiblePoints.Text);
            }

            #region Properties debug output
#if DEBUG
            Console.WriteLine("Properties window closed using OK - settings should have saved");

            Console.WriteLine("CPU checkbox: " + checkBox_cpu.Checked);
            Console.WriteLine("Memory checkbox: " + checkBox_memory.Checked);
            Console.WriteLine("Network checkbox: " + checkBox_network.Checked);
            Console.WriteLine("DiskIO checkbox: " + checkBox_diskio.Checked);
            Console.WriteLine("Ignore Time checkbox: " + checkBox_ignoreTime.Checked);

            Console.WriteLine("CPU property: " + Properties.Settings.Default.CPU);
            Console.WriteLine("Memory property: " + Properties.Settings.Default.Memory);
            Console.WriteLine("Network property: " + Properties.Settings.Default.Network);
            Console.WriteLine("DiskIO property: " + Properties.Settings.Default.DiskIO);
            Console.WriteLine("Real Time property: " + Properties.Settings.Default.RealTime);
            Console.WriteLine("Ignore Time property: " + Properties.Settings.Default.IgnoreTime);
#endif
            #endregion

            this.Close();
        }

        /// <summary>
        /// If the user denys the settings, close the window without saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e) {
            #region Properties debug output
#if DEBUG
            Console.WriteLine("Properties window closed using CANCEL - settings should not have saved");

            Console.WriteLine("CPU checkbox: " + checkBox_cpu.Checked);
            Console.WriteLine("Memory checkbox: " + checkBox_memory.Checked);
            Console.WriteLine("Network checkbox: " + checkBox_network.Checked);
            Console.WriteLine("DiskIO checkbox: " + checkBox_diskio.Checked);
            Console.WriteLine("Ignore Time checkbox: " + checkBox_ignoreTime.Checked);


            Console.WriteLine("CPU property: " + Properties.Settings.Default.CPU);
            Console.WriteLine("Memory property: " + Properties.Settings.Default.Memory);
            Console.WriteLine("Network property: " + Properties.Settings.Default.Network);
            Console.WriteLine("DiskIO property: " + Properties.Settings.Default.DiskIO);
            Console.WriteLine("Real Time property: " + Properties.Settings.Default.RealTime);
            Console.WriteLine("Ignore Time property: " + Properties.Settings.Default.IgnoreTime);
#endif
            #endregion
            this.Close();
        }
    }
}

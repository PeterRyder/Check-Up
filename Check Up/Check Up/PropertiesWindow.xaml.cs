﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Check_Up {
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow : Window {
        public PropertiesWindow() {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, RoutedEventArgs e) {

            double pollingTime = Convert.ToDouble(textbox_pollingTime.Text);
            double pollingInterval = Convert.ToDouble(textbox_pollingInterval.Text);

            if (pollingTime < pollingInterval) {
#if DEBUG
                Console.WriteLine("Polling time greater than polling interval");
#endif
                error1.Visibility = System.Windows.Visibility.Visible;
                return;
            }


            Properties.Settings.Default.CPU = (bool)checkbox_CPU.IsChecked;
            Properties.Settings.Default.Memory = (bool)checkbox_Memory.IsChecked;
            Properties.Settings.Default.Network = (bool)checkbox_Network.IsChecked;
            Properties.Settings.Default.DiskIO = (bool)checkbox_DiskIO.IsChecked;
            //Properties.Settings.Default.IgnoreTime = checkbox_ignoreTime.Checked;

            try {

                Properties.Settings.Default.PollingTime = pollingTime;
            }
            catch {
                Console.Error.WriteLine("Couldn't convert {0} to double", textbox_pollingTime.Text);
            }

            try {

                Properties.Settings.Default.PollingInterval = pollingInterval;
            }
            catch {
                Console.Error.WriteLine("Couldn't convert {0} to double", textbox_pollingInterval.Text);
            }

            try {
                //int VisiblePoints = Convert.ToInt32(visiblePoints.Text);
                //Properties.Settings.Default.VisiblePoints = VisiblePoints;
            }
            catch {
                //Console.Error.WriteLine("Couldn't convert {0} to int", visiblePoints.Text);
            }

            #region Properties debug output
#if DEBUG
            Console.WriteLine("Properties window closed using OK - settings should have saved");

            Console.WriteLine("CPU checkbox: " + checkbox_CPU.IsChecked);
            Console.WriteLine("Memory checkbox: " + checkbox_Memory.IsChecked);
            Console.WriteLine("Network checkbox: " + checkbox_Network.IsChecked);
            Console.WriteLine("DiskIO checkbox: " + checkbox_DiskIO.IsChecked);
            //Console.WriteLine("Ignore Time checkbox: " + checkBox_ignoreTime.Checked);

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

        private void button_Cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void Window_Initialized(object sender, EventArgs e) {
            checkbox_CPU.IsChecked = Properties.Settings.Default.CPU;
            checkbox_Memory.IsChecked = Properties.Settings.Default.Memory;
            checkbox_Network.IsChecked = Properties.Settings.Default.Network;
            checkbox_DiskIO.IsChecked = Properties.Settings.Default.DiskIO;
            checkbox_ignoreTime.IsChecked = Properties.Settings.Default.IgnoreTime;

            textbox_pollingTime.Text = "" + Properties.Settings.Default.PollingTime;
            textbox_pollingInterval.Text = "" + Properties.Settings.Default.PollingInterval;
            textbox_visiblePoints.Text = "" + Properties.Settings.Default.VisiblePoints;
        }
    }
}

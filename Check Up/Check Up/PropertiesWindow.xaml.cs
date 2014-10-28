using System;
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
using System.IO;
using Check_Up.Util;
using log4net;

namespace Check_Up {
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow : Window {

        private List<Disk> SelectedDisks;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PropertiesWindow() {
            InitializeComponent();
            
            SelectedDisks = new List<Disk>();
        }

        private void button_OK_Click(object sender, RoutedEventArgs e) {

            double pollingTime = Convert.ToDouble(textbox_pollingTime.Text);
            double pollingInterval = Convert.ToDouble(textbox_pollingInterval.Text);

            if (pollingTime < pollingInterval) {

                log.Warn("Polling time greater than polling interval");

                error1.Visibility = System.Windows.Visibility.Visible;
                return;
            }


            Properties.Settings.Default.CPU = (bool)checkbox_CPU.IsChecked;
            Properties.Settings.Default.Memory = (bool)checkbox_Memory.IsChecked;
            Properties.Settings.Default.Network = (bool)checkbox_Network.IsChecked;
            Properties.Settings.Default.DiskIO = (bool)checkbox_DiskIO.IsChecked;

            Properties.Settings.Default.MonitorProcesses = (bool)checkbox_monitorProcesses.IsChecked;

            Properties.Settings.Default.OSAvg = (bool)checkbox_osAvg.IsChecked;
            Properties.Settings.Default.OSMin = (bool)checkbox_osMin.IsChecked;
            Properties.Settings.Default.OSMax = (bool)checkbox_osMax.IsChecked;

            Properties.Settings.Default.ProcAvg = (bool)checkbox_processAvg.IsChecked;
            Properties.Settings.Default.ProcMin = (bool)checkbox_processMin.IsChecked;
            Properties.Settings.Default.ProcMax = (bool)checkbox_processMax.IsChecked;


            //Properties.Settings.Default.IgnoreTime = checkbox_ignoreTime.Checked;

            try {

                Properties.Settings.Default.PollingTime = pollingTime;
            }
            catch {
                log.Error(String.Format("Couldn't convert {0} to double", textbox_pollingTime.Text));
            }

            try {

                Properties.Settings.Default.PollingInterval = pollingInterval;
            }
            catch {
                log.Error(String.Format("Couldn't convert {0} to double", textbox_pollingInterval.Text));
            }

            try {
                //int VisiblePoints = Convert.ToInt32(visiblePoints.Text);
                //Properties.Settings.Default.VisiblePoints = VisiblePoints;
            }
            catch {
                //log.Error(String.Format("Couldn't convert {0} to int", visiblePoints.Text));
            }

            List<string> DiskNames = new List<string>();
            for (int i = 0; i < SelectedDisks.Count; i++) {
                Disk item = SelectedDisks[i];
                string DriveLetter = item.DiskLetter.TrimEnd('\\');
                DiskNames.Add(DriveLetter);
            }
            Properties.Settings.Default.Disks = DiskNames;

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

            checkbox_monitorProcesses.IsChecked = Properties.Settings.Default.MonitorProcesses;

            checkbox_osAvg.IsChecked = Properties.Settings.Default.OSAvg;
            checkbox_osMin.IsChecked = Properties.Settings.Default.OSMin;
            checkbox_osMax.IsChecked = Properties.Settings.Default.OSMax;

            checkbox_processAvg.IsChecked = Properties.Settings.Default.ProcAvg;
            checkbox_processMin.IsChecked = Properties.Settings.Default.ProcMin;
            checkbox_processMax.IsChecked = Properties.Settings.Default.ProcMax;

            if (checkbox_DiskIO.IsChecked == true) {
                PopulateDriveList();
            }

            textbox_pollingTime.Text = "" + Properties.Settings.Default.PollingTime;
            textbox_pollingInterval.Text = "" + Properties.Settings.Default.PollingInterval;
            textbox_visiblePoints.Text = "" + Properties.Settings.Default.VisiblePoints;
        }

        private void checkbox_DiskIO_Click(object sender, RoutedEventArgs e) {
            if (listview_disks.Items.Count == 0 && checkbox_DiskIO.IsChecked == true) {
                PopulateDriveList();
            }
            else {
                listview_disks.ItemsSource = null;
            }
        }

        private void PopulateDriveList() {
            List<Disk> items = new List<Disk>();
            foreach (DriveInfo drive in RandomInfo.drives) {
                items.Add(new Disk() { DiskType = drive.DriveType.ToString(), DiskLetter = drive.Name });
            }
            listview_disks.ItemsSource = items;
        }

        private void listview_disks_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            foreach (Disk item in e.RemovedItems) {
                SelectedDisks.Remove(item);
            }

            foreach (Disk item in e.AddedItems) {
                SelectedDisks.Add(item);
            }
        }
    }
}

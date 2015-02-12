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
using log4net;
using Check_Up.Util;

namespace Check_Up {
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow : Window {

        List<Disk> items;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ThemeManager themeManager;

        public PropertiesWindow() {
            items = new List<Disk>();
            InitializeComponent();
            
            themeManager = new ThemeManager();
            themeManager.LoadThemes();
            ComboBoxThemes.ItemsSource = themeManager.themes;
            ComboBoxThemes.SelectedItem = "ExpressionDark.xaml";
            
        }

        private void button_OK_Click(object sender, RoutedEventArgs e) {

            double pollingTime = Convert.ToDouble(textbox_pollingTime.Text);
            double pollingInterval = Convert.ToDouble(textbox_pollingInterval.Text);

            // Prevent program from polling more frequently than .2 seconds
            if (pollingInterval < .2) {
                pollingInterval = .2;
            }

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

            //Properties.Settings.Default.IgnoreTime = checkbox_ignoreTime.Checked;

            Properties.Settings.Default.PollingTime = pollingTime;

            Properties.Settings.Default.PollingInterval = pollingInterval;

            //int VisiblePoints = Convert.ToInt32(visiblePoints.Text);
            //Properties.Settings.Default.VisiblePoints = VisiblePoints;

            List<string> DiskNames = new List<string>();

                foreach (Disk item in items) {
                    if (item.IsChecked) {
                        Console.WriteLine("Saving disk as " + item.DiskLetter);
                        DiskNames.Add(item.DiskLetter);
                    }  
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

            if (checkbox_DiskIO.IsChecked == true) {
                Console.WriteLine("Populating Drive List");
                PopulateDriveList();
            }

            textbox_pollingTime.Text = Properties.Settings.Default.PollingTime.ToString();
            textbox_pollingInterval.Text = Properties.Settings.Default.PollingInterval.ToString();
            textbox_visiblePoints.Text = Properties.Settings.Default.VisiblePoints.ToString();

            // load selected disks from properties here
            List<string> selectedDisks = Properties.Settings.Default.Disks;

            if (selectedDisks == null) {
                //Console.WriteLine("No selected disks");
            }
            else {
                foreach (Disk disk in items) {
                    //Console.WriteLine("Disk: " + disk);
                    if (selectedDisks.Contains(disk.DiskLetter)) {
                        disk.IsChecked = true;
                    }
                }
            }
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
            foreach (DriveInfo drive in RandomInfo.drives) {
                items.Add(new Disk() { DiskType = drive.DriveType.ToString(), DiskLetter = drive.Name, IsChecked = false});
            }
            listview_disks.ItemsSource = items;
        }

        private void listview_disks_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            //Console.WriteLine("Selection Changed");
            //Console.WriteLine("Adding " + e.AddedItems.Count);
            //Console.WriteLine("Removing " + e.RemovedItems.Count);

            foreach (Disk item in e.RemovedItems) {
                //Console.WriteLine("Removed disk " + item.DiskLetter + " from checked items");
                item.IsChecked = false;
            }

            foreach (Disk item in e.AddedItems) {
                //Console.WriteLine("Added disk " + item.DiskLetter + " to checked items");
                item.IsChecked = true;
            }
        }

        private void ComboBoxThemes_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBox cmb = (ComboBox)sender;
            themeManager.ChangeTheme(cmb.SelectedItem.ToString());
        }
   
    }
}

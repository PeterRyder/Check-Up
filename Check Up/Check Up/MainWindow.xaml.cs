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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Controls.DataVisualization.Charting;
using Check_Up.Util;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using log4net;

namespace Check_Up {

    public class cpuData {
        public ObservableCollection<KeyValuePair<int, int>> ValueList { get; private set; }

        public cpuData() {
            this.ValueList = new ObservableCollection<KeyValuePair<int, int>>();
        }

        public void Add(KeyValuePair<int, int> data) {
            ValueList.Add(data);
        }
    }

    public class memoryData {
        public ObservableCollection<KeyValuePair<int, int>> ValueList { get; private set; }

        public memoryData() {
            this.ValueList = new ObservableCollection<KeyValuePair<int, int>>();
        }

        public void Add(KeyValuePair<int, int> data) {
            ValueList.Add(data);
        }
    }

    public class networkData {
        public ObservableCollection<KeyValuePair<int, int>> ValueList { get; private set; }

        public networkData() {
            this.ValueList = new ObservableCollection<KeyValuePair<int, int>>();
        }

        public void Add(KeyValuePair<int, int> data) {
            ValueList.Add(data);
        }
    }

    public class diskioData {
        public ObservableCollection<KeyValuePair<int, int>> ValueList { get; private set; }

        public diskioData() {
            this.ValueList = new ObservableCollection<KeyValuePair<int, int>>();
        }

        public void Add(KeyValuePair<int, int> data) {
            ValueList.Add(data);
        }
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        System.Windows.Forms.NotifyIcon ni;

        OSDataCollection osDataCollector;
        Scripts scripts;

        List<Window> subWindows;

        private BackgroundWorker backgroundWorker;

        int cycles = 0;

        private bool shouldGatherData;

        private cpuData cpuDataStorage;
        private memoryData memDataStorage;
        private networkData netDataStorage;
        private diskioData diskDataStorage;

        public MainWindow() {
            InitializeComponent();

            InitializeObjects();

            InitializeEventHandlers();

            ni.Visible = true;

            scripts.checkDirectory();
            scripts.runScripts();

            // initialize a cpuData collector
            
            if (!osDataCollector.canGatherNet) {
                listview_warnings.Items.Add("Could not find network adapter");
            }

            if (!Properties.Settings.Default.CPU &&
                !Properties.Settings.Default.Memory &&
                !Properties.Settings.Default.Network &&
                !Properties.Settings.Default.DiskIO) {

                shouldGatherData = false;
            }
            else {
                shouldGatherData = true;
            }

            // check if cpuData should be gathered
            if (shouldGatherData) {
                backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            }
            else {

                // Stop the backgroundWorker if cpuData shouldn't be gathered
                backgroundWorker.CancelAsync();
                backgroundWorker.ReportProgress(100);
            }

            button_stopMonitoring.IsEnabled = false;
            button_resetChart.IsEnabled = false;
        }

        private void InitializeObjects() {
            ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon("Check Up.ico");
            scripts = new Scripts();
            subWindows = new List<Window>();
            osDataCollector = new OSDataCollection();

            backgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));

            cpuDataStorage = new cpuData();
            memDataStorage = new memoryData();
            diskDataStorage = new diskioData();
            netDataStorage = new networkData();
        }

        private void InitializeEventHandlers() {
            this.Closed += new EventHandler(MainWindow_Closed);
            ni.DoubleClick += delegate(object sender, EventArgs args) {
                this.Show();
                this.WindowState = WindowState.Normal;
            };
        }

        /// <summary>
        /// Launches the PropertiesWindow when the menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemProperties_Click(object sender, RoutedEventArgs e) {
            PropertiesWindow subWindow = new PropertiesWindow();
            subWindows.Add(subWindow);
            subWindow.Show();
        }

        /// <summary>
        /// Exits the Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemExit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e) {
            AboutWindow subWindow = new AboutWindow();
            subWindows.Add(subWindow);
            subWindow.Show();
        }

        /// <summary>
        /// Starts the background worker when the Gather Data button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_gatherData_Click(object sender, RoutedEventArgs e) {
            this.button_gatherData.IsEnabled = false;
            this.button_resetChart.IsEnabled = false;
            button_stopMonitoring.IsEnabled = true;
            this.menuitem_Properties.IsEnabled = false;

            this.cycles = 1;

            #region Create Series
            if (Properties.Settings.Default.CPU) {
                createSeries("CPU");
            }

            if (Properties.Settings.Default.Memory) {
                createSeries("Memory");
            }

            if (Properties.Settings.Default.Network) {
                createSeries("Network");
            }

            if (Properties.Settings.Default.DiskIO) {
                createSeries("DiskIO");
            }
            #endregion

            // Begin the backgroundWorker
            try {
                backgroundWorker.RunWorkerAsync();
            }
            catch {
                log.Error("Could not start backgroundWorker");
            }
        }

        private void createSeries(string type) {

            var areaSeries = new LineSeries {
                DataPointStyle = new Style {
                    TargetType = typeof(DataPoint),
                    Setters = { new Setter(TemplateProperty, null) }
                }
            };

            areaSeries.DataPointStyle.Setters.Add(
                new Setter(BackgroundProperty, new SolidColorBrush(Colors.Red)));

            areaSeries.Title = type;
            areaSeries.Name = type;
            areaSeries.DependentValuePath = "Value";
            areaSeries.IndependentValuePath = "Key";

            if (type == "CPU") {
                areaSeries.ItemsSource = cpuDataStorage.ValueList;
            }
            else if (type == "Memory") {
                areaSeries.ItemsSource = memDataStorage.ValueList;
            }
            else if (type == "Network") {
                areaSeries.ItemsSource = netDataStorage.ValueList;
            }
            else if (type == "DiskIO") {
                areaSeries.ItemsSource = diskDataStorage.ValueList;
            }
            chart.Series.Add(areaSeries);
        }

        /// <summary>
        /// Stops the backgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_stopMonitoring_Click(object sender, RoutedEventArgs e) {
            // When the monitorStop button is clicked stop the backgroundWorker
            backgroundWorker.CancelAsync();

            // Announce that the backgroundWorker is 100 percent done
            backgroundWorker.ReportProgress(100);

            this.button_resetChart.IsEnabled = true;
            this.menuitem_Properties.IsEnabled = true;

        }

        /// <summary>
        /// Resets the chart using a helper function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_resetChart_Click(object sender, RoutedEventArgs e) {
            resetChartFunc();
        }

        private void backgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e) {

            if (shouldGatherData) {
                // Update the progress bar every time the backgroundWorker changes
                progressBar.Value = e.ProgressPercentage;

                // Update the label next to the progress bar
                //label_percentage.Text = e.ProgressPercentage.ToString() + "%";

                // If cpuData cpuData should be gathered, update the graph
                if (Properties.Settings.Default.CPU) {
                    updateGraph("cpuData", cycles, (int)osDataCollector.currentCPUUsage);
                }

                // If memoryData cpuData should be gathered, update the graph
                if (Properties.Settings.Default.Memory) {
                    updateGraph("memoryData", cycles, (int)osDataCollector.currentMemUsage);
                }

                // If networkData cpuData should be gathered, update the graph
                if (Properties.Settings.Default.Network && osDataCollector.canGatherNet) {
                    updateGraph("networkData", cycles, (int)osDataCollector.currentNetUsageMBs);
                }

                // If Disk IO cpuData should be gathered, update the graph
                if (Properties.Settings.Default.DiskIO) {
                    updateGraph("Disk", cycles, (int)osDataCollector.percentDiskTime);
                }
                shouldGatherData = false;
            }

        }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
            // Create a BackgroundWorker object out of the sender
            BackgroundWorker worker = sender as BackgroundWorker;

            // GatherData every time the background worker does work
            e.Result = GatherData(worker, e);
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
            log.Debug("Worker completed");
            // If the progress bar isn't full, force it
            progressBar.Value = 100;
            if (e.Error != null) {
                System.Windows.MessageBox.Show(e.Error.Message);
            }

            // Disable the monitorStop button when the backgroundWorker is completed
            button_stopMonitoring.IsEnabled = false;
            button_resetChart.IsEnabled = true;
        }

        /// <summary>
        /// Main cpuData gathering loop - gathers cpuData and tracks the progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool GatherData(BackgroundWorker sender, DoWorkEventArgs e) {
            // Store the pollingTime and pollingInterval settings
            double pollingTime = Properties.Settings.Default.PollingTime;
            double pollingInterval = Properties.Settings.Default.PollingInterval;

            for (double i = pollingInterval; i <= pollingTime; ) {
                // Get current time
                int timeMin = DateTime.Now.Minute;
                int timeSec = DateTime.Now.Second;
                int timeMsec = DateTime.Now.Millisecond;

                // If there is a pending cancellation break out of the loop
                if (backgroundWorker.CancellationPending) {
                    e.Cancel = true;
                    return true;
                }

                #region Data Gathering
                // Gather data on the devices
                if (Properties.Settings.Default.CPU) {
                    osDataCollector.GatherCPUData();
                }

                if (Properties.Settings.Default.Memory) {
                    osDataCollector.GatherMemoryData();
                }

                if (Properties.Settings.Default.Network) {
                    osDataCollector.GatherNetworkData();
                }

                if (Properties.Settings.Default.DiskIO) {
                    osDataCollector.GatherDiskData();
                }
                #endregion

                shouldGatherData = true;

                // If the pollingTime is to be used
                if (!Properties.Settings.Default.IgnoreTime) {

                    // Calculate the percentage of the work which has been done
                    double percentage = (i / pollingTime) * 100;

                    if (percentage >= 100 || i >= pollingTime) {
                        backgroundWorker.CancelAsync();
                        percentage = Math.Round(percentage);
                        backgroundWorker.ReportProgress((int)percentage);

                        log.Debug("Set cancellation to pending");

                    }
                    else {
                        // Report the progress percentage
                        percentage = Math.Round(percentage);
                        backgroundWorker.ReportProgress((int)percentage);
                    }

                    // Increment the amount of time elapsed
                    i += pollingInterval;
                }
                else {

                    // If the polling time is not to be used keep the progress bar at 0 percent
                    backgroundWorker.ReportProgress(0);
                }

                int timeElapsed = (DateTime.Now.Minute - timeMin) * 60 * 1000;
                timeElapsed += (DateTime.Now.Second - timeSec) * 1000;
                timeElapsed += (DateTime.Now.Millisecond - timeMsec);

                // Sleep the backgroundWorker for the pollingInterval minus time already elapsed
                if (pollingInterval * 1000 - timeElapsed >= 1) {
                    Thread.Sleep((int)(pollingInterval * 1000 - timeElapsed));
                }

                // Increment the amount of cycles which has elapsed
                cycles++;
            }

            // If the loop is finished stop the backgroundWorker
            backgroundWorker.CancelAsync();

            // Announce if there is a pending cancellation
            if (backgroundWorker.CancellationPending) {
                log.Debug("Cancellation is pending");
                e.Cancel = true;
                return true;
            }

            return true;
        }


        private void updateGraph(string type, int x, int y) {
            // Add data points to the correct lists depending on the type
            if (type == "cpuData") {
                cpuDataStorage.Add(new KeyValuePair<int, int>(x, y));
            }
            else if (type == "memoryData") {
                memDataStorage.Add(new KeyValuePair<int, int>(x, y));
            }
            else if (type == "networkData") {
                netDataStorage.Add(new KeyValuePair<int, int>(x, y));
            }
            else if (type == "diskioData") {
                diskDataStorage.Add(new KeyValuePair<int, int>(x, y));
            }
        }

        /// <summary>
        /// Resets the chart
        /// </summary>
        private void resetChartFunc() {

            // Remove the Series
            chart.Series.Clear();

            // Remove the points from the lists
            cpuDataStorage.ValueList.Clear();
            memDataStorage.ValueList.Clear();
            netDataStorage.ValueList.Clear();
            diskDataStorage.ValueList.Clear();

            this.button_gatherData.IsEnabled = true;
            this.button_resetChart.IsEnabled = false;
        }

        private void button_checkScripts_Click(object sender, RoutedEventArgs e) {
            scripts.runScripts();
        }

        protected override void OnStateChanged(EventArgs e) {
            if (WindowState == WindowState.Minimized) {
                this.Hide();
            }
            base.OnStateChanged(e);
        }

        private void button_analyzeProcesses_Click(object sender, RoutedEventArgs e) {

        }

        private void MainWindow_Closed(object sender, EventArgs e) {
            foreach (Window window in subWindows) {
                try {
                    window.Close();
                }
                catch {
                    Console.WriteLine("Couldn't close sub window {0}", window.Name);
                }
            }
            backgroundWorker.CancelAsync();
            try {
                base.OnClosing((CancelEventArgs)e);
            }
            catch {
                Console.WriteLine("Couldn't call base form close");
            }
            ni.Visible = false;
            System.Windows.Application.Current.Shutdown();
        }

        private void button_checkScripts_Click(object sender, EventArgs e) {
            scripts.checkNewScripts();
        }
    }
}


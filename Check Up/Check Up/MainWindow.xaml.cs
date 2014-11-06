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
using System.IO;
using ReadWriteCsv;

namespace Check_Up {

    public class GraphData {
        public ObservableCollection<KeyValuePair<int, int>> ValueList { get; private set; }

        public GraphData() {
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
        private Random rand = new Random();

        private string OutputPath = "Data";
        private string FullOutputPath = "";
        private string OutputFileName;

        private CsvFileWriter CsvWriter;

        System.Windows.Forms.NotifyIcon ni;

        OSDataCollection osDataCollector;
        ProcessesDataCollection processDataCollector;
        Scripts scripts;

        List<Window> subWindows;

        static EventWaitHandle handle = new AutoResetEvent(false);

        private BackgroundWorker backgroundWorkerChart;

        int cycles = 0;

        int updates = 0;

        private bool shouldGatherData;

        private Dictionary<string, GraphData> GraphDataDict = new Dictionary<string, GraphData>();

        public MainWindow() {
            InitializeComponent();

            InitializeObjects();

            InitializeEventHandlers();

            ni.Visible = true;

            CreateOutputDirectory();

            OutputFileName = "Data - " + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";

            try {
                CsvWriter = new CsvFileWriter(OutputFileName);
            }
            catch {
                log.Error(String.Format("Could not create output csv file {0}", OutputFileName));
            }

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
                backgroundWorkerChart.DoWork += backgroundWorkerChart_DoWork;
            }
            else {

                // Stop the backgroundWorker if cpuData shouldn't be gathered
                backgroundWorkerChart.CancelAsync();
                backgroundWorkerChart.ReportProgress(100);
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
            processDataCollector = new ProcessesDataCollection();

            backgroundWorkerChart = new BackgroundWorker();
        }

        private void InitializeEventHandlers() {
            this.Closed += new EventHandler(MainWindow_Closed);
            ni.DoubleClick += delegate(object sender, EventArgs args) {
                this.Show();
                this.WindowState = WindowState.Normal;
            };

            backgroundWorkerChart.ProgressChanged += backgroundWorkerChart_ProgressChanged;
            backgroundWorkerChart.RunWorkerCompleted += backgroundWorkerChart_RunWorkerCompleted;
            backgroundWorkerChart.WorkerReportsProgress = true;
            backgroundWorkerChart.WorkerSupportsCancellation = true;
        }

        private void CreateOutputDirectory() {
            FullOutputPath = System.IO.Path.GetFullPath(OutputPath);
            if (!Directory.Exists(FullOutputPath)) {
                Directory.CreateDirectory(FullOutputPath);
            }
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

            osDataCollector.InitializeCounters();
            List<string> CountersRemoved = osDataCollector.RemoveCounters();
            if (CountersRemoved.Count != 0) {
                for (int i = 0; i < CountersRemoved.Count; i++) {
                    GraphDataDict.Remove(CountersRemoved[i]);
                }
            }

            this.cycles = 1;

            #region Create Series
            if (Properties.Settings.Default.CPU) {
                createSeries(CounterNames.CPUName);
            }

            if (Properties.Settings.Default.Memory) {
                createSeries(CounterNames.MemName);
            }

            if (Properties.Settings.Default.Network) {
                createSeries(CounterNames.NetName);
            }

            if (Properties.Settings.Default.DiskIO) {
                List<string> disks = Properties.Settings.Default.Disks;
                for (int i = 0; i < disks.Count; i++) {
                    createSeries(disks[i]);
                    osDataCollector.AddDiskCounter(disks[i]);
                }
            }
            #endregion

            // Begin the backgroundWorker
            try {
                backgroundWorkerChart.RunWorkerAsync();
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

            // Generate a random color for the color of the line
            Color randColor = GetRandomColor();

            areaSeries.DataPointStyle.Setters.Add(
                new Setter(BackgroundProperty, new SolidColorBrush(randColor)));

            areaSeries.Title = type;
            areaSeries.Name = "Disk" + type.TrimEnd(':');
            areaSeries.DependentValuePath = "Value";
            areaSeries.IndependentValuePath = "Key";

            AddToGraphData(type);

            areaSeries.ItemsSource = GraphDataDict[type].ValueList;

            chart.Series.Add(areaSeries);
        }

        private Color GetRandomColor() {
            return Color.FromRgb((byte)rand.Next(20, 236), (byte)this.rand.Next(20, 236), (byte)this.rand.Next(20, 236));
        }

        /// <summary>
        /// Stops the backgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_stopMonitoring_Click(object sender, RoutedEventArgs e) {
            // When the monitorStop button is clicked stop the backgroundWorker
            backgroundWorkerChart.CancelAsync();

            // Announce that the backgroundWorker is 100 percent done
            backgroundWorkerChart.ReportProgress(100);

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

        private void backgroundWorkerChart_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e) {

            if (shouldGatherData) {
                // Update the progress bar every time the backgroundWorker changes
                progressBar.Value = e.ProgressPercentage;

                List<string> types = new List<string>(GraphDataDict.Keys);
                for (int i = 0; i < types.Count; i++) {
                    try {
                        updateGraph(types[i], cycles, osDataCollector.DataValues[types[i]]);
                    }
                    catch {
                        log.Error(String.Format("Could not update graph for type {0}", types[i]));
                        log.Info(String.Format("Removing type {0} from lists", types[i]));
                        GraphDataDict.Remove(types[i]);
                        osDataCollector.RemoveCounter(types[i]);
                    }
                }
                shouldGatherData = false;
            }
        }

        private void backgroundWorkerChart_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
            // Create a BackgroundWorker object out of the sender
            BackgroundWorker worker = sender as BackgroundWorker;

            // GatherData every time the background worker does work
            e.Result = GatherDataOS(worker, e);
        }

        private void backgroundWorkerChart_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
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
        private bool GatherDataOS(BackgroundWorker sender, DoWorkEventArgs e) {

            // Store the pollingTime and pollingInterval settings
            double pollingTime = Properties.Settings.Default.PollingTime;
            double pollingInterval = Properties.Settings.Default.PollingInterval;

            for (double i = pollingInterval; i <= pollingTime; ) {
                // Get current time
                int timeMin = DateTime.Now.Minute;
                int timeSec = DateTime.Now.Second;
                int timeMsec = DateTime.Now.Millisecond;

                // If there is a pending cancellation break out of the loop
                if (sender.CancellationPending) {
                    e.Cancel = true;
                    return true;
                }

                #region Data Gathering                
                
                List<string> types = new List<string>(osDataCollector.DataValues.Keys);
                for (int j = 0; j < types.Count; j++) {
                    if (osDataCollector.GatherData(types[j]) != true) {
                        Console.WriteLine("Removing {0}", types[j]);
                        GraphDataDict.Remove(types[j]);
                    }
                }

                if (Properties.Settings.Default.MonitorProcesses) {
                    //processDataCollector.GatherData();
                }
                
                #endregion

                shouldGatherData = true;

                // If the pollingTime is to be used
                if (!Properties.Settings.Default.IgnoreTime) {

                    // Calculate the percentage of the work which has been done
                    double percentage = (i / pollingTime) * 100;

                    if (percentage >= 100 || i >= pollingTime) {
                        sender.CancelAsync();
                        percentage = Math.Round(percentage);
                        sender.ReportProgress((int)percentage);

                        log.Debug("Set cancellation to pending");

                    }
                    else {
                        // Report the progress percentage
                        percentage = Math.Round(percentage);
                        sender.ReportProgress((int)percentage);
                    }

                    // Increment the amount of time elapsed
                    i += pollingInterval;
                }
                else {

                    // If the polling time is not to be used keep the progress bar at 0 percent
                    sender.ReportProgress(0);
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
            sender.CancelAsync();

            // Announce if there is a pending cancellation
            if (sender.CancellationPending) {
                log.Debug("Cancellation is pending");
                e.Cancel = true;
                return true;
            }

            return true;
        }


        private void updateGraph(string type, int x, int y) {
            GraphDataDict[type].Add(new KeyValuePair<int, int>(x, y));
        }

        /// <summary>
        /// Resets the chart
        /// </summary>
        private void resetChartFunc() {

            // Remove the Series
            chart.Series.Clear();

            // Remove the points from the lists
            foreach (KeyValuePair<string, GraphData> data in GraphDataDict) {
                data.Value.ValueList.Clear();
            }

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

        private void MainWindow_Closed(object sender, EventArgs e) {
            foreach (Window window in subWindows) {
                try {
                    window.Close();
                }
                catch {
                    log.Error(String.Format("Couldn't close sub window {0}", window.Name));
                }
            }
            backgroundWorkerChart.CancelAsync();
            try {
                base.OnClosing((CancelEventArgs)e);
            }
            catch {
                log.Error("Couldn't call base form close");
            }
            ni.Visible = false;
            System.Windows.Application.Current.Shutdown();
        }

        private void button_checkScripts_Click(object sender, EventArgs e) {
            scripts.checkNewScripts();
        }

        private void button_logData_Click(object sender, RoutedEventArgs e) {

            osDataCollector.InitializeCounters();
            List<string> CountersRemoved = osDataCollector.RemoveCounters();
            if (CountersRemoved.Count != 0) {
                for (int i = 0; i < CountersRemoved.Count; i++) {
                    GraphDataDict.Remove(CountersRemoved[i]);
                }
            }

            if (Properties.Settings.Default.CPU) {
                AddToGraphData(CounterNames.CPUName);
            }

            if (Properties.Settings.Default.Memory) {
                AddToGraphData(CounterNames.MemName);
            }

            if (Properties.Settings.Default.Network) {
                AddToGraphData(CounterNames.NetName);
            }

            if (Properties.Settings.Default.DiskIO) {
                List<string> disks = Properties.Settings.Default.Disks;
                for (int i = 0; i < disks.Count; i++) {
                    AddToGraphData(disks[i]);
                    osDataCollector.AddDiskCounter(disks[i]);
                }
            }

            try {
                new Thread(GatherDataProcesses).Start();
            }
            catch {
                log.Error("Could not start Process Thread");
            }
            button_stopLoggingData.IsEnabled = true;
            button_logData.IsEnabled = false;
        }

        private void AddToGraphData(string type) {
            if (!GraphDataDict.ContainsKey(type)) {
                GraphDataDict.Add(type, new GraphData());
                Console.WriteLine("Added {0} to GraphDataDict", type);
            }
        }

        void GatherDataProcesses() {

            if (Properties.Settings.Default.MonitorProcesses) {
                processDataCollector.GatherData();
            }
            else {
                Console.WriteLine("Will not monitor processes - setting is false");
            }

            handle.WaitOne();

            PrintProcessResults();

        }

        private void PrintProcessResults() {
            Console.WriteLine("Results from Process Monitoring");
            Console.WriteLine("  CPU");

            foreach (var item in processDataCollector.HighestCpuUsage) {
                Console.WriteLine("    {0} : {1}%", item.Key, item.Value);
            }

            Console.WriteLine("  Memory");
            foreach (var item in processDataCollector.HighestMemUsage) {
                Console.WriteLine("    {0} : {1}MBs", item.Key, item.Value);
            }
        }

        private void button_stopLoggingData_Click(object sender, RoutedEventArgs e) {
            handle.Set();
            button_logData.IsEnabled = true;
            button_stopLoggingData.IsEnabled = false;
        }
    }
}


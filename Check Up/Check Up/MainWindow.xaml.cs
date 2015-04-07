using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Diagnostics;
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
using System.IO;

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
        private Random rand = new Random();

        System.Windows.Forms.NotifyIcon ni;

        OSDataCollection osDataCollector;
        ProcessesDataCollection processDataCollector;
        ThemeManager themeManager;

        List<Window> subWindows;

        static EventWaitHandle handle = new AutoResetEvent(false);

        private BackgroundWorker backgroundWorkerChart;

        int cycles = 0;

        private bool shouldGatherData;

        private Dictionary<string, GraphData> GraphDataDict = new Dictionary<string, GraphData>();

        public MainWindow() {
#if DEBUG
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
#endif
            InitializeComponent();

            FolderManager.CreateFolders();
#if DEBUG
            stopwatch.Stop();
            Logger.Debug("[time] InitializeComponent: " + stopwatch.ElapsedMilliseconds + "ms");

            stopwatch.Reset();
            stopwatch.Start();
#endif
            InitializeObjects();
#if DEBUG
            stopwatch.Stop();
            Logger.Debug("[time] InitializeObjects: " + stopwatch.ElapsedMilliseconds + "ms");

            stopwatch.Reset();
            stopwatch.Start();
#endif
            InitializeEventHandlers();
#if DEBUG
            stopwatch.Stop();
            Logger.Debug("[time] InitializeEventHandlers: " + stopwatch.ElapsedMilliseconds + "ms");
#endif
            Console.WriteLine("Showing Notification Icon");
            ni.Visible = true;

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

        #region INITIALIZERS

        private void InitializeObjects() {
            ni = new System.Windows.Forms.NotifyIcon();
            subWindows = new List<Window>();
#if DEBUG
            Stopwatch stopwatch = Stopwatch.StartNew();
#endif
            osDataCollector = new OSDataCollection();
#if DEBUG
            stopwatch.Stop();
            Console.WriteLine("[time] OSDataCollection Constructor: " + stopwatch.ElapsedMilliseconds + "ms");
            stopwatch.Reset();
            stopwatch.Start();
#endif
            processDataCollector = new ProcessesDataCollection();
            processDataCollector.LoadProcessCounters();
#if DEBUG
            stopwatch.Stop();

            Console.WriteLine("[time] ProcessDataCollection constructor: " + stopwatch.ElapsedMilliseconds + "ms");
#endif
            themeManager = new ThemeManager();
            themeManager.LoadThemes();
            themeManager.ChangeTheme("ExpressionDark.xaml");

            backgroundWorkerChart = new BackgroundWorker();
        }

        public void InitializeContextMenu(){
            System.Windows.Forms.ContextMenu contextMenu;
            contextMenu = new System.Windows.Forms.ContextMenu();

            System.Windows.Forms.MenuItem foreground = new System.Windows.Forms.MenuItem();
            contextMenu.MenuItems.AddRange(
                    new System.Windows.Forms.MenuItem[] { foreground });
            foreground.Text = "Foreground";

            System.Windows.Forms.MenuItem foreground_start = new System.Windows.Forms.MenuItem();
            foreground.MenuItems.Add(foreground_start);
            foreground_start.Text = "Gather Data";
            foreground_start.Click += new System.EventHandler(contextMenu_GatherForegroundData);

            System.Windows.Forms.MenuItem foreground_stop = new System.Windows.Forms.MenuItem();
            foreground.MenuItems.Add(foreground_stop);
            foreground_stop.Text = "Stop Monitoring";
            foreground_stop.Click += new System.EventHandler(this.contextMenu_StopForegroundData);

            System.Windows.Forms.MenuItem background = new System.Windows.Forms.MenuItem();
            contextMenu.MenuItems.AddRange(
                    new System.Windows.Forms.MenuItem[] { background });
            background.Text = "Background";

            System.Windows.Forms.MenuItem background1 = new System.Windows.Forms.MenuItem();
            background.MenuItems.Add(background1);
            background1.Text = "Log Data";
            background1.Click += new System.EventHandler(contextMenu_startBackgroundData);

            System.Windows.Forms.MenuItem background2 = new System.Windows.Forms.MenuItem();
            background.MenuItems.Add(background2);
            background2.Text = "Stop Logging Data";
            background2.Click += new System.EventHandler(contextMenu_stopLoggingData);

            System.Windows.Forms.MenuItem properties = new System.Windows.Forms.MenuItem();
            contextMenu.MenuItems.AddRange(
                    new System.Windows.Forms.MenuItem[] { properties });
            properties.Text = "Properties";
            properties.Click += new System.EventHandler(contextMenu_PropertiesWindow);

            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem();
            contextMenu.MenuItems.AddRange(
                    new System.Windows.Forms.MenuItem[] { exit });
            exit.Text = "Exit";
            exit.Click += new System.EventHandler(MainWindow_Closed);

            ni.ContextMenu = contextMenu;
        }

        private void InitializeEventHandlers() {
            this.Closed += new EventHandler(MainWindow_Closed);
            
            ni.DoubleClick += delegate(object sender, EventArgs args) {
                this.Show();
                this.WindowState = WindowState.Normal;
            };

            try {
                ni.Icon = new System.Drawing.Icon(System.Windows.Application.GetResourceStream(new Uri("/Check Up.ico", UriKind.Relative)).Stream);
            }
            catch {
                Console.WriteLine("Couldn't set icon");
            }

            InitializeContextMenu();

            backgroundWorkerChart.ProgressChanged += backgroundWorkerChart_ProgressChanged;
            backgroundWorkerChart.RunWorkerCompleted += backgroundWorkerChart_RunWorkerCompleted;
            backgroundWorkerChart.WorkerReportsProgress = true;
            backgroundWorkerChart.WorkerSupportsCancellation = true;
        }

        #endregion

        #region CONTEXT MENU

        private void contextMenu_PropertiesWindow(object sender, EventArgs e) {
            PropertiesHelper();
        }

        private void contextMenu_startBackgroundData(object sender, EventArgs e) {
            StartBackgroundData();
        }

        private void contextMenu_stopLoggingData(object sender, EventArgs e) {
            StopBackgroundLogging();
        }

        private void contextMenu_GatherForegroundData(object sender, System.EventArgs e) {
            BeginForegroundMonitoring();
        }

        private void contextMenu_StopForegroundData(object sender, System.EventArgs e) {
            StopForegroundMonitoring();
        }

        private void menuItem_LogData(object sender, EventArgs e) {

            // Start the processes monitoring thread
            try {
                new Thread(GatherDataProcesses).Start();
            }
            catch {
                Logger.Error("Could not start Process Thread");
            }

            button_stopLoggingData.IsEnabled = true;
            button_logData.IsEnabled = false;
        }

        #endregion

        #region MENU BAR

        private void MenuItemProperties_Click(object sender, RoutedEventArgs e) {
            PropertiesHelper();
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e) {
            System.Windows.Forms.MessageBox.Show("About Window is WIP");
        }

        #endregion

        #region BUTTON_CLICK

        private void button_gatherData_Click(object sender, RoutedEventArgs e) {
            BeginForegroundMonitoring();
        }

        private void button_stopMonitoring_Click(object sender, RoutedEventArgs e) {
            StopForegroundMonitoring();
        }

        private void button_resetChart_Click(object sender, RoutedEventArgs e) {
            ResetChart();
        }

        private void button_checkScripts_Click(object sender, RoutedEventArgs e) {
            ScriptWindow subWindow = new ScriptWindow();
            subWindows.Add(subWindow);
            subWindow.Show();
        }

        private void button_logData_Click(object sender, RoutedEventArgs e) {
#if DEBUG
            Stopwatch stopwatch = Stopwatch.StartNew();
#endif
            StartBackgroundData();

            button_stopLoggingData.IsEnabled = true;
            button_logData.IsEnabled = false;
#if DEBUG
            stopwatch.Stop();
            Console.WriteLine("[time] Log Data Button completed in: " + stopwatch.ElapsedMilliseconds + "ms");
#endif
        }

        private void button_stopLoggingData_Click(object sender, RoutedEventArgs e) {
            StopBackgroundLogging();
        }

        private void button_Results_Click(object sender, RoutedEventArgs e) {

        }



        #endregion

        #region BACKGROUND WORKER CHART

        private void backgroundWorkerChart_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e) {

            if (shouldGatherData) {
                // Update the progress bar every time the backgroundWorker changes
                progressBar.Value = e.ProgressPercentage;

                List<string> types = new List<string>(GraphDataDict.Keys);
                for (int i = 0; i < types.Count; i++) {
                    try {
                        UpdateGraph(types[i], cycles, osDataCollector.DataValues[types[i]]);
                    }
                    catch {
                        Logger.Error(String.Format("Could not update graph for type {0}", types[i]));
                        Logger.Info(String.Format("Removing type {0} from lists", types[i]));
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
            Logger.Info("Worker completed");
            // If the progress bar isn't full, force it
            progressBar.Value = 100;
            if (e.Error != null) {
                System.Windows.MessageBox.Show(e.Error.Message);
            }

            // Disable the monitorStop button when the backgroundWorker is completed
            button_stopMonitoring.IsEnabled = false;
            button_resetChart.IsEnabled = true;
            this.menuitem_Properties.IsEnabled = true;
        }

        #endregion

        /// <summary>
        /// Launches the PropertiesWindow when the menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void PropertiesHelper(){
            PropertiesWindow subWindow = new PropertiesWindow();
            subWindows.Add(subWindow);
            subWindow.Show();
        }

        private void BeginForegroundMonitoring() {
            this.button_gatherData.IsEnabled = false;
            this.button_resetChart.IsEnabled = false;
            button_stopMonitoring.IsEnabled = true;
            this.menuitem_Properties.IsEnabled = false;

            ni.Text = "Monitoring Foreground Data";

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
                CreateSeries(CounterNames.CPUName);
            }

            if (Properties.Settings.Default.Memory) {
                CreateSeries(CounterNames.MemName);
            }

            if (Properties.Settings.Default.Network) {
                CreateSeries(CounterNames.NetName);
            }

            if (Properties.Settings.Default.DiskIO) {
                List<string> disks = Properties.Settings.Default.Disks;
                foreach (string disk in disks) {
                    string name = disk.TrimEnd('\\');
                    osDataCollector.AddDiskCounter(name);
                    CreateSeries(name);

                }
            }
            #endregion

            // Begin the backgroundWorker
            try {
                backgroundWorkerChart.RunWorkerAsync();
            }
            catch {
                Logger.Error("Could not start backgroundWorker");
            }
        }

        private void CreateSeries(string type) {

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

        private void StopForegroundMonitoring() {
            // When the monitorStop button is clicked stop the backgroundWorker
            backgroundWorkerChart.CancelAsync();

            // Announce that the backgroundWorker is 100 percent done
            backgroundWorkerChart.ReportProgress(100);

            this.button_resetChart.IsEnabled = true;

            ni.Text = "";
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

                        Logger.Info("Set cancellation to pending");

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
                Logger.Info("Cancellation is pending");
                e.Cancel = true;
                return true;
            }

            return true;
        }

        private void UpdateGraph(string type, int x, int y) {
            GraphDataDict[type].Add(new KeyValuePair<int, int>(x, y));
        }

        /// <summary>
        /// Resets the chart
        /// </summary>
        private void ResetChart() {

            // Remove the Series
            chart.Series.Clear();

            // Remove the points from the lists
            foreach (KeyValuePair<string, GraphData> data in GraphDataDict) {
                data.Value.ValueList.Clear();
            }

            this.button_gatherData.IsEnabled = true;
            this.button_resetChart.IsEnabled = false;
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
                    Logger.Error(String.Format("Couldn't close sub window {0}", window.Name));
                }
            }

            backgroundWorkerChart.CancelAsync();

            try {
                base.OnClosing((CancelEventArgs)e);
            }
            catch {
                Logger.Error("Couldn't call base form close");
            }

            ni.Visible = false;
            System.Windows.Application.Current.Shutdown();
        }

        private void StartBackgroundData() {
            try {
                new Thread(GatherDataProcesses).Start();
            }
            catch {
                Logger.Error("Could not start Process Thread");
            }
            button_stopLoggingData.IsEnabled = true;
            button_logData.IsEnabled = false;

            ni.Text = "Gathering Background Data";
        }

        private void AddToGraphData(string type) {
            if (!GraphDataDict.ContainsKey(type)) {
                GraphDataDict.Add(type, new GraphData());
                Console.WriteLine("Added {0} to GraphDataDict", type);
            }
        }

        private void StopBackgroundLogging(){
            handle.Set();
            button_logData.IsEnabled = true;
            button_stopLoggingData.IsEnabled = false;

            ni.Text = "";
        }

        /// <summary>
        /// Will Gather Data on All Processes
        /// </summary>
        void GatherDataProcesses() {
            if (Properties.Settings.Default.MonitorProcesses) {
                // Fire the NextValue function for all processes
                processDataCollector.GatherData(true);
            }
            else {
                Console.WriteLine("Will not monitor processes - setting is false");
            }

            // Sleep the thread until the stop logging button is pressed
            handle.WaitOne();

            // Fire the NextValue function again to calculate the average usage for all processes during the runtime
            processDataCollector.GatherData(false);

            // Log output to CSV file
            OutputProcessResults();
        }

        /// <summary>
        /// Debug Function to Output Results of Process Monitoring
        /// </summary>
        private void OutputProcessResults() {
#if DEBUG
            Stopwatch stopwatch = Stopwatch.StartNew();
#endif
            BackgroundDataManager.SerializeData(processDataCollector.DataValues);
            Logger.Info("Finished writing background data");
#if DEBUG
            stopwatch.Stop();
            Console.WriteLine("[time] OutputProcessResults function completed in: " + stopwatch.ElapsedMilliseconds + "ms");
#endif
        }

    }
}

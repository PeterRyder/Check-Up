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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Controls.DataVisualization.Charting;
using Check_Up.Util;
using System.Threading;
using System.Collections.ObjectModel;

namespace Check_Up {

    public class CPU {
        public ObservableCollection<KeyValuePair<int, int>> ValueList { get; private set; }

        public CPU() {
            this.ValueList = new ObservableCollection<KeyValuePair<int, int>>();
        }

        public void Add(KeyValuePair<int, int> data) {
            ValueList.Add(data);
        }
    }

    public class Memory {
        public ObservableCollection<KeyValuePair<int, int>> ValueList { get; private set; }

        public Memory() {
            this.ValueList = new ObservableCollection<KeyValuePair<int, int>>();
        }

        public void Add(KeyValuePair<int, int> data) {
            ValueList.Add(data);
        }
    }

    public class Network {
        public ObservableCollection<KeyValuePair<int, int>> ValueList { get; private set; }

        public Network() {
            this.ValueList = new ObservableCollection<KeyValuePair<int, int>>();
        }

        public void Add(KeyValuePair<int, int> data) {
            ValueList.Add(data);
        }
    }

    public class DiskIO {
        public ObservableCollection<KeyValuePair<int, int>> ValueList { get; private set; }

        public DiskIO() {
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

        OSDataCollection osDataCollector;
        //Scripts scripts;

        List<Window> subWindows;

        private BackgroundWorker backgroundWorker;

        int cycles = 0;

        private bool shouldGatherData;

        private CPU cpuData;
        private Memory memData;
        private Network netData;
        private DiskIO diskData;

        public MainWindow() {
            InitializeComponent();
            backgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));

            // initialize a cpuData collector
            osDataCollector = new OSDataCollection();
            if (!osDataCollector.canGatherNet) {
                listview_warnings.Items.Add("Could not find network adapter");
            }
            subWindows = new List<Window>();

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

            cpuData = new CPU();
            memData = new Memory();
            diskData = new DiskIO();
            netData = new Network();
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

        }

        /// <summary>
        /// Starts the background worker when the Gather Data button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_gatherData_Click(object sender, RoutedEventArgs e) {
            this.button_gatherData.IsEnabled = false;
            this.cycles = 1;
            button_stopMonitoring.IsEnabled = true;

            // Begin the backgroundWorker
            backgroundWorker.RunWorkerAsync();
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

            // Enable the gatherData button
            button_gatherData.IsEnabled = true;
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

                // If CPU cpuData should be gathered, update the graph
                if (Properties.Settings.Default.CPU) {
                    updateGraph("CPU", cycles, (int)osDataCollector.currentCPUUsage);
                }

                // If Memory cpuData should be gathered, update the graph
                if (Properties.Settings.Default.Memory) {
                    updateGraph("Memory", cycles, (int)osDataCollector.currentMemUsage);
                }

                // If Network cpuData should be gathered, update the graph
                if (Properties.Settings.Default.Network && osDataCollector.canGatherNet) {
                    updateGraph("Network", cycles, (int)osDataCollector.currentNetUsageMBs);
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
#if DEBUG
            Console.WriteLine("Worker completed");
#endif
            // If the progress bar isn't full, force it
            progressBar.Value = 100;
            if (e.Error != null) {
                MessageBox.Show(e.Error.Message);
            }

            // Enable the gatherData button when the backgroundWorker is completed
            button_gatherData.IsEnabled = true;

            // Disable the monitorStop button when the backgroundWorker is completed
            button_stopMonitoring.IsEnabled = false;
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
#if DEBUG
            Console.WriteLine("Polling Time: " + pollingTime);
            Console.WriteLine("Polling Interval: " + pollingInterval);
#endif
            for (double i = pollingInterval; i <= pollingTime; ) {
                // Get current time
                int timeMin = DateTime.Now.Minute;
                int timeSec = DateTime.Now.Second;
                int timeMsec = DateTime.Now.Millisecond;

                // If there is a pending cancellation break out of the loop
                if (backgroundWorker.CancellationPending) {
#if DEBUG
                    Console.WriteLine("Cancellation is pending - killing the loop");
#endif
                    e.Cancel = true;
                    return true;
                }

                // Gather cpuData on the devices
                osDataCollector.GatherData();
                shouldGatherData = true;

                // If the pollingTime is to be used
                if (!Properties.Settings.Default.IgnoreTime) {

                    // Calculate the percentage of the work which has been done
                    double percentage = (i / pollingTime) * 100;
#if DEBUG
                    Console.WriteLine("Percentage: " + percentage);
#endif
                    if (percentage >= 100 || i >= pollingTime) {
                        backgroundWorker.CancelAsync();
                        percentage = Math.Round(percentage);
                        backgroundWorker.ReportProgress((int)percentage);
#if DEBUG
                        Console.WriteLine("Set cancellation to pending");
#endif
                    }
                    else {
                        // Report the progress percentage
                        percentage = Math.Round(percentage);
                        backgroundWorker.ReportProgress((int)percentage);
#if DEBUG
                        Console.WriteLine("I {0}", i);
#endif
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
#if DEBUG
                Console.WriteLine(String.Format("Sleep time: {0}", pollingInterval * 1000 - timeElapsed));
#endif
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
#if DEBUG
                Console.WriteLine("Cancellation is pending");
#endif
                e.Cancel = true;
                return true;
            }

            return true;
        }

        
        private void updateGraph(string type, int x, int y) {
            bool foundSeries = false;
            foreach (Series sereis in chart.Series) {
                if (sereis.Name == type) {
                    Console.WriteLine("Found series");
                    foundSeries = true;
                }
            }

            if (!foundSeries) {
                Console.WriteLine("Creating Series");
                LineSeries areaSeries = new LineSeries();
                areaSeries.Title = type;
                areaSeries.Name = type;
                areaSeries.DependentValuePath = "Value";
                areaSeries.IndependentValuePath = "Key";
                if (type == "CPU") {
                    areaSeries.ItemsSource = cpuData.ValueList;
                }
                else if (type == "Memory") {
                    areaSeries.ItemsSource = memData.ValueList;
                }
                else if (type == "Network") {
                    areaSeries.ItemsSource = netData.ValueList;
                }
                else if (type == "DiskIO") {
                    areaSeries.ItemsSource = diskData.ValueList;
                }
                chart.Series.Add(areaSeries);
            }

            if (type == "CPU") {
                cpuData.Add(new KeyValuePair<int, int>(x, y));
            }
            else if (type == "Memory") {
                memData.Add(new KeyValuePair<int, int>(x, y));  
            }
            else if (type == "Network") {
                netData.Add(new KeyValuePair<int, int>(x, y));  
            }
            else if (type == "DiskIO") {
                diskData.Add(new KeyValuePair<int, int>(x, y));
            }
        }


        

        /// <summary>
        /// Resets the chart
        /// </summary>
        private void resetChartFunc() {
            for (int i = 0; i < chart.Series.Count; i++ ) {
                chart.Series.RemoveAt(i);
#if DEBUG
                Console.WriteLine("Reset Series");
#endif
            }
            cpuData.ValueList.Clear();
            memData.ValueList.Clear();
            netData.ValueList.Clear();
            diskData.ValueList.Clear();
        }

    }
}


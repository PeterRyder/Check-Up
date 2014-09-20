using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;

namespace Check_Up {
    public partial class MainWindow : Form {
        DataCollection dataCollector;
        int cycles = 1;

        public MainWindow() {
            InitializeComponent();

            // initialize a data collector
            dataCollector = new DataCollection();
        }

        private void MainWindow_Load(object sender, EventArgs e) {

        }

        /// <summary>
        /// Starts gathering data and updating the graph when the button "Gather Data" is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_gatherData_Click(object sender, EventArgs e) {
            this.button_gatherData.Enabled = false;
            this.cycles = 1;
            button_monitorStop.Enabled = true;

            // Load settings
            dataCollector.ReadSettings();

            // Begin the backgroundWorker
            backgroundWorker1.RunWorkerAsync();

            // Add the ProgressChanged function to the ProgressChangedEventHandler
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

            // check if data should be gathered
            if (dataCollector.shouldGatherData) {
                backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            }
            else {

                // Stop the backgroundWorker if data shouldn't be gathered
                backgroundWorker1.CancelAsync();
                backgroundWorker1.ReportProgress(100);
            }
        }

        /// <summary>
        /// Menu item to display the "Properties" form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            PropertiesForm subForm = new PropertiesForm();
            subForm.Show();

        }

        /// <summary>
        /// Menu item to exit the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        /// <summary>
        /// Menu item to display the "About" form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutCheckUpToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutForm subForm = new AboutForm();
            subForm.Show();
        }

        /// <summary>
        /// Fired whenever the background worked updates its progress
        /// Used for updating the graph and the progress bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {

            // Update the progress bar every time the backgroundWorker changes
            progressBar1.Value = e.ProgressPercentage;

            // Update the label next to the progress bar
            label_percentage.Text = e.ProgressPercentage.ToString() + "%";

            // If CPU data should be gathered, update the graph
            if (Properties.Settings.Default.CPU) {
                updateGraph("CPU", "" + cycles, "" + dataCollector.currentCPUUsage);
            }

            // If Memory data should be gathered, update the graph
            if (Properties.Settings.Default.Memory) {
                updateGraph("Memory", "" + cycles, "" + dataCollector.currentMemUsage);
            }

            // If Network data should be gathered, update the graph
            if (Properties.Settings.Default.Network && dataCollector.canGatherNet) {
                updateGraph("Network", "" + cycles, "" + dataCollector.currentNetUsageMBs);
            }

            // If Disk IO data should be gathered, update the graph
            if (Properties.Settings.Default.DiskIO) {
                updateGraph("Disk", "" + cycles, "" + dataCollector.percentDiskTime);
            }

        }

        /// <summary>
        /// DoWork method for the backgroundWorker - gathers data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            // Create a BackgroundWorker object out of the sender
            BackgroundWorker worker = sender as BackgroundWorker;

            // GatherData every time the background worker does work
            e.Result = GatherData(worker, e);
        }

        /// <summary>
        /// Fired whenever the background worker completes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e) {
#if DEBUG
            Console.WriteLine("Worker completed");
#endif
            // If the progress bar isn't full, force it
            progressBar1.Value = 100;
            if (e.Error != null) {
                MessageBox.Show(e.Error.Message);
            }

            // Enable the gatherData button when the backgroundWorker is completed
            button_gatherData.Enabled = true;

            // Disable the monitorStop button when the backgroundWorker is completed
            button_monitorStop.Enabled = false;
        }

        /// <summary>
        /// Main data gathering loop - gathers data and tracks the progress
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

                // If there is a pending cancellation break out of the loop
                if (backgroundWorker1.CancellationPending) {
#if DEBUG
                    Console.WriteLine("Cancellation is pending - killing the loop");
#endif
                    e.Cancel = true;
                    return true;
                }
                
                // Gather data on the devices
                dataCollector.GatherData();

                // If the pollingTime is to be used
                if (!Properties.Settings.Default.IgnoreTime) {

                    // Calculate the percentage of the work which has been done
                    double percentage = (i / pollingTime) * 100;
#if DEBUG
                    Console.WriteLine("Percentage: " + percentage);
#endif
                    if (percentage >= 100 || i >= pollingTime) {
                        backgroundWorker1.CancelAsync();
                        percentage = Math.Round(percentage);
                        backgroundWorker1.ReportProgress((int)percentage);
#if DEBUG
                        Console.WriteLine("Set cancellation to pending");
#endif
                    }
                    else {
                        // Report the progress percentage
                        percentage = Math.Round(percentage);
                        backgroundWorker1.ReportProgress((int)percentage);
#if DEBUG
                        Console.WriteLine("I {0}", i);
#endif
                    }

                    // Increment the amount of time elapsed
                    i += pollingInterval;
                }
                else {

                    // If the polling time is not to be used keep the progress bar at 0 percent
                    backgroundWorker1.ReportProgress(0);
                }

                // Sleep the backgroundWorker for the pollingInterval
                Thread.Sleep((int)(pollingInterval * 1000));

                // Increment the amount of cycles which has elapsed
                cycles++;
            }

            // If the loop is finished stop the backgroundWorker
            backgroundWorker1.CancelAsync();

            // Announce if there is a pending cancellation
            if (backgroundWorker1.CancellationPending) {
#if DEBUG
                Console.WriteLine("Cancellation is pending");
#endif
                e.Cancel = true;
                return true;
            }

            return true;
        }

        /// <summary>
        /// Function to add a data type, x, and y coordinate to the graph
        /// </summary>
        /// <param name="type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void updateGraph(string type, string x, string y) {

            // Could be useful when creating a doughnut type graph
            //resetChartFunc();

            // Check if the data type is already registered with the chart
            if (this.chart.Series.IndexOf(type) != -1) {
#if DEBUG
                //Console.WriteLine("Chart already has {0} in the series", type);
#endif
            }
            else {
                // If the data type is not registered with the chart add it
                this.chart.Series.Add(type);
            }

            try {
                // Create a line graph out of the data
                this.chart.Series[type].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            }
            catch {
                Console.WriteLine("Couldn't change {0} into line graph", type);
            }
            try {
                // Add the X,Y coordinate of the data to the graph
                this.chart.Series[type].Points.AddXY(x, y);

                // Display a tooltip when mouse hovers over the line graph
                chart.Series[type].ToolTip = "X: #VALX, Y: #VALY";
            }
            catch {
                Console.WriteLine("Couldn't create point on graph X: {0}, Y: {1}", x, y);
            }
        }

        /// <summary>
        /// Stops the DataCollector when the button "Stop Monitoring" is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void monitorStop_Click(object sender, EventArgs e) {

            // When the monitorStop button is clicked stop the backgroundWorker
            backgroundWorker1.CancelAsync();

            // Announce that the backgroundWorker is 100 percent done
            backgroundWorker1.ReportProgress(100);

            // Enable the gatherData button
            button_gatherData.Enabled = true;
        }

        /// <summary>
        /// Resets the graph and removes all coordinates when the button "Reset Graph" is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetChart_Click(object sender, EventArgs e) {
            resetChartFunc();
        }
        
        private void analyzeProcesses_Click(object sender, EventArgs e) {
            
        }

        /// <summary>
        /// Will reset the graph and remove all coordinates
        /// </summary>
        private void resetChartFunc() {
            foreach (var series in chart.Series) {
                series.Points.Clear();
            }
        }
    }
}

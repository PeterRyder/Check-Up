﻿using System;
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
using Check_Up.Util;

namespace Check_Up {
    public partial class MainWindow : Form {
        OSDataCollection osDataCollector;
        Scripts scripts;

        int cycles = 0;
        List<Form> subForms;

        private bool shouldGatherData;

        public MainWindow() {
            InitializeComponent();

            // Initialize the scripts and run them
            scripts = new Scripts();
            scripts.checkDirectory();
            scripts.runScripts();

            // initialize a data collector
            osDataCollector = new OSDataCollection();
            if (!osDataCollector.canGatherNet) {
                listView_warnings.Items.Add(new ListViewItem(new string[] { "Could not find network adapter" }));
            }
            subForms = new List<Form>();

            // Add the ProgressChanged function to the ProgressChangedEventHandler
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

            if (!Properties.Settings.Default.CPU &&
                !Properties.Settings.Default.Memory &&
                !Properties.Settings.Default.Network &&
                !Properties.Settings.Default.DiskIO) {
                shouldGatherData = false;
            }
            else {
                shouldGatherData = true;
            }

            // check if data should be gathered
            if (shouldGatherData) {
                backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            }
            else {

                // Stop the backgroundWorker if data shouldn't be gathered
                backgroundWorker1.CancelAsync();
                backgroundWorker1.ReportProgress(100);
            }
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

            // Begin the backgroundWorker
            backgroundWorker1.RunWorkerAsync();
        }

        /// <summary>
        /// Menu item to display the "Properties" form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            PropertiesForm subForm = new PropertiesForm();
            subForms.Add(subForm);
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
            subForms.Add(subForm);
            subForm.Show();
        }

        /// <summary>
        /// Fired whenever the background worked updates its progress
        /// Used for updating the graph and the progress bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            if (shouldGatherData) {
                // Update the progress bar every time the backgroundWorker changes
                progressBar1.Value = e.ProgressPercentage;

                // Update the label next to the progress bar
                label_percentage.Text = e.ProgressPercentage.ToString() + "%";

                // If CPU data should be gathered, update the graph
                if (Properties.Settings.Default.CPU) {
                    updateGraph("CPU", "" + cycles, "" + osDataCollector.currentCPUUsage);
                }

                // If Memory data should be gathered, update the graph
                if (Properties.Settings.Default.Memory) {
                    updateGraph("Memory", "" + cycles, "" + osDataCollector.currentMemUsage);
                }

                // If Network data should be gathered, update the graph
                if (Properties.Settings.Default.Network && osDataCollector.canGatherNet) {
                    updateGraph("Network", "" + cycles, "" + osDataCollector.currentNetUsageMBs);
                }

                // If Disk IO data should be gathered, update the graph
                if (Properties.Settings.Default.DiskIO) {
                    updateGraph("Disk", "" + cycles, "" + osDataCollector.percentDiskTime);
                }
                shouldGatherData = false;
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
                // Get current time
                int timeMin = DateTime.Now.Minute;
                int timeSec = DateTime.Now.Second;
                int timeMsec = DateTime.Now.Millisecond;

                // If there is a pending cancellation break out of the loop
                if (backgroundWorker1.CancellationPending) {
#if DEBUG
                    Console.WriteLine("Cancellation is pending - killing the loop");
#endif
                    e.Cancel = true;
                    return true;
                }

                // Gather data on the devices
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
        /// 
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
                Console.WriteLine("ERROR: Couldn't change {0} into line graph", type);
            }
            try {
                // Add the X,Y coordinate of the data to the graph
#if DEBUG
                Console.WriteLine("X: {0} Y: {1}", x, y);
#endif
                this.chart.Series[type].Points.AddXY(x, y);

                // Display a tooltip when mouse hovers over the line graph
                chart.Series[type].ToolTip = "X: #VALX, Y: #VALY";
            }
            catch {
                Console.WriteLine("ERROR: Couldn't create point on graph X: {0}, Y: {1}", x, y);
            }
        }

        /// <summary>
        /// Resets the graph and removes all coordinates when the button "Reset Graph" is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_resetChart_Click(object sender, EventArgs e) {
            resetChartFunc();
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

        private void analyzeProcesses_Click(object sender, EventArgs e) {
            ProcessListForm subForm = new ProcessListForm();
            subForms.Add(subForm);
            subForm.Show();
        }

        /// <summary>
        /// Will reset the graph and remove all coordinates
        /// </summary>
        /// 
        private void resetChartFunc() {
            foreach (var series in chart.Series) {
                series.Points.Clear();
#if DEBUG
                Console.WriteLine("Reset Series");
#endif
            }
        }

        /// <summary>
        /// Override the form closing event to close all sub forms
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e) {
            foreach (Form form in subForms) {
                try {
                    form.Close();
                }
                catch {
                    Console.WriteLine("Couldn't close subform {0}", form.Name);
                }
            }
            backgroundWorker1.CancelAsync();
            try {
                base.OnFormClosing(e);
            }
            catch {
                Console.WriteLine("Couldn't call base form close");
            }
            Application.Exit();
        }

        private void button_checkScripts_Click(object sender, EventArgs e) {
            scripts.checkNewScripts();
        }

        private void notifyIcon1_Click(object sender, EventArgs e) {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void MainWindow_Resize(object sender, EventArgs e) {
            if (WindowState == FormWindowState.Minimized) {
                this.Hide();
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using System.Diagnostics;
using System.Timers;

namespace ReadWriteCsv {
    public partial class DataGatheringForm : Form {

        #region Performance Counters
        // create perf mon objects
        private PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
        private PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");

        // currently cannot use these two performance metrics - dunno why not...
        //private PerformanceCounter perfNetCount = new PerformanceCounter("Network Interface", "Bytes Total/sec");
        //private PerformanceCounter perfDiskCount = new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total");
        #endregion

        public List<int> memAvailable = new List<int>();
        public List<int> cpuUsage = new List<int>();

        public bool shouldGatherData;

        public DataGatheringForm() {
            InitializeComponent();

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

            ReadSettings();

            Shown += new EventHandler(Form1_Shown);

            perfCpuCount.NextValue();
            perfMemCount.NextValue();

            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

            if (shouldGatherData) {
                backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            }
            else {
                backgroundWorker1.ReportProgress(100);
            }
        }

        void ReadSettings() {
            #region Debug Console Output
#if DEBUG
            Console.WriteLine("Gathering Window opened successfully");

            Console.WriteLine("CPU property: " + Properties.Settings.Default.CPU);
            Console.WriteLine("Memory property: " + Properties.Settings.Default.Memory);
            Console.WriteLine("Network property: " + Properties.Settings.Default.Network);
            Console.WriteLine("DiskIO property: " + Properties.Settings.Default.DiskIO);
#endif
            #endregion
            if (!Properties.Settings.Default.CPU &&
                !Properties.Settings.Default.Memory &&
                !Properties.Settings.Default.Network &&
                !Properties.Settings.Default.DiskIO) {
#if DEBUG
                Console.WriteLine("Settings indicate there is nothing to monitor");
#endif
                backgroundWorker1.CancelAsync();
                shouldGatherData = false;
            }
            else {
                shouldGatherData = true;
            }
        }

        void Form1_Shown(object sender, EventArgs e) {
            // Start the background worker
#if DEBUG
            Console.WriteLine("Started background worker");
#endif
            backgroundWorker1.RunWorkerAsync();

        }

        private void progressBar1_Click(object sender, EventArgs e) {

        }

        private void gathering_form_Load(object sender, EventArgs e) {


        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            // The progress percentage is a property of e
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = e.ProgressPercentage.ToString() + "%";
        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private bool GatherData(BackgroundWorker sender, DoWorkEventArgs e) {

            double pollingTime = Properties.Settings.Default.PollingTime;
            double pollingInterval = Properties.Settings.Default.PollingInterval;
#if DEBUG
            Console.WriteLine("Polling Time: " + pollingTime);
            Console.WriteLine("Polling Interval: " + pollingInterval);
#endif
            for (double i = pollingInterval; i <= pollingTime; ) {
                
                if (backgroundWorker1.CancellationPending) {
                    Console.WriteLine("Cancellation is pending - killing the loop");
                    e.Cancel = true;
                    return true;
                }
                else {

                    // gather and record CPU utilization
                    if (Properties.Settings.Default.CPU) {
                        int currentCpuPercentage = (int)perfCpuCount.NextValue();
                        cpuUsage.Add(currentCpuPercentage);
#if DEBUG
                        Console.WriteLine("Cpu Load: {0}%", currentCpuPercentage);
#endif
                    }
                    // gather and record available memory
                    if (Properties.Settings.Default.Memory) {
                        int currentMemUsage = (int)perfMemCount.NextValue();
                        memAvailable.Add(currentMemUsage);
#if DEBUG
                        Console.WriteLine("Available Memory: {0}MB", currentMemUsage);
#endif
                    }

                    Thread.Sleep((int)(pollingInterval * 1000));
                    i += pollingInterval;
                    double percentage = (i / pollingTime) * 100;
                    
#if DEBUG
                    Console.WriteLine("Percentage: " + percentage);
#endif
                    if (percentage >= 100 || i >= pollingTime) {
                        backgroundWorker1.CancelAsync();
                        Console.WriteLine("Set cancellation to pending");
                    }
                    else {
                        percentage = Math.Round(percentage);
                        backgroundWorker1.ReportProgress((int)percentage);

                        Console.WriteLine("I {0}", i);
                    }
                }
            }

            backgroundWorker1.CancelAsync();

            if (backgroundWorker1.CancellationPending) {
                Console.WriteLine("Cancellation is pending");
                e.Cancel = true;
                return true;
            }

            // will be used later to monitor specific processes
            //Process[] processes = Process.GetProcesses();
            return true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;
            e.Result = GatherData(worker, e);
        }

        private void backgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e) {
#if DEBUG
            Console.WriteLine("Worker completed");
#endif
            if (e.Error != null) {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled) {
                button1.Enabled = true;
            }
            else {
                button1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
#if DEBUG
            Console.WriteLine("Windows closed using OK button");
#endif
            try {
                using (CsvFileWriter writer = new CsvFileWriter("DataOutput.csv")) {
                    for (int i = 0; i < cpuUsage.Count; i++) {
                        CsvRow row = new CsvRow();
                        row.Add(String.Format("CPU"));
                        row.Add(String.Format("{0}", cpuUsage[i]));
                        writer.WriteRow(row);
                    }

                    for (int i = 0; i < memAvailable.Count; i++) {
                        CsvRow row = new CsvRow();
                        row.Add("Memory");
                        row.Add(String.Format("{0}", memAvailable[i]));
                        writer.WriteRow(row);
                    }
                }
            }
            catch {
                Console.Error.WriteLine("Could not read data from file");
            }

            this.Close();
        }

        private void label2_Click(object sender, EventArgs e) {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using System.Diagnostics;
using System.Timers;

namespace Check_Up {
    public partial class gathering_form : Form {

        #region Performance Counters
        // create perf mon objects
        private PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
        private PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
        //private PerformanceCounter perfNetCount = new PerformanceCounter("Network Interface", "Bytes Total/sec");
        //private PerformanceCounter perfDiskCount = new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total");
        #endregion

        public gathering_form() {
            InitializeComponent();
            Shown += new EventHandler(Form1_Shown);

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

            perfCpuCount.NextValue();
            perfMemCount.NextValue();

            backgroundWorker1.DoWork += new DoWorkEventHandler(TimerGather);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

        }

        void Form1_Shown(object sender, EventArgs e) {
            // Start the background worker
            Console.WriteLine("Started background worker");
            backgroundWorker1.RunWorkerAsync();
        }

        private void progressBar1_Click(object sender, EventArgs e) {

        }

        private void gathering_form_Load(object sender, EventArgs e) {
            
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            // The progress percentage is a property of e
            progressBar1.Value = e.ProgressPercentage;
        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void TimerGather(object sender, DoWorkEventArgs e) {
            double pollingTime = Properties.Settings.Default.PollingTime;
            double pollingInterval = Properties.Settings.Default.PollingInterval;

            Console.WriteLine("Polling Time: " + pollingTime);
            Console.WriteLine("Polling Interval: " + pollingInterval);

            for (double i = 0f; i < pollingTime; ) {
                if (backgroundWorker1.CancellationPending) {
                    e.Cancel = true;
                    break;
                }
                else {
                    GatherData();
                    Thread.Sleep((int)pollingInterval * 1000);
                    double percentage = (pollingInterval / pollingTime) * 100f * i / pollingTime * 100;
                    Console.WriteLine("Percentage: " + percentage);

                    if (percentage >= 100) {
                        backgroundWorker1.CancelAsync();
                    }

                    backgroundWorker1.ReportProgress((int)percentage);

                    i += pollingInterval;
                }
            }
        }

        private void GatherData() {

            // get performance values
            int currentCpuPercentage = (int)perfCpuCount.NextValue();
            int currentMemUsage = (int)perfMemCount.NextValue();

            Process[] processes = Process.GetProcesses();

            // prints cpu load in percentage
            Console.WriteLine("Cpu Load: {0}%", currentCpuPercentage);
            Console.WriteLine("Available Memory: {0}MB", currentMemUsage);

        }
    }
}

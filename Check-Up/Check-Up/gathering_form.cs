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

        // currently cannot use these two performance metrics - dunno why not...
        //private PerformanceCounter perfNetCount = new PerformanceCounter("Network Interface", "Bytes Total/sec");
        //private PerformanceCounter perfDiskCount = new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total");
        #endregion

        public List<int> memAvailable = new List<int>();
        public List<int> cpuUsage = new List<int>();

        public bool shouldGatherData;

        public gathering_form() {
            InitializeComponent();
            Shown += new EventHandler(Form1_Shown);

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

            perfCpuCount.NextValue();
            perfMemCount.NextValue();

            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

            if (shouldGatherData) {
                backgroundWorker1.DoWork += new DoWorkEventHandler(TimerGather);
            }
            else {
                backgroundWorker1.ReportProgress(100);
            }
        }

        void Form1_Shown(object sender, EventArgs e) {
            // Start the background worker
            Console.WriteLine("Started background worker");
            backgroundWorker1.RunWorkerAsync();
        }

        private void progressBar1_Click(object sender, EventArgs e) {

        }

        private void gathering_form_Load(object sender, EventArgs e) {
            if (!Properties.Settings.Default.CPU ||
                !Properties.Settings.Default.Memory ||
                !Properties.Settings.Default.Network ||
                !Properties.Settings.Default.DiskIO) {

                    Console.WriteLine("Settings indicate there is nothing to monitor");
                    backgroundWorker1.CancelAsync();
                    shouldGatherData = false;
            }
            else {
                shouldGatherData = true;
            }
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

            // gather and record CPU utilization
            if (Properties.Settings.Default.CPU) {
                int currentCpuPercentage = (int)perfCpuCount.NextValue();
                cpuUsage.Add(currentCpuPercentage);
                Console.WriteLine("Cpu Load: {0}%", currentCpuPercentage);
            }

            // gather and record available memory
            if (Properties.Settings.Default.Memory) {
                int currentMemUsage = (int)perfMemCount.NextValue();
                Console.WriteLine("Available Memory: {0}MB", currentMemUsage);
                memAvailable.Add(currentMemUsage);
            }

            // will be used later to monitor specific processes
            //Process[] processes = Process.GetProcesses();

        }
    }
}

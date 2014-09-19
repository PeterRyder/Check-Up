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
            dataCollector = new DataCollection();
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        // OK Button
        private void button2_Click(object sender, EventArgs e) {
            this.gatherData.Enabled = false;
            this.cycles = 1;
            monitorStop.Enabled = true;

            dataCollector.ReadSettings();

            backgroundWorker1.RunWorkerAsync();

            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

            if (dataCollector.shouldGatherData) {
                backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            }
            else {
                backgroundWorker1.CancelAsync();
                backgroundWorker1.ReportProgress(100);
            }
        }

        // Cancel Button
        private void deny_Click(object sender, EventArgs e) {
            Application.Exit();

        }

        private void confirm_MouseHover(object sender, EventArgs e) {

        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            PropertiesForm subForm = new PropertiesForm();
            subForm.Show();

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void aboutCheckUpToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutForm subForm = new AboutForm();
            subForm.Show();
        }


        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = e.ProgressPercentage.ToString() + "%";
            if (Properties.Settings.Default.CPU) {
                updateGraph("CPU", "" + cycles, "" + dataCollector.currentCPUUsage);
            }

            if (Properties.Settings.Default.Memory) {
                updateGraph("Memory", "" + cycles, "" + dataCollector.currentMemUsage);
            }

            if (Properties.Settings.Default.Network && dataCollector.canGatherNet) {
                updateGraph("Network", "" + cycles, "" + dataCollector.currentNetUsageMBs);
            }

            if (Properties.Settings.Default.DiskIO) {
                updateGraph("Disk", "" + cycles, "" + dataCollector.percentDiskTime);
            }

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

            }
            else {

            }

            gatherData.Enabled = true;
            monitorStop.Enabled = false;

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
#if DEBUG
                    Console.WriteLine("Cancellation is pending - killing the loop");
#endif
                    e.Cancel = true;
                    return true;
                }

                dataCollector.GatherData();

                if (!Properties.Settings.Default.IgnoreTime) {

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
                        percentage = Math.Round(percentage);
                        backgroundWorker1.ReportProgress((int)percentage);
#if DEBUG
                        Console.WriteLine("I {0}", i);
#endif
                    }
                    i += pollingInterval;
                }
                else {
                    backgroundWorker1.ReportProgress(0);
                }

                Thread.Sleep((int)(pollingInterval * 1000));
                cycles++;
            }

            backgroundWorker1.CancelAsync();

            if (backgroundWorker1.CancellationPending) {
#if DEBUG
                Console.WriteLine("Cancellation is pending");
#endif
                e.Cancel = true;
                return true;
            }

            return true;
        }

        private void updateGraph(string type, string x, string y) {

            //resetChartFunc();

            if (this.chart1.Series.IndexOf(type) != -1) {
#if DEBUG
                //Console.WriteLine("Chart already has {0} in the series", type);
#endif
            }
            else {
                this.chart1.Series.Add(type);
            }

            try {
                this.chart1.Series[type].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            }
            catch {
                Console.WriteLine("Couldn't change {0} into line graph", type);
            }
            try {
                this.chart1.Series[type].Points.AddXY(x, y);
                chart1.Series[type].ToolTip = "X: #VALX, Y: #VALY";
            }
            catch {
                Console.WriteLine("Couldn't create point on graph X: {0}, Y: {1}", x, y);
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e) {

        }

        private void MainWindow_Load(object sender, EventArgs e) {

        }

        private void monitorStop_Click(object sender, EventArgs e) {
            backgroundWorker1.CancelAsync();
            backgroundWorker1.ReportProgress(100);
            gatherData.Enabled = true;
        }

        private void resetChart_Click(object sender, EventArgs e) {
            resetChartFunc();
        }

        private void analyzeProcesses_Click(object sender, EventArgs e) {
            Console.WriteLine("test");
        }

        private void resetChartFunc() {
            foreach (var series in chart1.Series) {
                series.Points.Clear();
            }
        }
    }
}

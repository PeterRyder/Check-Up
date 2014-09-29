using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Check_Up.Util;
using System.Threading;

namespace Check_Up {
    public partial class ProcessListForm : Form {

        // 
        ProcessesDataCollection processDataCollector;

        public ProcessListForm() {
            InitializeComponent();
            processDataCollector = new ProcessesDataCollection();
            initializeListView();

            // Add the ProgressChanged function to the ProgressChangedEventHandler
            this.backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

            this.backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);

            // Begin the backgroundWorker
            this.backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;
            // GatherData every time the background worker does work
            e.Result = GatherData(worker, e);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (worker.CancellationPending) {
#if DEBUG
                Console.WriteLine("Cancellation Pending - Cancelling");
#endif
                return;
            }            

            foreach (ProcessMonitor proc in processDataCollector.procMonitors) {
                if (proc.getPrevCpuUsage() != proc.getCpuUsage()) {
                    try {
                        ListViewItem item = listView1.FindItemWithText(proc.getName(), false, 0, false);
                        item.SubItems[1].Text = Math.Round(proc.getCpuUsage()) + "%";
                    }
                    catch {
#if DEBUG
                        Console.WriteLine("Couldn't update ListViewItem {0}", proc.getName());
#endif    
                    }
                }
            }
        }

        private void initializeListView() {
            foreach (ProcessMonitor proc in processDataCollector.procMonitors) {
                listView1.Items.Add(new ListViewItem(new string[] { proc.getName(), proc.getCpuUsage() + "%" }));
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
#if DEBUG
            Console.WriteLine("Worker Completed");
#endif
        }

        private bool GatherData(BackgroundWorker sender, DoWorkEventArgs e) {
            
            for (int i = 0; i < 100; i++) {
                if (backgroundWorker1.CancellationPending) {
#if DEBUG
                    Console.WriteLine("Background Worker Cancelled");
#endif
                    backgroundWorker1.ReportProgress(100);
                    e.Cancel = true;
                    return true;
                }

                processDataCollector.GatherData();

                backgroundWorker1.ReportProgress(0);
                Thread.Sleep(1000);
            }

            backgroundWorker1.CancelAsync();

            if (backgroundWorker1.CancellationPending) {
#if DEBUG
                Console.WriteLine("Cancellation is pending");
#endif
                e.Cancel = true;
            }
            return true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            backgroundWorker1.CancelAsync();
            base.OnFormClosing(e);
        }
    }
}

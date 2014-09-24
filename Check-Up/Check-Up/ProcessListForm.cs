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

        ProcessesDataCollection processDataCollector;
        List<ProcessMonitor> procMonitors;

        public ProcessListForm() {
            InitializeComponent();
            processDataCollector = new ProcessesDataCollection();
            procMonitors = processDataCollector.getProcMonitors();
            initializeListView();

            // Begin the backgroundWorker
            backgroundWorker1.RunWorkerAsync();

            // Add the ProgressChanged function to the ProgressChangedEventHandler
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;

            // GatherData every time the background worker does work
            e.Result = GatherData(worker, e);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (worker.CancellationPending) {
                return;
            }

            procMonitors = processDataCollector.getProcMonitors();
            procMonitors.Sort();

            foreach (ProcessMonitor proc in procMonitors) {
#if DEBUG
                //Console.WriteLine("Previous CPU Usage: {0}", proc.getPrevCpuUsage());
                //Console.WriteLine("Current CPU Usage: {0}", proc.getCpuUsage());
#endif

                if (proc.getPrevCpuUsage() != proc.getCpuUsage()) {
                    try {
                        ListViewItem item = listView1.FindItemWithText(proc.getName(), false, 0, false);
                        item.SubItems[1].Text = Math.Round(proc.getCpuUsage()) + "%";
                        
                    }
                    catch {
                        Console.WriteLine("Couldn't update ListViewItem {0}", proc.getName());
                        
                    }
                }
            }
        }

        private void initializeListView() {
            foreach (ProcessMonitor proc in procMonitors) {
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
#if DEBUG
                Console.WriteLine("Gathered Data");
#endif
                backgroundWorker1.ReportProgress(0);
                Thread.Sleep(1000);
            }


            return true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            backgroundWorker1.CancelAsync();
            base.OnFormClosing(e);
        }
    }
}

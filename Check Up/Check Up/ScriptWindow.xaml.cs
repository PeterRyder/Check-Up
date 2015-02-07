using System;
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
using System.Windows.Shapes;
using System.ComponentModel;
using Check_Up.Util;
using System.IO;
using System.Collections.ObjectModel;
using log4net;

namespace Check_Up {
    /// <summary>
    /// Interaction logic for ScriptWindow.xaml
    /// </summary>
    public partial class ScriptWindow : Window {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public class ScriptData {
            public string ScriptName { get; set; }
            public string FullPath { get; set; }
        }

        ObservableCollection<ScriptData> _ScriptCollection = new ObservableCollection<ScriptData>();

        private ScriptControl scripts;

        BackgroundWorker worker;

        public ScriptWindow() {
            InitializeComponent();
            scripts = new ScriptControl();
            
            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            InitializeListView();
        }

        public ObservableCollection<ScriptData> ScriptCollection { get { return _ScriptCollection; } }

        public void InitializeListView() {
            worker.RunWorkerAsync();
        }

        private void StartScript(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            ScriptData scriptData = b.CommandParameter as ScriptData;

            if (scripts.runScript(scriptData.FullPath)) {
                //b.IsEnabled = false;
            }

        }

        private void StopScript(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            ScriptData scriptData = b.CommandParameter as ScriptData;

            if (scripts.stopScript(scriptData.FullPath)) {
                //b.IsEnabled = false;
            }
        }

        private void worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
            string[] files = Directory.GetFiles(scripts.fullScriptPath);
            BackgroundWorker w = sender as BackgroundWorker;

            int i = 1;
            foreach (string filename in files) {
                scripts.checkNewScript(filename);
                float progress = ((float)i / files.Length) * 100;
                w.ReportProgress((int)progress);
                i++;
            }
        }

        private void worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e) {
            Console.WriteLine("Progress Changed " + e.ProgressPercentage);
        }

        private void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
            log.Debug("Script Window Worker completed");
            List<string> scriptList = scripts.getScripts();

            foreach (string script in scriptList) {
                _ScriptCollection.Add(new ScriptData {
                    ScriptName = System.IO.Path.GetFileName(script),
                    FullPath = script
                });
            }
        }
    }
}

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

namespace Check_Up {
    /// <summary>
    /// Interaction logic for ScriptWindow.xaml
    /// </summary>
    public partial class ScriptWindow : Window {

        public class ScriptData {
            public string ScriptName { get; set; }
            public string FullPath { get; set; }
        }

        ObservableCollection<ScriptData> _ScriptCollection = new ObservableCollection<ScriptData>();

        private ScriptControl scripts;

        private BackgroundWorker worker;

        public ScriptWindow() {
            InitializeComponent();
            scripts = new ScriptControl();

            InitializeWorker();
            InitializeListView();
            Console.WriteLine("Script Window constructor finished");
        }

        private void InitializeWorker() {
            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
        }

        public void InitializeListView() {
            worker.RunWorkerAsync();
        }

        public ObservableCollection<ScriptData> ScriptCollection { get { return _ScriptCollection; } }

        private void StartScript(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            ScriptData scriptData = b.CommandParameter as ScriptData;

            if (scripts.RunScript(scriptData.FullPath)) {
                //b.IsEnabled = false;
            }

        }

        private void StopScript(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            ScriptData scriptData = b.CommandParameter as ScriptData;

            if (scripts.StopScript(scriptData.FullPath)) {
                //b.IsEnabled = false;
            }
        }

        private void worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
            string[] files = Directory.GetFiles(ScriptControl.fullScriptPath);
            BackgroundWorker w = sender as BackgroundWorker;

            int i = 1;
            foreach (string filename in files) {
                scripts.CheckNewScript(filename);
                float progress = ((float)i / files.Length) * 100;
                w.ReportProgress((int)progress);
                i++;
            }
        }

        private void worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e) {
            //Console.WriteLine("Progress Changed " + e.ProgressPercentage);
        }

        private void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
            Logger.Info("Script Window Worker completed");
            List<string> scriptList = scripts.GetScripts();

            foreach (string script in scriptList) {
                _ScriptCollection.Add(new ScriptData {
                    ScriptName = System.IO.Path.GetFileName(script),
                    FullPath = script
                });
            }

            Console.WriteLine("Background worker completed");
        }
    }
}

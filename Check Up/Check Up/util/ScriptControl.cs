using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using log4net;

namespace Check_Up.Util {
    class ScriptControl {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<string> scripts = new List<string>();
        private Dictionary<String, dynamic> runningScripts = new Dictionary<String, dynamic>();

        private Dictionary<String, BackgroundWorker> workers = new Dictionary<String, BackgroundWorker>();

        // relative path
        private string scriptPath = "scripts";

        private string fullScriptPath;

        ScriptRuntime ipy = Python.CreateRuntime();
       

        public ScriptControl() {
            fullScriptPath = Path.GetFullPath(scriptPath);
            checkNewScripts();
        }

        public void checkDirectory() {
            if (!Directory.Exists(fullScriptPath)) {
                Directory.CreateDirectory(fullScriptPath);
            }
        }

        public void checkNewScripts() {
            string[] files = Directory.GetFiles(fullScriptPath);

            foreach (string filename in files) {

                if (!scripts.Contains(filename)) {
                    log.Debug(String.Format("Found new script {0}", Path.GetFileName(filename)));
                    if (SanityCheckScript(Path.GetFileName(filename))) {
                        scripts.Add(filename);
                    }
                }
            }
        }

        private bool SanityCheckScript(String filename) {
            if (Path.GetExtension(filename) != ".py") {
                log.Error(String.Format("Script must be a Python file. Not executing file {0}", filename));
                return false;
            }
            else {
                return true;
            }
        }

        public bool runScript(String scriptName) {

            if (!scripts.Contains(scriptName)) {
                Console.WriteLine(" Couldn't find script {0} to start", scriptName);
                return false;
            }

            if (workers.Keys.Contains(scriptName)) {
                Console.WriteLine("Script {0} is already running", scriptName);
                return false;
            }

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker.WorkerSupportsCancellation = true;

            backgroundWorker.RunWorkerAsync(scriptName);
            workers.Add(scriptName, backgroundWorker);

            Console.WriteLine("Started Script {0}", scriptName);

            return true;
        }

        public bool stopScript(String scriptName) {
            if (!runningScripts.Keys.Contains(scriptName)) {
                Console.WriteLine("Couldn't find script {0} to close", scriptName);
                return false;
            }

            foreach (KeyValuePair<String, dynamic> pair in runningScripts) {
                if (pair.Key.Equals(scriptName)) {
                    pair.Value.close();
                }
            }

            string toRemove = "";
            foreach (KeyValuePair<String, BackgroundWorker> pair in workers) {
                if (pair.Key.Equals(scriptName)) {
                    pair.Value.CancelAsync();
                    toRemove = pair.Key;
                }
            }

            if (toRemove != "") {
                Console.WriteLine("Removing worker for script {0}", scriptName);
                if (workers.Keys.Contains(toRemove)) {
                    workers.Remove(toRemove);
                }
                else {
                    Console.WriteLine("Workers does not contain {0}", toRemove);
                }

                if (runningScripts.Keys.Contains(toRemove)) {
                    runningScripts.Remove(toRemove);
                }
                else {
                    Console.WriteLine("Running Scripts does not contain {0}", toRemove);
                }
                
            }
            else {
                Console.WriteLine("Couldn't find worker to remove");
            }

            return true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            string filename = (string)e.Argument;

            dynamic script = ipy.UseFile(filename);
            Console.WriteLine("Starting script {0}", filename);
            runningScripts.Add(filename, script);
            script.main();
            e.Cancel = true;
            return;
        }

        public List<string> getScripts() {
            return scripts;
        }

        public Dictionary<String, dynamic> getRunningScripts() {
            return runningScripts;
        }
    }
}

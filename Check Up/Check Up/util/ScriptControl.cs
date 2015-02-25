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

namespace Check_Up.Util {
    public class ScriptControl {

        private List<string> scripts = new List<string>();
        private Dictionary<String, dynamic> runningScripts = new Dictionary<String, dynamic>();

        private Dictionary<String, BackgroundWorker> workers = new Dictionary<String, BackgroundWorker>();

        public static string fullScriptPath = RandomInfo.roamingDir + "\\" + RandomInfo.scriptDir;
        ScriptRuntime ipy = Python.CreateRuntime();

        public ScriptControl() {
            fullScriptPath = Path.GetFullPath(fullScriptPath);
            CheckDirectory();
        }

        public void CheckDirectory() {
            if (!Directory.Exists(fullScriptPath)) {
                Directory.CreateDirectory(fullScriptPath);
            }
        }

        public void CheckNewScript(string filename) {
            if (!scripts.Contains(filename)) {
                Logger.Debug(String.Format("Found new script {0}", Path.GetFileName(filename)));
                if (SanityCheckScript(filename)) {
                    scripts.Add(filename);
                }
            }
        }

        internal bool SanityCheckScript(String filename) {
            if (Path.GetExtension(Path.GetFileName(filename)) != ".py") {
                Logger.Error(String.Format("Script must be a Python file. Not executing file {0}", filename));
                return false;
            }
            else {
                if (CheckFunctions(filename)) {
                    Logger.Info(string.Format("Script {0} has both a main and close method", Path.GetFileName(filename)));
                }
                else {
                    Logger.Error(string.Format("Script {0} does not contain a close or main method", Path.GetFileName(filename)));
                }
                return true;
            }
        }

        internal bool CheckFunctions(String filename) {
            string line;
            System.IO.StreamReader file = null;

            try {
                file = new System.IO.StreamReader(filename);
            }
            catch {
                Logger.Error(string.Format("Couldn't open file {0}", Path.GetFileName(filename)));
                return false;
            }

            bool foundMain = false;
            bool foundClose = false;

            while ((line = file.ReadLine()) != null) {
                if (line == "def main():") {
                    foundMain = true;
                }

                if (line == "def close():") {
                    foundClose = true;
                }
            }

            // close the file
            file.Close();

            if (foundMain && foundClose) {
                return true;
            }
            else {
                return false;
            }
        }

        public bool RunScript(String scriptName) {

            if (!scripts.Contains(scriptName)) {
                Logger.Error(string.Format("Couldn't find script {0} to start", Path.GetFileName(scriptName)));
                return false;
            }

            if (workers.Keys.Contains(scriptName)) {
                Logger.Warn(string.Format("Script {0} is already running", Path.GetFileName(scriptName)));
                return false;
            }

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            backgroundWorker.WorkerSupportsCancellation = true;

            backgroundWorker.RunWorkerAsync(scriptName);
            workers.Add(scriptName, backgroundWorker);

            Logger.Info(string.Format("Started BackgroundWorker for script {0}", Path.GetFileName(scriptName)));

            return true;
        }

        public bool StopScript(String scriptName) {
            if (!runningScripts.Keys.Contains(scriptName)) {
                Logger.Error(string.Format("Couldn't find script {0} to close", scriptName));
                return false;
            }

            foreach (KeyValuePair<String, dynamic> pair in runningScripts) {
                if (pair.Key.Equals(scriptName)) {
                    try {
                        // call the close method within the script to end the loop
                        pair.Value.close();
                    }
                    catch {
                        Logger.Error(string.Format("Couldn't find close method in {0} script", scriptName));
                    }
                }
            }

            string toRemove = "";
            foreach (KeyValuePair<String, BackgroundWorker> pair in workers) {
                if (pair.Key.Equals(scriptName)) {
                    pair.Value.CancelAsync();
                    // store the name of the script to remove
                    toRemove = pair.Key;
                }
            }

            if (toRemove != "") {
                Logger.Debug(string.Format("Removing worker for script {0}", Path.GetFileName(scriptName)));
                if (workers.Keys.Contains(toRemove)) {
                    // remove the worker from the list of workers
                    workers.Remove(toRemove);
                }
                else {
                    Logger.Error(string.Format("Workers does not contain {0}", toRemove));
                }

                if (runningScripts.Keys.Contains(toRemove)) {
                    // remove the script from running scri[ts
                    runningScripts.Remove(toRemove);
                }
                else {
                    Logger.Error(string.Format("Running Scripts does not contain {0}", toRemove));
                }

            }
            else {
                Logger.Error("Couldn't find worker to remove");
            }

            return true;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            string filename = (string)e.Argument;

            dynamic script = ipy.UseFile(filename);
            Logger.Info(string.Format("BackgroundWorker starting script {0}", Path.GetFileName(filename)));
            runningScripts.Add(filename, script);

            // call the scripts main method
            script.main();
            e.Cancel = true;
            return;
        }

        public List<string> GetScripts() {
            return scripts;
        }

        public Dictionary<String, dynamic> GetRunningScripts() {
            return runningScripts;
        }
    }
}

﻿using System;
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
    class Scripts {

        private List<string> scripts = new List<string>();
        private List<BackgroundWorker> workers = new List<BackgroundWorker>();

        private string scriptPath = "scripts";
        private string fullScriptPath;

        ScriptRuntime ipy = Python.CreateRuntime();

        public Scripts() {
            fullScriptPath = Path.GetFullPath(scriptPath);
        }

        public void checkDirectory() {
            if (!Directory.Exists(fullScriptPath)) {
                Directory.CreateDirectory(fullScriptPath);
            }
            else {
            }
        }

        public void runScripts() {
            checkNewScripts();
        }

        public void checkNewScripts() {
            string[] files = Directory.GetFiles(fullScriptPath);

            foreach (string filename in files) {

                if (!scripts.Contains(filename)) {
#if DEBUG
                    Console.WriteLine("Found new script {0}", Path.GetFileName(filename));
#endif
                    if (SanityCheckScript(Path.GetFileName(filename))) {
                        BackgroundWorker backgroundWorker = new BackgroundWorker();
                        backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);

                        backgroundWorker.RunWorkerAsync(filename);
                        workers.Add(backgroundWorker);
                        scripts.Add(filename);
                    }

                }
                else {
#if DEBUG
                    Console.WriteLine("Script {0} already running", Path.GetFileName(filename));
#endif
                }
            }
        }

        private bool SanityCheckScript(String filename) {
            if (Path.GetExtension(filename) != ".py") {
#if DEBUG
                Console.WriteLine("Script must be a Python file! Not executing file {0}", filename);
#endif
                return false;
            }
            else {
                return true;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            string filename = (string)e.Argument;

            dynamic test = ipy.UseFile(filename);
            scripts.Add(filename);
            test.main();
        }
    }
}

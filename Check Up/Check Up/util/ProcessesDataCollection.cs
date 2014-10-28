﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using log4net;

namespace Check_Up.Util {
    class ProcessesDataCollection {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<ProcessMonitor> procMonitors = new List<ProcessMonitor>();
        public List<ProcessMonitor> procMonitorsToRemove = new List<ProcessMonitor>();

        public Dictionary<string, Dictionary<string, List<Double>>> RawData = new Dictionary<string, Dictionary<string, List<Double>>>();
        public Dictionary<string, Dictionary<string, double>> CalculatedData = new Dictionary<string, Dictionary<string, double>>();
        
        public ProcessesDataCollection() {
            foreach (Process proc in Process.GetProcesses()) {
                ProcessMonitor procMonitor = new ProcessMonitor(proc.ProcessName);
                procMonitors.Add(procMonitor);
            }
            procMonitors.Sort();
        }

        public void GatherData() {
            Console.WriteLine("Gathering Data in Processes");
            foreach (ProcessMonitor proc in procMonitors) {
                try {
                    proc.GatherData();
                    if (!RawData.Keys.Contains(proc.Name)) {
                        RawData[proc.Name]["cpu"].Add(proc.cpuUsage);
                        RawData[proc.Name]["mem"].Add(proc.privateWorkingSet);
                    }
                }
                catch {
                    log.Debug(String.Format("Removing process {0}", proc.Name));
                    procMonitorsToRemove.Add(proc);
                }
            }

            foreach (ProcessMonitor proc in procMonitorsToRemove) {
                procMonitors.Remove(proc);
                ///procMonitorsToRemove.Remove(proc);
            }
            procMonitorsToRemove.Clear();
        }

        public void AnalyzeData() {
            foreach (KeyValuePair<string, Dictionary<string, List<Double>>> pairs in RawData) {
                for (int i = 0; i < pairs.Value.Count; i++) {
                    if (Properties.Settings.Default.ProcAvg) {
                        CalculatedData[pairs.Key]["avg"] = (CalculatedData[pairs.Key]["avg"] + pairs.Value[i]) / 2;
                    }
                    if (Properties.Settings.Default.ProcMax) {
                        if (pairs.Value[i] > CalculatedData[pairs.Key]["max"]) {
                            CalculatedData[pairs.Key]["max"] = pairs.Value[i];

                        }
                    }
                    if (Properties.Settings.Default.ProcMin) {
                        if (pairs.Value[i] < CalculatedData[pairs.Key]["min"]) {
                            CalculatedData[pairs.Key]["min"] = pairs.Value[i];
                        }
                    }
                }
            }

            RawData.Clear();
        }

        public void PrintCalculatedData() {
            foreach (KeyValuePair<string, Dictionary<string, double>> pairs in CalculatedData) {
                Console.WriteLine("{0}: ", pairs.Key);
                foreach (KeyValuePair<string, double> pairs1 in pairs.Value) {
                    Console.WriteLine("    {0}, {1}", pairs1.Key, pairs1.Value);
                }
            }
        }

        public void PrintRawData() {
            foreach (KeyValuePair<string, List<Double>> pairs in RawData) {
                Console.WriteLine("{0} {1}", pairs.Key, pairs.Value);
            }
        }

    }
}

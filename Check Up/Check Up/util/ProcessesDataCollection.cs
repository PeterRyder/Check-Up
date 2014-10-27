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

        public ProcessesDataCollection() {
            foreach (Process proc in Process.GetProcesses()) {
                ProcessMonitor procMonitor = new ProcessMonitor(proc.ProcessName);
                procMonitors.Add(procMonitor);
            }
            procMonitors.Sort();
        }

        public void GatherData() {
            foreach (ProcessMonitor proc in procMonitors) {
                try {
                    proc.GatherData();
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
    }
}

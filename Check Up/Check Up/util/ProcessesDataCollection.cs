﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Check_Up.Util {
    class ProcessesDataCollection {

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
#if DEBUG
                    Console.WriteLine("Could not gather data for process {0}", proc.Name);
                    Console.WriteLine("Removing process {0}", proc.Name);
#endif
                    procMonitorsToRemove.Add(proc);
                }
            }

            foreach (ProcessMonitor proc in procMonitorsToRemove) {
                procMonitors.Remove(proc);
                procMonitorsToRemove.Remove(proc);
            }
        }
    }
}

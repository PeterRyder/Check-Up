using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Check_Up.Util {
    class ProcessesDataCollection {

        public List<ProcessMonitor> procMonitors = new List<ProcessMonitor>();

        public ProcessesDataCollection() {
            foreach (Process proc in Process.GetProcesses()) {
                //Console.WriteLine("Initializing {0}", proc.ProcessName);
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
                    Console.WriteLine("Could not gather data for process {0}", proc.getName());
                }
                
            }
        }
    }
}

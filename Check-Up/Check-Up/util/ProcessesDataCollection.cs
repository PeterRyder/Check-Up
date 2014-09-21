using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Check_Up.Util {
    class ProcessesDataCollection {

        public Process[] RunningProcesses;
        List<ProcessMonitor> procMonitors = new List<ProcessMonitor>();

        public ProcessesDataCollection() {
            foreach (Process proc in Process.GetProcesses()) {
                ProcessMonitor procMonitor = new ProcessMonitor(proc.ProcessName);
                procMonitors.Add(procMonitor);
            }
        }

        private void getProcesses() {
            RunningProcesses = Process.GetProcesses();
        }

        public void GatherData() {
            foreach (ProcessMonitor counter in procMonitors) {
                counter.GatherData();
                float cpuUsage = counter.getCpuUsage();

                if (counter.getCpuUsage() != 0) {
                    Console.WriteLine("Process {0} {1}% CPU Usage", counter.getName(), counter.getCpuUsage());
                }
            }
        }
    }
}

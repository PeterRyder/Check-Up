using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Check_Up.Util {
    class ProcessMonitor : IComparable<ProcessMonitor> {

        public string Name { get; set; }

        public float CPUUsage { get; set; }
        public float PrevCpuUsage { get; set; }

        PerformanceCounter ProcessCPUUsage;

        public ProcessMonitor(string name) {
            this.Name = name;
            try {
                ProcessCPUUsage = new PerformanceCounter("Process", "% Processor Time", name);
                ProcessCPUUsage.NextValue();
            }
            catch {
#if DEBUG
                Console.WriteLine("Could not initialize procMonitor for process {0}", name);
#endif
            }

        }

        public void GatherData() {
            this.PrevCpuUsage = this.CPUUsage;
            this.CPUUsage = ProcessCPUUsage.NextValue() / RandomInfo.logicalCpuCount;
        }

        public override string ToString() {
            return Name;
        }

        public int CompareTo(ProcessMonitor process) {
            // A null value means that this object is greater. 
            if (process == null)
                return 1;

            else
                return this.Name.CompareTo(process.Name);
        }
    }
}

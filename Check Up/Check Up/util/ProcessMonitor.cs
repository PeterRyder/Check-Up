using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Check_Up.Util {
    class ProcessMonitor : IComparable<ProcessMonitor> {

        public string Name { get; set; }

        long peakPagedMem;
        long peakWorkingSet;

        long peakVirtualMem;
        long privateWorkingSet;

        float cpuUsage;

        float prevCpuUsage;
        long prevPrivateWorkingSet;

        PerformanceCounter ProcessCPUUsage;
        PerformanceCounter ProcessMemUsage;

        public ProcessMonitor(string name) {
            this.Name = name;
            try {
                ProcessCPUUsage = new PerformanceCounter("Process", "% Processor Time", name);
                ProcessCPUUsage.NextValue();
                ProcessMemUsage = new PerformanceCounter("Process", "Working Set - Private", name);
                ProcessMemUsage.NextValue();
            }
            catch {
#if DEBUG
                Console.WriteLine("Could not initialize procMonitor for process {0}", name);
#endif
            }

        }

        public void GatherData() {
            this.prevCpuUsage = this.cpuUsage;
            this.cpuUsage = ProcessCPUUsage.NextValue() / RandomInfo.logicalCpuCount;
            this.prevPrivateWorkingSet = this.privateWorkingSet;
            this.privateWorkingSet = Convert.ToInt64(ProcessMemUsage.NextValue());
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

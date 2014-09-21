using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Check_Up.Util {
    class ProcessMonitor : IComparable<ProcessMonitor> {

        string name = "";
        private long peakPagedMem = 0,
                     peakWorkingSet = 0,
                     peakVirtualMem = 0;
        float cpuUsage = 0;
        PerformanceCounter ProcessCPUUsage;

        public ProcessMonitor(string name) {
            this.name = name;
            ProcessCPUUsage = new PerformanceCounter("Process", "% Processor Time", name);
            ProcessCPUUsage.NextValue();
        }

        public void GatherData() {
            try {
                cpuUsage = ProcessCPUUsage.NextValue() / 4;
            }
            catch {
                Console.WriteLine("ERROR: Could not gather CPU data for process {0} ", this.name);
            }
            
        }

        public override string ToString() {
            return name;
        }

        public int CompareTo(ProcessMonitor process) {
            // A null value means that this object is greater. 
            if (process == null)
                return 1;

            else
                return this.name.CompareTo(process.name);
        }

        #region Set Functions
        public void setPeakPagedMem(long val) {
            this.peakPagedMem = val;
        }

        public void setPeakWorkingSet(long val) {
            this.peakWorkingSet = val;
        }

        public void setPeakVirtualMem(long val) {
            this.peakVirtualMem = val;
        }

        public void setCpuUsage(long val) {
            this.cpuUsage = val;
        }
        #endregion


        #region Get Functions
        public long getPeakPagedMem() {
            return this.peakPagedMem;
        }

        public long getPeakWorkingSet() {
            return this.peakWorkingSet;
        }

        public long getPeakVirtualMem() {
            return this.peakVirtualMem;
        }

        public float getCpuUsage() {
            return this.cpuUsage;
        }

        public string getName() {
            return this.name;
        }
        #endregion
    }
}

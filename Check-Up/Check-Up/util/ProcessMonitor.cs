﻿using System;
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
        float prevCpuUsage = -1;
        PerformanceCounter ProcessCPUUsage;

        public ProcessMonitor(string name) {
            this.name = name;
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
            this.prevCpuUsage = this.cpuUsage;
            this.cpuUsage = ProcessCPUUsage.NextValue() / RandomInfo.logicalCpuCount;
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

        public float getPrevCpuUsage() {
            return this.prevCpuUsage;
        }

        public string getName() {
            return this.name;
        }
        #endregion
    }
}
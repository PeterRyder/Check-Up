using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using log4net;

namespace Check_Up.Util {
    class ProcessMonitor : IComparable<ProcessMonitor>, IDisposable {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                log.Error(String.Format("Could not initialize procMonitor for process {0}", name));
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

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing) {
            if (disposing) {
                ProcessCPUUsage.Dispose();
            }
        }
    }
}

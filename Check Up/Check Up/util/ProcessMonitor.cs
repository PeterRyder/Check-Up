using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Check_Up.Util {
    static class ProcessMonitor {

        public static PerformanceCounter GetPerfCounterForProcessId(int processId, string processCounterName) {
            string instance = GetInstanceNameForProcessId(processId);
            if (string.IsNullOrEmpty(instance))
                return null;

            return new PerformanceCounter("Process", processCounterName, instance);
        }

        public static string GetInstanceNameForProcessId(int processId) {
            Process process;
            try {
                process = Process.GetProcessById(processId);
            }
            catch {
                Logger.Error(string.Format("Could not get process {0} by ID", processId));
                return null;
            }
            string processName = Path.GetFileNameWithoutExtension(process.ProcessName);

            PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");
            string[] instances = cat.GetInstanceNames()
                .Where(inst => inst.StartsWith(processName))
                .ToArray();

            foreach (string instance in instances) {
                using (PerformanceCounter cnt = new PerformanceCounter("Process",
                    "ID Process", instance, true)) {
                    float val = (float)cnt.RawValue;
                    if (val == processId) {
                        return instance;
                    }
                }
            }
            return null;
        }

        public static List<PerformanceCounter> GetPerfCountersOfProcesses(string processCounterName) {
            PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");
            string[] instances = cat.GetInstanceNames().ToArray();
            List<PerformanceCounter> counters = new List<PerformanceCounter>();

            foreach (string instance in instances) {
                PerformanceCounter cnt = new PerformanceCounter("Process", processCounterName, instance, true);
                if (!counters.Contains(cnt)) {
                    counters.Add(cnt);
                }
            }

            return counters;
        }

    }
}

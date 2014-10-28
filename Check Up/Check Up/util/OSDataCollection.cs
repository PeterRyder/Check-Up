using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Net.NetworkInformation;
using log4net;
using Check_Up.Util;

namespace Check_Up.Util {
    class OSDataCollection : IDisposable {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<string, PerformanceCounter> PerfCounters = new Dictionary<string,PerformanceCounter>();

        public Dictionary<string, int> DataValues = new Dictionary<string, int>();

        public double totalMemMBs { get; set; }
        public double availableMemMBs { get; set; }

        public int currentNetUsageBytes { get; set; }

        public bool canGatherNet { get; set; }

        

        public OSDataCollection() {
            
        }

        public void InitializeCounters() {
            #region CPU Counter Initialization
            if (Properties.Settings.Default.CPU && !DataValues.ContainsKey(CounterNames.CPUName)) {
                Console.WriteLine("Initializing a CPU Counter");
                PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
                PerfCounters.Add(CounterNames.CPUName, perfCpuCount);
                DataValues.Add(CounterNames.CPUName, 0);
            }
            #endregion

            #region Memory Counter Initialization
            if (Properties.Settings.Default.Memory && !DataValues.ContainsKey(CounterNames.MemName)) {
                Console.WriteLine("Initializing a Memory Counter");
                ulong totalMemBytes = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
                totalMemMBs = (int)(totalMemBytes / 1024 / 1024);
                PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
                PerfCounters.Add(CounterNames.MemName, perfMemCount);
                DataValues.Add(CounterNames.MemName, 0);
            }
            #endregion

            #region Network Counter Initialization
            if (Properties.Settings.Default.Network && !DataValues.ContainsKey(CounterNames.NetName)) {
                Console.WriteLine("Attempting to Initialize a Network Counter");
                string WifiNicDescription = "";
                string ethernetNicDescription = "";

                // find the wifi network interface
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces()) {
                    if (nic.Name == "Wi-Fi" || nic.Name == "Wireless Network Connection") {
                        WifiNicDescription = nic.Description;
                        WifiNicDescription = WifiNicDescription.Replace("(", "[");
                        WifiNicDescription = WifiNicDescription.Replace(")", "]");
                    }

                    if (nic.Name == "Ethernet" || nic.Name == "Local Area Connection") {
                        ethernetNicDescription = nic.Description;
                        ethernetNicDescription = ethernetNicDescription.Replace("(", "[");
                        ethernetNicDescription = ethernetNicDescription.Replace(")", "]");
                    }
                }
                PerformanceCounter perfNetCount;
                if (WifiNicDescription != "") {
                    try {
                        perfNetCount = new PerformanceCounter("Network Adapter", "Bytes Total/sec", WifiNicDescription);
                        PerfCounters.Add(CounterNames.NetName, perfNetCount);
                        canGatherNet = true;
                    }
                    catch {
                        log.Error("Could not find a counter for your network adapter");

                        canGatherNet = false;
                    }

                    try {
                        perfNetCount = new PerformanceCounter("Network Interface", "Bytes Total/sec", WifiNicDescription);
                        PerfCounters.Add(CounterNames.NetName, perfNetCount);
                        canGatherNet = true;
                    }
                    catch {
                        log.Error("Could not find a counter for your network interface");
                        canGatherNet = false;
                    }
                }
                else {
                    perfNetCount = new PerformanceCounter("Network Adapter", "Bytes Total/sec", ethernetNicDescription);
                    PerfCounters.Add(CounterNames.NetName, perfNetCount);
                }

                DataValues.Add(CounterNames.NetName, 0);
            }
            #endregion

        }

        public List<string> RemoveCounters() {
            List<string> CountersToRemove = new List<string>();
            if (!Properties.Settings.Default.CPU && DataValues.ContainsKey(CounterNames.CPUName)) {
                RemoveCounter(CounterNames.CPUName);
                CountersToRemove.Add(CounterNames.CPUName);
            }

            if (!Properties.Settings.Default.Memory && DataValues.ContainsKey(CounterNames.MemName)) {
                RemoveCounter(CounterNames.MemName);
                CountersToRemove.Add(CounterNames.MemName);
            }

            if (!Properties.Settings.Default.Network && DataValues.ContainsKey(CounterNames.NetName)) {
                RemoveCounter(CounterNames.NetName);
                CountersToRemove.Add(CounterNames.NetName);
            }

            List<string> disks = Properties.Settings.Default.Disks;
            List<string> counters = new List<string>(DataValues.Keys);

            for (int i = 0; i < counters.Count; i++) {
                if (counters[i] != CounterNames.CPUName &&
                    counters[i] != CounterNames.MemName &&
                    counters[i] != CounterNames.NetName &&
                    !disks.Contains(counters[i])) {
                        RemoveCounter(counters[i]);
                        CountersToRemove.Add(counters[i]);
                }
            }
            return CountersToRemove;
        }

        public void RemoveCounter(string CounterType) {
            Console.WriteLine("Attempting to remove counter {0}", CounterType);
            try {
                DataValues.Remove(CounterType);
            }
            catch {
                log.Error(String.Format("Could not remove data {0} from list", CounterType));
            }
            try {
                PerfCounters.Remove(CounterType);
            }
            catch {
                log.Error(String.Format("Could not remove counter {0} from list", CounterType));
            }
        }

        public void AddDiskCounter(string disk) {
            try {
                PerformanceCounter perfDisk = new PerformanceCounter("LogicalDisk", "% Disk Time", disk);
                PerfCounters.Add(disk, perfDisk);
                DataValues.Add(disk, 0);
            }
            catch {
                log.Error(String.Format("Could not create performance counter for disk {0}", disk));
            }
        }

        public bool GatherData(string type) {
            if (type == CounterNames.MemName) {
                availableMemMBs = (int)PerfCounters[type].NextValue();
                DataValues[type] = (int)Math.Round((totalMemMBs - availableMemMBs) / totalMemMBs * 100d);
            }
            else if (type == CounterNames.NetName) {
                if (canGatherNet) {
                    currentNetUsageBytes = (int)PerfCounters[type].NextValue();
                    DataValues[type] = (int)Math.Round(currentNetUsageBytes / 1024d / 1024d, 2);
                }
            }
            else {
                try {
                    DataValues[type] = (int)PerfCounters[type].NextValue();
                }
                catch {
                    System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show(String.Format("Cannot find drive {0} Is it a CD drive? \nNot monitoring drive {0}", type));
                    return false;
                }
            }
            return true;
            
        }

        /// <summary>
        /// Debug function to list all counters on the system
        /// </summary>
        /// <param name="categoryName"></param>
        public void ListCounters(string categoryName) {
            PerformanceCounterCategory category = PerformanceCounterCategory.GetCategories().First(c => c.CategoryName == categoryName);
            Console.WriteLine("{0} [{1}]", category.CategoryName, category.CategoryType);

            string[] instanceNames = category.GetInstanceNames();

            if (instanceNames.Length > 0) {
                // MultiInstance categories
                foreach (string instanceName in instanceNames) {
                    ListInstances(category, instanceName);
                }
            }
            else {
                // SingleInstance categories
                ListInstances(category, string.Empty);
            }
        }

        /// <summary>
        /// Debug function to list all instances of a performanceCounterCategory
        /// </summary>
        /// <param name="category"></param>
        /// <param name="instanceName"></param>
        private static void ListInstances(PerformanceCounterCategory category, string instanceName) {
            Console.WriteLine("    {0}", instanceName);
            PerformanceCounter[] counters = category.GetCounters(instanceName);

            foreach (PerformanceCounter counter in counters) {
                Console.WriteLine("        {0}", counter.CounterName);
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing) {
            if (disposing) {
                foreach (KeyValuePair<string, PerformanceCounter> count in PerfCounters) {
                    count.Value.Dispose();
                }
            }
        }
    }
}

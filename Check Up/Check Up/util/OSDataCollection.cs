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


        // create perf mon objects
        private Dictionary<string, PerformanceCounter> PerfCounters = new Dictionary<string,PerformanceCounter>();

        public Dictionary<string, int> DataValues = new Dictionary<string, int>();

        public double totalMemMBs { get; set; }
        public double availableMemMBs { get; set; }

        public int currentNetUsageBytes { get; set; }

        public List<int> percentDiskTimes { get; set; }

        public bool canGatherNet { get; set; }

        public OSDataCollection() {
            InitializeCounters();
        }

        private void InitializeCounters() {
            #region CPU Counter Initialization
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            PerfCounters.Add(CounterNames.CPUName, perfCpuCount);
            DataValues.Add(CounterNames.CPUName, 0);
            #endregion

            #region Memory Counter Initialization
            ulong totalMemBytes = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
            totalMemMBs = (int)(totalMemBytes / 1024 / 1024);
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
            PerfCounters.Add(CounterNames.MemName, perfMemCount);
            DataValues.Add(CounterNames.MemName, 0);
            #endregion

            #region Network Counter Initialization
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
            #endregion

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

        public void GatherData(string type) {
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
                DataValues[type] = (int)PerfCounters[type].NextValue();
                Console.WriteLine("Disk {0} using {1}", type, DataValues[type]);
            }
            
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

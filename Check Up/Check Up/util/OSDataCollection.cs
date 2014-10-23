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

namespace Check_Up.Util {
    class OSDataCollection : IDisposable{
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Performance Counters
        // create perf mon objects
        private PerformanceCounter perfCpuCount;
        private PerformanceCounter perfMemCount;
        private PerformanceCounter perfNetCount;
        private PerformanceCounter perfDiskCount;
        #endregion

        public int currentCPUUsage { get; set; }

        public double totalMemMBs { get; set; }
        public double availableMemMBs { get; set; }
        public double currentMemUsage { get; set; }

        public int currentNetUsageBytes { get; set; }
        public double currentNetUsageMBs { get; set; }

        public int percentDiskTime { get; set; }

        public bool canGatherNet { get; set; }

        public OSDataCollection() {
            InitializeCounters();
        }

        private void InitializeCounters() {
            #region CPU Counter Initialization
            perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            #endregion

            #region Memory Counter Initialization
            ulong totalMemBytes = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
            totalMemMBs = (int)(totalMemBytes / 1024 / 1024);
            perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
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

            if (WifiNicDescription != "") {
                try {
                    perfNetCount = new PerformanceCounter("Network Adapter", "Bytes Total/sec", WifiNicDescription);
                    canGatherNet = true;
                }
                catch {
                    log.Error("Could not find a counter for your network adapter");

                    canGatherNet = false;
                }

                try {
                    perfNetCount = new PerformanceCounter("Network Interface", "Bytes Total/sec", WifiNicDescription);
                    canGatherNet = true;
                }
                catch {
                    log.Error("Could not find a counter for your network interface");
                    canGatherNet = false;
                }
            }
            else {
                perfNetCount = new PerformanceCounter("Network Adapter", "Bytes Total/sec", ethernetNicDescription);
            }

            #endregion

            #region Disk Counter Initialization
            perfDiskCount = new PerformanceCounter("LogicalDisk", "% Disk Time", "C:");
            #endregion
        }

        public void GatherCPUData() {
            currentCPUUsage = (int)perfCpuCount.NextValue();
        }

        public void GatherMemoryData() {
            availableMemMBs = (int)perfMemCount.NextValue();
            currentMemUsage = Math.Round((totalMemMBs - availableMemMBs) / totalMemMBs * 100d, 2);
        }

        public void GatherNetworkData() {
            if (canGatherNet) {
                currentNetUsageBytes = (int)perfNetCount.NextValue();
                currentNetUsageMBs = Math.Round(currentNetUsageBytes / 1024d / 1024d, 2);
            }
        }

        public void GatherDiskData() {
            percentDiskTime = (int)perfDiskCount.NextValue();
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
                perfCpuCount.Dispose();
                perfMemCount.Dispose();
                perfNetCount.Dispose();
                perfDiskCount.Dispose();
            }
        }
    }
}

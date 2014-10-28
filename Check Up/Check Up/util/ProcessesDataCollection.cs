﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using log4net;

namespace Check_Up.Util {
    class ProcessesDataCollection {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Dictionary<string, float> HighestCpuUsage = new Dictionary<string, float>();
        public Dictionary<string, float> HighestMemUsage = new Dictionary<string, float>();
        
        List<PerformanceCounter> perfCounters = new List<PerformanceCounter>();

        public ProcessesDataCollection() {
            
            foreach (var p in Process.GetProcesses()) {
                var cpuCounter = ProcessMonitor.GetPerfCounterForProcessId(p.Id, "% Processor Time");
                var memCounter = ProcessMonitor.GetPerfCounterForProcessId(p.Id, "Working Set - Private");

                if (cpuCounter != null) {
                    cpuCounter.NextValue();
                    perfCounters.Add(cpuCounter);
                }

                if (memCounter != null) {
                    memCounter.NextValue();
                    perfCounters.Add(memCounter);
                } 
            }
        }

        public void GatherData() {

            for (int i = 0; i < perfCounters.Count; i++ ) {
                float data;
                PerformanceCounter counter = perfCounters[i];

                try {
                    data = counter.NextValue();
                }
                catch {
                    Console.WriteLine("Could not get information on process {0}", counter.InstanceName);
                    Console.WriteLine("Removing process {0}", counter.InstanceName);

                    perfCounters.Remove(counter);
                    continue;
                }

                if (counter.CounterName == "% Processor Time") {
                    data = data / (float)RandomInfo.logicalCpuCount;
                    //Console.WriteLine("{0} {1} {2}%", counter.InstanceName, counter.CounterName, data);
                    if (HighestCpuUsage.Count < 5) {
                        HighestCpuUsage[counter.InstanceName] = data;
                    }
                    else {
                        List<string> keys = new List<string>(HighestCpuUsage.Keys);
                        foreach (string key in keys) {
                            if (HighestCpuUsage[key] < data) {
                                HighestCpuUsage.Remove(key);
                                HighestCpuUsage[counter.InstanceName] = data;

                                var items = from pair in HighestCpuUsage
                                            orderby pair.Value ascending
                                            select pair;

                                HighestCpuUsage = items.ToDictionary<KeyValuePair<string, float>, string, float>(pair => pair.Key, pair => pair.Value);
                                break;
                            }
                        }
                        
                    }
                    continue;
                }

                if (counter.CounterName == "Working Set - Private") {
                    var MemMbs = data / 1024 / 1024;
                    //Console.WriteLine("{0} {1} {2}Mbs", counter.InstanceName, counter.CounterName, MemMbs);
                    if (HighestMemUsage.Count < 5) {
                        HighestMemUsage[counter.InstanceName] = MemMbs;
                    }
                    else {
                        List<string> keys = new List<string>(HighestMemUsage.Keys);
                        foreach (string key in keys) {
                            if (HighestMemUsage[key] < MemMbs) {
                                HighestMemUsage.Remove(key);
                                HighestMemUsage[counter.InstanceName] = MemMbs;

                                var items = from pair in HighestMemUsage
                                            orderby pair.Value ascending
                                            select pair;

                                HighestMemUsage = items.ToDictionary<KeyValuePair<string, float>, string, float>(pair => pair.Key, pair => pair.Value);
                                break;
                            }
                        }
                    }
                }

            }
        }

    }
}

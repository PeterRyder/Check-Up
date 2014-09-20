using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Check_Up.Util {
    class ProcessesDataCollection {

        public Process[] RunningProcesses;
        List<PerformanceCounter> processCPUCounter;
        List<PerformanceCounter> processMemCounter;

        public ProcessesDataCollection() {
            processCPUCounter = new List<PerformanceCounter>();
            foreach (Process proc in Process.GetProcesses()) {

                using (PerformanceCounter pcProcess = new PerformanceCounter("Process", "% Processor Time", proc.ProcessName)) {
                    pcProcess.NextValue();
                    processCPUCounter.Add(pcProcess);
                }
            }
        }

        private void getProcesses() {
            RunningProcesses = Process.GetProcesses();
        }

        public void GatherData() {

            for (int i = 0; i < 10; i++) {

                foreach (PerformanceCounter counter in processCPUCounter) {
                    float cpu = counter.NextValue() / 4;
                    if (cpu != 0) {
                        Console.WriteLine("Process:{0} CPU% {1}", counter.InstanceName, cpu);
                    }
                    
                }

                Thread.Sleep(500);
                Console.WriteLine("");
            }


        }

    }
}

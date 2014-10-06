'''
Created on Aug 12, 2014

@author: pwryder
'''

import sys
import time
import System.Diagnostics


def main():
    print("Running script...")

    perf = System.Diagnostics.PerformanceCounter("Processor Information",
                                                 "% Processor Time",
                                                 "_Total")
    perf.NextValue()

    while(True):
        cpuUsage = int(perf.NextValue())
        print("Python: Cpu Usage " + str(cpuUsage))
        time.sleep(1)


if __name__ == "__main__":
    sys.exit(main())

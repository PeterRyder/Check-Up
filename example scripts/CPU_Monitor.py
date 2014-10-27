'''
Created on Aug 12, 2014

@author: pwryder
'''

import sys
import time

# import this C# library to monitor the CPU
from Check_Up.Util import *


def main():
    print("Running script...")

    # create a PerformanceMonitor object
    data_collector = OSDataCollection()

    while(True):

        data_collector.GatherCPUData()

        # do something with the data
        print("Python: Cpu Usage " + str(data_collector.currentCPUUsage))

        # sleep for at least .05 seconds
        time.sleep(1)


if __name__ == "__main__":
    sys.exit(main())

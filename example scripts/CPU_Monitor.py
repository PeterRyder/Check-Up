
import time

# import this C# library to monitor the CPU
import System.Diagnostics


def main():
    print("Running script...")

    # create a PerformanceMonitor object
    perf = System.Diagnostics.PerformanceCounter("Processor Information",
                                                 "% Processor Time",
                                                 "_Total")

    # call NextValue() once to initialize the counter
    perf.NextValue()

    while(True):

        # poll the cpu
        cpuUsage = int(perf.NextValue())

        # do something with the data
        print("Python: Cpu Usage " + str(cpuUsage))

        # sleep for at least .05 seconds
        time.sleep(1)

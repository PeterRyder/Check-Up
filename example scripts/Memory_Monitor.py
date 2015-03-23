
import time

# import this C# library to monitor the Memory
import System.Diagnostics

should_run = True


def main():
    global should_run
    print("Running script...")

    should_run = True

    # create a PerformanceMonitor object
    perf = System.Diagnostics.PerformanceCounter("Memory",
                                                 "Available MBytes")

    # call NextValue() once to initialize the counter
    perf.NextValue()

    while(should_run):

        # poll the cpu
        cpuUsage = int(perf.NextValue())

        # do something with the data
        print("[python] Memory Usage " + str(cpuUsage) + "MB")

        # sleep for at least .05 seconds
        time.sleep(1)


def close():
    global should_run
    should_run = False
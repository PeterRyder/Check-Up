Check-Up
========
[![Build status](https://ci.appveyor.com/api/projects/status/w5pdra6uagyhc4hm?svg=true)](https://ci.appveyor.com/project/PeterRyder/check-up)
Currently done:
- CPU, RAM, network, and disk monitoring
- Python scripts for custom monitoring
- Background process monitoring
- GUI fixes using WPF


Planning to do:
- OS level monitoring in background
- Different color palette presets to choose from
- Performance improvements
- Add user manual to README

User manual:

After starting Check Up, the first step is opening the Properties window from
the File drop down menu. From here, numerous data logging settings can be changed.

First, up to four different processes can be monitored: CPU usage, Memory
usage, Network activity, and Disk I/O. The total polling time over which
data will be collected and interval between polls can also be changed.

Under the Chart Control tab, there are two options. Checking the box next to
ignore time will cause the program to log data until the user stops it by
clicking "Stop Monitoring" on the main screen next to the chart.

Under the Background tab, the Monitor Processes check box controls whether the
program will still collect data when minimized to the taskbar.
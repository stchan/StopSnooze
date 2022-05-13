<i>**StopSnooze**</i> is a console application which prevents Windows from sleeping.

**Requirements**<br/>
64-bit Windows with .NET 6.0 support.

**Binaries**<br/>
Check the [releases](https://github.com/stchan/StopSnooze) page for prebuilt executables.

**License**<br/>
StopSnooze is GPLv3.

**Usage:**
```
    StopSnooze [{-p PID | -x COMMAND}] [-w TIME]
```
    Options:

        -p, --pid       Wait on process. Mutually exclusive with -x
        -x, --shx       Execute command, then wait on spawned process. Mutually exclusive with -p
        -w, --wait      Wait for specified number of seconds

**Examples:**
```
    StopSnooze -w 60
```
Prevents sleep for 60 seconds.
```
    StopSnooze -p 2942
```
Prevents sleep while process with PID 2942 is running.
```
    StopSnooze -p 2942 -w 60
```
Prevents sleep while process with PID 2942 is running, or for up to 60 seconds, whichever is shorter.
```
    StopSnooze -x "notepad.exe"
```
Starts "notepad.exe", and prevents sleep while it is running.
```
    StopSnooze -x "notepad.exe" -w 60
```
Starts "notepad.exe", and prevents sleep while it is running, or for up to 60 seconds, whichever is shorter.
```
    StopSnooze -x "cmd /k"
```
Starts a new shell, and prevents sleep until it exits.
```
    StopSnooze
```
Prevents sleep until the user presses any key, or terminates the shell.


<br/>**Changelog**

1.0.3 - Renamed to StopSnooze. NoSnooze is a brand of caffeine pills sold by Circle K in the USA.

1.0.2 - First public release as NoSnooze.



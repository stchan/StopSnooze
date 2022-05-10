<i>**NoSnooze**</i> is a console application which prevents Windows from sleeping while it is running.

**Requirements**<br/>
A version of 64-bit Windows with .NET 6.0 single file deployment support.

**Binaries**<br/>
Check the [releases](https://github.com/stchan/NoSnooze) page for prebuilt executables.

**License**<br/>
NoSnooze is released under GPLv3.

**Usage:**
```
    NoSnooze [{-p PID | -x COMMAND}] [-w TIME]
```
    Options:

        -p, --pid       Wait on process. Mutually exclusive with -x
        -x, --shx       Execute command, then wait on spawned process. Mutually exclusive with -p
        -w, --wait      Wait for specified number of seconds

**Examples:**
```
    NoSnooze -w 60
```
Prevents sleep for 60 seconds.
```
    NoSnooze -p 2942
```
Prevents sleep while process with PID 2942 is running.
```
    NoSnooze -p 2942 -w 60
```
Prevents sleep while process with PID 2942 is running, or for up to 60 seconds, whichever is shorter.
```
    NoSnooze -x "notepad.exe"
```
Starts "notepad.exe", and prevents sleep while it is running.
```
    NoSnooze -x "notepad.exe" -w 60
```
Starts "notepad.exe", and prevents sleep while it is running, or for up to 60 seconds, whichever is shorter.
```
    NoSnooze -x "cmd /k"
```
Starts a new shell, and prevents sleep until it exits.
```
    NoSnooze
```
Prevents sleep until the user presses any key, or terminates the shell.






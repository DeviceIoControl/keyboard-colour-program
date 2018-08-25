## CLEVO Keyboard Colour Program  

**This is my FIRST commit.**

This is a Windows program that I created in C# that is used to create keyboard themes. 

I initially created this to prevent the 7 startup applications from being launched at boot when I needed the 
CLEVO Control Center to start the keyboard colour animations.

You can take this program and do anything you want with it.


### DISCLAIMER

This is **OLD** and **UNFINISHED** code. I will most likely abandon this code because, I will be reprogramming this in C++/CLI or C++ later on (when I have time & and done more research into this). 

Nothing much has been documented about this code (Not many comments in the code explaining what parts of the code do for the program..)

This program was created for my laptop (CLEVO P650RS-G). I cannot guarantee that this will work for other Models of CLEVO laptops.
(It should / might work for other 3 Colour Zone CLEVO laptop keyboards).

### UPDATES -> I have created a C++ version of this program that is more optimised... link: https://github.com/DeviceIoControl/CppKeyboardColour

### Current Feature List

## Contains a couple of basic animated themes:
- Basic Colour Wave 
- Basic Slow Breathing
- Flashing Lights
- Transforming Lights
- Christmas Lights (Not even christmas themed??)
- White Keyboard
- BGR (Reversed RGB) Keyboard


### CURRENT FINDINGS & PROGRAMMER NOTES

I have seen a couple of websites that say you require the "Clevo Keyboard Driver" in order to change the colour of the keyboard.

**NO!**

- I had to "reverse engineer" the Clevo Control Center program (which I found out was actually programmed in C#) using a program called "ILSpy" in order to get this program working and the program code **DOES NOT REQUIRE A CLEVO DRIVER**. I even uninstalled all of the drivers that came with the Clevo Control Center and my keyboard remains in a functioning state... (it still changes colour)

- This program actually requires event log (and yes i mean "Windows Event Log") to communicate with the "PowerBiosServer" service which then in turn uses WMI (Windows Management Instrumentation) to communicate with the EC (Embedded Controller) of the laptop / system.
(Windows has a pre-installed WMI driver that is used to communicate with the EC...)


### TIPS ON RECODING...

- In order to increase this application's performance, I recommend reverse engineering the "PowerBiosServer" service (also created in C#) and try to build an application that interfaces with WMI directly so that the "PowerBiosServer" service is no longer needed (therefore meaning no more event log). 

Or if your smart enough..

Build a Windows driver that communicates with the EC of the computer...

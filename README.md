# TSWRDConnector

RailDriver connector program for Train Sim World. This program connects to RailDriver via USB, reads control lever value from TSW and synchornizes them by applying keystrokes.

As such:

 - It allows faster lever movements and supports moving multiple levers "verbosely". (Don't get too excited and rip it off your controller please.)
 - It operates under synchornized lever positions, preventing misaligned levers.
 - "Percise" lever movements, it is tailered to different kinds of levers in TSW. (e.g. levers with/without notchs are handled differently)
 - Many customizations are possible.

Hopefully, this makes your TSW experience better, more realistic and exciting. 

## ⚠ ACHTUNG: READ FIRST BEFORE STARTING ⚠

 - Anti-Virus Software: As the program reads value from TSW by accessing memory locations. Anti-Virus software may produce false positive, most likely on the underline library `Memory.dll`. This program IS NOT A VIRUS. If you are not sure, you can always download the source code and compile from source.
 
 - This program is NOT endorsed by either P.I. Engineering (RailDriver) or Dovetail Games (TSW). This is simply a fan creation. I am NOT affilated with either of them.

 - It may be very flaky or doesn't work for your PC as it's not been well tested. Please use `Issues` to address any specific problems.

 - No discussion/works here will be related to circumventing any digital rights protection mechanisms. Please supports the developers! (Yes, even though there's no official RailDriver support, it is still a very good game)

## Prerequisites

 - Train Sim World 2020 (I've not tested on any other versions)
 - RailDriver and [MacroWorks](https://xkeys.com/software/softwarewindows/softwaremacroworks.html). Please run calibration first.

## Getting Started

 1. Dowload the program from `Release` page, unzip it.
 2. Run TSW 2020.
 3. Run `TSWMod.exe` under administrator mode.
 4. Click `Load RailDriver Calibration` and browse to the RailDriver calibration file `ModernCalibration.rdm`. (It should be under `<Your MacroWorks Install Location>/Devices/RailDriver`).
 5. Load up a game in TSW.
 6. IMPORTANT: In a moment, observe your RailDriver LED, it should read "CL" 
 7. After the game loads, while the RailDriver reads "CL". Honk the horn of the train you are controlling (sit down first). You will need to do it a couple times, probalby 2-3.
 8. If the train is supported (see below for full list of supported trains), your RailDriver will return to display "rd". You may switch window the the program and see the name of the train being displayed.
 9. That's it. Enjoy.

## Supported Trains and Functionalities

For now, the focus has been on supporting the control levers in the train. Limited buttons are supported (Alerter, Bell, Sand etc.). More supports are yet to come.

The full list of supported trains and their respective packages:

 - Mann-Spessart Bahn BR146.2
 - Mann-Spessart Bahn BR185.2
 - Mann-Spessart Bahn DBpbzfa
 - CSX Heavy Haul AC4400CW

## Building

To build this from source, Visual Studio 2019, C# WindowsForm Development Package is needed.

## Credits

Big shout-out to Gandalf @ railsim-fr.com and crrispy on Steam. Their detailed scripts really inspired this work. You can find them at [here](https://www.railsim-fr.com/forum/index.php?/files/file/1682-train-sim-world-raildriver-interface). Thread on [RailsimFR](https://www.railsim-fr.com/forum/index.php?/topic/12446-tsw-et-raildriver-cest-parti) (in french)

## Related Links

[Train Sim World on Steam](https://store.steampowered.com/app/530070/Train_Sim_World_2020/)

[Steam TSW RailDriver Thread](https://steamcommunity.com/app/530070/discussions/0/1797403972728914718/)

[RailDriver Website](http://raildriver.com/)

[Memory.dll Library](https://github.com/erfg12/memory.dll/) (GPL-3.0 Licensed)

## License

GPL-3
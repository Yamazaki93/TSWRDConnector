# TSWRDConnector

RailDriver connector for Train Sim World. This program connects to RailDriver via USB, reads control lever value on RailDriver and from TSW and synchornizes them by applying keystrokes.

As such:

 - It allows faster lever movements and supports moving multiple levers "verbosely". (Don't get too excited and rip it off your controller please.)
 - It operates under synchornized lever positions, preventing misaligned levers.
 - "Percise" lever movements, it is tailered to different kinds of levers in TSW. (e.g. levers with/without notchs are handled differently)
 - Many customizations are possible.

Hopefully, this makes your TSW experience better, more realistic and exciting. 

## ⚠ ACHTUNG: READ FIRST BEFORE STARTING ⚠

 - Anti-Virus Software: As the program reads value from TSW by accessing memory locations. Anti-Virus software may produce false positive, most likely on the underline library `Memory.dll`. This program IS NOT A VIRUS. If you are not sure, you can always download the source code and compile from source.
 
 - This program is NOT endorsed by either P.I. Engineering (RailDriver) or Dovetail Games (TSW). This is simply a fan creation. I am NOT affilated with either of them.

 - Please use `Issues` to discuss any specific problems or request new features.

 - No discussion/works here will be related to circumventing any digital rights protection mechanisms. Please supports the developers! (Yes, even though there's no official RailDriver support, it is still a very good game)


## Prerequisites

 - Train Sim World 2020 (I've not tested on any other versions)
 - RailDriver and [MacroWorks](https://xkeys.com/software/softwarewindows/softwaremacroworks.html). Please run calibration first.

## Getting Started

Here's a video showing how it works.

[![Alt text](https://img.youtube.com/vi/ZyiEsUbQjms/0.jpg)](https://youtu.be/ZyiEsUbQjms)

You do NOT need MacroWorks running for this to work. In fact, it may not work if MacroWorks is running so exit it first.

Note, you will need to switch the TSW keyboard mappings back to default if you have adjusted them. Otherwise it won't work.

 1. Dowload the program from `Release` page, unzip it.
 2. Run TSW 2020.
 3. Run `TSWMod.exe` under administrator mode.
 4. Click `Load RailDriver Calibration` and browse to the RailDriver calibration file `ModernCalibration.rdm`. (It should be under `<Your MacroWorks Install Location>/Devices/RailDriver`).
 5. Load up a game in TSW.
 6. IMPORTANT: In a moment, observe your RailDriver LED, it should read "CL" 
 7. After the game loads, while the RailDriver reads "CL". Sit down at the train you are controlling and honk the horn. You will need to do it just a couple times, probalby 2-3.
 8. If the train is supported (see below for full list of supported trains), your RailDriver will return to display "rd". You may switch window to the connector and see the name of the train being displayed.
 9. That's it. Enjoy.

## Switching Trains

  1. To switch train in session. Click the top right most function key (to the left of the "UP" key of the up-down combo key.)
  2. Observe the RailDriver LED display "CL" again.
  3. You can now sit down in another train and repeat the calibration step (step 7. above).

## Supported Trains and Functionalities

For now, the focus has been on supporting the control levers in the train. Limited buttons are supported (Alerter, Bell, Sand etc.). More supports are yet to come.

The full list of supported trains and their respective packages:

 - Mann-Spessart Bahn BR146.2
 - Mann-Spessart Bahn BR185.2
 - Mann-Spessart Bahn DBpbzfa
 - CSX Heavy Haul AC4400CW

## Known Issues and Limitations

 - Limited buttons support only, EStop, Alert, Bell, Sand, Pantograph, Horn, PZB are supported. Wiper, Lights and other function buttons are NOT supported at the moment. Key legend coming soon.
 - In case of locomotive with 2 cabs, only the front cab is supported at the moment. (Back cab support coming soon)
 - Lever may still "run away" in some cases, especially on those with small knotches. I will try to improve them in a future release. Meanwhile, tapping the corresponding control key on the keyboard briefly (1 or 2 times) should get them back in-sync.
 - Lever adjustment speed is determined by the game. So it may run slow depending on which lever you are adjusting. But it should still go to the commanded position on RailDriver.
 - In very rare cases, the connector may mis-identify your train or show wrong status. In this case, restarting the connector mid-session should fix this. The connector should directly enter Calibration mode if it's started after the session is loaded.

## Building

To build this from source, Visual Studio 2019, C# WindowsForm Development Option is needed.

## Credits

Big shout-out to Gandalf @ railsim-fr.com. The detailed scripts really inspired this work. You can find them at [here](https://www.railsim-fr.com/forum/index.php?/files/file/1682-train-sim-world-raildriver-interface). Thread on [RailsimFR](https://www.railsim-fr.com/forum/index.php?/topic/12446-tsw-et-raildriver-cest-parti) (in french)

## Related Links

[Train Sim World on Steam](https://store.steampowered.com/app/530070/Train_Sim_World_2020/)

[Steam TSW RailDriver Thread](https://steamcommunity.com/app/530070/discussions/0/1797403972728914718/)

[RailDriver Website](http://raildriver.com/)

[Memory.dll Library](https://github.com/erfg12/memory.dll/) (GPL-3.0 Licensed)

## License

GPL-3
# Forge page (copy these fields into the upload form; this file is not shipped in the archive)

## Name
Pause

## GUID
com.netVnum.pause

## Version
1.5.0

## SPT version constraint
~4.1.0

## Category / tags
Category: Client / Quality of Life
Tags: pause, qol, client, raid

## Teaser (short description)
Pause your raid with one key: you, bots, time of day, audio and the raid timer all freeze.

## Description

Pause lets you freeze a raid at any moment with a single key (**F9** by default). Step away from the PC, answer the door or plan your next move without losing raid time.

### Features
- One key toggles pause on and off
- Freezes your character: movement, health, hydration and energy, stamina
- Freezes all bots in place
- Stops time of day
- Pauses in-raid audio and resumes it where it stopped
- The raid timer shows **PAUSED**, and time spent paused is not taken from your raid
- Camera zoom is restored correctly after unpausing, including when you were aiming

### Installation
1. Extract the archive into your SPT root folder (where `EscapeFromTarkov.exe` is).
2. Check that `BepInEx\plugins\netVnum-Pause\netVnum.Pause.dll` exists.
3. If you used an older Pause, delete the old `BepInEx\plugins\netVnum.Pause.dll`.

### Usage
Press **F9** in raid to pause, press it again to resume.
Change the key in `BepInEx\config\com.netVnum.pause.cfg` or with the Configuration Manager (F12).

### Compatibility
- SPT 4.1.x (tested on 4.1.6). Not compatible with 4.0.x.
- Singleplayer only, not for Fika.
- Mods that also change time scale or freeze bots may conflict.

### Uninstall
Delete the `BepInEx\plugins\netVnum-Pause` folder.

### Dependencies
None besides SPT 4.1.x.

### Bug reports
Please open an issue on GitHub and attach `BepInEx\LogOutput.log`: https://github.com/Senyasensey2026/Pause/issues

### Credits
Original mod by netVnum and previous contributors. Updated for SPT 4.1 with the original author's permission.

## Version changelog (for the 1.5.0 version entry)
- Updated for SPT 4.1.x
- Fixed camera zoom after unpausing, including when aim is released during the pause
- Extra safety checks; new install folder `BepInEx\plugins\netVnum-Pause\`

## Links
- Source code: https://github.com/Senyasensey2026/Pause
- Download: https://github.com/Senyasensey2026/Pause/releases/download/v1.5.0/netVnum-Pause-1.5.0.zip
- VirusTotal: <link to the scan of the zip>
- License: MIT

## Images to prepare
- Thumbnail (square): in-raid screenshot with the timer showing PAUSED
- 2-3 screenshots: the PAUSED timer; bots frozen mid-movement; the Configuration Manager entry

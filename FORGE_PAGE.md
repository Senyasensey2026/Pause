# Forge page draft (copy into the upload form, not shipped in the archive)

## Name
Pause

## GUID
com.netVnum.pause

## Version
1.5.0

## SPT version
~4.1.0  (tested on 4.1.6)

## Teaser / short description
Pause your raid with one key: you, bots, time of day, audio and the raid timer all freeze.

## Description

Pause lets you freeze a raid at any moment by pressing **F9** (configurable).

**What gets paused**
- Your character: movement, health, hydration and energy, stamina
- All bots (frozen in place)
- Time of day
- In-raid audio
- The raid timer shows "PAUSED", and time spent paused is not taken from your raid time

**Installation**
1. Extract the archive into your SPT root folder (where `EscapeFromTarkov.exe` is).
2. Check that `BepInEx\plugins\netVnum-Pause\netVnum.Pause.dll` exists.
3. If you used an older Pause, delete the old `BepInEx\plugins\netVnum.Pause.dll`.

**Usage**
Press F9 in raid to toggle pause. Change the key in `BepInEx\config\com.netVnum.pause.cfg` or with the Configuration Manager (F12).

**Uninstall**
Delete `BepInEx\plugins\netVnum-Pause`.

**Dependencies**
None besides SPT 4.1.x.

**Credits**
Original mod by netVnum and previous contributors: <link to original repo / Forge page>. Updated for SPT 4.1 with the author's permission.

## Changelog for this version
- Updated for SPT 4.1.x (de-obfuscated client names)
- Fixed camera zooming in after unpausing
- Extra null checks; new install folder `BepInEx\plugins\netVnum-Pause\`

## Links
- Source code: https://github.com/<your-github>/Pause
- Download (GitHub release asset): https://github.com/<your-github>/Pause/releases/download/v1.5.0/netVnum-Pause-1.5.0.zip
- VirusTotal: <link after scanning the zip>
- License: MIT

# Pause

[![SPT](https://img.shields.io/badge/SPT-4.1.x-blue)](https://sp-mod.com/)
[![Version](https://img.shields.io/badge/version-1.5.0-green)](CHANGELOG.md)
[![License: MIT](https://img.shields.io/badge/license-MIT-lightgrey)](LICENSE)

A BepInEx client mod for SPT that lets you pause a raid with a single key (**F9** by default).
Step away from the PC, answer the door or think about your next move without losing raid time.

**Compatible with SPT 4.1.x** (tested on 4.1.6, EFT 0.16.9.5.40743). Not compatible with SPT 4.0.x.

## Features
- One key toggles pause on and off
- Freezes your character: movement, health, hydration and energy, stamina
- Freezes all bots in place, they resume what they were doing afterwards
- Stops time of day
- Pauses in-raid audio and resumes it exactly where it stopped
- The raid timer shows **PAUSED**, and time spent paused is not taken from your raid
- Camera zoom is restored correctly after unpausing, including when you were aiming down sights

## Installation
1. Download `netVnum-Pause-x.y.z.zip` from [Releases](../../releases) or the Forge page.
2. Extract it into your SPT root folder (the one containing `EscapeFromTarkov.exe`).
3. Check that `BepInEx\plugins\netVnum-Pause\netVnum.Pause.dll` exists.
4. If you used an older version of Pause, delete the old `BepInEx\plugins\netVnum.Pause.dll`.

## Usage
Press **F9** in raid to pause, press it again to resume.

## Configuration
The config file `BepInEx\config\com.netVnum.pause.cfg` is created after the first launch.
You can also change settings in-game with the BepInEx Configuration Manager (F12), if you have it installed.

| Section | Setting | Default | Description |
|---|---|---|---|
| Keybinds | Toggle Pause | F9 | Key or key combination that pauses and unpauses the raid |

## Compatibility
- Singleplayer SPT only. Not intended for Fika or any multiplayer setup.
- Mods that also change `Time.timeScale` or freeze bots may conflict.

## Uninstall
Delete the `BepInEx\plugins\netVnum-Pause` folder. Optionally delete `BepInEx\config\com.netVnum.pause.cfg`.

## Reporting bugs
Open an [issue](../../issues/new/choose) and attach `BepInEx\LogOutput.log` from the session where the problem happened.

## Building from source
Requirements: .NET SDK 8 or newer, and an SPT 4.1.x install (game DLLs are referenced from it and are not included in this repo).

```
build-release.bat "C:\SPT"
```
In PowerShell use `.\build-release.bat "C:\SPT"`.

The script builds the DLL against the given SPT install, copies it into that install for testing,
and creates `release\netVnum-Pause-<version>.zip` with the release layout.
To only build: `dotnet build source\Pause.csproj -c Release -p:SptPath="C:\SPT"`.

## Credits
- Original mod by **netVnum** and previous contributors (see git history).
- SPT 4.1 update by **Arsenii**.

## License
[MIT](LICENSE)

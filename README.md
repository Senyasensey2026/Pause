# Pause

A BepInEx client mod for SPT that lets you pause a raid with a single key (F9 by default).

**Compatible with SPT 4.1.x** (tested on 4.1.6, EFT 0.16.9.5.40743). Not compatible with SPT 4.0.x.

## What gets paused
- You: movement, health, hydration and energy, stamina
- AI (bots are frozen in place)
- Time of day
- In-raid audio
- The raid timer (it shows "PAUSED"; time spent paused is not deducted from the raid)

## Installation
1. Download `netVnum-Pause-x.y.z.zip`.
2. Extract it into your SPT root folder (the one containing `EscapeFromTarkov.exe`).
3. You should end up with `BepInEx\plugins\netVnum-Pause\netVnum.Pause.dll`.
4. If you had an older version of Pause, delete its old `netVnum.Pause.dll` (it may sit directly in `BepInEx\plugins`).

## Usage
- Press **F9** in raid to pause or unpause.
- Change the key in `BepInEx\config\com.netVnum.pause.cfg`, or in-game with the BepInEx Configuration Manager (F12) if you have it installed.

## Uninstall
Delete the `BepInEx\plugins\netVnum-Pause` folder (and optionally `BepInEx\config\com.netVnum.pause.cfg`).

## Building from source
Requirements: .NET SDK 8 or newer, and an SPT 4.1.x install.

```
build-release.bat "C:\SPT"
```

This builds the DLL against the given SPT install, copies it into that install for testing, and creates `release\netVnum-Pause-<version>.zip` with the Forge layout.
To only build: `dotnet build source\Pause.csproj -c Release -p:SptPath="C:\SPT"`.

## Credits
- Original mod by netVnum and previous contributors (see git history and LICENSE).
- SPT 4.1 update by Arsenii.

## License
MIT, see `LICENSE`.

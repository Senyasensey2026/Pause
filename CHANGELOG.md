# Changelog

## 1.5.0
- Continued as a separate mod, **Pause Continued** (GUID `com.rejzbihai.pause`, DLL `rejzbihai.Pause.dll`), with the original author's permission.
- Refuses to load while the original Pause (`com.netVnum.pause`) is installed, so the two never run together.
- Imports the pause key from the original Pause config on first launch.
- Updated for SPT 4.1.x (EFT 0.16.9.5.40743), where most game class and member names were de-obfuscated:
  - `GameTimerClass` -> `GameTimer`, patched `method_0` -> `GetPastTime`
  - timer fields `Nullable_0..3` -> `_startDateTime`, `_escapeDateTime`, `nullable_2`, `_sessionTime`
  - `TimerPanel.dateTime_0` -> `_dateTime`, `GameDateTime.RealtimeSinceStartup` -> `_realtimeSinceStartup`
  - `CameraClass` -> `CameraManager`
- Fixed camera FOV after unpausing: the mod waits for the game to finish entering/leaving ADS, then restores the correct hip-fire or aimed FOV (no more zoom stuck after releasing aim during pause).
- Added null checks around the timer panel and bot AI data, and a log error if timer fields cannot be found.
- Config option now has a description (visible in Configuration Manager).
- Release archive now installs into `BepInEx\plugins\rejzbihai-Pause\`.
- SPT install path is configurable when building (`-p:SptPath=...`).

## 1.4.0
- Updated for SPT 4.0.1.

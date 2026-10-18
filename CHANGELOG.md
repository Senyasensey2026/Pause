# Changelog

## 1.5.0
- Updated for SPT 4.1.x (EFT 0.16.9.5.40743), where most game class and member names were de-obfuscated:
  - `GameTimerClass` -> `GameTimer`, patched `method_0` -> `GetPastTime`
  - timer fields `Nullable_0..3` -> `_startDateTime`, `_escapeDateTime`, `nullable_2`, `_sessionTime`
  - `TimerPanel.dateTime_0` -> `_dateTime`, `GameDateTime.RealtimeSinceStartup` -> `_realtimeSinceStartup`
  - `CameraClass` -> `CameraManager`
- Fixed camera zooming in after unpausing: the camera FOV is saved when pausing and restored exactly on unpause.
- Added null checks around the timer panel and bot AI data, and a log error if timer fields cannot be found.
- Release archive now installs into `BepInEx\plugins\netVnum-Pause\`.
- SPT install path is configurable when building (`-p:SptPath=...`).

## 1.4.0
- Updated for SPT 4.0.1.

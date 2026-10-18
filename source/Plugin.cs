using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using EFT;
using JetBrains.Annotations;
using SPT.Reflection.Patching;
using UnityEngine;

namespace Pause
{
    [BepInPlugin(Plugin.PluginGuid, Plugin.PluginName, Plugin.PluginVersion)]
    // The original Pause by netVnum uses this GUID. Both patch the same game code, so if the old
    // plugin is still installed BepInEx skips this one (and says so in the log) instead of running both.
    [BepInIncompatibility(Plugin.OriginalGuid)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.rejzbihai.pause";
        public const string PluginName = "rejzbihai-Pause";
        public const string PluginVersion = "1.5.0";

        internal const string OriginalGuid = "com.netVnum.pause";

        internal static ConfigEntry<KeyboardShortcut> TogglePause;
        internal static ManualLogSource Log;

        [UsedImplicitly]
        private void Awake()
        {
            Log = Logger;

            var isFirstRun = !File.Exists(Config.ConfigFilePath);
            TogglePause = Config.Bind("Keybinds", "Toggle Pause", new KeyboardShortcut(KeyCode.F9),
                "Key (or key combination) that pauses and unpauses the raid.");
            if (isFirstRun)
            {
                ImportOriginalKeybind();
            }

            Logger.LogInfo("PAUSE: Loading");

            new NewGamePatch().Enable();

            new WorldTickPatch().Enable();
            new OtherWorldTickPatch().Enable();
            new GameTimerUpdatePatch().Enable();
            new TimerPanelPatch().Enable();
            new PlayerUpdatePatch().Enable();
            new EndByTimerScenarioUpdatePatch().Enable();

            new BaseLocalGameUpdatePatch().Enable();
        }

        /// <summary>
        /// Players coming from the original Pause keep their key: on first run, copy "Toggle Pause"
        /// from the original mod's config file if it exists.
        /// </summary>
        private void ImportOriginalKeybind()
        {
            try
            {
                var oldConfigPath = Path.Combine(Paths.ConfigPath, OriginalGuid + ".cfg");
                if (!File.Exists(oldConfigPath))
                {
                    return;
                }

                var line = File.ReadAllLines(oldConfigPath)
                    .Select(l => l.Trim())
                    .FirstOrDefault(l => l.StartsWith("Toggle Pause", StringComparison.OrdinalIgnoreCase) && l.Contains("="));
                if (line == null)
                {
                    return;
                }

                var value = line.Substring(line.IndexOf('=') + 1).Trim();
                var shortcut = KeyboardShortcut.Deserialize(value);
                if (shortcut.MainKey == KeyCode.None)
                {
                    return;
                }

                TogglePause.Value = shortcut;
                Logger.LogInfo($"PAUSE: imported key '{value}' from {OriginalGuid}.cfg");
            }
            catch (Exception e)
            {
                Logger.LogWarning($"PAUSE: could not import key from the original Pause config: {e.Message}");
            }
        }

        internal class NewGamePatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod() => typeof(GameWorld).GetMethod(nameof(GameWorld.OnGameStarted));

            [PatchPrefix]
            private static void PatchPrefix()
            {
                PauseController.Enable();
            }
        }
    }
}

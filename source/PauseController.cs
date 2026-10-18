using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Comfort.Common;
using EFT;
using EFT.Animations;
using EFT.CameraControl;
using EFT.UI.BattleTimer;
using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Pause
{
    public class PauseController : MonoBehaviour
    {
        internal static bool IsPaused { get; private set; }

        private DateTime? _pausedDate;
        private DateTime? _unpausedDate;
        private GameTimer _gameTimer;
        private MainTimerPanel _mainTimerPanel;
        private AbstractGame _abstractGame;
        private List<AudioSource> _pausedAudioSources;

        internal static ManualLogSource Logger;

        private static GameWorld GameWorld;

        private static Player MainPlayer;
        private static FieldInfo IsAimingField;

        // SPT 4.1+ (EFT 0.16.9.5.40743) restored most original names:
        private static FieldInfo StartDateTime; // GameTimer._startDateTime (DateTime?), was Nullable_0
        private static FieldInfo EscapeDateTime; // GameTimer._escapeDateTime (DateTime?), was Nullable_1
        private static FieldInfo StopDateTime; // GameTimer.nullable_2 (DateTime?), was Nullable_2
        private static FieldInfo SessionTime; // GameTimer._sessionTime (TimeSpan?), was Nullable_3

        private static FieldInfo TimerPanelField;
        private static FieldInfo GameDateTimeField;

        [UsedImplicitly]
        private void Awake()
        {
            Logger = BepInEx.Logging.Logger.CreateLogSource(nameof(PauseController));

            IsPaused = false;
            _abstractGame = Singleton<AbstractGame>.Instance;
            _mainTimerPanel = FindObjectOfType<MainTimerPanel>();
            _gameTimer = _abstractGame?.GameTimer;
            _pausedAudioSources = new List<AudioSource>();

            IsAimingField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_isAiming");
            StartDateTime = AccessTools.Field(typeof(GameTimer), "_startDateTime");
            EscapeDateTime = AccessTools.Field(typeof(GameTimer), "_escapeDateTime");
            StopDateTime = AccessTools.Field(typeof(GameTimer), "nullable_2");
            SessionTime = AccessTools.Field(typeof(GameTimer), "_sessionTime");
            TimerPanelField = AccessTools.Field(typeof(TimerPanel), "_dateTime");
            GameDateTimeField = AccessTools.Field(typeof(GameDateTime), "_realtimeSinceStartup");

            if (StartDateTime == null || EscapeDateTime == null || StopDateTime == null || TimerPanelField == null || GameDateTimeField == null)
            {
                Logger.LogError("PAUSE: some timer fields were not found - raid timer may not be corrected after unpausing. Is this the right SPT version?");
            }
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
            IsPaused = false;
            GameWorld = null;
            MainPlayer = null;
            Logger = null;
            _pausedAudioSources.Clear();
            StartDateTime = null;
            EscapeDateTime = null;
            StopDateTime = null;
            SessionTime = null;
            TimerPanelField = null;
            GameDateTimeField = null;
        }

        [UsedImplicitly]
        private void Update()
        {
            if (!IsKeyPressed(Plugin.TogglePause.Value))
            {
                return;
            }

            IsPaused = !IsPaused;

            if (IsPaused)
            {
                Pause();
            }
            else
            {
                Unpause();
                ResetFov();
            }
        }

        private void Pause()
        {
            Time.timeScale = 0f;
            _pausedDate = DateTime.UtcNow;

            MainPlayer.enabled = false;
            MainPlayer.PauseAllEffectsOnPlayer();

            foreach (var player in GetPlayers().Where(p => !p.IsYourPlayer))
            {
                Logger.LogInfo($"Deactivating player: {player.name}");
                SetPlayerState(player, false);
            }

            PauseAllAudio();
            ShowTimer();
        }

        private void Unpause()
        {
            Time.timeScale = 1f;
            _unpausedDate = DateTime.UtcNow;

            MainPlayer.enabled = true;
            MainPlayer.UnpauseAllEffectsOnPlayer();

            foreach (var player in GetPlayers().Where(p => !p.IsYourPlayer))
            {
                Logger.LogInfo($"Reactivating player: {player.name}");
                SetPlayerState(player, true);
            }

            ResumeAllAudio();

            if (_mainTimerPanel != null && !_mainTimerPanel.ForcePull)
            {
                StartCoroutine(CoHideTimer());
            }
            UpdateTimers(GetTimePaused());
        }

        private void PauseAllAudio()
        {
            _pausedAudioSources.Clear();
            foreach (var audioSource in FindObjectsOfType<AudioSource>().Where(s => s.isPlaying))
            {
                audioSource.Pause();
                _pausedAudioSources.Add(audioSource);
            }
        }

        private void ResumeAllAudio()
        {
            foreach (var audioSource in _pausedAudioSources)
            {
                audioSource.UnPause();
            }

            _pausedAudioSources.Clear();
        }

        private static IEnumerable<Player> GetPlayers()
        {
            return GameWorld?.AllAlivePlayersList ?? new List<Player>();
        }

        private static void SetPlayerState(Player player, bool active)
        {
            if (player == null)
            {
                return;
            }

            if (player.PlayerBones != null)
            {
                foreach (var r in player.PlayerBones.GetComponentsInChildren<Rigidbody>())
                {
                    if (active)
                    {
                        r.WakeUp();
                    }
                    else
                    {
                        r.Sleep();
                    }
                }
            }

            var weaponRigidBody = player.HandsController?.ControllerGameObject?.GetComponent<Rigidbody>();
            if (weaponRigidBody != null)
            {
                weaponRigidBody.angularVelocity = UnityEngine.Vector3.zero;
                weaponRigidBody.velocity = UnityEngine.Vector3.zero;
                weaponRigidBody.Sleep();
            }

            if (!active)
            {
                player.AIData?.BotOwner?.DecisionQueue?.Clear();
                player.gameObject.SetActive(false);
            }
            else
            {
                player.gameObject.SetActive(true);
                player.AIData?.BotOwner?.CalcGoal();
            }
        }

        private void ShowTimer()
        {
            _mainTimerPanel?.DisplayTimer();
        }

        private IEnumerator CoHideTimer()
        {
            if (_mainTimerPanel == null)
            {
                yield break;
            }

            yield return new WaitForSeconds(4f);
            _mainTimerPanel.HideTimer();
        }

        private TimeSpan GetTimePaused()
        {
             return _pausedDate.HasValue && _unpausedDate.HasValue ? 
                _unpausedDate.Value - _pausedDate.Value : 
                TimeSpan.Zero;
        }

        private void UpdateTimers(TimeSpan timePaused)
        {
            _gameTimer = _gameTimer ?? Singleton<AbstractGame>.Instance?.GameTimer;
            if (_gameTimer == null || _mainTimerPanel == null || GameWorld == null ||
                StartDateTime == null || EscapeDateTime == null || StopDateTime == null || TimerPanelField == null || GameDateTimeField == null)
            {
                Logger.LogWarning("PAUSE: unable to correct raid timers (missing timer, panel or fields)");
                return;
            }

            // _startDateTime - Start Date/Time of the Raid.
            var startDateTime = StartDateTime.GetValue(_gameTimer) as DateTime?;

            // _escapeDateTime - Start Date/Time of the Raid + Total Raid Time = Time the raid should end with no additional pauses.
            var escapeDateTime = EscapeDateTime.GetValue(_gameTimer) as DateTime?;
                    
            // TimerPanel._dateTime
            var timerPanelDate = TimerPanelField.GetValue(_mainTimerPanel) as DateTime?;

            var realTimeSinceStartup = GameDateTimeField.GetValue(GameWorld.GameDateTime) as float?;

            if (!startDateTime.HasValue || !escapeDateTime.HasValue || !timerPanelDate.HasValue || !realTimeSinceStartup.HasValue)
            {
                return;
            }

            // SET UPDATED VALUES
            // _startDateTime
            StartDateTime.SetValue(_gameTimer, startDateTime.Value.Add(timePaused));
            // _escapeDateTime
            EscapeDateTime.SetValue(_gameTimer, escapeDateTime.Value.Add(timePaused));
            // nullable_2 (stop time) - Keeping this null is more reliable to prevent MIA raid endings. Some game conditions can set this variable and it changes how the game calculates remaining raid time.
            StopDateTime.SetValue(_gameTimer, null);
            // Game world timing should not include any time spent during pause.
            GameDateTimeField.SetValue(GameWorld.GameDateTime, realTimeSinceStartup.Value + (float)timePaused.TotalSeconds);
            // Add paused time to the UI timer(s).
            TimerPanelField.SetValue(_mainTimerPanel, timerPanelDate.Value.Add(timePaused));
        }
        
        private static void ResetFov()
        {
            if (MainPlayer == null || MainPlayer.ProceduralWeaponAnimation == null || CameraManager.Instance == null)
            {
                return;
            }

            var baseFov = MainPlayer.ProceduralWeaponAnimation.FieldOfView; // was Single_2
            var targetFov = baseFov;

            var isAiming = (bool)(IsAimingField?.GetValue(MainPlayer.ProceduralWeaponAnimation) ?? false);
            var scopeAimTransformsCount = MainPlayer.ProceduralWeaponAnimation.ScopeAimTransforms?.Count ?? 0;

            if (MainPlayer.ProceduralWeaponAnimation.PointOfView != EPointOfView.FirstPerson || MainPlayer.ProceduralWeaponAnimation.AimIndex >= scopeAimTransformsCount)
            {
                return;
            }

            if (isAiming)
            {
                targetFov = MainPlayer.ProceduralWeaponAnimation.CurrentScope?.IsOptic ?? false ? 35f : baseFov - 15f;
            }

            Logger.LogDebug($"Current FOV (When Unpausing): {CameraManager.Instance.Fov}, Base FOV: {baseFov}, Target FOV: {targetFov}");
            CameraManager.Instance.SetFov(targetFov, 1f, !isAiming);
        }

        internal static void Enable()
        {
            if (!Singleton<IBotGame>.Instantiated)
            {
                return;
            }

            GameWorld = Singleton<GameWorld>.Instance;
            GameWorld.GetOrAddComponent<PauseController>();
            MainPlayer = GameWorld.MainPlayer;
            Logger.LogDebug("PauseController enabled.");
        }

        internal static bool IsKeyPressed(KeyboardShortcut key)
        {
            return UnityInput.Current.GetKeyDown(key.MainKey) && key.Modifiers.All(modifier => UnityInput.Current.GetKey(modifier));
        }
    }
}

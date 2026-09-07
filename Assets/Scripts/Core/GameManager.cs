using UnityEngine;
using System.Collections.Generic;

namespace FNAFVR.Core
{
    /// <summary>
    /// Main game manager for FNAF VR experience
    /// Handles game state, night progression, and core systems
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game Settings")]
        [SerializeField] private int currentNight = 1;
        [SerializeField] private float nightDuration = 480f; // 8 minutes in seconds
        [SerializeField] private float timeRemaining;

        [Header("Power Management")]
        [SerializeField] private float maxPower = 100f;
        [SerializeField] private float currentPower;
        [SerializeField] private float powerDrainRate = 0.5f;

        [Header("Game State")]
        private bool isGameActive = false;
        private bool isPowerOut = false;
        private float nightStartTime;

        // Events
        public delegate void GameStateChanged(GameState newState);
        public event GameStateChanged OnGameStateChanged;

        public delegate void PowerChanged(float currentPower, float maxPower);
        public event PowerChanged OnPowerChanged;

        public delegate void NightProgressed(int night, float timeRemaining);
        public event NightProgressed OnNightProgressed;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            currentPower = maxPower;
            timeRemaining = nightDuration;
            nightStartTime = Time.time;
            StartNight();
        }

        private void Update()
        {
            if (!isGameActive)
                return;

            UpdateTime();
            UpdatePower();
            CheckGameConditions();
        }

        private void UpdateTime()
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                EndNight(true); // Night survived
            }

            OnNightProgressed?.Invoke(currentNight, timeRemaining);
        }

        private void UpdatePower()
        {
            if (currentPower > 0)
            {
                currentPower -= powerDrainRate * Time.deltaTime;
                currentPower = Mathf.Max(0, currentPower);

                if (currentPower <= 0)
                {
                    TriggerPowerOut();
                }
            }

            OnPowerChanged?.Invoke(currentPower, maxPower);
        }

        private void CheckGameConditions()
        {
            // Game over conditions can be added here
            // (e.g., animatronic caught player, etc.)
        }

        public void StartNight()
        {
            isGameActive = true;
            isPowerOut = false;
            currentPower = maxPower;
            timeRemaining = nightDuration;
            OnGameStateChanged?.Invoke(GameState.NightStarted);
            Debug.Log($"Night {currentNight} started!");
        }

        public void EndNight(bool survived)
        {
            isGameActive = false;
            if (survived)
            {
                OnGameStateChanged?.Invoke(GameState.NightSurvived);
                currentNight++;
                Debug.Log($"Night {currentNight - 1} survived! Moving to Night {currentNight}");
            }
            else
            {
                OnGameStateChanged?.Invoke(GameState.GameOver);
                Debug.Log("Game Over!");
            }
        }

        public void TriggerPowerOut()
        {
            if (!isPowerOut)
            {
                isPowerOut = true;
                OnGameStateChanged?.Invoke(GameState.PowerOut);
                Debug.Log("Power is out!");
            }
        }

        public void RestorePower(float amount)
        {
            currentPower = Mathf.Min(currentPower + amount, maxPower);
            if (currentPower > 0)
            {
                isPowerOut = false;
                OnGameStateChanged?.Invoke(GameState.PowerRestored);
            }
        }

        // Getters
        public int GetCurrentNight() => currentNight;
        public float GetTimeRemaining() => timeRemaining;
        public float GetCurrentPower() => currentPower;
        public float GetMaxPower() => maxPower;
        public bool IsPowerOut() => isPowerOut;
        public bool IsGameActive() => isGameActive;
    }

    public enum GameState
    {
        NightStarted,
        NightSurvived,
        GameOver,
        PowerOut,
        PowerRestored,
        Paused
    }
}

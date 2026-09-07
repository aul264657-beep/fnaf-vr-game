using UnityEngine;
using FNAFVR.Core;

namespace FNAFVR.Animatronics
{
    /// <summary>
    /// Base class for animatronic AI behavior
    /// Handles movement patterns, state changes, and interaction with the player
    /// </summary>
    public abstract class AnimatronicAI : MonoBehaviour
    {
        [Header("Animatronic Settings")]
        [SerializeField] protected string animatronicName = "Animatronic";
        [SerializeField] protected float aggressiveness = 0.5f;
        [SerializeField] protected float detectionRange = 20f;
        [SerializeField] protected float moveSpeed = 3f;

        [Header("AI Behavior")]
        [SerializeField] protected float updateInterval = 1f;
        protected float lastUpdateTime;

        protected AnimatronicState currentState = AnimatronicState.Idle;
        protected Transform playerPosition;
        protected bool isPlayerDetected = false;

        // Events
        public delegate void StateChanged(AnimatronicState newState);
        public event StateChanged OnStateChanged;

        protected virtual void Start()
        {
            playerPosition = Camera.main?.transform;
            InvokeRepeating(nameof(UpdateAI), updateInterval, updateInterval);
        }

        protected virtual void OnDestroy()
        {
            CancelInvoke(nameof(UpdateAI));
        }

        protected virtual void UpdateAI()
        {
            if (!GameManager.Instance.IsGameActive())
                return;

            DetectPlayer();
            UpdateState();
            UpdateBehavior();
        }

        protected virtual void DetectPlayer()
        {
            if (playerPosition == null)
                return;

            float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);
            isPlayerDetected = distanceToPlayer < detectionRange;
        }

        protected virtual void UpdateState()
        {
            AnimatronicState newState = currentState;

            if (GameManager.Instance.IsPowerOut())
            {
                newState = AnimatronicState.Aggressive;
            }
            else if (isPlayerDetected)
            {
                newState = AnimatronicState.Hunting;
            }
            else if (Random.value < aggressiveness * 0.1f)
            {
                newState = AnimatronicState.Moving;
            }
            else
            {
                newState = AnimatronicState.Idle;
            }

            if (newState != currentState)
            {
                ChangeState(newState);
            }
        }

        protected virtual void ChangeState(AnimatronicState newState)
        {
            currentState = newState;
            OnStateChanged?.Invoke(newState);
            Debug.Log($"{animatronicName} state changed to: {newState}");
        }

        protected virtual void UpdateBehavior()
        {
            switch (currentState)
            {
                case AnimatronicState.Idle:
                    IdleBehavior();
                    break;
                case AnimatronicState.Moving:
                    MovingBehavior();
                    break;
                case AnimatronicState.Hunting:
                    HuntingBehavior();
                    break;
                case AnimatronicState.Aggressive:
                    AggressiveBehavior();
                    break;
            }
        }

        protected abstract void IdleBehavior();
        protected abstract void MovingBehavior();
        protected abstract void HuntingBehavior();
        protected abstract void AggressiveBehavior();

        public string GetAnimatronicName() => animatronicName;
        public AnimatronicState GetCurrentState() => currentState;
        public bool IsPlayerDetected() => isPlayerDetected;
    }

    public enum AnimatronicState
    {
        Idle,
        Moving,
        Hunting,
        Aggressive,
        Stunned,
        Disabled
    }
}

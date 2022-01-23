using System;
using UnityEngine;
using UnityEngine.Events;

namespace Magthylius
{
    [Serializable]
    public enum TimeTickMode
    {
        Update = 0,
        LateUpdate,
        FixedUpdate
    }
    
    public class Timer : MonoBehaviour
    {
        [Header("Main Settings")]
        public float timeTarget = 1f;
        [SerializeField] private TimeTickMode tickMode = TimeTickMode.Update;
        public bool clearCache = true;
        public bool pauseOnTarget = false;
        
        [Header("Events")]
        public UnityEvent targetReachedEvent = new UnityEvent();

        [Header("Data")]
        [SerializeField, ReadOnly] private float ticker = 0f;
        [SerializeField, ReadOnly] private bool paused = false;

        private Action _updateDelegate;
        private Action _lateUpdateDelegate;
        private Action _fixedUpdateDelegate;

        ~Timer()
        {
            targetReachedEvent.RemoveAllListeners();
            _updateDelegate = null;
            _lateUpdateDelegate = null;
            _fixedUpdateDelegate = null;
        }

        private void Awake()
        {
            switch (tickMode)
            {
                case TimeTickMode.Update: _updateDelegate += Tick; break;
                case TimeTickMode.LateUpdate: _lateUpdateDelegate += Tick; break;
                case TimeTickMode.FixedUpdate: _fixedUpdateDelegate += Tick; break;
            }
        }

        public void Update()
        {
            _updateDelegate?.Invoke();
        }

        public void LateUpdate()
        {
            _lateUpdateDelegate?.Invoke();
        } 
        
        public void FixedUpdate()
        {
            _fixedUpdateDelegate?.Invoke();
        }

        void Tick()
        {
            if (paused) return;
                        
            ticker += Time.deltaTime;

            if (ticker >= timeTarget)
            {
                targetReachedEvent?.Invoke();
                
                if (clearCache) ticker = 0f;
                else ticker -= timeTarget;
                
                if (pauseOnTarget) Pause();
            }
        }

        public void ChangeTickMode(TimeTickMode newMode)
        {
            _updateDelegate = null;
            _lateUpdateDelegate = null;
            _fixedUpdateDelegate = null;
            
            tickMode = newMode;
            
            switch (tickMode)
            {
                case TimeTickMode.Update: _updateDelegate += Tick; break;
                case TimeTickMode.LateUpdate: _lateUpdateDelegate += Tick; break;
                case TimeTickMode.FixedUpdate: _fixedUpdateDelegate += Tick; break;
            }
        }

        #region Timer Controls

        /// <summary> Starts/Continues the timer. </summary>
        public void Continue() => paused = false;
        /// <summary> Temporarily pauses the timer. </summary>
        public void Pause() => paused = true;
        /// <summary> Stops the timer and clears the progress. </summary>
        public void Stop()
        {
            paused = true;
            ticker = 0f;
        }
        public void Restart()
        {
            paused = false;
            ticker = 0f;
        }

        #endregion

        /// <summary> Normalized progression of time passed. </summary>
        public float Progress => ticker / timeTarget;
        /// <summary> Time left to target. </summary>
        public float RemainingTime => timeTarget - ticker;
        /// <summary> Paused state of timer. </summary>
        public bool IsPaused => paused;
    }
}

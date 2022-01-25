using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Magthylius
{
    public abstract class TickerObject
    {
        private bool _enableLerp = true;
        public abstract void Update();

        public bool LerpEnabled => _enableLerp;

        protected TickerObject()
        {
            MagCore.Attach(this);
        }
        
        ~TickerObject()
        {
            MagCore.Detach(this);
        }
    }

    [Serializable]
    public struct LerpSettings
    {
        [Min(0f)] public float Speed;
        [Min(0f)] public float Precision;
        public bool AllowTransition;

        public LerpSettings(float speed, float precision, bool allowTransition = false)
        {
            Speed = speed;
            Precision = precision;
            AllowTransition = allowTransition;
        }
    }

    public static class MagCore
    {
        public static void Attach(TickerObject tickerObject)
        {
            MagCoreManager managerInstance = Object.FindObjectOfType<MagCoreManager>();
            
            if (managerInstance == null)
            {
                managerInstance = new GameObject("MagCore Manager").AddComponent<MagCoreManager>();
            }

            managerInstance.AddTicker(tickerObject);
        }

        public static void Detach(TickerObject tickerObject)
        {
            MagCoreManager managerInstance = Object.FindObjectOfType<MagCoreManager>();
            if (managerInstance == null)
            {
                Debug.LogWarning($"Mag core not found, unable to detach!");
                return;
            }

            managerInstance.RemoveTicker(tickerObject);
        }

        public static Timer CreateTimer(float timeTarget)
        {
            MagCoreManager managerInstance = Object.FindObjectOfType<MagCoreManager>();
            
            if (managerInstance == null)
            {
                managerInstance = new GameObject("MagCore Manager").AddComponent<MagCoreManager>();
            }
            
            Timer timer = managerInstance.gameObject.AddComponent<Timer>();
            timer.timeTarget = timeTarget;
            
            return timer;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public class MagCoreManager : MonoBehaviour
    {
        public static MagCoreManager Instance;
        private List<TickerObject> _tickerObjects = new List<TickerObject>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Update()
        {
            foreach (TickerObject tickerObject in _tickerObjects)
            {
                tickerObject.Update();
            }
            
            Debug.Log($"Ticking {_tickerObjects.Count} objects");
        }

        public void AddTicker(TickerObject tickerObject) => _tickerObjects.Add(tickerObject);
        public void RemoveTicker(TickerObject tickerObject) => _tickerObjects.Remove(tickerObject);
    }
}

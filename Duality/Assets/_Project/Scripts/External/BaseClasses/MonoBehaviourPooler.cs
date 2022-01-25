using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public class MonoBehaviourPoolable : MonoBehaviour, IPoolable
    {
        public System.Action DumpedEvent;
        public System.Action ScoopedEvent;
        public void SetName(string name) => gameObject.name = name;

        public void Dump()
        {
            gameObject.SetActive(false);
            DumpedEvent?.Invoke();
        }

        public void Scoop()
        {
            gameObject.SetActive(true);
            ScoopedEvent?.Invoke();
        }

        public bool IsActive() => gameObject.activeInHierarchy;
    }

    public class MonoBehaviourPooler<T> : ObjectPooler<T> where T : MonoBehaviourPoolable
    {
        public GameObject Prefab;

        public override void Dump(T poolableObject)
        {
            poolableObject.Dump();
        }

        public override T Scoop()
        {
            foreach (T drop in pool)
            {
                if (!drop.IsActive())
                {
                    drop.Scoop();
                    return drop;
                }
            }

            return Fill();
        }

        public override List<T> ScoopSome(int amount)
        {
            List<T> newDropList = new List<T>();
            for (int i = 0; i < amount; i++) newDropList.Add(Scoop());

            return newDropList;
        }

        protected override T Fill()
        {
            T newDrop = Instantiate(Prefab, transform).GetComponent<T>();
            pool.Add(newDrop);
            newDrop.SetName($"{Prefab.name} {pool.Count}");
            newDrop.Dump();

            return newDrop;
        }

        protected override List<T> FillSome(int amount)
        {
            List<T> newDropList = new List<T>();
            for (int i = 0; i < amount; i++) newDropList.Add(Fill());

            return newDropList;
        }
    }
}

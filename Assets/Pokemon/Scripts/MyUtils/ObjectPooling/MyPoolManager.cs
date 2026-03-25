using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.MyUtils.ObjectPooling
{
    public class MyPoolManager : Singleton<MyPoolManager>
    {
        private Dictionary<GameObject, MyPool> pools = new Dictionary<GameObject, MyPool>();
        public GameObject GetFromPool(GameObject baseObject, Transform parent = null)
        {
            if (!pools.ContainsKey(baseObject))
            {
                pools.Add(baseObject, new MyPool(baseObject, parent));
            }
            return pools[baseObject].Get(parent);
        }

        public void DeleteKey(GameObject key)
        {
            if (!pools.ContainsKey(key)) return;
            pools[key].ClearPool();
            pools.Remove(key);
        }
        public void ClearPool()
        {
            foreach (var pool in pools.Values)
            {
                pool.ClearPool();
            }
            pools.Clear();
        }
    }
}
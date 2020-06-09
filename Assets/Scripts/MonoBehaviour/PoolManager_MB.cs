using System.Collections.Generic;
using UnityEngine;

public class PoolManager_MB : MonoBehaviour
{
    private Dictionary<GameObject, List<IPoolable_MB>> _pools;

    private static PoolManager_MB _instance;
    public static PoolManager_MB Instance
    {
        get
        {
            return _instance;
        }
        set
        {
            if(_instance)
            {
                Destroy(_instance);
            }
            _instance = value;
        }
    }

    private void Awake()
    {
        Instance = this;
        _pools = new Dictionary<GameObject, List<IPoolable_MB>>();
    }

    public void CreateInstances(GameObject prefab, int ammount)
    {
        if (!_pools.ContainsKey(prefab))
        {
            if (prefab.GetComponent<IPoolable_MB>() != null)
            {
                _pools[prefab] = new List<IPoolable_MB>();
                for(var i = 0; i<ammount; i++)
                {
                    var newPoolable = Instantiate(prefab).GetComponent<IPoolable_MB>();
                    newPoolable.Available = true;
                    _pools[prefab].Add(newPoolable);
                }
            }
        }
    }

    public GameObject GetInstance(GameObject prefab,
        Vector3 position = default,
        Quaternion rotation = default)
    {
        List<IPoolable_MB> poolables;
        if(_pools.TryGetValue(prefab, out poolables))
        {
            foreach(var poolable in poolables)
            {
                if(poolable.Available)
                {
                    poolable.GO.transform.position = position;
                    poolable.GO.transform.rotation = rotation;
                    poolable.Available = false;
                    return poolable.GO;
                }
            }

            var newPoolable = Instantiate(prefab, position, rotation).GetComponent<IPoolable_MB>();
            newPoolable.Available = false;
            _pools[prefab].Add(newPoolable);
            return newPoolable.GO;
        }
        else
        {
            if(prefab.GetComponent<IPoolable_MB>() != null)
            {
                _pools[prefab] = new List<IPoolable_MB>();
                return GetInstance(prefab, position, rotation);
            }
            else
            {
                return null;
            }
        }
    }
}

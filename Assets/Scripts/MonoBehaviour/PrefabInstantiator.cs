using UnityEngine;

public class PrefabInstantiator : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab = default;
    [SerializeField]
    private int ammount = default;

    void Start()
    {
        for(var i=0; i<ammount; i++)
        {
            PoolManager_MB.Instance.CreateInstances(prefab, ammount);
        }
    }
}

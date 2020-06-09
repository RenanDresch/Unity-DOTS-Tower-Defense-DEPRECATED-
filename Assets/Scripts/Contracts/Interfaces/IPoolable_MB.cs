using UnityEngine;

public interface IPoolable_MB
{
    bool Available { get; set; }
    GameObject GO { get; }
}
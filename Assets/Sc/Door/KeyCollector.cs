using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    [SerializeField] private int totalKeys = 3;
    private int collectedKeys = 0;

    public void CollectKey()
    {
        collectedKeys++;
        Debug.Log("Keys Collected: " + collectedKeys + "/" + totalKeys);
    }

    public bool HasAllKeys()
    {
        return collectedKeys >= totalKeys;
    }
}

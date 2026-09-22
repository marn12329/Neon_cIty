using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public string playerLayer = "PlayerMain";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // เช็ค Layer ของ Player เพื่อป้องกันการตรวจสอบซ้ำซ้อนจาก Collider อื่น
        if (other.gameObject.layer == LayerMask.NameToLayer(playerLayer) && other.transform == other.transform.root)
        {
            KeyCollector keyCollector = other.GetComponent<KeyCollector>();
            if (keyCollector != null)
            {
                keyCollector.CollectKey();
                Destroy(gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  // ตรวจจับว่าชนกับผู้เล่น
        {
            collision.GetComponent<PlayerController>().Die();  // เรียกฟังก์ชันตายของผู้เล่น
        }
    }
}

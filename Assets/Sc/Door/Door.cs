using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string nextSceneName; // ใส่ชื่อ Scene ที่ต้องการไป

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            KeyCollector keyCollector = other.GetComponent<KeyCollector>();

            if (keyCollector != null)
            {
                if (keyCollector.HasAllKeys())
                {
                    Debug.Log("Door Unlocked! Loading next scene...");
                    SceneManager.LoadScene(nextSceneName);
                }
                else
                {
                    Debug.Log("You need more keys to open the door! (" + keyCollector.HasAllKeys() + " / " + keyCollector + ")");
                }
            }
        }
    }
}

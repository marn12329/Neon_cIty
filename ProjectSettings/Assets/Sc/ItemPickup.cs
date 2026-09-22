using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int energyAmount = 20; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            PlayerEnergy playerEnergy = collision.GetComponent<PlayerEnergy>();

            if (playerEnergy != null)
            {
                playerEnergy.AddEnergy(energyAmount); 
                Destroy(gameObject); 
            }
        }
    }
}

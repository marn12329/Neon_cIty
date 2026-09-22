using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum PickupType { Health, Energy, StatCoin } // เพิ่ม StatCoin
    public PickupType pickupType;
    public int restoreAmount = 20;

    public AudioClip pickupSound;  // เสียงเก็บไอเท็ม
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                switch (pickupType)
                {
                    case PickupType.Health:
                        player.Heal(restoreAmount);
                        break;

                    case PickupType.Energy:
                        player.RestoreEnergy(restoreAmount);
                        break;

                    case PickupType.StatCoin:
                        ShopSystem.Instance.AddStatCoin(restoreAmount); // ✅ ใช้ชื่อฟังก์ชันที่ถูกต้อง
                        break;
                }

                PlayPickupSound();
                gameObject.SetActive(false);
                Destroy(gameObject, 1f);
            }
        }
    }

    void PlayPickupSound()
    {
        if (pickupSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pickupSound, 1.0f);
        }
    }
}

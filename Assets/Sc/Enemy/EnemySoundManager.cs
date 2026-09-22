using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip walkSound;
    public AudioClip attackSound;
    public AudioClip hurtSound;
    public AudioClip dieSound;

    [Range(0f, 1f)] public float walkVolume = 0.2f;
    [Range(0f, 1f)] public float attackVolume = 0.4f;
    [Range(0f, 1f)] public float hurtVolume = 0.7f;
    [Range(0f, 1f)] public float dieVolume = 1.0f;

    public float maxDistance = 10f;  // ระยะห่างสูงสุดที่เสียงจะยังดัง
    private Transform player;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;  // ค้นหาผู้เล่นในฉาก
    }

    void Update()
    {
        // คำนวณระยะห่างระหว่างศัตรูและผู้เล่น
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            float volume = Mathf.Clamp01(1 - (distanceToPlayer / maxDistance));  // ระดับเสียงจะลดลงตามระยะห่าง
            audioSource.volume = volume;
        }
    }

    public void PlayWalkSound()
    {
        if (walkSound != null)
        {
            audioSource.PlayOneShot(walkSound, walkVolume);
        }
    }

    public void PlayAttackSound()
    {
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound, attackVolume);
        }
    }

    public void PlayHurtSound()
    {
        if (hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound, hurtVolume);
        }
    }

    public void PlayDieSound()
    {
        if (dieSound != null)
        {
            audioSource.PlayOneShot(dieSound, dieVolume);
        }
    }
}

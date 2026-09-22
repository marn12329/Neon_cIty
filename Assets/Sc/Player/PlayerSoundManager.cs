using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip walkSound;
    public AudioClip attackSound;

    [Range(0f, 1f)] public float walkVolume = 0.5f;  // ปรับเสียงเดิน
    [Range(0f, 1f)] public float attackVolume = 0.8f; // ปรับเสียงโจมตี

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // เรียกใช้ AudioSource ที่เราเพิ่มใน Inspector
        audioSource = GetComponent<AudioSource>();

        // เริ่มเล่นเพลงพื้นหลัง
        audioSource.Play();

        //DontDestroyOnLoad(gameObject); // ทำให้ GameObject ที่มี AudioSource อยู่ไม่หายไปเมื่อเปลี่ยนฉาก
    }

    void Update()
    {
        // สามารถเพิ่มการควบคุมเสียง (เช่น ปรับเสียง)
        if (Input.GetKeyDown(KeyCode.M)) // ถ้ากด M จะหยุดเพลง
        {
            audioSource.Pause();
        }

        if (Input.GetKeyDown(KeyCode.N)) // ถ้ากด N จะเล่นเพลง
        {
            audioSource.Play();
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // ใช้สำหรับ UI

public class PlayerEnergy : MonoBehaviour
{
    public int maxEnergy = 100; // พลังงานสูงสุด
    private int currentEnergy;  // พลังงานปัจจุบัน

    public float energyDrainRate = 5f; // ลดพลังงานทุก 5 วินาที
    public Slider energyBar; // แสดงพลังงานใน UI

    void Start()
    {
        currentEnergy = maxEnergy;
        energyBar.maxValue = maxEnergy;
        energyBar.value = currentEnergy;

        StartCoroutine(DrainEnergyOverTime());
    }

    IEnumerator DrainEnergyOverTime()
    {
        while (currentEnergy > 0)
        {
            yield return new WaitForSeconds(energyDrainRate);
            RemoveEnergy(10); // ลดพลังงานทีละ 10 ทุก 5 วินาที
        }
    }

    public void AddEnergy(int amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, maxEnergy); // ป้องกันค่าติดลบหรือเกิน max
        UpdateEnergyUI();
    }

    public void RemoveEnergy(int amount)
    {
        currentEnergy = Mathf.Max(currentEnergy - amount, 0); // ป้องกันค่าติดลบ
        UpdateEnergyUI();

        if (currentEnergy <= 0)
        {
            Die();
        }
    }

    private void UpdateEnergyUI()
    {
        if (energyBar != null)
        {
            energyBar.value = currentEnergy;
        }
    }

    void Die()
    {
        Debug.Log("พลังงานหมด! รีเซ็ตเกม...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // โหลดฉากใหม่
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnergyItem")) // ตรวจสอบว่าเป็นไอเท็มพลังงาน
        {
            AddEnergy(20); // เพิ่มพลังงาน 20
            Destroy(collision.gameObject); // ทำลายไอเท็มพลังงานหลังจากเก็บ
        }
    }
}

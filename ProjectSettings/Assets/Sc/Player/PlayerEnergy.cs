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
            yield return new WaitForSeconds(5f);
            RemoveEnergy(10); // ลดพลังงานทีละ 10 ทุก 5 วินาที
        }
    }

    public void AddEnergy(int amount)
    {
        currentEnergy += amount;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy; // ไม่ให้เกินค่าสูงสุด
        }
        energyBar.value = currentEnergy;
    }

    public void RemoveEnergy(int amount)
    {
        currentEnergy -= amount;
        energyBar.value = currentEnergy;

        if (currentEnergy <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("พลังงานหมด! รีเซ็ตเกม...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // โหลดฉากใหม่
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public string characterName; // ชื่อของตัวละคร
    public int maxHealth = 100;
    public int currentHealth;
    public int attackPower = 10;
    public int defense = 5;

    [HideInInspector] public int baseAttackPower;
    [HideInInspector] public int baseDefense;

    void Start()
    {
        currentHealth = maxHealth;
        baseAttackPower = attackPower;
        baseDefense = defense;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Damage before defense: " + damage);
        int damageTaken = Mathf.Max(damage - defense, 1);
        Debug.Log("Damage after defense: " + damageTaken);
        currentHealth -= damageTaken;
        Debug.Log(characterName + " โดนโจมตี! พลังชีวิตเหลือ: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        Debug.Log(characterName + " ฟื้นฟูพลังชีวิต! ตอนนี้มี: " + currentHealth);
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    private void Die()
    {
        Debug.Log(characterName + " ตายแล้ว!");
        gameObject.SetActive(false);
    }
}

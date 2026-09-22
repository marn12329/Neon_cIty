using System.Collections;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public float skillCooldown = 5f;
    public float damagePercentageBoost = 0.2f;
    public float effectDuration = 10f;

    private float lastUseTime = -10f;
    private CharacterStats characterStats;
    private bool isSkillActive = false;

    // ✅ เพิ่ม property 'cooldown' เพื่อให้ UpgradeSystem เข้าถึงได้
    public float cooldown
    {
        get { return skillCooldown; }
        set { skillCooldown = Mathf.Max(0.5f, value); }
    }

    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    public void UseSkill()
    {
        if (Time.time - lastUseTime < skillCooldown || isSkillActive) return;

        lastUseTime = Time.time;
        if (!isSkillActive)
        {
            StartCoroutine(ActivateSkillEffects());
        }
    }

    private IEnumerator ActivateSkillEffects()
    {
        isSkillActive = true;

        int originalAttack = characterStats.attackPower;
        characterStats.attackPower = (int)(characterStats.baseAttackPower * (1 + damagePercentageBoost));

        Debug.Log("✅ Skill: Attack Power Boosted!");

        yield return new WaitForSeconds(effectDuration);

        characterStats.attackPower = characterStats.baseAttackPower;
        isSkillActive = false;

        Debug.Log("❎ Skill Effect Ended.");
    }

    public void UpgradeDamageBoost(float amount)
    {
        damagePercentageBoost += amount;
    }

    public void ReduceCooldown(float amount)
    {
        skillCooldown = Mathf.Max(0.5f, skillCooldown - amount);
    }
}

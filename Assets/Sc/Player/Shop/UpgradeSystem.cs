using UnityEngine;
using TMPro;

public class UpgradeSystem : MonoBehaviour
{
    public TMP_Text statCoinText, statCoinText_2;
    public TMP_Text skillCoinText, skillCoinText_2;

    public TMP_Text maxHealthText, attackPowerText, defenseText;
    public TMP_Text skillCooldownText, fireballCooldownText, skillDamageBoostText;
    public TMP_Text maxHealthCostText, attackCostText, defenseCostText;
    public TMP_Text skillDamageCostText, skillCooldownCostText;

    public int healthUpgradeCost = 1, attackUpgradeCost = 1, defenseUpgradeCost = 1;
    public int skillDamageUpgradeCost = 2, skillCooldownUpgradeCost = 2;

    public float skillDamageBoostAmount = 0.1f;
    public float cooldownReduceAmount = 0.5f;

    private CharacterStats characterStats;
    private PlayerSkills playerSkills;
    private FireballSkill fireballSkill;

    void Start()
    {
        Invoke(nameof(AssignPlayerReferences), 0.2f);
    }

    void AssignPlayerReferences()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            characterStats = player.GetComponent<CharacterStats>();
            playerSkills = player.GetComponent<PlayerSkills>();
            fireballSkill = player.GetComponent<FireballSkill>();
        }

        UpdateAllUI();
    }

    public void UpgradeMaxHealth()
    {
        if (ShopSystem.Instance.SpendStatCoins(healthUpgradeCost) && characterStats)
        {
            characterStats.maxHealth += 10;
            characterStats.currentHealth += 10;

            PlayerController pc = characterStats.GetComponent<PlayerController>();
            if (pc != null)
                pc.RefreshHealthBar();

            UpdateAllUI();
        }
    }

    public void UpgradeAttack()
    {
        if (ShopSystem.Instance.SpendStatCoins(attackUpgradeCost) && characterStats)
        {
            characterStats.attackPower += 2;
            characterStats.baseAttackPower += 2;
            UpdateAllUI();
        }
    }

    public void UpgradeDefense()
    {
        if (ShopSystem.Instance.SpendStatCoins(defenseUpgradeCost) && characterStats)
        {
            characterStats.defense += 2;
            characterStats.baseDefense += 2;
            UpdateAllUI();
        }
    }

    public void UpgradeSkillDamage()
    {
        if (ShopSystem.Instance.SpendSkillCoins(skillDamageUpgradeCost))
        {
            if (playerSkills)
                playerSkills.UpgradeDamageBoost(skillDamageBoostAmount);

            if (fireballSkill)
                fireballSkill.UpgradeDamageBoost(skillDamageBoostAmount);

            UpdateAllUI();
        }
    }

    public void UpgradeSkillCooldown()
    {
        if (ShopSystem.Instance.SpendSkillCoins(skillCooldownUpgradeCost))
        {
            if (playerSkills)
                playerSkills.ReduceCooldown(cooldownReduceAmount);

            if (fireballSkill)
                fireballSkill.ReduceCooldown(cooldownReduceAmount);

            UpdateAllUI();
        }
    }

    // ✅ เปลี่ยนจาก private เป็น public
    public void UpdateAllUI()
    {
        // Coins
        int statCoins = ShopSystem.Instance.statCoin;
        int skillCoins = ShopSystem.Instance.skillCoin;

        statCoinText.text = statCoins.ToString();
        statCoinText_2.text = statCoins.ToString();

        skillCoinText.text = skillCoins.ToString();
        skillCoinText_2.text = skillCoins.ToString();

        // Stats
        if (characterStats)
        {
            maxHealthText.text = "Max Health: " + characterStats.maxHealth;
            attackPowerText.text = "Attack Power: " + characterStats.attackPower;
            defenseText.text = "Defense: " + characterStats.defense;
        }
        else
        {
            maxHealthText.text = "Max Health: N/A";
            attackPowerText.text = "Attack Power: N/A";
            defenseText.text = "Defense: N/A";
        }

        // Skill info
        if (playerSkills)
        {
            skillCooldownText.text = "Cooldown: " + playerSkills.cooldown.ToString("0.0") + "s";
            skillDamageBoostText.text = "Damage Boost: +" + (playerSkills.damagePercentageBoost * 100).ToString("0") + "%";
        }
        else if (fireballSkill)
        {
            skillCooldownText.text = "Cooldown: " + fireballSkill.cooldown.ToString("0.0") + "s";
            skillDamageBoostText.text = "Damage Boost: +" + (fireballSkill.damagePercentageBoost * 100).ToString("0") + "%";
        }
        else
        {
            skillCooldownText.text = "Cooldown: N/A";
            skillDamageBoostText.text = "Damage Boost: N/A";
        }

        // Fireball cooldown
        if (fireballSkill)
        {
            fireballCooldownText.text = "Cooldown: " + fireballSkill.cooldown.ToString("0.0") + "s";
        }
        else if (playerSkills)
        {
            fireballCooldownText.text = "Cooldown: " + playerSkills.cooldown.ToString("0.0") + "s";
        }
        else
        {
            fireballCooldownText.text = "Cooldown: N/A";
        }

        // Upgrade costs
        maxHealthCostText.text = healthUpgradeCost.ToString();
        attackCostText.text = attackUpgradeCost.ToString();
        defenseCostText.text = defenseUpgradeCost.ToString();
        skillDamageCostText.text = skillDamageUpgradeCost.ToString();
        skillCooldownCostText.text = skillCooldownUpgradeCost.ToString();

        // Color indicators
        SetTextColor(maxHealthCostText, statCoins >= healthUpgradeCost);
        SetTextColor(attackCostText, statCoins >= attackUpgradeCost);
        SetTextColor(defenseCostText, statCoins >= defenseUpgradeCost);
        SetTextColor(skillDamageCostText, skillCoins >= skillDamageUpgradeCost);
        SetTextColor(skillCooldownCostText, skillCoins >= skillCooldownUpgradeCost);
    }

    void SetTextColor(TMP_Text text, bool canAfford)
    {
        text.color = canAfford ? Color.white : Color.red;
    }
}

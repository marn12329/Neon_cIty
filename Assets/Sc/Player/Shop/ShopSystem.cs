using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    public static ShopSystem Instance;

    public int statCoin = 0;
    public int skillCoin = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool SpendStatCoins(int amount)
    {
        if (statCoin >= amount)
        {
            statCoin -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    public bool SpendSkillCoins(int amount)
    {
        if (skillCoin >= amount)
        {
            skillCoin -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    public void AddStatCoin(int amount)
    {
        statCoin += amount;
        UpdateUI();
    }

    public void AddSkillCoin(int amount)
    {
        skillCoin += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        UpgradeSystem upgradeSystem = FindObjectOfType<UpgradeSystem>();
        if (upgradeSystem != null)
        {
            upgradeSystem.UpdateAllUI();
        }
    }
}

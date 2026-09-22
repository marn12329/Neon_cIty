using UnityEngine;

public class FireballSkill : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float fireballSpeed = 10f;
    public Transform firePoint;

    private float lastFireTime = 0f;
    public float fireballCooldown = 3f;
    public float damagePercentageBoost = 0f;

    private CharacterStats characterStats;

    // ✅ Property สำหรับใช้งานร่วมกับ UpgradeSystem
    public float cooldown
    {
        get { return fireballCooldown; }
        set { fireballCooldown = Mathf.Max(0.5f, value); } // ไม่ให้ต่ำกว่า 0.5
    }

    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    public void CastFireball()
    {
        if (Time.time - lastFireTime < fireballCooldown) return;

        lastFireTime = Time.time;

        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        rb.velocity = direction * fireballSpeed;

        // ✅ ส่งดาเมจพร้อมบูสต์ไปที่กระสุน
        Projectile projectile = fireball.GetComponent<Projectile>();
        if (projectile != null && characterStats != null)
        {
            int boostedDamage = (int)(characterStats.attackPower * (1 + damagePercentageBoost));
            projectile.SetDamage(boostedDamage);
        }

        Debug.Log("🔥 Fireball Casted!");
    }

    // ✅ รองรับการอัปเกรดสกิล
    public void UpgradeDamageBoost(float amount)
    {
        damagePercentageBoost += amount;
    }

    public void ReduceCooldown(float amount)
    {
        fireballCooldown = Mathf.Max(0.5f, fireballCooldown - amount);
    }
}

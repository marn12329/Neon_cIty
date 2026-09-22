using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;

    // เรียกจาก RangedCharacter เพื่อส่งค่า attackPower เข้ามา
    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ชนศัตรู
        if (other.CompareTag("Enemy"))
        {
            CharacterStats enemyStats = other.GetComponent<CharacterStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        // ชนกำแพงหรือพื้น
        else if (other.gameObject.layer == LayerMask.NameToLayer("Wall") ||
                 other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}

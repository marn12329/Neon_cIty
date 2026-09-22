using System.Collections;
using UnityEngine;

public class RangedCharacter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float attackCooldown = 1f;

    private bool canAttack = true;
    private CharacterStats characterStats;

    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            Attack();
        }
    }

    void Attack()
    {
        canAttack = false;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * projectileSpeed;

        // ส่งค่า damage จาก CharacterStats ไปที่ projectile
        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null && characterStats != null)
        {
            projScript.SetDamage(characterStats.attackPower);
        }

        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}

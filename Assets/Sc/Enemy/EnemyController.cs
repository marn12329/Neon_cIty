using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    public int health = 50;
    public int damage = 10; // ปรับดาเมจศัตรูให้สามารถแก้ไขได้
    public float moveDistance = 5f;
    public float attackRange = 1f; // เพิ่มระยะโจมตี
    public float detectRange = 5f; // ระยะตรวจจับผู้เล่น
    public float attackCooldown = 2f;

    private Vector3 startPosition;
    private bool movingRight = true;
    private Transform player;
    private Animator animator;
    private float lastAttackTime = 0f;
    private bool isChasing = false;
    private bool isAttacking = false;

    public Transform attackPoint; // จุดโจมตี
    public float attackRadius = 0.5f; // รัศมีโจมตี
    public LayerMask playerLayer; // Layer ของผู้เล่น

    private EnemySoundManager soundManager;

    // ตัวคูณดาเมจจากการชน
    public float collisionDamageMultiplier = 1f; // ตัวคูณดาเมจจากการชน

    void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        soundManager = GetComponent<EnemySoundManager>(); // หาตัว EnemySoundManager
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange) 
            {
                isChasing = false;
                isAttacking = true;
                Attack();
            }
            else if (distanceToPlayer <= detectRange)
            {
                isChasing = true;
                isAttacking = false;
                ChasePlayer();
            }
            else
            {
                isChasing = false;
                isAttacking = false;
                Patrol();
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (!isAttacking) 
        {
            animator.SetBool("IsWalking", true);
        }

        if (movingRight)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition + Vector3.right * moveDistance, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, startPosition + Vector3.right * moveDistance) < 0.1f)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, startPosition) < 0.1f)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void ChasePlayer()
    {
        if (!isAttacking) 
        {
            animator.SetBool("IsWalking", true);
        }

        if (player.position.x > transform.position.x)
        {
            movingRight = true;
        }
        else
        {
            movingRight = false;
        }

        Flip();
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    public void Attack()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            isAttacking = true;
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("Attack");

            StartCoroutine(DealDamage());
        }
    }

    IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(0.5f); // รอให้อนิเมชั่นโจมตีออกไปข้างหน้า

        // คำนวณดาเมจจากการชน
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);
        foreach (Collider2D playerCollider in hitPlayers)
        {
            int finalDamage = Mathf.RoundToInt(damage * collisionDamageMultiplier); 
            Debug.Log("Enemy hit player! Damage: " + finalDamage);
            playerCollider.GetComponent<PlayerController>().TakeDamage(finalDamage);
        }

        isAttacking = false;
    }

    void Flip()
    {
        transform.eulerAngles = new Vector3(0, movingRight ? 0 : 180, 0);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        animator.SetTrigger("Hurt");

        soundManager.PlayHurtSound();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        Destroy(gameObject, 0.5f);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }

    // ฟังก์ชันที่ใช้ในการจัดการการชน
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // คำนวณดาเมจจากการชน
            int finalDamage = Mathf.RoundToInt(damage * collisionDamageMultiplier);
            collision.GetComponent<PlayerController>().TakeDamage(finalDamage);
        }
    }
}

using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    public int health = 50;
    public int damage = 20;
    public float moveDistance = 5f;
    public float attackRange = 0.1f; // ระยะโจมตีที่ปรับใหม่
    public float detectRange = -0.5f;   // ระยะตรวจจับผู้เล่น
    public float attackCooldown = 10f; // คูลดาวน์โจมตี

    private Vector3 startPosition;
    private bool movingRight = true;
    private Transform player;
    private Animator animator;
    private float lastAttackTime = 0f;
    private bool isChasing = false;
    private bool isAttacking = false;

    void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange) // อยู่ในระยะโจมตี
            {
                isChasing = false;
                isAttacking = true;
                Attack();
            }
            else if (distanceToPlayer <= detectRange) // อยู่ในระยะตรวจจับ แต่ยังไม่ถึงโจมตี
            {
                isChasing = true;
                isAttacking = false;
                ChasePlayer();
            }
            else // ผู้เล่นออกจากระยะ -> กลับไปเดินปกติ
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
        if (!isAttacking) // ป้องกันเปลี่ยนท่าระหว่างโจมตี
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
        if (!isAttacking) // ป้องกันเปลี่ยนท่าระหว่างโจมตี
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

    void Attack()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            isAttacking = true;
            animator.SetBool("IsWalking", false); // หยุดเดินขณะโจมตี
            animator.SetTrigger("Attack");

            // รอให้แอนิเมชันโจมตีจบก่อนจะกลับไปไล่ล่าหรือเดิน
            StartCoroutine(ResetAttackState());
        }
    }

    IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(attackCooldown); // รอให้คูลดาวน์โจมตีเสร็จ
        isAttacking = false;
        animator.ResetTrigger("Attack");  // รีเซ็ต Trigger เพื่อป้องกันค้าง
    }

    void Flip()
    {
        transform.eulerAngles = new Vector3(0, movingRight ? 0 : 180, 0);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        animator.SetTrigger("Hurt");

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}

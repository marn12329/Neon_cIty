using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // ใช้สำหรับ UI

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    public float jumpSpeed = 9f;
    public float maxSpeed = 10f;
    public float JumpPower = 20f;
    public bool grounded;
    public float fireRate = 0.2f;
    private float nextFireRate = 0.0f;

    private Rigidbody2D rigidBody2D;
    private Animator animator;

    // ระบบเลือดของผู้เล่น
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    // เชื่อมต่อกับระบบพลังงาน
    private PlayerEnergy playerEnergy;

    // สำหรับระบบดับเบิ้ลจั้ม
    public int maxJumpCount = 2;
    private int jumpCount = 0;

    // สำหรับระบบแดช
    public float dashSpeed = 15f;
    public float dashTime = 0.3f;
    private bool isDashing = false;
    private float dashTimeRemaining = 0f;
    private float dashCooldown = 1f;
    private float lastDashTime = -10f;

    // เชื่อมกับ UI Health Bar
    public Slider healthBar;

    // สำหรับการโจมตี
    public int attackDamage = 10;  // ความเสียหายจากการโจมตี

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    void Update()
    {
        if (isDead) return;

        animator.SetBool("Grounded", grounded);
        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));

        if (!isDashing)
        {
            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                transform.eulerAngles = new Vector2(0, 180);
            }
            else if (Input.GetAxis("Horizontal") > 0.1f)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                transform.eulerAngles = new Vector2(0, 0);
            }
        }

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            animator.SetBool("Jump", true);
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0);
            rigidBody2D.AddForce(jumpSpeed * (Vector2.up * JumpPower));
            jumpCount++;
        }

        if (rigidBody2D.velocity.y <= 0)
        {
            animator.SetBool("Jump", false);
        }

        // ตรวจจับการโจมตี
        if (Input.GetMouseButton(0) && Time.time > nextFireRate)
        {
            nextFireRate = Time.time + fireRate;
            animator.SetBool("Attack", true);
            Attack();  // เรียกฟังก์ชันโจมตี
        }
        else
        {
            animator.SetBool("Attack", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDashTime + dashCooldown && !isDashing)
        {
            Dash();
        }

        if (isDashing)
        {
            dashTimeRemaining -= Time.deltaTime;
            if (dashTimeRemaining <= 0f)
            {
                isDashing = false;
                animator.SetBool("Dash", false);
            }
        }
    }

    private void Dash()
    {
        isDashing = true;
        dashTimeRemaining = dashTime;
        lastDashTime = Time.time;
        animator.SetBool("Dash", true);

        float direction = Input.GetAxis("Horizontal");
        if (direction != 0)
        {
            rigidBody2D.velocity = new Vector2(direction * dashSpeed, rigidBody2D.velocity.y);
        }
        else
        {
            rigidBody2D.velocity = new Vector2(dashSpeed, rigidBody2D.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // ถ้าชนศัตรู
        {
            TakeDamage(5);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        healthBar.value = currentHealth; // อัปเดตค่า Health Bar

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(RestartAfterDelay(2f));
    }

    IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ฟังก์ชันการโจมตี
    private void Attack()
    {
        // ค้นหาศัตรูในพื้นที่ที่กำหนด
        float attackRadius = 0.5f;  // ระยะโจมตีที่ต้องการให้เหมาะสม
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))  // ถ้าชนกับศัตรู
            {
                // เพิ่มการ Debug เพื่อเช็คว่าได้เจอศัตรูหรือไม่
               Debug.Log("Enemy Hit: " + enemy.name);
               enemy.GetComponent<EnemyController>().TakeDamage(attackDamage);  // เรียกฟังก์ชัน TakeDamage ของศัตรู
            }
        }
    }
    

    public void Heal(int amount)
    {
        if (isDead) return;


        currentHealth += amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;

        }

        healthBar.value = currentHealth;
        Debug.Log("เพิ่มเลือดเเล้ว:" + amount +"/Hpตอนนี้"+currentHealth);
    
    }
}

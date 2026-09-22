using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 9f;
    public float maxSpeed = 10f;
    public float JumpPower = 20f;
    public bool grounded;

    private Rigidbody2D rigidBody2D;
    private Animator animator;
    private CharacterStats characterStats;
    private FireballSkill fireballSkill;
    private PlayerSkills swordSkill;

    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    public int maxEnergy = 100;
    private int currentEnergy;

    public int maxJumpCount = 2;
    private int jumpCount = 0;

    public float dashSpeed = 15f;
    public float dashTime = 0.3f;
    private bool isDashing = false;
    private float dashCooldown = 1f;
    private float lastDashTime = -10f;

    public Slider healthBar;

    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;

    [Header("UI Game Over")]
    public GameObject gameOverPanel;
    public Button retryButton;

    private bool isSwordOnCooldown = false;
    private bool isFireballOnCooldown = false;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        fireballSkill = GetComponent<FireballSkill>();
        swordSkill = GetComponent<PlayerSkills>();

        maxHealth = characterStats.maxHealth;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (retryButton != null)
            retryButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        if (isDead) return;

        //animator.SetBool("Grounded", grounded);
        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));

        if (!isDashing)
        {
            Move();
        }

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDashTime + dashCooldown && !isDashing)
        {
            Dash();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            UseSwordSkill();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            UseFireballSkill();
        }
    }

    private void Move()
    {
        float move = Input.GetAxis("Horizontal");
        rigidBody2D.velocity = new Vector2(move * speed, rigidBody2D.velocity.y);

        if (move < -0.1f)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (move > 0.1f)
            transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void Jump()
    {
        animator.SetBool("Jump", true);
        rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0);
        rigidBody2D.AddForce(jumpSpeed * (Vector2.up * JumpPower));
        jumpCount++;
    }

    private void Dash()
    {
        if (isDashing) return;
        isDashing = true;
        lastDashTime = Time.time;
        animator.SetBool("Dash", true);

        float direction = Input.GetAxis("Horizontal");
        if (direction == 0) direction = transform.eulerAngles.y == 0 ? 1 : -1;

        StartCoroutine(PerformDash(direction));
    }

    private IEnumerator PerformDash(float direction)
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            rigidBody2D.velocity = new Vector2(direction * dashSpeed, rigidBody2D.velocity.y);
            yield return null;
        }

        isDashing = false;
        animator.SetBool("Dash", false);
    }

    private void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
            StartCoroutine(ResetAttackAnimation());
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(characterStats.attackPower);
            }
        }
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        animator.ResetTrigger("Attack");
    }

    private void UseSwordSkill()
    {
        if (swordSkill != null)
        {
            swordSkill.UseSkill();
        }
    }

    private void UseFireballSkill()
    {
        if (fireballSkill != null)
        {
            fireballSkill.CastFireball();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            jumpCount = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            int enemyDamage = 0;

            CharacterStats enemyStats = collision.GetComponent<CharacterStats>();
            if (enemyStats != null)
            {
                enemyDamage = enemyStats.attackPower;
            }
            else
            {
                EnemyController enemyController = collision.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyDamage = enemyController.damage;
                }
            }

            if (enemyDamage > 0)
            {
                TakeDamage(enemyDamage);
            }
        }

        if (collision.CompareTag("HealthItem"))
        {
            Heal(20);
            collision.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        healthBar.value = currentHealth;
        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.simulated = false;
        GetComponent<Collider2D>().enabled = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestoreEnergy(int amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        Debug.Log("Energy Restored: " + amount);
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthBar.value = currentHealth;
        Debug.Log("เพิ่มเลือดแล้ว: " + amount + " / Hp ตอนนี้: " + currentHealth);
    }

    public void RefreshHealthBar()
    {
        maxHealth = characterStats.maxHealth;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
}

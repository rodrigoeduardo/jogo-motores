using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoss : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private float attackVariationInterval;

    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform player;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer = Mathf.Infinity;
    private float attackVariationTimer = 0;
    private float initialYPosition;
    private bool isAttacking = false;  // Flag to check if the boss is attacking
    private bool isActive = false; // Flag to check if the boss is active

    // References
    private Animator anim;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        initialYPosition = transform.position.y;
        DisableBoss(); // Disable the boss initially
    }

    private void Update()
    {
        if (!isActive) return; // If the boss is not active, do nothing

        cooldownTimer += Time.deltaTime;
        attackVariationTimer += Time.deltaTime;

        // Check if the player is in sight
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                PerformAttack();
            }
        }
        else
        {
            // Move towards the player if not in range
            MoveTowardsPlayer();
        }
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    private void MoveTowardsPlayer()
    {
        if (isAttacking) return;

        anim.SetBool("IsRunning", true);

        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 targetPosition = new Vector3(transform.position.x + direction.x * moveSpeed * Time.deltaTime, initialYPosition, transform.position.z);
        rb.MovePosition(targetPosition);

        // Flip the boss's sprite to face the player
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void PerformAttack()
    {
        anim.SetBool("IsRunning", false);
        isAttacking = true;

        if (attackVariationTimer >= attackVariationInterval)
        {
            int attackType = Random.Range(0, 3);
            switch (attackType)
            {
                case 0:
                    anim.SetTrigger("Attack1");
                    break;
                case 1:
                    anim.SetTrigger("Attack2");
                    break;
                case 2:
                    anim.SetTrigger("Attack3");
                    break;
            }
            attackVariationTimer = 0;
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isAttacking) return;

        anim.SetTrigger("Hit");
        enemyHealth.TakeDamage(damageAmount);
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<PlayerHealth>();

        return hit.collider != null;
    }

    private void DamagePlayer()
    {
        if (PlayerInSight() && playerHealth != null)
            playerHealth.TakeDamage(damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public void ActivateBoss()
    {
        isActive = true;
        anim.SetTrigger("BossActivate"); // Trigger boss activation animation
    }

    public void DisableBoss()
    {
        isActive = false;
        anim.SetBool("IsRunning", false);
    }
}

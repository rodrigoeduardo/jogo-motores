using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
     [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private CapsuleCollider2D boxCollider;
    [SerializeField] private float colliderDistance;

    [Header("Enemy Layer")]
    [SerializeField] private LayerMask enemyLayer;

    private float cooldownTimer = Mathf.Infinity;
    private Animator anim;
    private EnemyHealth enemyHealth;
    private BossHealth bossHealth;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // Verifica se o jogador quer atacar e se o cooldown já passou
        if (Input.GetKeyDown(KeyCode.Mouse0) && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            anim.SetTrigger("Attack");
        }
    }

    // Este método deve ser chamado no evento da animação de ataque
    private void DamageEnemy()
{
    if (EnemyInSight())
    {
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
        else if (bossHealth != null)
        {
            bossHealth.TakeDamage(damage);
        }
    }
}

private bool EnemyInSight()
{
    RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
        0, Vector2.left, 0, enemyLayer);

    if (hit.collider != null)
    {
        enemyHealth = hit.transform.GetComponent<EnemyHealth>();
        bossHealth = hit.transform.GetComponent<BossHealth>();

        return enemyHealth != null || bossHealth != null;
    }

    return false;
}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}

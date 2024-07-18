using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;

    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int attackDamage = 20;
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    private bool facingRight = true;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButton(0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        // Flip attack point based on player direction
        if (Input.GetAxisRaw("Horizontal") > 0 && !facingRight)
        {
            Flip();
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && facingRight)
        {
            Flip();
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(attackDamage);
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = attackPoint.localPosition;
        scaler.x *= -1;
        attackPoint.localPosition = scaler;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

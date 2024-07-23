using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed;

    public float endX;
    private float startX;
    private float left;
    private float right;
    private bool movingRight;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        startX = transform.position.x;

        left = startX;
        right = endX;
        movingRight = true;

        if (startX - endX > 0f)
        {
            left = endX;
            right = startX;
            movingRight = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (isDead) return;

        if (movingRight)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            sr.flipX = false;
            if (transform.position.x >= right)
            {
                movingRight = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            sr.flipX = true;
            if (transform.position.x <= left)
            {
                movingRight = true;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        Debug.Log("Damage taken");
        anim.SetTrigger("Hit");

        if (health <= 0)
        {
            isDead = true;
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        anim.SetTrigger("Death");

        // Wait for the length of the death animation
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        Destroy(gameObject);
    }
}

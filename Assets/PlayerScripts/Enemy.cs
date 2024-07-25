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
    
    private bool isKnockback;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D cl;
    private bool isDead = false;

    public float kbForce = 1f;
    public float kbCount;
    public float kbTime = 0.2f;

    public bool isKnockbackRight;

    void Start()
    {
        cl = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.isKinematic = true;
        startX = transform.position.x;

        left = startX;
        right = endX;
        movingRight = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        KnockLogic();
    }

    void Patrol(){
        if (startX - endX > 0f)
        {
            left = endX;
            right = startX;
            movingRight = false;
        }
    }

   void KnockLogic(){
    if(kbCount < 0 || isDead){
        HandleMovement();
        Patrol();   
    }

    else{
        if(isKnockbackRight){
            rb.velocity = new Vector2(-kbForce, 0);
        }
        if(!isKnockbackRight){
            rb.velocity = new Vector2(kbForce,0);
        }
    }
        kbCount -= Time.deltaTime;
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

    public void TakeDamage(int damage, Transform player)
    {
        if (isDead) return;
        kbCount = kbForce;
        if(player.transform.position.x <= transform.position.x){
                isKnockbackRight = false;
            }

        if(player.transform.position.x >= transform.position.x){
            isKnockbackRight = true;
        }

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
        rb.velocity = new Vector2(0,0);

        gameObject.tag = "Dead";
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length-0.2f);

        Destroy(gameObject);
    }
}

using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private Rigidbody2D rb;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;
    private EnemyPatrol enemyPatrol;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    public void TakeDamage(float _damage)
{
    if (invulnerable) return;
    currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

    if (currentHealth > 0)
    {
        anim.SetTrigger("Hit");
        StartCoroutine(Invunerability());
        //SoundManager.instance.PlaySound(hurtSound);
    }
    else if (currentHealth <= 0 && !dead)
    {
        dead = true;

        // Desabilita o patrulhamento e outros comportamentos
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = false;
        }

        // Desativa outros componentes (como o ataque)
        foreach (var component in components)
        {
            component.enabled = false;
        }

        StartCoroutine(Die());
    }
}

    private IEnumerator Die()
    {
        anim.SetTrigger("Dead");
        rb.velocity = Vector2.zero;

        gameObject.tag = "Dead";
        gameObject.layer = LayerMask.NameToLayer("Dead");


        // Espera a animação de morte terminar antes de destruir o objeto
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        Destroy(transform.parent.gameObject);
    }

    public void AddHealth(float _value)
    {
        if (dead) return;
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }
}
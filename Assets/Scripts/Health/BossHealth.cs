using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public int startingHealth;
    public int currentHealth { get; private set; }

    public BossHealthBar bossHealthBar;
    private Animator anim;
    private Rigidbody2D rb;
    private bool dead; // Ensure this is set to false initially

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        bossHealthBar.SetMaxHealth(startingHealth);
        dead = false;
    }

    public void TakeDamage(int _damage)
    {
        // Prevent taking damage if already dead
        if (dead || invulnerable) return;

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        bossHealthBar.SetHealth(currentHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hit");
            StartCoroutine(Invulnerability());
        }
        else
        {
            dead = true;

            // Disable other components (like attack)
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

        // Play death sound
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        // Wait for the death animation to finish before destroying the object
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - 0.2f);

        Destroy(transform.parent.gameObject);
    }

    private IEnumerator Invulnerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(6, 8, true);

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(6, 8, false);
        invulnerable = false;
    }

    public void AddHealth(int _value)
    {
        if (dead) return;
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
        bossHealthBar.SetHealth(currentHealth);
    }
}

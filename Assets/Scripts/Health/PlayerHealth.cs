using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
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

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    [Header("Knockback")]
    public float kbTime; // Duração do knockback
    public float kbCount; // Contador do knockback
    public bool isKnockbackRight; // Direção do knockback

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Threats" || other.gameObject.CompareTag("Enemy"))
        {
            if (invulnerable) return;

            // Configuração do Knockback
            kbCount = kbTime;
            if (other.transform.position.x <= transform.position.x)
            {
                isKnockbackRight = false;
            }
            else
            {
                isKnockbackRight = true;
            }

            anim.SetTrigger("Hit");
            TakeDamage(1);
        }
        else if (other.gameObject.name == "Instakill")
        {
            TakeDamage(startingHealth);
        }
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
        if(currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        anim.SetTrigger("die");
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + 0.4f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddHealth(float _value)
    {
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

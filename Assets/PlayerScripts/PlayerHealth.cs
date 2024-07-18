using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public Animator anim;
    public int maxHealth;
    public int currentHealth;
    public bool isInvulnerable = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        anim.SetTrigger("Hit");
        currentHealth -= damage;

        StartInvulnerability();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void StartInvulnerability()
    {
        isInvulnerable = true;

        SpriteRenderer sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        Color halfTransparent = sr.color;
        halfTransparent.a = 0.5f;

        sr.color = halfTransparent;

        Invoke(nameof(EndInvulnerability), 2f);
    }

    void EndInvulnerability()
    {
        isInvulnerable = false;

        SpriteRenderer sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        Color normal = sr.color;
        normal.a = 1f;
        sr.color = normal;
    }

    void Die()
    {
        anim.SetTrigger("Death");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.name == "Threats")
        {
            if (isInvulnerable) return;

            TakeDamage(1);

            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            // Verifica se o Rigidbody2D não é nulo
            if (rb != null)
            {
                // Calcula a direção oposta à da colisão
                Vector2 oppositeForce = -other.contacts[0].normal * 100f; // Ajuste a magnitude da força conforme necessário

                // Aplica a força contrária
                rb.AddForce(oppositeForce, ForceMode2D.Impulse);
            }
        }
    }
}

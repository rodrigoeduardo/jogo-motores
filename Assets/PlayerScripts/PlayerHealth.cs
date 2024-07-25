using System;
using System.Collections;
using System.Collections.Generic;
using TarodevController;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public Animator anim;
    public int maxHealth;
    public int currentHealth;
    public bool isInvulnerable = false;
    private Rigidbody2D rb;
    public PlayerController player;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
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
        Color transparent = sr.color;
        transparent.a = 0.1f;

        sr.color = transparent;

        Invoke(nameof(EndInvulnerability), 0.5f);
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Threats" || other.gameObject.CompareTag("Enemy"))
        {
            if (isInvulnerable) return;
            player.kbCount = player.kbTime;
            if(other.transform.position.x <= transform.position.x){
                player.isKnockbackRight = false;
            }

            if(other.transform.position.x >= transform.position.x){
                player.isKnockbackRight = true;
            }
            anim.SetTrigger("Hit");
            TakeDamage(1);
        }

        else if(other.gameObject.name == "Instakill"){
            TakeDamage(maxHealth);
        }
    }
}

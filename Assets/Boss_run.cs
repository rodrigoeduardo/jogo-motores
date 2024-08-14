using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_run : StateMachineBehaviour
{
    public float speed = 2.5f;
	public float attackRange = 3f;

    private float cooldownTimer = 0f; // Tempo acumulado desde o último ataque
    public float attackCooldown = 2f; // Cooldown entre ataques (ajuste conforme necessário)

    private float attackVariationTimer = 0;
    [SerializeField] private float attackVariationInterval = 1f; // Tempo entre as variações de ataques

	Transform player;
	Rigidbody2D rb;
	Boss boss;

    // OnStateEnter é chamado quando a transição começa e a máquina de estado começa a avaliar esse estado
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		rb = animator.GetComponent<Rigidbody2D>();
		boss = animator.GetComponent<Boss>();

        cooldownTimer = attackCooldown; // Inicia o cooldown para evitar ataques imediatos ao entrar no estado
	}

	// OnStateUpdate é chamado a cada quadro entre OnStateEnter e OnStateExit
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		boss.LookAtPlayer();

		Vector2 target = new Vector2(player.position.x, rb.position.y);
		Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
		rb.MovePosition(newPos);

        cooldownTimer += Time.deltaTime;

		if (Vector2.Distance(player.position, rb.position) <= attackRange && cooldownTimer >= attackCooldown)
		{
			attackVariationTimer += Time.deltaTime;

            if (attackVariationTimer >= attackVariationInterval)
            {
                int attackType = Random.Range(0, 3); // Random.Range inclui 0, 1 e 2

                switch (attackType)
                {
                    case 0:
                        animator.SetTrigger("Attack1");
                        break;
                    case 1:
                        animator.SetTrigger("Attack2");
                        break;
                    case 2:
                        animator.SetTrigger("Attack3");
                        break;
                }

                attackVariationTimer = 0;
                cooldownTimer = 0f; // Reseta o cooldown após realizar o ataque
            }
		}
	}

	// OnStateExit é chamado quando uma transição termina e a máquina de estado termina de avaliar esse estado
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.ResetTrigger("Attack1");
		animator.ResetTrigger("Attack2");
		animator.ResetTrigger("Attack3");
	}
}

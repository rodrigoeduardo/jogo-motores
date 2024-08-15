using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [Header("Boss Components")]
    [SerializeField] private GameObject boss; // Referência ao GameObject do Boss
    [SerializeField] private Canvas bossHealthCanvas; // Referência ao Canvas de vida do Boss
    [SerializeField] private AudioClip bossFightMusic; // Música da boss fight

    private Animator bossAnimator;

    private void Start()
    {
        bossAnimator = boss.GetComponent<Animator>();

        // Desativa o boss e o canvas no início
        boss.SetActive(false);
        bossHealthCanvas.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Ativa o boss e a animação de entrada
            boss.SetActive(true);

            // Exibe o canvas de vida do Boss
            bossHealthCanvas.enabled = true;

            // Troca a música para a música da boss fight
            SoundManager.instance.PlayMusic(bossFightMusic);

            // Destrói o trigger para que não seja ativado novamente
            Destroy(gameObject);
        }
    }
}

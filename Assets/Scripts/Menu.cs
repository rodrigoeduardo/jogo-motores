using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text story;
    public GameObject texts;
    private bool tellingStory = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayGame")
        {
            StartCoroutine(WaitStory());
        }
        else if (other.gameObject.name == "QuitGame")
        {
            Application.Quit();
        }
    }

    private void Update()
    {
        if (tellingStory)
        {
            story.rectTransform.position = new Vector3(story.rectTransform.position.x, story.rectTransform.position.y + Time.deltaTime * 30f);
        }
    }

    IEnumerator WaitStory()
    {
        texts.SetActive(false);

        tellingStory = true;

        yield return new WaitUntil(() => story.rectTransform.position.y >= 760f);

        SceneManager.LoadScene("Fase 1");
    }
}

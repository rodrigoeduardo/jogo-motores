using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayGame")
        {
            SceneManager.LoadScene("Fase 1");
        }
        else if (other.gameObject.name == "QuitGame")
        {
            Application.Quit();
        }
    }
}

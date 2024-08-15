using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource soundSource;
    private AudioSource musicSource;

    [SerializeField] private AudioClip initialMusic; // Música inicial da cena
    private AudioClip currentMusic; // Música que está tocando no momento

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        // Ouve eventos de mudança de cena
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Toca a música inicial ao carregar
        PlayMusic(initialMusic);

        // Define os volumes iniciais
        ChangeMusicVolume(0);
        ChangeSoundVolume(0);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ao carregar uma nova cena, toca a música inicial novamente
        PlayMusic(initialMusic);
    }

    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    public void PlayMusic(AudioClip _music)
    {
        if (currentMusic == _music) return; // Evita tocar a mesma música novamente

        currentMusic = _music;
        musicSource.clip = _music;
        musicSource.Play();
    }

    public void ChangeSoundVolume(float _change)
    {
        ChangeSourceVolume(1, "soundVolume", _change, soundSource);
    }

    public void ChangeMusicVolume(float _change)
    {
        ChangeSourceVolume(0.3f, "musicVolume", _change, musicSource);
    }

    private void ChangeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source)
    {
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += change;

        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;

        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;

        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }
}

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Music")]
    public AudioClip backgroundMusic;

    [Header("Sound Effects")]
    public AudioClip blockClear;
    public AudioClip blockDrop;
    public AudioClip invalidMove;
    public AudioClip gameOver;
    public AudioClip buttonClick;
    public AudioClip shuffle;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ADD THIS METHOD HERE
    void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayClear() => sfxSource.PlayOneShot(blockClear);
    public void PlayDrop() => sfxSource.PlayOneShot(blockDrop);
    public void PlayInvalid() => sfxSource.PlayOneShot(invalidMove);
    public void PlayGameOver() => sfxSource.PlayOneShot(gameOver);
    public void PlayButton() => sfxSource.PlayOneShot(buttonClick);
    public void PlayShuffle() => sfxSource.PlayOneShot(shuffle);
}
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource audioSource;
    public AudioClip keyboardSound;
    public AudioClip backgroundMusic;

    public void PlayBackgroundMusic()
    {
        audioSource.PlayOneShot(backgroundMusic);
    }

    public void PlayAudio()
    {
        audioSource.PlayOneShot(keyboardSound);
    }

    private void Awake()
    {
        SetInstance();
        PlayBackgroundMusic();
    }

    private void SetInstance()
    {
        if (Instance != null && this != Instance)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
}

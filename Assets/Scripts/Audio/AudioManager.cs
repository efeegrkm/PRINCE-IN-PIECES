using System.Collections.Generic;
using UnityEngine;

// Inspector'da sesleri liste halinde g�rmek i�in �zel bir s�n�f
[System.Serializable]
public class Sound
{
    public string name;        // Sese verece�in isim (�rn: "Jump", "Click", "BGM_Main")
    public AudioClip clip;     // Ses dosyas�n�n kendisi
    [Range(0f, 1f)]
    public float volume = 1f;  // Bu sese �zel varsay�lan ses seviyesi
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Ses Kaynakları (Audio Sources)")]
    [Tooltip("Müzik için kullanılacak AudioSource (Loop açık olmalı)")]
    public AudioSource musicSource;
    [Tooltip("Efektler için kullanılacak AudioSource")]
    public AudioSource sfxSource;

    [Header("Ses Kütüphanesi")]
    public List<Sound> musicSounds;
    public List<Sound> sfxSounds;

    private void Awake()
    {
        // Singleton Kurulumu
        if (Instance == null)
        {
            Instance = this;
            // Eğer sahneler arası geçiş yapacaksan bu objenin yok olmamasını sağlar
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- MÜZİK METOTLARI ---

    public void PlayMusic(string name)
    {
        Sound s = musicSounds.Find(x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning("Müzik bulunamadı: " + name);
            return;
        }

        // Eğer zaten bu müzik çalıyorsa baştan başlatma
        if (musicSource.clip == s.clip && musicSource.isPlaying) return;

        musicSource.clip = s.clip;
        musicSource.volume = s.volume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }


    public void PlaySFX(string name)
    {
        Sound s = sfxSounds.Find(x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning("SFX bulunamadı: " + name);
            return;
        }

        // PlayOneShot kullanıyoruz çünkü efektler (örneğin art arda alınan altınlar) 
        // birbirini kesmeden üst üste çalabilmeli.
        sfxSource.PlayOneShot(s.clip, s.volume);
    }
}
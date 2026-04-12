using System.Collections.Generic;
using UnityEngine;

// Inspector'da sesleri liste halinde görmek için özel bir sýnýf
[System.Serializable]
public class Sound
{
    public string name;        // Sese vereceðin isim (Örn: "Jump", "Click", "BGM_Main")
    public AudioClip clip;     // Ses dosyasýnýn kendisi
    [Range(0f, 1f)]
    public float volume = 1f;  // Bu sese özel varsayýlan ses seviyesi
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Ses Kaynaklarý (Audio Sources)")]
    [Tooltip("Müzik için kullanýlacak AudioSource (Loop açýk olmalý)")]
    public AudioSource musicSource;
    [Tooltip("Efektler için kullanýlacak AudioSource")]
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
            // Eðer sahneler arasý geçiþ yapacaksan bu objenin yok olmamasýný saðlar
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- MÜZÝK METOTLARI ---

    public void PlayMusic(string name)
    {
        Sound s = musicSounds.Find(x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning("Müzik bulunamadý: " + name);
            return;
        }

        // Eðer zaten bu müzik çalýyorsa baþtan baþlatma
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
            Debug.LogWarning("SFX bulunamadý: " + name);
            return;
        }

        // PlayOneShot kullanýyoruz çünkü efektler (örneðin art arda alýnan altýnlar) 
        // birbirini kesmeden üst üste çalabilmeli.
        sfxSource.PlayOneShot(s.clip, s.volume);
    }
}
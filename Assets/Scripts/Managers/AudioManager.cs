using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music / SFX")]
    public AudioClip musicClip;
    public AudioClip pelletClip;
    public AudioClip powerupClip;
    public AudioClip deathClip;
    public AudioClip eatGhostClip;
    public AudioClip cherryClip;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }
    }

    public void PlayMusic()
    {
        if (musicClip == null || musicSource == null) return;
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic() => musicSource?.Stop();

    public void PlaySfx(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume));
    }

    // Удобные обёртки
    public void PlayPellet() => PlaySfx(pelletClip);
    public void PlayPowerup() => PlaySfx(powerupClip);
    public void PlayDeath() => PlaySfx(deathClip);
    public void PlayEatGhost() => PlaySfx(eatGhostClip);
    public void PlayCherry() => PlaySfx(cherryClip);
}
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM Clips")]
    public AudioClip menuBGM;
    public AudioClip gameplayBGM;

    [Header("SFX Clips")]
    public AudioClip fishSFX;
    public AudioClip gameOverSFX;
    public AudioClip victorySFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioClip musicClip)
    {
        if (bgmSource == null || musicClip == null) return;
        if (bgmSource.clip == musicClip && bgmSource.isPlaying) return;

        bgmSource.clip = musicClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource != null) bgmSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void TriggerGameOver()
    {
        StopBGM();
        PlaySFX(gameOverSFX);
    }

    public void TriggerVictory()
    {
        StopBGM();
        PlaySFX(victorySFX);
    }
}
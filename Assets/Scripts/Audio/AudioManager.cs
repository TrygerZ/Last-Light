using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("SFX Clips")]
    public AudioClip footstepSFX;
    public AudioClip pickupWoodSFX;
    public AudioClip depositWoodSFX;
    public AudioClip enemyAttackSFX;
    public AudioClip winSFX;
    public AudioClip loseSFX;

    [Header("Music Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;

    private AudioSource sfxSource;
    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Setup SFX audio source
        sfxSource = GetComponent<AudioSource>();
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        // Setup music audio source (separate from SFX)
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = 0.5f;
        musicSource.playOnAwake = false;
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    public void PlaySFXSliced(AudioClip clip, float startTime, float endTime, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;
        if (startTime >= endTime || startTime < 0f) return;

        sfxSource.volume = volume;

        sfxSource.clip = clip;
        sfxSource.time = startTime;
        sfxSource.Play();
        float duration = endTime - startTime;
        sfxSource.SetScheduledEndTime(AudioSettings.dspTime + duration);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;

        // Don't restart if same clip is already playing
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = Mathf.Clamp01(volume);
    }
}

using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [Header("Music Setting")]
    [SerializeField] private AudioClip musicClip;

    private void Start()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMusic(musicClip);
    }
}

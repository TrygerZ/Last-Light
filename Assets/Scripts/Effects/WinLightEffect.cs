using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class WinLightEffect : MonoBehaviour
{
    [Header("Light Reference")]
    [SerializeField] private Light2D targetLight;

    [Header("Timing")]
    [SerializeField] private float fadeOutDelay = 0.5f;
    [SerializeField] private float effectDuration = 3f;

    [Header("Intensity Animation")]
    [SerializeField] private float intensityStart = 0f;
    [SerializeField] private float intensityEnd = 5f;

    [Header("Falloff Animation")]
    [SerializeField] private float falloffStart = 1f;
    [SerializeField] private float falloffEnd = 0f;

    [Header("Scene Transition")]
    [SerializeField] private float sceneTransitionDelay = 10f;

    private bool hasPlayed;

    private void Awake()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light2D>();

        if (targetLight != null)
        {
            targetLight.intensity = intensityStart;
            targetLight.falloffIntensity = falloffStart;
        }
    }

    private void Start()
    {
        PlayEffect();
    }

    public void PlayEffect()
    {
        if (hasPlayed) return;
        hasPlayed = true;
        StartCoroutine(WinEffectCoroutine());
    }

    private System.Collections.IEnumerator WinEffectCoroutine()
    {
        if (targetLight == null)
        {
            Debug.LogError("WinLightEffect: No Light2D reference!");
            yield break;
        }

        yield return new WaitForSecondsRealtime(fadeOutDelay);

        float elapsed = 0f;

        while (elapsed < effectDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / effectDuration);

            float smoothT = t * t * (3f - 2f * t);

            targetLight.intensity = Mathf.Lerp(intensityStart, intensityEnd, smoothT);
            targetLight.falloffIntensity = Mathf.Lerp(falloffStart, falloffEnd, smoothT);

            yield return null;
        }

        targetLight.intensity = intensityEnd;
        targetLight.falloffIntensity = falloffEnd;

        Debug.Log("✨ Win light effect complete!");

        yield return new WaitForSecondsRealtime(sceneTransitionDelay);

        SceneManager.LoadScene("Main Menu");
    }
}

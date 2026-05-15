using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WinLightEffect : MonoBehaviour
{
    [Header("Light Reference")]
    [SerializeField] private Light2D targetLight;

    [Header("Timing")]
    [Tooltip("Delay before light effect starts (for fadeout)")]
    [SerializeField] private float fadeOutDelay = 0.5f;
    [Tooltip("How long the light animation lasts")]
    [SerializeField] private float effectDuration = 3f;

    [Header("Intensity Animation")]
    [SerializeField] private float intensityStart = 0f;
    [SerializeField] private float intensityEnd = 5f;

    [Header("Falloff Animation")]
    [Tooltip("Falloff starts at this value (1=very soft/wide, 0=hard/sharp)")]
    [SerializeField] private float falloffStart = 1f;
    [Tooltip("Falloff ends at this value")]
    [SerializeField] private float falloffEnd = 0f;

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
        // Auto-play when WinScene loads
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

        // Step 1: Wait for fadeout
        yield return new WaitForSecondsRealtime(fadeOutDelay);

        // Step 2: Animate light
        float elapsed = 0f;

        while (elapsed < effectDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / effectDuration);

            // Smooth step interpolation
            float smoothT = t * t * (3f - 2f * t);

            // Animate intensity
            targetLight.intensity = Mathf.Lerp(intensityStart, intensityEnd, smoothT);

            // Animate falloff: 1 (soft/wide) → 0 (hard/sharp)
            targetLight.falloffIntensity = Mathf.Lerp(falloffStart, falloffEnd, smoothT);

            yield return null;
        }

        // Step 3: Final values
        targetLight.intensity = intensityEnd;
        targetLight.falloffIntensity = falloffEnd;

        Debug.Log("✨ Win light effect complete!");
    }
}

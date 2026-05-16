using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchBurnout : MonoBehaviour
{
    [SerializeField] GameObject fireBar;
    private TorchDuration fireScript;
    [SerializeField] private Animator playerAnimator;

    [Header("Settings")]
    [SerializeField] private float maxTorchDuration = 60f;

    [Header("Debug (Readonly)")]
    [SerializeField] private float remainingTime;
    [SerializeField] private bool isLit = true;
    private Light2D light2D;
    private Damage damageComponent;

    public bool IsLit => isLit;
    public float MaxTorchDuration => maxTorchDuration;
    public float RemainingTime => remainingTime;
    public float NormalizedTime => Mathf.Clamp01(remainingTime / maxTorchDuration);

    private void Awake()
    {
        light2D = GetComponent<Light2D>();
        damageComponent = GetComponent<Damage>();

        if (light2D == null)
            Debug.LogError("TorchBurnout: Light2D component not found!");
        if (damageComponent == null)
            Debug.LogError("TorchBurnout: Damage component not found!");
    }

    private void Start()
    {
        fireScript = fireBar.GetComponent<TorchDuration>();
        remainingTime = maxTorchDuration;
    }

    private void Update()
    {
        if (!isLit) return;

        remainingTime -= Time.deltaTime;
        fireScript.setFire(remainingTime);

        light2D.intensity = Mathf.Lerp(0f, 1f, NormalizedTime);

        if (remainingTime <= 0f)
            Extinguish();
    }

    private void Extinguish()
    {
        isLit = false;
        remainingTime = 0f;
        light2D.intensity = 0f;
        damageComponent.enabled = false;

        if (playerAnimator != null) playerAnimator.SetBool("IsTorchLit", false);
    }

    public void RefillFull()
    {
        remainingTime = maxTorchDuration;
        isLit = true;
        damageComponent.enabled = true;
        light2D.intensity = 1f;

        if (playerAnimator != null) playerAnimator.SetBool("IsTorchLit", true);
    }

    public void RefillTorch(float amount)
    {
        remainingTime = Mathf.Min(remainingTime + amount, maxTorchDuration);
        isLit = true;
        damageComponent.enabled = true;

        if (playerAnimator != null) playerAnimator.SetBool("IsTorchLit", true);
    }
}

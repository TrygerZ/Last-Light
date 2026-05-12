using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CampfireBurnout : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float baseCampfireDuration = 120f;

    [Header("Debug (Readonly)")]
    [SerializeField] private float remainingTime;
    [SerializeField] private bool isLit = true;
    private Light2D light2D;
    private Damage damageComponent;

    public bool IsLit => isLit;
    public float RemainingTime => remainingTime;
    public float NormalizedTime => Mathf.Clamp01(remainingTime / baseCampfireDuration);

    private void Awake()
    {
        light2D = GetComponent<Light2D>();
        damageComponent = GetComponent<Damage>();

        if (light2D == null)
            Debug.LogError("CampfireBurnout: Light2D component not found!");

        if (damageComponent == null)
            Debug.LogError("CampfireBurnout: Damage component not found!");
    }

    private void Start()
    {
        remainingTime = baseCampfireDuration;
    }


    private void Update()
    {
        if (!isLit) return;

        remainingTime -= Time.deltaTime;

        // Lerp light intensity based on remaining time
        light2D.intensity = Mathf.Lerp(0f, 1f, NormalizedTime);

        if (remainingTime <= 0f)
        {
            Extinguish();
        }
    }

    private void Extinguish()
    {
        isLit = false;
        remainingTime = 0f;
        light2D.intensity = 0f;
        damageComponent.enabled = false;

        Debug.Log("🔥🔥 Campfire has been extinguished! Bring wood to relight it.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isLit) return;

        if (IsPlayer(other))
        {
            DepositAllWood();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isLit) return;

        if (IsPlayer(other))
        {
            RefillPlayerTorch(other);
        }
    }

    private bool IsPlayer(Collider2D other)
    {
        if (other == null) return false;

        if (other.CompareTag("PlayerBody") || other.CompareTag("Player"))
            return true;

        if (other.transform.root.CompareTag("Player"))
            return true;

        return false;
    }

    private void DepositAllWood()
    {
        Backpack backpack = Backpack.Instance;
        if (backpack == null) return;

        // Hitung total waktu dari semua kayu di inventory
        float totalTime = backpack.DepositAllToCampfire();

        if (totalTime > 0f)
        {
            remainingTime += totalTime;

            // Kalau campfire sebelumnya mati, hidupkan lagi
            if (!isLit)
            {
                isLit = true;
                damageComponent.enabled = true;
                light2D.intensity = 1f;
            }

            Debug.Log($"🔥 Campfire received {totalTime}s of fuel from all wood!");
        }
    }

    private void RefillPlayerTorch(Collider2D other)
    {
        TorchBurnout torch = other.transform.root.GetComponentInChildren<TorchBurnout>();
        if (torch != null)
        {
            torch.RefillFull();
        }
    }
}

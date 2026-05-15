using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CampfireBurnout : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float baseCampfireDuration = 120f;

    [Header("Gradual Refill Settings")]
    [SerializeField] private float torchRefillRate = 5f;
    [SerializeField] private int healthHealRate = 2;
    [SerializeField] private float refillInterval = 0.25f;

    [Header("Debug (Readonly)")]
    [SerializeField] private float remainingTime;
    [SerializeField] private bool isLit = true;
    private Light2D light2D;
    private Damage damageComponent;

    private float refillTimer;
    private bool playerInRange;
    private Transform playerRoot;

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
        light2D.intensity = Mathf.Lerp(0f, 1f, NormalizedTime);

        if (remainingTime <= 0f)
        {
            Extinguish();
        }

        if (playerInRange && playerRoot != null)
        {
            refillTimer += Time.deltaTime;
            if (refillTimer >= refillInterval)
            {
                refillTimer = 0f;
                GradualRefill(playerRoot);
            }
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
        if (!IsPlayer(other)) return;

        playerRoot = other.transform.root;
        playerInRange = true;
        refillTimer = 0f;

        DepositAllWood();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayer(other))
        {
            playerInRange = false;
            playerRoot = null;
            refillTimer = 0f;
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

        float totalTime = backpack.DepositAllToCampfire();

        if (totalTime > 0f)
        {
            remainingTime = Mathf.Min(remainingTime + totalTime, baseCampfireDuration);

            if (!isLit)
            {
                isLit = true;
                damageComponent.enabled = true;
                light2D.intensity = 1f;
            }

            Debug.Log($"🔥 Campfire received {totalTime}s of fuel from all wood!");
        }
    }

    private void GradualRefill(Transform root)
    {
        TorchBurnout torch = root.GetComponentInChildren<TorchBurnout>();
        if (torch != null)
        {
            float refillAmount = torchRefillRate * refillInterval;
            torch.RefillTorch(refillAmount);
        }

        Health health = root.GetComponentInChildren<Health>();
        if (health != null && health.CurrentHealth < health.MaxHealth)
        {
            int healAmount = Mathf.RoundToInt(healthHealRate * refillInterval);
            if (healAmount < 1) healAmount = 1;
            health.Heal(healAmount);
        }
    }
}

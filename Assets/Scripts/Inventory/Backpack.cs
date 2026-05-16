using TMPro;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    public static Backpack Instance { get; private set; }

    // Shared pickup lock — prevents picking multiple woods at once
    public static bool IsPlayerPickingUp { get; set; } = false;

    public int wood1Count = 0;
    public int wood2Count = 0;
    public int wood3Count = 0;

    [Header("Capacity Settings")]
    [SerializeField] private float maxCapacity = 10f;
    [Tooltip("Current total weight carried — readonly in Inspector")]
    [SerializeField] private float currentWeight = 0f;

    [Header("Wood Prefab References (for weight & timeValue)")]
    [SerializeField] private Wood1 wood1Prefab;
    [SerializeField] private Wood2 wood2Prefab;
    [SerializeField] private Wood3 wood3Prefab;

    [Header("ItemHotbar")]
    [SerializeField] GameObject wood1;
    [SerializeField] GameObject wood2;
    [SerializeField] GameObject wood3;
    [SerializeField] GameObject capacity;

    private ItemAmount woodText1;
    private ItemAmount woodText2;
    private ItemAmount woodText3;
    private CapacityText capacityText;

    private float wood1Weight;
    private float wood2Weight;
    private float wood3Weight;

    public float MaxCapacity => maxCapacity;
    public float CurrentWeight => currentWeight;
    public float RemainingCapacity => maxCapacity - currentWeight;
    public bool IsFull => currentWeight >= maxCapacity;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CacheWeightsFromPrefabs();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CacheWeightsFromPrefabs()
    {
        wood1Weight = wood1Prefab != null ? wood1Prefab.Weight : 1f;
        wood2Weight = wood2Prefab != null ? wood2Prefab.Weight : 2f;
        wood3Weight = wood3Prefab != null ? wood3Prefab.Weight : 4f;
    }

    public float GetWoodWeight(string woodType)
    {
        switch (woodType)
        {
            case "Wood1": return wood1Weight;
            case "Wood2": return wood2Weight;
            case "Wood3": return wood3Weight;
            default: return 0f;
        }
    }

    private void Start()
    {
        woodText1 = wood1.GetComponent<ItemAmount>();
        woodText2 = wood2.GetComponent<ItemAmount>();
        woodText3 = wood3.GetComponent<ItemAmount>();
        capacityText = capacity.GetComponent<CapacityText>();
    }

    public bool CanAddWood(string woodType)
    {
        float weight = GetWoodWeight(woodType);
        return (currentWeight + weight) <= maxCapacity;
    }

    public bool AddWood(string woodType)
    {
        if (!CanAddWood(woodType))
        {
            Debug.LogWarning($"Backpack: Cannot add {woodType} — backpack is full! "
                + $"({currentWeight}/{maxCapacity} weight)");
            return false;
        }

        float weight = GetWoodWeight(woodType);
        currentWeight += weight;
        capacityText.getCapacity(currentWeight);

        switch (woodType)
        {
            case "Wood1": wood1Count++; woodText1.textCount(wood1Count); break;
            case "Wood2": wood2Count++; woodText2.textCount(wood2Count); break;
            case "Wood3": wood3Count++; woodText3.textCount(wood3Count); break;
        }

        Debug.Log($"Backpack: {woodType} (+{weight}) added. "
            + $"Weight: {currentWeight}/{maxCapacity} | "
            + $"W1:{wood1Count} W2:{wood2Count} W3:{wood3Count}");
        return true;
    }

    public bool RemoveWood(string woodType)
    {
        float weight = GetWoodWeight(woodType);
        if (weight <= 0f) return false;

        bool hasItem = false;
        switch (woodType)
        {
            case "Wood1": hasItem = wood1Count > 0; break;
            case "Wood2": hasItem = wood2Count > 0; break;
            case "Wood3": hasItem = wood3Count > 0; break;
        }

        if (!hasItem)
        {
            Debug.LogWarning($"Backpack: No {woodType} to remove!");
            return false;
        }

        currentWeight = Mathf.Max(0f, currentWeight - weight);
        capacityText.getCapacity(currentWeight);

        switch (woodType)
        {
            case "Wood1": wood1Count--;
            woodText1.textCount(wood1Count);
            break;
            case "Wood2": wood2Count--; break;
            case "Wood3": wood3Count--; break;
        }

        Debug.Log($"Backpack: {woodType} (-{weight}) removed. "
            + $"Weight: {currentWeight}/{maxCapacity} | "
            + $"W1:{wood1Count} W2:{wood2Count} W3:{wood3Count}");
        return true;
    }

    public float GetWoodTimeValue(string woodType)
    {
        switch (woodType)
        {
            case "Wood1": return wood1Prefab != null ? wood1Prefab.TimeValue : 10f;
            case "Wood2": return wood2Prefab != null ? wood2Prefab.TimeValue : 20f;
            case "Wood3": return wood3Prefab != null ? wood3Prefab.TimeValue : 40f;
            default: return 0f;
        }
    }

    public int GetWoodCount(string woodType)
    {
        switch (woodType)
        {
            case "Wood1": return wood1Count;
            case "Wood2": return wood2Count;
            case "Wood3": return wood3Count;
            default: return 0;
        }
    }

    public float DepositAllToCampfire()
    {
        float time1 = wood1Prefab != null ? wood1Prefab.TimeValue : 10f;
        float time2 = wood2Prefab != null ? wood2Prefab.TimeValue : 20f;
        float time3 = wood3Prefab != null ? wood3Prefab.TimeValue : 40f;

        float totalTime = (wood1Count * time1) +
                          (wood2Count * time2) +
                          (wood3Count * time3);

        if (totalTime > 0f)
        {
            Debug.Log($"Backpack: Deposit all wood! "
                + $"W1:{wood1Count}(x{time1}s) "
                + $"W2:{wood2Count}(x{time2}s) "
                + $"W3:{wood3Count}(x{time3}s) "
                + $"= +{totalTime}s fuel. Weight cleared: {currentWeight}");
        }

        wood1Count = 0;
        woodText1.textCount(wood1Count);
        wood2Count = 0;
        woodText2.textCount(wood2Count);
        wood3Count = 0;
        woodText3.textCount(wood3Count);
        currentWeight = 0f;
        capacityText.getCapacity(currentWeight);

        return totalTime;
    }
}

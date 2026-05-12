using UnityEngine;

public class Backpack : MonoBehaviour
{
    public static Backpack Instance { get; private set; }

    [Header("Wood Prefab References (for timeValue)")]
    [SerializeField] private Wood1 wood1Prefab;
    [SerializeField] private Wood2 wood2Prefab;
    [SerializeField] private Wood3 wood3Prefab;

    // Langsung variable biasa — ga perlu Dictionary
    public int wood1Count = 0;
    public int wood2Count = 0;
    public int wood3Count = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddWood(string woodType)
    {
        switch (woodType)
        {
            case "Wood1": wood1Count++; break;
            case "Wood2": wood2Count++; break;
            case "Wood3": wood3Count++; break;
        }
        Debug.Log($"Backpack: {woodType} ditambahkan. W1:{wood1Count} W2:{wood2Count} W3:{wood3Count}");
    }

    public int GetWood(string woodType)
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
        float time3 = wood3Prefab != null ? wood3Prefab.TimeValue : 20f;

        float totalTime = (wood1Count * time1) +
                          (wood2Count * time2) +
                          (wood3Count * time3);

        if (totalTime > 0f)
        {
            Debug.Log($"Backpack: Deposit all wood! W1:{wood1Count}(x{time1}s) W2:{wood2Count}(x{time2}s) W3:{wood3Count}(x{time3}s) = +{totalTime}s");
        }

        wood1Count = 0;
        wood2Count = 0;
        wood3Count = 0;

        return totalTime;
    }
}

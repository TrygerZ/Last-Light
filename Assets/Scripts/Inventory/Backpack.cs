using UnityEngine;   // ← cuma butuh ini, tidak perlu Generic

public class Backpack : MonoBehaviour
{
    public static Backpack Instance { get; private set; }

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
}

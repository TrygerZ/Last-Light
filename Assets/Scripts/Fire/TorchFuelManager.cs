using UnityEngine;

public class TorchFuelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TorchBurnout torchBurnout;
    [SerializeField] private Backpack backpack;

    [Header("Settings")]
    [SerializeField] private KeyCode consumeKey = KeyCode.Q;
    [SerializeField] private string woodType = "Wood1";

    private void Awake()
    {
        // Auto-resolve references if not set in Inspector
        if (torchBurnout == null)
            torchBurnout = GetComponentInChildren<TorchBurnout>();

        if (backpack == null)
            backpack = GetComponent<Backpack>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(consumeKey))
        {
            TryConsumeWoodForTorch();
        }
    }

    private void TryConsumeWoodForTorch()
    {
        // Validate references
        if (backpack == null)
        {
            Debug.LogWarning("TorchFuelManager: Backpack reference is missing!");
            return;
        }

        if (torchBurnout == null)
        {
            Debug.LogWarning("TorchFuelManager: TorchBurnout reference is missing!");
            return;
        }

        // Check if player has Wood1 in backpack
        if (backpack.GetWoodCount(woodType) <= 0)
        {
            Debug.Log($"TorchFuelManager: No {woodType} available in backpack to consume!");
            return;
        }

        // Get the time value this wood provides
        float timeValue = backpack.GetWoodTimeValue(woodType);
        if (timeValue <= 0f)
        {
            Debug.LogWarning($"TorchFuelManager: {woodType} has no valid time value!");
            return;
        }

        // Remove the wood from backpack (reduces count + weight)
        bool removed = backpack.RemoveWood(woodType);
        if (!removed)
        {
            Debug.LogWarning("TorchFuelManager: Failed to remove wood from backpack!");
            return;
        }

        // Add fuel to torch
        torchBurnout.RefillTorch(timeValue);

        Debug.Log($"TorchFuelManager: Consumed 1 {woodType} → added {timeValue}s to torch. " +
                  $"Remaining Wood1: {backpack.GetWoodCount(woodType)}");
    }
}

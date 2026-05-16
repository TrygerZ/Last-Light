using UnityEngine;

public class Wood2 : MonoBehaviour
{
    [Header("Wood Properties")]
    [SerializeField] private float timeValue = 25f;
    [SerializeField] private float weight = 2f;

    public float TimeValue => timeValue;
    public float Weight => weight;

    [Header("Type Identifier")]
    [SerializeField] private string woodType = "Wood2";

    private WoodSpawner spawner;
    private bool isPickedUp;
    private bool playerInRange;

    public void SetSpawner(WoodSpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
            Backpack.IsPlayerPickingUp = false;

        if (!playerInRange || isPickedUp || Backpack.IsPlayerPickingUp)
            return;

        // Wood2 prioritizes Wood3 if nearby
        if (HasWood3Nearby())
            return;

        if (Input.GetKeyDown(KeyCode.E))
            PickUp();
    }

    private bool HasWood3Nearby()
    {
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (Collider2D col in nearby)
        {
            if (col.GetComponent<Wood3>() != null && col.GetComponent<Wood3>().enabled)
                return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayer(other)) playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayer(other)) playerInRange = false;
    }

    private bool IsPlayer(Collider2D other)
    {
        if (other == null) return false;
        if (other.CompareTag("Player")) return true;
        if (other.transform.root.CompareTag("Player")) return true;
        if (other.GetComponentInParent<Movement_Input>() != null) return true;
        return false;
    }

    private void PickUp()
    {
        Backpack.IsPlayerPickingUp = true;

        if (Backpack.Instance == null) return;

        if (!Backpack.Instance.CanAddWood(woodType))
        {
            Debug.LogWarning($"Cannot pick up {woodType} (weight: {weight}) — backpack full! "
                + $"({Backpack.Instance.CurrentWeight}/{Backpack.Instance.MaxCapacity})");
            return;
        }

        isPickedUp = true;

        if (!Backpack.Instance.AddWood(woodType))
        {
            isPickedUp = false;
            return;
        }

        spawner?.OnWoodPickedUp();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pickupWoodSFX);

        Destroy(gameObject);
    }
}

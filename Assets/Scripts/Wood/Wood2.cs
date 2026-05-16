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
        if (!playerInRange || isPickedUp)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayer(other))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayer(other))
        {
            playerInRange = false;
        }
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
        if (Backpack.Instance == null)
            return;

        // Check capacity before picking up
        if (!Backpack.Instance.CanAddWood(woodType))
        {
            Debug.LogWarning($"Cannot pick up {woodType} (weight: {weight}) — backpack full! "
                + $"({Backpack.Instance.CurrentWeight}/{Backpack.Instance.MaxCapacity})");
            return;
        }

        isPickedUp = true;

        bool added = Backpack.Instance.AddWood(woodType);
        if (!added)
        {
            isPickedUp = false;
            return;
        }

        if (spawner != null)
        {
            spawner.OnWoodPickedUp();
        }

        // SFX pickup kayu
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pickupWoodSFX);

        Destroy(gameObject);
    }
}

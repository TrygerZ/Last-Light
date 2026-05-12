using UnityEngine;

public class Wood3 : MonoBehaviour
{
    [SerializeField] private float timeValue = 40f;
    public float TimeValue => timeValue;
    [SerializeField] private string woodType = "Wood3";

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
        isPickedUp = true;

        if (Backpack.Instance != null)
        {
            Backpack.Instance.AddWood(woodType);
        }

        if (spawner != null)
        {
            spawner.OnWoodPickedUp();
        }

        Destroy(gameObject);
    }
}
using UnityEngine;

public class Wood1 : MonoBehaviour
{
    [SerializeField] private float timeValue = 10f;

    private Wood1Spawner spawner;
    private bool isPickedUp;
    private bool playerInRange;

    public void SetSpawner(Wood1Spawner spawnerRef)
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
        if (other.GetComponentInParent<Player>() != null) return true;
        return false;
    }

    private void PickUp()
    {
        isPickedUp = true;

        if (spawner != null)
        {
            spawner.OnWoodPickedUp();
        }

        Destroy(gameObject);
    }
}
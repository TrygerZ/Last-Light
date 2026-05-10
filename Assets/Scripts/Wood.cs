using UnityEngine;

public class WoodPickup : MonoBehaviour
{
    private WoodSpawner spawner;

    public void SetSpawner(WoodSpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (spawner != null)
        {
            spawner.OnWoodPickedUp();
        }
        Destroy(gameObject);
    }
}
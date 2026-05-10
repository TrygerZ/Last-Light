using UnityEngine;

public class Wood1Spawner : MonoBehaviour
{
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-121f, -2.9f);
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(-10f, -2.9f);
    [SerializeField, Range(0, 100)] private int spawnWeight = 10;
    [SerializeField] private int maxWoodCount = 10;
    [SerializeField] private Transform woodContainer;

    private float spawnTimer;
    private int currentWoodCount;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer < spawnInterval)
        {
            return;
        }

        spawnTimer = 0f;

        if (currentWoodCount >= maxWoodCount)
        {
            return;
        }

        if (woodPrefab == null || woodContainer == null)
        {
            return;
        }

        SpawnWood();
    }

    private void SpawnWood()
    {
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector3 spawnPos = new Vector3(randomX, randomY, 0f);

        GameObject newWood = Instantiate(woodPrefab, spawnPos, Quaternion.identity, woodContainer);
        currentWoodCount++;

        Wood1 woodPickup = newWood.GetComponent<Wood1>();
        if (woodPickup != null)
        {
            woodPickup.SetSpawner(this);
        }
    }

    public void OnWoodPickedUp()
    {
        if (currentWoodCount > 0)
        {
            currentWoodCount--;
        }
    }
}
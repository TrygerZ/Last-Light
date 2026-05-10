using UnityEngine;

public class WoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private float spawnInterval = 5f;  // Interval spawn dalam detik
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-10f, -5f);  // Batas minimum area
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(10f, 5f);    // Batas maksimum area
    [SerializeField] private int maxWoodCount = 10;  // Maksimal kayu di map
    [SerializeField] private Transform WoodContainer;  // Parent untuk kayu yang di-spawn   
    
    private float spawnTimer = 0f;
    private int currentWoodCount = 0;

    void Update()
    {
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= spawnInterval && currentWoodCount < maxWoodCount)
        {
            SpawnWood();
            spawnTimer = 0f;
        }
    }

    void SpawnWood()
    {
        // Generate posisi random di dalam area yang ditentukan
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector3 spawnPos = new Vector3(randomX, randomY, 0f);
        
        // Spawn kayu
        GameObject newWood = Instantiate(woodPrefab, spawnPos, Quaternion.identity, WoodContainer);
        currentWoodCount++;
        
        // Jika ada script untuk menghapus kayu, beri tahu spawner
        WoodPickup woodPickup = newWood.GetComponent<WoodPickup>();
        if (woodPickup != null)
        {
            woodPickup.SetSpawner(this);
        }
    }

    public void OnWoodPickedUp()
    {
        currentWoodCount--;
    }
}
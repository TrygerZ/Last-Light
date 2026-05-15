using UnityEngine;

public class WoodSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private Transform woodContainer;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxWoodCount = 10;

    [Header("Spawn Area")]
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-10f, -8f);
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(10f, 8f);

    [Header("Exclusion Pillar (Vertical)")]
    [SerializeField] private float exclusionXMin = -2f;
    [SerializeField] private float exclusionXMax = 2f;

    [Header("Zone Distribution (Weighted Random per Wood Type)")]
    [Tooltip("Probabilities for Wood1, Wood2, Wood3 spawn. Must add up to 100. Example: 60,30,10")]
    [SerializeField] private int wood1Weight = 60;
    [SerializeField] private int wood2Weight = 30;
    [SerializeField] private int wood3Weight = 10;

    [Header("Wood Prefabs (assign all 3 for zone spawning)")]
    [SerializeField] private GameObject wood1Prefab;
    [SerializeField] private GameObject wood2Prefab;
    [SerializeField] private GameObject wood3Prefab;

    private float spawnTimer;
    private int currentWoodCount;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer < spawnInterval)
            return;

        spawnTimer = 0f;

        if (currentWoodCount >= maxWoodCount)
            return;

        if (woodContainer == null)
            return;

        SpawnWood();
    }

    private void SpawnWood()
    {
        Vector3 spawnPos = GetValidSpawnPosition();

        // Determine which wood type to spawn based on weighted random
        GameObject prefabToSpawn = GetWeightedWoodPrefab();
        if (prefabToSpawn == null)
            prefabToSpawn = woodPrefab; // fallback to default
        if (prefabToSpawn == null)
            return;

        GameObject newWood = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity, woodContainer);
        currentWoodCount++;

        // Set spawner reference on whichever wood type was spawned
        Wood1 wood1 = newWood.GetComponent<Wood1>();
        if (wood1 != null) { wood1.SetSpawner(this); return; }

        Wood2 wood2 = newWood.GetComponent<Wood2>();
        if (wood2 != null) { wood2.SetSpawner(this); return; }

        Wood3 wood3 = newWood.GetComponent<Wood3>();
        if (wood3 != null) { wood3.SetSpawner(this); }
    }

    private GameObject GetWeightedWoodPrefab()
    {
        int totalWeight = wood1Weight + wood2Weight + wood3Weight;
        if (totalWeight <= 0) return woodPrefab;

        int roll = Random.Range(0, totalWeight);

        if (roll < wood1Weight)
            return wood1Prefab != null ? wood1Prefab : woodPrefab;
        else if (roll < wood1Weight + wood2Weight)
            return wood2Prefab != null ? wood2Prefab : woodPrefab;
        else
            return wood3Prefab != null ? wood3Prefab : woodPrefab;
    }

    public void OnWoodPickedUp()
    {
        if (currentWoodCount > 0)
            currentWoodCount--;
    }

    private Vector3 GetValidSpawnPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

        if (x >= exclusionXMin && x <= exclusionXMax)
        {
            float jarakKeKiri = x - exclusionXMin;
            float jarakKeKanan = exclusionXMax - x;
            x = (jarakKeKiri < jarakKeKanan) ? exclusionXMin : exclusionXMax;
        }

        return new Vector3(x, y, 0f);
    }
}

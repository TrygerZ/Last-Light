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

        if (woodPrefab == null || woodContainer == null)
            return;

        SpawnWood();
    }

    private void SpawnWood()
    {
        Vector3 spawnPos = GetValidSpawnPosition();

        GameObject newWood = Instantiate(woodPrefab, spawnPos, Quaternion.identity, woodContainer);
        currentWoodCount++;

        Wood1 wood1 = newWood.GetComponent<Wood1>();
        if (wood1 != null)
        {
            wood1.SetSpawner(this);
            return;
        }

        Wood2 wood2 = newWood.GetComponent<Wood2>();
        if (wood2 != null)
        {
            wood2.SetSpawner(this);
            return;
        }

        Wood3 wood3 = newWood.GetComponent<Wood3>();
        if (wood3 != null)
        {
            wood3.SetSpawner(this);
        }
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

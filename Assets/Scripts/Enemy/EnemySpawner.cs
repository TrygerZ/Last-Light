using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Base Spawn Settings")]
    [SerializeField] GameObject enemy;
    [SerializeField] Transform EnemySpawnerPoint;

    [Header ("where the enemy won't spawn")]
    [SerializeField] float left;
    [SerializeField] float right;

    [Header ("how far out from the range left or right limit")]
    [SerializeField] float outsideDistance;

    [Header("Difficulty Scaling References")]
    [SerializeField] private MoonTimer moonTimer;

    [Header("Phase Awal (00:00 - 01:00)")]
    [SerializeField] private float awalMinCD = 8f;
    [SerializeField] private float awalMaxCD = 14f;
    [SerializeField] private int awalLimit = 3;

    [Header("Phase Tengah (01:01 - 03:00)")]
    [SerializeField] private float tengahMinCD = 5f;
    [SerializeField] private float tengahMaxCD = 9f;
    [SerializeField] private int tengahLimit = 5;

    [Header("Phase Akhir (03:01 - 05:00)")]
    [SerializeField] private float akhirMinCD = 2.5f;
    [SerializeField] private float akhirMaxCD = 5f;
    [SerializeField] private int akhirLimit = 7;

    private float currentMinCD;
    private float currentMaxCD;
    private int currentLimit;
    private int currentPhase = -1;

    private float timeCounter;
    [Header("How many enemies currently spawned")]
    [SerializeField] int currentSpawn = 0;

    private void Awake()
    {
        if (moonTimer == null)
            moonTimer = FindFirstObjectByType<MoonTimer>();

        if (moonTimer == null)
            Debug.LogError("EnemySpawner: MoonTimer tidak ditemukan! Assign MoonTimer di Inspector.");

        ApplyPhaseValues(0);
        setTimeUntilSpawn();
    }

    private void Update()
    {
        UpdatePhase();

        if (timeCounter >= 0)
            timeCounter -= Time.deltaTime;

        if(timeCounter <= 0 && currentSpawn < currentLimit)
        {
            spawnEnemy();
            currentSpawn++;
            setTimeUntilSpawn();
        }
    }

    private void UpdatePhase()
    {
        int newPhase = GetCurrentPhase();
        if (newPhase != currentPhase)
        {
            currentPhase = newPhase;
            ApplyPhaseValues(newPhase);
        }
    }

    private int GetCurrentPhase()
    {
        if (moonTimer == null) return 0;

        float elapsed = moonTimer.TotalDuration - moonTimer.RemainingTime;

        if (elapsed <= 60f)
            return 0;
        else if (elapsed <= 180f)
            return 1;
        else
            return 2;
    }

    private void ApplyPhaseValues(int phase)
    {
        switch (phase)
        {
            case 0:
                currentMinCD = awalMinCD;
                currentMaxCD = awalMaxCD;
                currentLimit = awalLimit;
                break;
            case 1:
                currentMinCD = tengahMinCD;
                currentMaxCD = tengahMaxCD;
                currentLimit = tengahLimit;
                break;
            case 2:
                currentMinCD = akhirMinCD;
                currentMaxCD = akhirMaxCD;
                currentLimit = akhirLimit;
                break;
        }
    }

    private void spawnEnemy()
    {
        float x;
        if (Random.value < 0.5f)
            x = Random.Range(-outsideDistance, left);
        else
            x = Random.Range(right, outsideDistance);

        Vector2 randomSpawn = new Vector2(x, -2.166439f);

        GameObject newEnemy = Instantiate(enemy, randomSpawn, Quaternion.identity, EnemySpawnerPoint);

        EnemyDeath enemyScript = newEnemy.GetComponent<EnemyDeath>();

        if (enemyScript != null)
            enemyScript.spawner = this;
    }

    private void setTimeUntilSpawn()
    {
        timeCounter = Random.Range(currentMinCD, currentMaxCD);
    }

    public void DecreaseSpawnCount(int count)
    {
        currentSpawn -= count;
    }
}

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

    [Header("Phase Awal (00:00 - 01:00) — Easy")]
    [SerializeField] private float awalMinCD = 8f;
    [SerializeField] private float awalMaxCD = 14f;
    [SerializeField] private int awalLimit = 3;

    [Header("Phase Tengah (01:01 - 03:00) — Medium")]
    [SerializeField] private float tengahMinCD = 5f;
    [SerializeField] private float tengahMaxCD = 9f;
    [SerializeField] private int tengahLimit = 5;

    [Header("Phase Akhir (03:01 - 05:00) — Hard")]
    [SerializeField] private float akhirMinCD = 2.5f;
    [SerializeField] private float akhirMaxCD = 5f;
    [SerializeField] private int akhirLimit = 7;

    // Current active values used for spawning
    private float currentMinCD;
    private float currentMaxCD;
    private int currentLimit;

    private float timeCounter;
    private Vector2 randomSpawn;
    [Header("How many enemies currently spawned")]
    [SerializeField] int currentSpawn = 0;

    // Current phase tracker
    public static int CurrentPhase { get; private set; } = 0; // 0=Awal, 1=Tengah, 2=Akhir

    private void Awake()
    {
        // Auto-find MoonTimer if not assigned
        if (moonTimer == null)
        {
            moonTimer = FindFirstObjectByType<MoonTimer>();
            Debug.LogError("EnemySpawner: MoonTimer tidak ditemukan! Assign MoonTimer di Inspector untuk difficulty scaling.");
            return;
        }

        ApplyPhaseValues(0);
        setTimeUntilSpawn();
    }

    private void Update()
    {
        // Determine current phase based on elapsed time
        UpdatePhase();

        if (timeCounter >= 0)
        {
            timeCounter -= Time.deltaTime;
        }

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
        if (newPhase != CurrentPhase)
        {
            CurrentPhase = newPhase;
            ApplyPhaseValues(newPhase);
        }
    }

    private int GetCurrentPhase()
    {
        if (moonTimer == null) return 0;

        float elapsed = moonTimer.TotalDuration - moonTimer.RemainingTime; // totalDuration = 300f

        if (elapsed <= 60f)        // 00:00 - 01:00
            return 0;              // Awal
        else if (elapsed <= 180f)  // 01:01 - 03:00
            return 1;              // Tengah
        else                       // 03:01 - 05:00
            return 2;              // Akhir
    }

    private void ApplyPhaseValues(int phase)
    {
        switch (phase)
        {
            case 0: // Awal
                currentMinCD = awalMinCD;
                currentMaxCD = awalMaxCD;
                currentLimit = awalLimit;
                break;
            case 1: // Tengah
                currentMinCD = tengahMinCD;
                currentMaxCD = tengahMaxCD;
                currentLimit = tengahLimit;
                break;
            case 2: // Akhir
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
        {
            x = Random.Range(-outsideDistance, left);
        }
        else
        {
            x = Random.Range(right, outsideDistance);
        }

        Vector2 randomSpawn = new Vector2(x, -2.166439f);

        GameObject newEnemy = Instantiate(enemy, randomSpawn, Quaternion.identity, EnemySpawnerPoint);

        EnemyDeath enemyScript = newEnemy.GetComponent<EnemyDeath>();

        if (enemyScript != null)
        {
            enemyScript.spawner = this;
        }
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

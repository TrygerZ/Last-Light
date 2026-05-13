using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float MinCoolDown;
    [SerializeField] float MaxCoolDown;
    [SerializeField] int limit;
    [SerializeField] GameObject enemy;
    [SerializeField] Transform EnemySpawnerPoint;

    [Header ("where the enemy won't spawn")]
    [SerializeField] float left;
    [SerializeField] float right;

    [Header ("how far out from the range left or right limit")]
    [SerializeField] float outsideDistance;

    private float timeCounter;
    private Vector2 randomSpawn;
    [Header("How many enemies currently spawned")]
    [SerializeField] int currentSpawn = 0;

    private void Awake()
    {
        setTimeUntilSpawn();
    }

    private void Update()
    {
        if (timeCounter >= 0)
        {
            timeCounter -= Time.deltaTime;
        }

        if(timeCounter <= 0 && currentSpawn < limit)
        {
            spawnEnemy();
            currentSpawn++;
            setTimeUntilSpawn();
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
        timeCounter = Random.Range(MinCoolDown, MaxCoolDown);
    }

    public void DecreaseSpawnCount(int count)
    {
        currentSpawn -= count;
    }
    
}

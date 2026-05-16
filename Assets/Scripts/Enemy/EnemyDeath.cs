using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public EnemySpawner spawner;

    private void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
    }

    private void OnDestroy()
    {
        if (spawner != null)
            spawner.DecreaseSpawnCount(1);
    }
}

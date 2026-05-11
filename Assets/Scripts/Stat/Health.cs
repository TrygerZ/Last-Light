using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Basic Stats")]
    [SerializeField] private int maxHealth;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;

    public EnemySpawner spawner;


    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    private void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnDamage(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnDamage(collision.gameObject);
    }

    private void OnDamage(GameObject obj)
    {
        Damage damageSource = obj.GetComponent<Damage>();
        if (damageSource == null) return;

        if(CompareTag("Enemy") && obj.CompareTag("Enemy"))
        {
            return;
        }

        if (CompareTag("PlayerBody") && !obj.CompareTag("Enemy")) {
            return;
        }

        if (CompareTag("Enemy") && !obj.CompareTag("PlayerTorch")) {
            return;
        }

        TakeDamage(damageSource.damage);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || CurrentHealth <= 0) {
            return;
        }

        CurrentHealth--;

        Debug.Log($"{gameObject.name} took {amount} damage ({CurrentHealth}/{maxHealth})");

        if (CurrentHealth <= 0)
        {
            if (CompareTag("Enemy"))
            {
                spawner.DecreaseSpawnCount(1);
            }
            Destroy(gameObject);
        }
    }

}

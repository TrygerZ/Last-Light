using UnityEngine;

public class Health : MonoBehaviour
{
    public GameOver deathScreen;

    [Header("Basic Stats")]
    [SerializeField] private int maxHealth;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;

    [Header("For Enemy or player Bar")]
    [SerializeField] SanityBar sanity;
    [SerializeField] GameObject enemyBar;
    private EnemyHealthBar enemyBarScript;


    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    private void Start()
    {
        enemyBarScript = enemyBar.GetComponent<EnemyHealthBar>();
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || CurrentHealth <= 0)
        {
            return;
        }

        CurrentHealth -= amount;

        Debug.Log($"{gameObject.name} took {amount} damage ({CurrentHealth}/{maxHealth})");

        if (CompareTag("PlayerBody"))
        {
            sanity.setSanity(CurrentHealth);
        }

        else if (CompareTag("Enemy"))
        {
            enemyBarScript.setHealth(CurrentHealth);
        }

        if (CurrentHealth <= 0)
        {
            if (CompareTag("PlayerBody"))
            {
                deathScreen.setUp();
                transform.parent.gameObject.SetActive(false);
            }
            else Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || CurrentHealth >= maxHealth)
        {
            return;
        }

        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);

        Debug.Log($"{gameObject.name} healed +{amount} ({CurrentHealth}/{maxHealth})");

        if (CompareTag("PlayerBody"))
        {
            sanity.setSanity(CurrentHealth);
        }
    }
}

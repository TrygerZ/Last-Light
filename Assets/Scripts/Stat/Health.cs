using UnityEngine;

public class Health : MonoBehaviour
{
    public GameOver deathScreen;

    [Header("Basic Stats")]
    [SerializeField] private int maxHealth;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;

    [Header("Bars")]
    [SerializeField] SanityBar sanity;
    [SerializeField] GameObject enemyBar;
    private EnemyHealthBar enemyBarScript;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    private void Start()
    {
        if (enemyBar != null)
            enemyBarScript = enemyBar.GetComponent<EnemyHealthBar>();
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || CurrentHealth <= 0)
            return;

        CurrentHealth -= amount;

        if (CompareTag("PlayerBody"))
            sanity.setSanity(CurrentHealth);
        else if (CompareTag("Enemy") && enemyBarScript != null)
            enemyBarScript.setHealth(CurrentHealth);

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
            return;

        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);

        if (CompareTag("PlayerBody"))
            sanity.setSanity(CurrentHealth);
    }
}

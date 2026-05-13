using UnityEngine;

public class Health : MonoBehaviour
{
    public GameOver deathScreen;

    [Header("Basic Stats")]
    [SerializeField] private int maxHealth;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;

    [SerializeField] SanityBar sanity;


    private void Awake()
    {
        CurrentHealth = maxHealth;
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
            sanity.setHealth(CurrentHealth);
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
}
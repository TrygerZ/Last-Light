using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Basic Stats")]
    [SerializeField] private int maxHealth;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;


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

        if (CompareTag("Player"))
        {
            SanityBar sanitybar;
            sanitybar = GameObject.FindGameObjectWithTag("SanityBar").GetComponent<SanityBar>();
            sanitybar.setHealth(CurrentHealth);
        }

        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
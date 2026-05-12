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
        if (!damageSource.enabled) return;

        if(CompareTag("Enemy") && obj.CompareTag("Enemy"))
        {
            return;
        }

        if (CompareTag("PlayerBody") && !obj.CompareTag("Enemy")) {
            return;
        }

        if (CompareTag("Enemy") && !obj.CompareTag("Light")) {
            return;
        }

        TakeDamage(damageSource.damage);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || CurrentHealth <= 0) 
        {
            return;
        }

        CurrentHealth--;

        Debug.Log($"{gameObject.name} took {amount} damage ({CurrentHealth}/{maxHealth})");

        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
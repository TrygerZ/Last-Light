using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damage;
    [SerializeField] float damageCooldown;
    [SerializeField] private bool isEnemy;
    private Health health;
    private float timeCheck;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damage otherDamage = collision.GetComponent<Damage>();
        if (otherDamage != null)
        {
            if (otherDamage.isEnemy == isEnemy)
            {
                return;
            }
        }

        health = collision.GetComponent<Health>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (health != null)
        {
            health = null;
            timeCheck = 0;
        }
    }

    private void Update()
    {
        if (health != null)
        {
            // Safety check: jika object health sudah di-destroy, reset
            if (health.gameObject == null)
            {
                health = null;
                timeCheck = 0;
                return;
            }

            timeCheck += Time.deltaTime;
            if (timeCheck >= damageCooldown)
            {
                health.TakeDamage(damage);
                timeCheck = 0;
            }
        }
    }
}
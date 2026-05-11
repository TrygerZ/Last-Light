using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkDamage(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        checkDamage(collision.gameObject);
    }

    private void checkDamage(GameObject obj)
    {
        Damage enemy = obj.gameObject.GetComponent<Damage>();

        if (enemy != null)
        {
            takeDamage(enemy.damage);
            Debug.Log($"{gameObject.name} Damage Taken: {enemy.damage}");
        }
    }

    private void takeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}

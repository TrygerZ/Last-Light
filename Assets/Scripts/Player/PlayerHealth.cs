using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int health;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyBehaviour enemy = collision.gameObject.GetComponent<EnemyBehaviour>();

        if (enemy != null)
        {
            takeDamage(enemy.damage);
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

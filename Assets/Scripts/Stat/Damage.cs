using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage;
    [SerializeField] float damageCooldown;

    [Header("Invulnerability Frames (Player Only)")]
    [Tooltip("Duration of invulnerability after taking damage (shared across all enemies). Prevents death-spiral from multiple enemies.")]
    [SerializeField] private float invulnerabilityDuration = 0.5f;
    [SerializeField] private bool isEnemy;

    private Health health;
    private float timeCheck;
    private float invulnerabilityTimer;

    // Static shared invulnerability for player across ALL enemy Damage components
    private static float sharedInvulnerabilityTimer = 0f;

    public float DamageCooldown => damageCooldown;

    private void Awake()
    {
        timeCheck = damageCooldown;
    }

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
        Health exitingHealth = collision.GetComponent<Health>();
        if (exitingHealth != null && exitingHealth == health)
        {
            health = null;
            timeCheck = damageCooldown;
        }
    }

    private void Update()
    {
        // Update shared invulnerability timer globally
        if (sharedInvulnerabilityTimer > 0f)
        {
            sharedInvulnerabilityTimer -= Time.deltaTime;
        }

        if (health != null)
        {
            if (health.gameObject == null)
            {
                health = null;
                timeCheck = 0;
                return;
            }

            // Skip damage ONLY if an enemy tries to hit player during invulnerability frames
            // Player can ALWAYS attack enemies regardless of i-frame status
            if (isEnemy && sharedInvulnerabilityTimer > 0f)
            {
                return;
            }

            timeCheck += Time.deltaTime;
            if (timeCheck >= damageCooldown)
            {
                health.TakeDamage(damage);

                // SFX musuh menyerang player
                if (isEnemy && AudioManager.Instance != null)
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyAttackSFX);

                // Trigger invulnerability frames if this is an enemy hitting a non-enemy (player)
                if (isEnemy)
                {
                    sharedInvulnerabilityTimer = invulnerabilityDuration;
                }

                timeCheck = 0;
            }
        }
    }
}
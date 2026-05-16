using System.Collections.Generic;
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

    private List<Health> targetsInRange = new List<Health>();
    private float timeCheck;

    // Static shared invulnerability for player across ALL enemy Damage components
    private static float sharedInvulnerabilityTimer = 0f;

    private void Start()
    {
        timeCheck = damageCooldown;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Friendly fire check
        Damage otherDamage = collision.GetComponent<Damage>();
        if (otherDamage != null)
        {
            if (otherDamage.isEnemy == isEnemy)
                return;
        }

        Health newHealth = collision.GetComponent<Health>();
        if (newHealth != null && !targetsInRange.Contains(newHealth))
        {
            targetsInRange.Add(newHealth);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Health exitingHealth = collision.GetComponent<Health>();
        if (exitingHealth != null)
        {
            targetsInRange.Remove(exitingHealth);
        }
    }

    private void Update()
    {
        // Update shared invulnerability timer globally
        if (sharedInvulnerabilityTimer > 0f)
        {
            sharedInvulnerabilityTimer -= Time.deltaTime;
        }

        // Remove null/destroyed targets
        targetsInRange.RemoveAll(t => t == null || t.gameObject == null);

        if (targetsInRange.Count == 0) return;

        timeCheck += Time.deltaTime;
        if (timeCheck >= damageCooldown)
        {
            timeCheck = 0f;

            // Damage ALL valid targets in range
            for (int i = targetsInRange.Count - 1; i >= 0; i--)
            {
                Health target = targetsInRange[i];
                if (target == null || target.gameObject == null)
                {
                    targetsInRange.RemoveAt(i);
                    continue;
                }

                // Skip if enemy tries to hit player during invulnerability frames
                if (isEnemy && sharedInvulnerabilityTimer > 0f)
                    continue;

                target.TakeDamage(damage);

                // Trigger invulnerability frames if enemy hits player
                if (isEnemy)
                {
                    sharedInvulnerabilityTimer = invulnerabilityDuration;
                }

                // Remove dead targets
                if (target.CurrentHealth <= 0)
                {
                    targetsInRange.RemoveAt(i);
                }
            }

            // SFX musuh menyerang player
            if (isEnemy && AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyAttackSFX);
        }
    }
}

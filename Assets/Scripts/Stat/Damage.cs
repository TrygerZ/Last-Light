using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage;
    [SerializeField] float damageCooldown;

    [Header("Invulnerability Frames")]
    [Tooltip("Duration of invulnerability after taking damage (shared across all enemies).")]
    [SerializeField] private float invulnerabilityDuration = 0.5f;
    [SerializeField] private bool isEnemy;

    private List<Health> targetsInRange = new List<Health>();
    private float timeCheck;

    private static float sharedInvulnerabilityTimer = 0f;

    private void Start()
    {
        timeCheck = damageCooldown;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damage otherDamage = collision.GetComponent<Damage>();
        if (otherDamage != null)
        {
            if (otherDamage.isEnemy == isEnemy)
                return;
        }

        Health newHealth = collision.GetComponent<Health>();
        if (newHealth != null && !targetsInRange.Contains(newHealth))
            targetsInRange.Add(newHealth);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Health exitingHealth = collision.GetComponent<Health>();
        if (exitingHealth != null)
            targetsInRange.Remove(exitingHealth);
    }

    private void Update()
    {
        if (sharedInvulnerabilityTimer > 0f)
            sharedInvulnerabilityTimer -= Time.deltaTime;

        targetsInRange.RemoveAll(t => t == null || t.gameObject == null);

        if (targetsInRange.Count == 0) return;

        timeCheck += Time.deltaTime;
        if (timeCheck >= damageCooldown)
        {
            timeCheck = 0f;

            for (int i = targetsInRange.Count - 1; i >= 0; i--)
            {
                Health target = targetsInRange[i];
                if (target == null || target.gameObject == null)
                {
                    targetsInRange.RemoveAt(i);
                    continue;
                }

                if (isEnemy && sharedInvulnerabilityTimer > 0f)
                    continue;

                target.TakeDamage(damage);

                if (isEnemy)
                    sharedInvulnerabilityTimer = invulnerabilityDuration;
            }

            if (isEnemy && AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyAttackSFX);
        }
    }
}

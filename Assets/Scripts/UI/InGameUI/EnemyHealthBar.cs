using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Slider slider;
    private Health health;

    private void Start()
    {
        health = transform.parent.parent.GetComponent<Health>();
        slider = GetComponent<Slider>();
        slider.maxValue = health.MaxHealth;
        slider.value = health.MaxHealth;
    }
    public void setHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }
}

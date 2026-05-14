using UnityEngine;
using UnityEngine.UI;
public class SanityBar : MonoBehaviour
{
    private Slider slider;
    private Health health;

    private void Start()
    {
        health = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Health>();
        slider = GetComponent<Slider>();
        slider.maxValue = health.MaxHealth;
        slider.value = health.MaxHealth;
    }
    public void setHealth(int currenthealth)
    {
        slider.value = currenthealth;
    }
}

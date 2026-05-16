using UnityEngine;
using UnityEngine.UI;
public class SanityBar : MonoBehaviour
{
    private Slider slider;
    private Health sanity;

    private void Start()
    {
        sanity = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Health>();
        slider = GetComponent<Slider>();
        slider.maxValue = sanity.MaxHealth;
        slider.value = sanity.MaxHealth;
    }
    public void setSanity(int currentSanity)
    {
        slider.value = currentSanity;
    }
}

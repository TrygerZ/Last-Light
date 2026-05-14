using UnityEngine;
using UnityEngine.UI;

public class TorchDuration : MonoBehaviour
{
    private Slider slider;
    [SerializeField] GameObject fire;
    private TorchBurnout burn;

    private void Start()
    {
        burn = fire.GetComponent<TorchBurnout>();
        slider = GetComponent<Slider>();

        slider.maxValue = burn.MaxTorchDuration;
        slider.value = burn.RemainingTime;
    }
    public void setFire(float currentFire)
    {
        slider.value = currentFire;
    }
}

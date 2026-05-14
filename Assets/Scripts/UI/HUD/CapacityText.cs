using UnityEngine;
using TMPro;

public class CapacityText : MonoBehaviour
{
    private TMP_Text text;
    [SerializeField] int maxCapacity;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = string.Format("Capacity: {0}/{1}", 0, maxCapacity);
    }

    public void getCapacity(float amount)
    {
        text.text = string.Format("Capacity: {0}/{1}", amount, maxCapacity);
    }
}

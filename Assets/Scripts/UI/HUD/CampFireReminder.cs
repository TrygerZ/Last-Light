using TMPro;
using UnityEngine;

public class CampFireReminder : MonoBehaviour
{
    [SerializeField] GameObject campFire;
    [SerializeField] float reminderTimer;

    private CampfireBurnout campTimeDuration;
    private TMP_Text campFireText;

    private void Start()
    {
        campTimeDuration = campFire.GetComponent<CampfireBurnout>();
        campFireText = gameObject.GetComponent<TMP_Text>();
    }

    private void Update()
    {
        float timer = campTimeDuration.RemainingTime;
        if (timer <= reminderTimer)GetComponent<TMP_Text>().enabled = true;
        else GetComponent<TMP_Text>().enabled = false;
    }
}

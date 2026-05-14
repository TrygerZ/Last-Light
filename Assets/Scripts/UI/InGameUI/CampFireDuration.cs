using System.Threading;
using TMPro;
using UnityEngine;

public class CampFireDuration : MonoBehaviour
{
    [SerializeField] GameObject campFire;
    private CampfireBurnout campTimeDuration;
    private TMP_Text torchDurationText;


    private void Start()
    {
        campTimeDuration = campFire.GetComponent<CampfireBurnout>();
        torchDurationText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (campTimeDuration != null)
        {
            float time = campTimeDuration.RemainingTime;

            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);

            torchDurationText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

}

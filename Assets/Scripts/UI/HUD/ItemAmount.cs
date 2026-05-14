using UnityEngine;
using TMPro;
using System;
public class ItemAmount : MonoBehaviour
{
    private TMP_Text amount;

    private void Start()
    {
        amount = GetComponent<TMP_Text>();
        amount.text = 0.ToString();
    }

    public void textCount(int quantity)
    {
        amount.text = quantity.ToString();
    }
}

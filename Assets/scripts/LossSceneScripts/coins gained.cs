 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coinsgained : MonoBehaviour
{

    public TextMeshProUGUI coinsGainedText;  

    void Update()
    {
        coinsGainedText.text = "Coins Gained: " + DataHolder.Coins.ToString();
    }
}
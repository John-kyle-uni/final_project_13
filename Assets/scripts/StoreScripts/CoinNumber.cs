 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinNumber : MonoBehaviour
{

    public TextMeshProUGUI coinCountText;  

    void Update()
    {
        coinCountText.text = "Coins: " + DataHolder.Coins.ToString();
    }
}

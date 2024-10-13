using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyHealth : MonoBehaviour
{
    public void BuyHP() {
        if (DataHolder.Coins >= 5) {
            DataHolder.Coins -= 5;
            DataHolder.PlayerHealth += 20;
            Debug.Log(DataHolder.PlayerHealth);
        }
    }
}

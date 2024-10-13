using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyDamage : MonoBehaviour
{
    public void BuyDmg() {
        if (DataHolder.Coins >= 5) {
            DataHolder.Coins -= 5;
            DataHolder.PlayerDamage += 5;
            Debug.Log(DataHolder.PlayerDamage);
        }
    }
}

using UnityEngine;
using TMPro;  

public class CoinsCollected : MonoBehaviour
{
    public TextMeshProUGUI coinText;  
    public int coinsCollected = 0;

    void Start()
    {
        UpdateCoinUI();  
    }
    public void CollectCoin()
    {
        coinsCollected++;  // Increment the coin count
        UpdateCoinUI();    // Update the UI to display the new coin count
    }

    // Updates the UI text with the current number of coins
    void UpdateCoinUI()
    {
        coinText.text = "Coins: " + coinsCollected.ToString() + "/10";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Add this line to use SceneManager

public class LevelManager : MonoBehaviour
{
    public float elapsedTime = 0f;  // Timer to track how much time has passed
    public int CoinsCollected = 0;


    void Start()
    {   
        // Start the timer when the level begins
        elapsedTime = 0f; // Reset the timer
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (CoinsCollected >= 10) {
            SceneManager.LoadScene("Victory Screen");
        }
    }

    public void CoinCollected() {
        CoinsCollected++;
        Debug.Log(CoinsCollected);
    }

}

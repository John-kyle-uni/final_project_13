using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Add this line to use SceneManager

public class LevelManager : MonoBehaviour
{
    public float elapsedTime = 0f;  // Timer to track how much time has passed
    public int CoinsCollected = 0;
    public int maxEntries = 5;





    void Start()
    {   
        // Start the timer when the level begins
        elapsedTime = 0f; // Reset the timer
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (CoinsCollected >= 10) {

            Cursor.visible = true;  // Show the cursor
            Cursor.lockState = CursorLockMode.None;  // Free the cursor from being locked

            for (int i = 0; i < maxEntries; i++)
                {
                    float savedTime = PlayerPrefs.GetFloat("Time_" + i, float.MaxValue);
                    if (elapsedTime < savedTime)
                    {
                        // Shift the entries down
                        for (int j = maxEntries - 1; j > i; j--)
                        {
                            PlayerPrefs.SetFloat("Time_" + j, PlayerPrefs.GetFloat("Time_" + (j - 1)));
                        }
                        // Save the new time
                        PlayerPrefs.SetFloat("Time_" + i, elapsedTime);
                        break;
                    }
                }
                PlayerPrefs.Save();

            SceneManager.LoadScene("Victory Screen");
        }
    }

    public void CoinCollected() {
        CoinsCollected++;
        Debug.Log(CoinsCollected);
    }

}

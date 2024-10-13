using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float elapsedTime = 0f;  // Timer to track how much time has passed
    public int CoinsCollected = 0;
    public int maxEntries = 5;
    public CoinsCollected coinsCollected;

    void Start()
    {
        // Start the timer when the level begins
        elapsedTime = 0f; // Reset the timer
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (coinsCollected.coinsCollected >= 10)
        {
            Cursor.visible = true;  // Show the cursor
            Cursor.lockState = CursorLockMode.None;  // Free the cursor from being locked

            DataHolder.levelTime = elapsedTime;  // Save the time to the DataHolder


            // Save the player's time
            SavePlayerTime();

            DataHolder.Coins += coinsCollected.coinsCollected;
            // Load the victory screen scene
            SceneManager.LoadScene("Victory Screen");
        }
    }

void SavePlayerTime()
{
    if (elapsedTime <= 0)  // Ignore times that are zero or negative
    {
        return;
    }

    bool timeSaved = false;
    float savedTime;

    // First, check for an empty slot (float.MaxValue or zero) and add the new time there
    for (int i = 0; i < maxEntries; i++)
    {
        savedTime = PlayerPrefs.GetFloat("Time_" + i, float.MaxValue);

        // If the slot is empty (either float.MaxValue or zero), save the new time
        if (savedTime == float.MaxValue || savedTime == 0f)
        {
            PlayerPrefs.SetFloat("Time_" + i, elapsedTime);
            PlayerPrefs.SetString("Name_" + i, PlayerPrefs.GetString("PlayerName", "Unknown"));

            timeSaved = true;  // Mark that the time has been saved
            break;
        }
    }

    // If no empty slot was found, compare with existing times
    if (!timeSaved)
    {
        for (int i = 0; i < maxEntries; i++)
        {
            savedTime = PlayerPrefs.GetFloat("Time_" + i, float.MaxValue);

            // Compare if the current time is better (lower) than the saved one
            if (elapsedTime < savedTime)
            {
                // Shift the times and names down to make space for the new entry
                for (int j = maxEntries - 1; j > i; j--)
                {
                    PlayerPrefs.SetFloat("Time_" + j, PlayerPrefs.GetFloat("Time_" + (j - 1)));
                    PlayerPrefs.SetString("Name_" + j, PlayerPrefs.GetString("Name_" + (j - 1)));
                }

                // Save the new time and name at the correct position
                PlayerPrefs.SetFloat("Time_" + i, elapsedTime);
                PlayerPrefs.SetString("Name_" + i, PlayerPrefs.GetString("PlayerName", "Unknown"));

                timeSaved = true;
                break;
            }
        }
    }
    PlayerPrefs.Save();
}


}



using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // Add this line at the top



public class LeaderboardDsiplay : MonoBehaviour
{
    public TextMeshProUGUI[] leaderboardTexts; // Change to TextMeshProUGUI
    public int maxEntries = 5; // Maximum number of entries to show

    void Start()
    {
        DisplayLeaderboard();
    }

    void DisplayLeaderboard()
    {
        List<float> times = new List<float>();

        // Load times from PlayerPrefs
        for (int i = 0; i < maxEntries; i++)
        {
            float time = PlayerPrefs.GetFloat("Time_" + i, float.MaxValue);
            if (time != float.MaxValue)
            {
                times.Add(time);
            }
        }

        // Sort times
        times.Sort();

        // Display times in the UI
        for (int i = 0; i < leaderboardTexts.Length; i++)
        {
            if (i < times.Count)
            {
                leaderboardTexts[i].text = "Player " + (i + 1) + ": " + times[times.Count - 1 - i].ToString("F2") + " seconds";
            }
            else
            {
                leaderboardTexts[i].text = "Player " + (i + 1) + ": ---"; // Empty entry
            }
        }
    }
}

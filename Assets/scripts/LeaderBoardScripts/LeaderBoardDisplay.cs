using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LeaderboardDisplay : MonoBehaviour
{
    public TextMeshProUGUI[] leaderboardTexts; 
    public int maxEntries; 

    void Start()
    {
        DisplayLeaderboard();
    }

    void DisplayLeaderboard()
    {
        List<float> times = new List<float>();
        List<string> names = new List<string>();

        // Load times and names from PlayerPrefs
        for (int i = 0; i < maxEntries; i++)
        {
            float time = PlayerPrefs.GetFloat("Time_" + i, float.MaxValue);
            string name = PlayerPrefs.GetString("Name_" + i, "___");  // Load names
            
            // Only add valid entries (not float.MaxValue and greater than 0)
            if (time != float.MaxValue && time > 0)
            {
                times.Add(time);
                names.Add(name);
            }
        }

        // Sort times and corresponding names in ascending order
        for (int i = 0; i < times.Count; i++)
        {
            for (int j = i + 1; j < times.Count; j++)
            {
                if (times[j] < times[i]) // Sort in ascending order
                {
                    // Swap times
                    float tempTime = times[i];
                    times[i] = times[j];
                    times[j] = tempTime;

                    // Swap names accordingly
                    string tempName = names[i];
                    names[i] = names[j];
                    names[j] = tempName;
                }
            }
        }

        // Display times and names in the UI
        for (int i = 0; i < leaderboardTexts.Length; i++)
        {
            if (i < times.Count)
            {
                leaderboardTexts[i].text = names[i] + ": " + times[i].ToString("F2") + " seconds";
            }
            else
            {
                leaderboardTexts[i].text = "Player " + (i + 1) + ": ---"; // Empty entry
            }
        }
    }
}

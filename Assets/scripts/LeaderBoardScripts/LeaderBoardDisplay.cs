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

        for (int i = 0; i < maxEntries; i++)
        {
            float time = PlayerPrefs.GetFloat("Time_" + i, float.MaxValue);
            string name = PlayerPrefs.GetString("Name_" + i, "___");  // Load names
            
            if (time != float.MaxValue && time > 0)
            {
                times.Add(time);
                names.Add(name);
            }
        }

        for (int i = 0; i < times.Count; i++)
        {
            for (int j = i + 1; j < times.Count; j++)
            {
                if (times[j] < times[i]) 
                {
                    float tempTime = times[i];
                    times[i] = times[j];
                    times[j] = tempTime;

                    string tempName = names[i];
                    names[i] = names[j];
                    names[j] = tempName;
                }
            }
        }

        for (int i = 0; i < leaderboardTexts.Length; i++)
        {
            if (i < times.Count)
            {
                leaderboardTexts[i].text = names[i] + ": " + times[i].ToString("F2") + " seconds";
            }
            else
            {
                leaderboardTexts[i].text = "Player " + (i + 1) + ": ---"; 
            }
        }
    }
}

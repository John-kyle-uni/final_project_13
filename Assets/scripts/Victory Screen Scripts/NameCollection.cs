using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NameCollection : MonoBehaviour
{
    public TMP_InputField nameInputField;  
    public TextMeshProUGUI messageText; 

    public void OnSubmitName()
    {
        string playerName = nameInputField.text.Trim();  

        if (string.IsNullOrEmpty(playerName))
        {
            return;  
        }


        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        for (int i = 0; i < DataHolder.maxEntries; i++)
        {
            if (PlayerPrefs.GetFloat("Time_" + i, float.MaxValue) == DataHolder.levelTime)
            {
                PlayerPrefs.SetString("Name_" + i, playerName);  
                break;
            }
        }

        PlayerPrefs.Save();

        SceneManager.LoadScene("Leaderboard");  
    }
}

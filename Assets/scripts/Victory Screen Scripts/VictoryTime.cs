using UnityEngine;
using TMPro;


public class VictoryTime : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    void Start() {
        uiText.text = "Time: " + DataHolder.levelTime.ToString("F2") + " seconds";
    }
}

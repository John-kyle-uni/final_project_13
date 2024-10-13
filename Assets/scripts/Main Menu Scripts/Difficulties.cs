using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown dropdownMenu;  // Reference to the Dropdown component
    public Button saveButton;      // Reference to the Button component

    void Start()
    {
        // Load the saved choice, if any, when the game starts
        int savedChoice = PlayerPrefs.GetInt("SelectedDropdownChoice", 0);
        dropdownMenu.value = savedChoice;

        // Add a listener to the save button
        saveButton.onClick.AddListener(SaveDropdownChoice);
    }

    // Save the selected choice to PlayerPrefs when the button is clicked
    public void SaveDropdownChoice()
    {
        int selectedIndex = dropdownMenu.value;

        // Call different methods based on the selected option
        HandleDropdownChoice(selectedIndex);
    }

    // Handle different outcomes based on the selected option
    void HandleDropdownChoice(int choice)
    {
        switch (choice)
        {
            case 0:
                DataHolder.Difficulty = 1;
                break;
            case 1:
                DataHolder.Difficulty = 2;
                break;
            case 2:
                DataHolder.Difficulty = 3;
                break;
            case 3:
                DataHolder.Difficulty = 5;
                break;
            case 4:
                DataHolder.Difficulty = 7;
                break;
            case 5:
                DataHolder.Difficulty = 10;
                break;
        }
        Debug.Log(DataHolder.Difficulty);
    }
}

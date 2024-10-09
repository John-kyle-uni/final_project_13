using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class MainMenuReturn : MonoBehaviour
{
    // Call this method to start the game
    public void MenuReturn()
    {
        SceneManager.LoadScene("main menu"); // Replace with your game scene name
    }
}

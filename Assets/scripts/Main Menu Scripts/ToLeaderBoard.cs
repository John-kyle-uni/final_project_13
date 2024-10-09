using UnityEngine;
using UnityEngine.SceneManagement; 

public class ToLeaderBoard : MonoBehaviour
{
    // Call this method to start the game
    public void LeaderboardTravel()
    {
        SceneManager.LoadScene("Leaderboard"); // Replace with your game scene name
    }
}

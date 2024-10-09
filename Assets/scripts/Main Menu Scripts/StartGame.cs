using UnityEngine;
using UnityEngine.SceneManagement; 

public class StartGame : MonoBehaviour
{
    public void Startlevel()
    {
        SceneManager.LoadScene("level1"); 
    }
}

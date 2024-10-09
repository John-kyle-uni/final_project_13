using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Add this line to use SceneManager

public class LevelManager : MonoBehaviour
{
    public float elapsedTime = 0f;  // Timer to track how much time has passed


    void Start()
    {
        // Start the timer when the level begins
        elapsedTime = 0f; // Reset the timer
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }
}

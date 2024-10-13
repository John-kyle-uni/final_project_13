using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ToCredits : MonoBehaviour
{
    public void StartCredits()
    {
        SceneManager.LoadScene("Credits"); 
    }
}

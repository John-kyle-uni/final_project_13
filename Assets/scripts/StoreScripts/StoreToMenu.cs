using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreToMenu : MonoBehaviour
{
    public void StoreToMain() {
        SceneManager.LoadScene("main menu");
    }
}

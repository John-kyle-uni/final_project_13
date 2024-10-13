using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToStore : MonoBehaviour
{
    public void storeTravel() {
        SceneManager.LoadScene("ShopScene");
    }
}

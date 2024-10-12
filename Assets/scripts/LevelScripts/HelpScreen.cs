using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : MonoBehaviour
{
        void Update()
    {
        // Check if the Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return))  
        {
            Destroy(gameObject);  // Destroy the UI element this script is attached to
        }
    }


}

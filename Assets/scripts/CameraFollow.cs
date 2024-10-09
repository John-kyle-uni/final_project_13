using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

// Separating camera rotation from the character ensures smooth movement by avoiding conflicts 
// between their motions. LateUpdate is used for the camera to follow the character smoothly, updating 
// after all movements to prevent stuttering, especially when using a CharacterController
public class CameraFollow : MonoBehaviour
{

    // Treat this target as camera when working on your crouching mechanic, don't edit this script.
    public Transform cameraTarget;
    

    private void Update()
    {
        transform.position = cameraTarget.position;
    }
}
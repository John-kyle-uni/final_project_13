using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    public float rotationSpeed = 100f; 
    public float bobbingSpeed = 2f;     
    public float bobbingAmount = 0.5f;  
    public float pickupRadius = 2f;

    private Vector3 initialPosition;  
    private Transform playerTransform;


  
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        initialPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        float newY = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
        transform.position = new Vector3(initialPosition.x, initialPosition.y + newY + 1, initialPosition.z);

        if (Vector3.Distance(playerTransform.position, transform.position) <= pickupRadius)
        {
            //gameManager.KeyCollected();

            Debug.Log("coin collected");

            Destroy(gameObject);
        }
    }
}

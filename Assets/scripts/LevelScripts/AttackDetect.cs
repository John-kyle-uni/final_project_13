using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetect : MonoBehaviour
{
    public SwordControl sc;
    public GameObject HitParticle;

    public int damage = 50;
    private Vector3 newPos;

    void Update()
	{

		// see if bullet hits a collider
		RaycastHit hit;
		if (Physics.Linecast(transform.position, newPos, out hit))
		{
			if (hit.collider)
			{




				
				// apply damage to object
				GameObject obj = hit.collider.gameObject;
				if ((obj.tag == "Player") | (obj.tag == "enemy"))
					obj.SendMessage("ApplyDamage", damage);
			}
		}
	}
    // private void OnTrigger(Collider other){

    //     if(other.tag == "enemy" && sc.isAttacking)
    //     {
    //         Debug.Log(other.name);
    //         GameObject obj = hit.collider.gameObject;
	// 			if (obj.tag == "enemy")
	// 				obj.SendMessage("ApplyDamage", damage);
    //     }
    // }
}

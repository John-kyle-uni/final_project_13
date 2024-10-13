using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordControl : MonoBehaviour
{
    public GameObject Sword;

    public bool canAttack =  true;
    public float AttackCD = 1.0f;
    public bool isAttacking = false;
    public AttackDetect attackDetect;
    public float damage = 50.0f;


    private void Update(){
        if(Input.GetMouseButtonDown(0)){
            if(canAttack){
                SwordAttack();
            }
        }
        
    }
    public void SwordAttack()
    {
        isAttacking = true;
        canAttack = false;
        Animator anim = Sword.GetComponent<Animator>();
        anim.SetTrigger("Attack");
        StartCoroutine(resetATK());
    }

    IEnumerator resetATK()
    {
        StartCoroutine(attackReset());
        yield return new WaitForSeconds(AttackCD);
        canAttack = true;
    }
    IEnumerator attackReset(){
        yield return new WaitForSeconds(1.0f);
        attackDetect.isHit = false;
        isAttacking = false;
    }
}

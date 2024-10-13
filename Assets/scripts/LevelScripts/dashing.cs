using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashing : MonoBehaviour
{
    //----------------------------------------
    public Transform orientation;
    public Transform playercam;
    private Rigidbody rb;
    private PlayerMovement pm;
    //--------------------------------

    public float dashForce;
    public float dashUpForce;
    public float dashDur;
    public float maxDashY;
    //-----------------------

    public float dashCD;
    
    private float CDtimer;

    public KeyCode dashkey = KeyCode.Q;

    //------------------------------------

    public bool camForward = false;
    public bool allowDir = true;
    public bool disableGrav = false;
    public bool resetVel = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(dashkey) ) {
            Dash();    
        }
        if(CDtimer > 0)
            CDtimer -= Time.deltaTime;
    }

    private void Dash()
    {   
        if(CDtimer > 0) return;
        else CDtimer = dashCD;
        pm.dashing = true;
        pm.maxYspeed = maxDashY;

        Transform forwardT;
        if(camForward)
            forwardT = playercam;
        else
            forwardT = orientation;


        Vector3 direction = GetDir(forwardT);

        Vector3 forceApply = direction * dashForce + orientation.up * dashUpForce;

        if(disableGrav)
            rb.useGravity = false;
        
        delayDashForce = forceApply;
        Invoke(nameof(DelayDash), 0.025f);
        Invoke(nameof(ResetDash), dashDur);
    }
    private Vector3 delayDashForce;
    private void DelayDash()
    {
        if(resetVel)
            rb.velocity = Vector3.zero;
        rb.AddForce(delayDashForce, ForceMode.Impulse);
    }
    private void ResetDash(){
        pm.dashing = false;
        pm.maxYspeed = 0;
        if(disableGrav)
            rb.useGravity = true;

    }
     private Vector3 GetDir(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowDir)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
}

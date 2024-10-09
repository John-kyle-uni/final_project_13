using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{

    [Header("UI")]
    public float maxEnergy = 100f;
    public float energySprintDrain = 20f; // per second
    public float energyRecoveryRate = 30f; // per second
    public float energyRecoveryDelay = 15f;

    public float energyRegenDur = 5f;

    public float CurrentEnergy = 100f;

    public float maxHealth = 100f;
    public float health = 100f;
    [Header("Movement")]
    public float walkSpeed = 5f; 
    public float sprintSpeed = 10f;
    private float moveSpeed;
    public float dragV;

    //----------------------------------
    public float jumpCD;
    bool jumpReady;
    public float AirMulti;
    public float jumpHeight ;

    //--------------------------------------
    // public Image energyBar;

    // public Image HPBar;

    
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;


    //------------------------------------------

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintkey = KeyCode.LeftShift;
    public KeyCode backwardskey = KeyCode.S;
    public KeyCode crouchkey = KeyCode.LeftControl;

    //-------------------------------------------------

    public float playerHeight;
    public LayerMask onGround;
    bool grounded;

    //----------------------------------------

    
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool slopeExit;

    // ----------------------------------
    private Rigidbody rb;
    // private CameraFollow CamMove;
    // private Camera cam;
    private float xInput;
    private float yInput;
    public Transform orientation;
 
    Vector3 moveDir;
    public float shootRate = 0.5f;
	protected float elapsedTime;

    public GameObject bullet;
	public GameObject bulletSpawnPoint;

    //sounds


    
    public moveState state;
    public enum moveState
    {
        walking,
        sprinting,
        backwards,
        crouching,
        air

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        jumpReady = true;

        startYScale = transform.localScale.y;
        elapsedTime = shootRate;

    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f , onGround);

        
        
        myInput();
        stateHandle();
        speedControl();


        
        // if(health <= 0)
        // {
            
        // }

        if (grounded)
        {
            rb.drag = dragV;
        }
        else
        {
            rb.drag = 0;
        }
 
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    void myInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && jumpReady && grounded) {

            jumpReady = false;
            UpdateJump();
            Invoke(nameof(ResetJump), jumpCD);
        }

        if (Input.GetKeyDown(crouchkey))
        {
            transform.localScale = new Vector3 (transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchkey))
        {
            transform.localScale = new Vector3 (transform.localScale.x, startYScale, transform.localScale.z);
        }

        if (Input.GetButtonDown("Fire1"))
		{
				if ((bulletSpawnPoint) & (bullet))
					Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
	
		}
    }
      void stateHandle()
    {
        // if (grounded && Input.GetKey(sprintkey) && CurrentEnergy > 0f) 
        // {
        //     UseSprint(1);
        //     state = moveState.sprinting;
        //     moveSpeed = sprintSpeed;
        // }
        
        if (Input.GetKey(crouchkey))
        {
            state = moveState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (grounded && Input.GetKey(sprintkey))
        {
            UseSprint(1);
            state = moveState.sprinting;
            moveSpeed = sprintSpeed;
        }
        
        else if (grounded)
        {
            state = moveState.walking;
            moveSpeed = walkSpeed;
        }
        
        // else if (grounded && CurrentEnergy <= 0f)
        // {
        //     state = moveState.walking;
        //     moveSpeed = walkSpeed * 0.75f;
        //     regenEnergy();
        // }
        else if (grounded && Input.GetKey(backwardskey))
        {
            state = moveState.backwards;
            moveSpeed = walkSpeed * 0.5f;
        }
        
        else 
        {
            state = moveState.air;
            
        }
        
    }

    void UpdateMovement()
    {
        moveDir = orientation.forward * yInput + orientation.right * xInput;

        if (OnSlope() && !slopeExit)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
 
        else if(grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);

        else if(!grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * AirMulti, ForceMode.Force);

        rb.useGravity = !OnSlope();
        
    }

    private void speedControl()
    {
        if (OnSlope() && !slopeExit)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitVel = flatVel.normalized*moveSpeed;
            rb.velocity = new Vector3( limitVel.x, rb.velocity.y, limitVel.z);
        }
        }
        
    }

  

    void UpdateJump()
    {
        slopeExit = true;

        rb.velocity = new Vector3 (rb.velocity.x, 0f, rb.velocity.z);
        
        rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);

    }
    void ResetJump()
    {
        jumpReady = true;
    }
    void UpdateCrouch()
    {

    }

    void UseSprint(float useEnergy)
    {
        CurrentEnergy -= useEnergy * 0.1f;
        // energyBar.fillAmount = CurrentEnergy / 100f;
    }

    public void regenEnergy ()
    {
        StartCoroutine(UpdateEnergy());
    }

    private IEnumerator UpdateEnergy( )
    {
        float regenTick = energyRecoveryRate;
        float elapsedTime = 0f;

        while ( elapsedTime < energyRegenDur)
        {
            if ( CurrentEnergy < maxEnergy)
            {
                CurrentEnergy += regenTick;
                CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0f, maxEnergy);
                // energyBar.fillAmount = CurrentEnergy / 100f;

            }

            elapsedTime += energyRecoveryDelay;
            yield return new WaitForSeconds( energyRecoveryDelay);
        }


        

    }
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }


  
    
    public void ApplyDamage(int damage ) {
    	health -= damage * 0.5f;
        health = Mathf.Clamp(health, 0f , maxHealth);

        // HPBar.fillAmount = health / maxHealth;

        if (health <= 0f)
        {
            die();
        }
    }
    public void die()
    {

    }

}
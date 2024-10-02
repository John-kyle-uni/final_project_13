using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // Some helpful variables with pre-set values to use - this won't be all the variables you will need
    public float walkSpeed = 5f; // Walking backwards and crouching will be half walkSpeed
    public float sprintSpeed = 10f;
    private float moveSpeed;
    public float sprintFOV = 75f;
    public float normalFOV = 60f;
    public float jumpHeight = 4f;
    public float gravity = -10f;
    public float maxEnergy = 100f;
    public float energySprintDrain = 20f; // per second
    public float energyRecoveryRate = 30f; // per second
    public float energyRecoveryDelay = 15f;

    public float energyRegenDur = 5f;

    public float CurrentEnergy = 100f;

    public float maxHealth = 100f;
    public float health = 100f;

    // public Image energyBar;

    // public Image HPBar;


    
    private Rigidbody rb;
    private CameraFollow CamMove;
    private Camera cam;
    private float xInput;
    private float yInput;
    public Transform orientation;
    public float playerHeight;
    public LayerMask onGround;
    bool grounded;
    public float dragV;
    Vector3 moveDir;

    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    public float jumpCD;
    bool jumpReady;
    public float AirMulti;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintkey = KeyCode.LeftShift;
    public KeyCode backwardskey = KeyCode.S;
    public KeyCode crouchkey = KeyCode.LeftControl;

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
        air,
        backwards,
        crouching
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
        elapsedTime = shootRate;

    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, onGround);

        UpdateMovement();
        UpdateCrouch();
        UpdateFOV();
        myInput();
        stateHandle();

        
        if(health <= 0)
        {
            
        }

        if (grounded)
        {
            rb.drag = dragV;
        }
        else
        {
            rb.drag = 0;
        }

        

        
    }

    void UpdateMovement()
    {
        moveDir = orientation.forward * yInput + orientation.right * xInput;

        if(grounded)
        {
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!grounded)
        {
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * AirMulti, ForceMode.Force);
        }
        
    

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitVel = flatVel.normalized*moveSpeed;
            rb.velocity = new Vector3( limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    void stateHandle()
    {
        if (grounded && Input.GetKey(sprintkey) && CurrentEnergy > 0f) 
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
        else
        {
            state = moveState.air;
            
        }

        if (grounded && CurrentEnergy <= 0f)
        {
            state = moveState.walking;
            moveSpeed = walkSpeed * 0.75f;
            regenEnergy();
        }
        if (grounded && Input.GetKey(backwardskey))
        {
            state = moveState.backwards;
            moveSpeed = walkSpeed * 0.5f;
        }
        if (Input.GetKey(crouchkey))
        {
            state = moveState.crouching;
            moveSpeed = crouchSpeed;
        }
        
    }

    void UpdateJump()
    {
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

    void UpdateFOV()
    {

    }
    void myInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(jumpKey) && grounded) {
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
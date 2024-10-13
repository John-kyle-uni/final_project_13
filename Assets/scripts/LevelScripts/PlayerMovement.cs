using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [Header("UI")]
    public float maxEnergy = 100f;
    public float energySprintDrain = 20f;
    public float energyRecoveryRate = 30f;
    public float energyRecoveryDelay = 15f;
    public float energyRegenDur = 5f;

    public float CurrentEnergy = 100f;

    public float maxHealth = 100f;
    public float health = 100f;

    [Header("Movement")]
    public float walkSpeed; 
    public float sprintSpeed;
    public float dashSpeed;
    public float dashTransition;
    private float moveSpeed;
    public float dragV;
    public float maxYspeed;

    public float jumpCD;
    bool jumpReady;
    public float AirMulti;
    public float jumpHeight;

    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintkey = KeyCode.LeftShift;
    public KeyCode backwardskey = KeyCode.S;
    public KeyCode crouchkey = KeyCode.LeftControl;

    public float playerHeight;
    public LayerMask onGround;
    bool grounded;

    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool slopeExit;

    private Rigidbody rb;
    private float xInput;
    private float yInput;
    public Transform orientation;
    Vector3 moveDir;

    public float shootRate = 0.5f;
    protected float elapsedTime;

    public GameObject bullet;
    public GameObject bulletSpawnPoint;

    public AudioSource audioSource;
    public AudioClip[] walkClips;  
    public AudioClip[] runClips;    
    public float timeBetweenStepsWalk = 0.5f;  
    public float timeBetweenStepsRun = 0.3f;   

    private bool isWalking = false;
    private bool isRunning = false;
    private float stepTimer = 0f;

    public moveState state;
    public enum moveState
    {
        walking,
        sprinting,
        backwards,
        crouching,
        dashing,
        air
    }
    
    public bool dashing;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        jumpReady = true;
        startYScale = transform.localScale.y;
        elapsedTime = shootRate;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, onGround);
        myInput();
        stateHandle();
        speedControl();
        HandleWalkingSound();
        rb.drag = (state == moveState.walking || state == moveState.sprinting || state == moveState.crouching) ? dragV : 0;
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    void myInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && jumpReady && grounded)
        {
            jumpReady = false;
            UpdateJump();
            Invoke(nameof(ResetJump), jumpCD);
        }

        if (Input.GetKeyDown(crouchkey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchkey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if ((bulletSpawnPoint) && (bullet))
                Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        }
    }

    private float desiredSpeed;
    private float lastDesiredSpeed;
    private moveState lastState;
    private bool keepMomentum;

    void stateHandle()
    {
        if (dashing)
        {
            state = moveState.dashing;
            desiredSpeed = dashSpeed;
            speedChangeSmooth = dashTransition;
        }
        else if (Input.GetKey(crouchkey))
        {
            state = moveState.crouching;
            desiredSpeed = crouchSpeed;
        }
        else if (grounded && Input.GetKey(sprintkey))
        {
            UseSprint(1);
            state = moveState.sprinting;
            desiredSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = moveState.walking;
            desiredSpeed = walkSpeed;
        }
        else if (grounded && Input.GetKey(backwardskey))
        {
            state = moveState.backwards;
            desiredSpeed = walkSpeed * 0.5f;
        }
        else 
        {
            state = moveState.air;
            audioSource.Stop();
            desiredSpeed = (desiredSpeed < sprintSpeed) ? walkSpeed : sprintSpeed;
        }

        bool desiredSpeedChanged = desiredSpeed != lastDesiredSpeed;
        if (lastState == moveState.dashing) keepMomentum = true;

        if (desiredSpeedChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothSpeed());
            }
            else
            {   
                StopAllCoroutines();
                moveSpeed = desiredSpeed;
            }
        }
        lastDesiredSpeed = desiredSpeed;
        lastState = state;
    }

    private float speedChangeSmooth;

    private IEnumerator SmoothSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredSpeed - moveSpeed);
        float startValue = moveSpeed;

        float booster = speedChangeSmooth;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredSpeed, time / difference);
            time += Time.deltaTime * booster;
            yield return null;
        }

        moveSpeed = desiredSpeed;
        speedChangeSmooth = 1f;
        keepMomentum = false;
    }

    void UpdateMovement()
{
    if (state == moveState.dashing) return;

    Vector3 platformVelocity = Vector3.zero;
    if (IsOnPlatform(out Transform platform))
    {
        platformVelocity = platform.GetComponent<Rigidbody>().velocity;
    }

    moveDir = orientation.forward * yInput + orientation.right * xInput;

    if (OnSlope() && !slopeExit)
    {
        rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
        if (rb.velocity.y > 0)
            rb.AddForce(Vector3.down * 80f, ForceMode.Force);
    }
    else if (grounded)
    {
        Vector3 desiredVelocity = moveDir.normalized * moveSpeed;
        rb.velocity = new Vector3(desiredVelocity.x + platformVelocity.x, rb.velocity.y, desiredVelocity.z + platformVelocity.z);
    }
    else if (!grounded)
    {
        rb.AddForce(moveDir.normalized * moveSpeed * 10f * AirMulti, ForceMode.Force);
    }

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
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
            }
        }

        if (maxYspeed != 0 && rb.velocity.y > maxYspeed)
            rb.velocity = new Vector3(rb.velocity.x, maxYspeed, rb.velocity.z);
    }

    void UpdateJump()
    {
        slopeExit = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
    }

    void ResetJump()
    {
        jumpReady = true;
        slopeExit = false;
    }

    void UseSprint(float useEnergy)
    {
        CurrentEnergy -= useEnergy * 0.1f;
    }

    public void regenEnergy()
    {
        StartCoroutine(UpdateEnergy());
    }

    private IEnumerator UpdateEnergy()
    {
        float regenTick = energyRecoveryRate;
        float elapsedTime = 0f;

        while (elapsedTime < energyRegenDur)
        {
            if (CurrentEnergy < maxEnergy)
            {
                CurrentEnergy += regenTick;
                CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0f, maxEnergy);
            }

            elapsedTime += energyRecoveryDelay;
            yield return new WaitForSeconds(energyRecoveryDelay);
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
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

    private bool IsOnPlatform(out Transform platform)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f + 0.3f))
        {
            if (hit.transform.CompareTag("MovingPlatform"))
            {
                platform = hit.transform;
                return true;
            }
        }
        platform = null;
        return false;
    }

    private void HandleWalkingSound()
    {
        if (grounded && (state == moveState.walking || state == moveState.sprinting))
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= (isRunning ? timeBetweenStepsRun : timeBetweenStepsWalk))
            {
                PlayStepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f; // Reset step timer when not grounded
        }
    }

    private void PlayStepSound()
    {
        AudioClip clipToPlay = isRunning ? runClips[Random.Range(0, runClips.Length)] : walkClips[Random.Range(0, walkClips.Length)];
        audioSource.PlayOneShot(clipToPlay);
    }
}

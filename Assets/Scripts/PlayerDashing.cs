using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashing : MonoBehaviour
{
    //References
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerDashingDown dashDown;
    private PlayerDashingUp dashUp;
    private PlayerInput playerInput;

    //Variables
    [HideInInspector]
    public bool isDashing = false;
    private float dashingTime = 0.15f;
    public static float dashingRecoverTime = 4;
    public static int horizontalDashingDamage = 5;
    [SerializeField]
    private float dashingSpeed;
    private float originalGravityScale;
    private float oppositeFactor = 1;
    private bool canDash = true;
    private float zero = 0;
    private float timeSinceDashing = 0f;
    private float timeSinceStopDashing = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dashDown = GetComponent<PlayerDashingDown>();
        dashUp = GetComponent<PlayerDashingUp>();
        playerInput = GetComponent<PlayerInput>();
        isDashing = false;
        originalGravityScale = rb.gravityScale;
        canDash = true;
    }

    public void UpdateDashing()
    {
        animator.SetBool("isDashing", isDashing);
    }

    private void DashInputCheck()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (dashUp.isDashingUp == false && dashDown.isDashingDown == false)
            {
                DashCheck();
            }
        }
    }

    private void DashCheck()
    {
        if (Input.GetKeyDown(KeyCode.D) && isDashing == false && canDash)
        {
            oppositeFactor = 1;
            canDash = false;
            isDashing = true;
            UpdateDashing();
            timeSinceDashing = 0f;
        }

        else if (Input.GetKeyDown(KeyCode.A) && isDashing == false && canDash)
        {
            oppositeFactor = -1;
            canDash = false;
            isDashing = true;
            UpdateDashing();
            timeSinceDashing = 0f;
        }
    }

    private void Dash()
    {
        if (isDashing)
        {
            playerInput.enabled = false;
            rb.velocity = new Vector2(oppositeFactor * dashingSpeed, 0);
            rb.gravityScale = zero;
            timeSinceDashing += Time.deltaTime;
            if(timeSinceDashing >= dashingTime)
            {
                rb.gravityScale = originalGravityScale;
                rb.velocity = Vector2.zero;
                isDashing = false;
                playerInput.enabled = true;
                UpdateDashing();
                timeSinceStopDashing = 0f;
            }
        }
        else
        {
            return;
        }
    }

    private void DashLimitRecover()
    {
        if(canDash == false && !isDashing)
        {
            timeSinceStopDashing += Time.deltaTime;
            if (timeSinceStopDashing >= dashingRecoverTime)
            {
                canDash = true;
            }
        }
    }

    private void Update()
    {
        DashInputCheck();
        DashLimitRecover();
    }

    private void FixedUpdate()
    {
        Dash();
    }

}

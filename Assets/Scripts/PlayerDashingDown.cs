using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashingDown : MonoBehaviour
{
    //References
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerDashing playerDashing;
    private PlayerDashingUp playerDashingUp;
    private PlayerWallSliding playerWallSliding;

    //Variables
    [HideInInspector]
    public bool isDashingDown = false;
    private bool canDashDown = true;
    private float dashDownTime = 0.15f;
    private float dashDownRecoverTime = 1f;
    [SerializeField]
    private float dashingDownSpeed;
    private int negativeFactor = -1;
    private const int zero = 0;
    private float originalGravityScale;
    private float timeSinceDashingDown = 0f;
    private float timeSinceStopDashingDown = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerDashing = GetComponent<PlayerDashing>();
        playerDashingUp = GetComponent<PlayerDashingUp>();
        playerWallSliding = GetComponent<PlayerWallSliding>();
        originalGravityScale = rb.gravityScale;
        canDashDown = true;
    }

    void Update()
    {
        DashDownInputCheck();
        DashDownLimitRecover();
    }

    private void DashDownInputCheck()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (playerDashing.isDashing == false && playerDashingUp.isDashingUp == false && !playerWallSliding.isWallSliding)
            {
                if (Input.GetKeyDown(KeyCode.S) && canDashDown && isDashingDown == false)
                {
                    canDashDown = false;
                    isDashingDown = true;
                    UpdateDashingDown();
                    timeSinceDashingDown = 0f;
                }
            }
        }
    }

    private void DashDown()
    {
        if (isDashingDown)
        {
            rb.gravityScale = zero;
            rb.velocity = new Vector2(zero, dashingDownSpeed * negativeFactor);
            timeSinceDashingDown += Time.deltaTime;
            if(timeSinceDashingDown >= dashDownTime)
            {
                rb.gravityScale = originalGravityScale;
                rb.velocity = Vector2.zero;
                isDashingDown = false;
                UpdateDashingDown();
                timeSinceStopDashingDown = 0f;
            }
        }
        else
        {
            return;
        }
    }

    private void DashDownLimitRecover()
    {
        if(canDashDown == false && isDashingDown == false)
        {
            timeSinceStopDashingDown += Time.deltaTime; 
            if(timeSinceStopDashingDown >= dashDownRecoverTime)
            {
                canDashDown = true;
            }
        }
    }

    private void UpdateDashingDown()
    {
        animator.SetBool("isDashingDown", isDashingDown);
    }

    private void FixedUpdate()
    {
        DashDown();
    }

}

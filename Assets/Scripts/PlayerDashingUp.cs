using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashingUp : MonoBehaviour
{
    //References
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerDashing playerDashing;
    private PlayerDashingDown playerDashingDown;
    private PlayerWallSliding playerWallSliding;

    //Variables:
    [SerializeField]
    private float dashingUpSpeed;
    [HideInInspector]
    public bool isDashingUp = false;
    private bool canDashUp = true;
    private float dashUpTime = 0.15f;
    private float dashRecoverTime = 1;
    private float originalGravityScale;
    private float timeSinceDashingUp = 0f;
    private float timeSinceStopDashingUp = 0f;
    private const int zero = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerDashing = GetComponent<PlayerDashing>();
        playerDashingDown = GetComponent<PlayerDashingDown>();
        playerWallSliding = GetComponent<PlayerWallSliding>();
        originalGravityScale = rb.gravityScale;
        canDashUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        DashUpInputCheck();
        DashUpLimitRecover();
    }

    private void DashUpInputCheck()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (playerDashing.isDashing == false && playerDashingDown.isDashingDown == false && !playerWallSliding.isWallSliding)
            {
                if (Input.GetKeyDown(KeyCode.W) && canDashUp && isDashingUp == false)
                {
                    canDashUp = false;
                    isDashingUp = true;
                    UpdateDashingUp();
                    timeSinceDashingUp = 0f;
                }
            }
        }
    }

    private void DashUp()
    {
        if (isDashingUp)
        {
            rb.gravityScale = zero;
            rb.velocity = new Vector2(zero, dashingUpSpeed);
            timeSinceDashingUp += Time.deltaTime;
            if(timeSinceDashingUp >= dashUpTime)
            {
                rb.gravityScale = originalGravityScale;
                rb.velocity = Vector2.zero;
                isDashingUp = false;
                UpdateDashingUp();
                timeSinceStopDashingUp = 0f;
            }
        }
        else
        {
            return;
        }
    }

    private void DashUpLimitRecover()
    {
        if(canDashUp == false && isDashingUp == false)
        {
            timeSinceStopDashingUp += Time.deltaTime;
            if(timeSinceStopDashingUp >= dashRecoverTime)
            {
                canDashUp = true;
            }
        }
    }

    private void UpdateDashingUp()
    {
        animator.SetBool("isDashingUp", isDashingUp);
    }
    
    private void FixedUpdate()
    {
        DashUp();
    }
}

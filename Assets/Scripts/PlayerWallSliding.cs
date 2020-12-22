using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSliding : MonoBehaviour
{
    #region variables
    //references
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerInput playerInput;
    private PlayerDashing playerDashing;
    private PlayerDashingDown playerDashingDown;
    private PlayerDashingUp playerDashingUp;

    //parameters
    private float wallSlidingSpeed = -0.9f;
    [HideInInspector]
    public bool canWallSlide = false;
    [HideInInspector]
    public bool isWallSliding;
    [HideInInspector]
    public bool isWallJumping = false;
    private float timeSinceWallJumping = 0f;
    [SerializeField]
    private float wallJumpingTime;
    private const int oppositeDir = -1;

    //RaycastHit2D
    private RaycastHit2D hit;

    //LayerMask
    [SerializeField]
    private LayerMask whatIsGround;

    //Vectors
    private Vector2 leftdirection = new Vector2(-0.4f, 0);
    private Vector2 rightdirection = new Vector2(0.39f, 0);
    [SerializeField]
    private Vector2 wallJumpForce;

    //Ray Distances
    private float leftdistance;
    private float rightdistance;
    #endregion


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerDashing = GetComponent<PlayerDashing>();
        playerDashingDown = GetComponent<PlayerDashingDown>();
        playerDashingUp = GetComponent<PlayerDashingUp>();
        rightdistance = Mathf.Abs(rightdirection.x);
        leftdistance = Mathf.Abs(leftdirection.x);
    }

    //Checks to see if you can wall slide, checks to see if user inputted correctly to do wall jumping
    private void Update()
    {
        CanWallSlideCondition();
        WallJumpCheck();
    }

    //Check to see if you are wall jumping
    private void WallJumpCheck()
    {
        if (Input.GetKeyDown(KeyCode.W) && isWallSliding && playerInput.canJump)
        {
            isWallJumping = true;
            playerInput.canJump = false;
        }
    }

    //Checking to see if you are able to wall slide
    private void CanWallSlideCondition()
    {
        if (playerInput.isFalling && !playerDashing.isDashing && !playerDashingDown.isDashingDown && !playerDashingUp.isDashingUp)
        {
            canWallSlide = true;
        }
        else
        {
            canWallSlide = false;
        }
    }

    //Shoots raycasts, wall jumps if user input is correct, wall slides if user input is correct
    private void FixedUpdate()
    {
        ShootRayCast();
        WallJump();
        SlideCondition();
    }

    //wall jumps if you inputted correctly
    private void WallJump()
    {
        if (isWallJumping)
        {
            playerInput.enabled = false;
            playerInput.isJumping = true;
            playerInput.isFalling = false;
            playerInput.isGrounded = false;
            playerInput.isRunning = false;
            playerInput.SetAnimatorBools();
            rb.velocity = WallJumpDirectionCheck();
            if(timeSinceWallJumping == 0)
            {
                playerInput.Flip();
            }
            isWallSliding = false;
            UpdateWallSliding();
            playerInput.enabled = true;
            timeSinceWallJumping += Time.deltaTime;
            if(timeSinceWallJumping >= wallJumpingTime)
            {
                playerInput.isJumping = false;
                playerInput.SetAnimatorBools();
                isWallJumping = false;
                timeSinceWallJumping = 0f;
            }
        }
        else
        {
            return;
        }
    }

    //Returns the right way to wall jump
    private Vector2 WallJumpDirectionCheck()
    {
        if (playerInput.isFlip)
        {
            return wallJumpForce;
        }
        else
        {
            return new Vector2(wallJumpForce.x * oppositeDir, wallJumpForce.y);
        }
    }

    //Shoots raycast in the corresponding direction
    private void ShootRayCast()
    {
        if(!playerInput.isFlip)
        {
            hit = Physics2D.Raycast(transform.position, rightdirection, rightdistance, whatIsGround);
            Debug.DrawRay(transform.position, rightdirection);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, leftdirection, leftdistance, whatIsGround);
            Debug.DrawRay(transform.position, leftdirection);
        }
    }

    //Updates the isWallSliding animator bool with our instance's isWallSliding
    public void UpdateWallSliding()
    {
        animator.SetBool("isWallSliding", isWallSliding);
    }

    //Check to see if user entered correct inputs to wall slide
    private void SlideCondition()
    {
        if (hit && canWallSlide && !isWallJumping)
        {
            if (playerInput.isRunning)
            {
                WallSlide();
            }
            else
            {
                isWallSliding = false;
                UpdateWallSliding();
            }
        }
        else
        {
            isWallSliding = false;
            UpdateWallSliding();
        }
    }

    //Does the actual wall sliding
    private void WallSlide()
    {
        isWallSliding = true;
        UpdateWallSliding();
        rb.velocity = new Vector2(0, wallSlidingSpeed);
    }

}

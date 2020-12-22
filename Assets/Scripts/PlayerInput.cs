using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //References
    private Rigidbody2D PlayerRB;
    private Animator animator;
    private Transform rayTransform;
    private Transform rayTransform2;
    private PlayerWallSliding playerWallSliding;

    //Variables: Floats
    private float MoveX;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float speed;
    private float distance;

    //Variables: Bools
    [HideInInspector]
    public bool isRunning = false;
    [HideInInspector]
    public bool canJump = true;
    [HideInInspector]
    public bool isGrounded = false;
    [HideInInspector]
    public bool isJumping = false;
    [HideInInspector]
    public bool isFalling = false;
    private bool holdingLeft = false;
    private bool holdingRight = false;
    [HideInInspector]
    public bool isFlip = false;

    //Variables: Other
    private RaycastHit2D hit;
    [SerializeField]
    private LayerMask whatIsGround;
    private Vector2 direction = new Vector2(0, -0.27f);
    private Transform[] rayTransforms;

    //Quaternion
    private Quaternion rightRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    private Quaternion leftRotation = Quaternion.Euler(new Vector3(0, 180, 0));

    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerWallSliding = GetComponent<PlayerWallSliding>();
        rayTransform = transform.GetChild(0);
        rayTransform2 = transform.GetChild(1);
        rayTransforms = new Transform[] {rayTransform, rayTransform2};
        distance = -direction.y;
    }

    private void Update()
    {
        MoveInputsCheck();
        CheckForJumpInput();
        CheckReleaseInputs();
    }

    //Checks to see if you are falling
    private void FallingCheck()
    {
        if (PlayerRB.velocity.y < 0)
        {
            isFalling = true;
            SetAnimatorBools();
        }
        else
        {
            isFalling = false;
            SetAnimatorBools();
        }
    }

    //Sets the animator bools to the corresponding bools in this script
    public void SetAnimatorBools()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
    }

    //Checks to see if you inputted correctly to jump
    private void CheckForJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && canJump && !playerWallSliding.isWallSliding)
        {
            canJump = false;
            isJumping = true;
            isGrounded = false;
            isFalling = false;
            SetAnimatorBools();
        }
    }

    private void FixedUpdate()
    {
        isJumpingCheck();
        isRunningCheck();
        GroundCheck();
    }

    //Checks to see if you can perform a jump
    private void isJumpingCheck()
    {
        if (isJumping)
        {
            Jump();
        }
        else
        {
            return;
        }
    }

    //Performes a jump
    private void Jump()
    {
        PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, Vector2.up.y * jumpPower);
        isJumping = false;
    }

    //Determines if you are going to run or not
    private void isRunningCheck()
    {
        if (isRunning)
        {
            PlayerRB.velocity = new Vector2(MoveX * speed, PlayerRB.velocity.y);
        }
        else
        {
            PlayerRB.velocity = new Vector2(0, PlayerRB.velocity.y);
        }
    }

    //Checking Moving Inputs
    private void MoveInputsCheck()
    {
        if (Input.GetKey(KeyCode.D) && !playerWallSliding.isWallJumping)
        {
            if (holdingLeft == false)
            {
                MoveRight();
            }
        }
        if (Input.GetKey(KeyCode.A) && !playerWallSliding.isWallJumping)
        {
            if (holdingRight == false)
            {
                MoveLeft();
            }
        }
    }

    //Sets up to move left
    private void MoveLeft()
    {
        holdingLeft = true;
        MoveX = -1;
        isRunning = true;
        transform.localRotation = leftRotation;
        isFlip = true;
        SetAnimatorBools();
    }

    //Sets up to move right
    private void MoveRight()
    {
        holdingRight = true;
        MoveX = 1;
        isRunning = true;
        transform.localRotation = rightRotation;
        isFlip = false;
        SetAnimatorBools();
    }

    //Flips sprite
    public void Flip()
    {
        isFlip = !isFlip;
        if (isFlip)
        {
            transform.localRotation = leftRotation;
            isFlip = !isFlip;
        }
        else
        {
            transform.localRotation = rightRotation;
            isFlip = !isFlip;
        }
    }

    //Checking to see if you released any of the two running buttons
    private void CheckReleaseInputs()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            holdingRight = false;
            isRunning = false;
            SetAnimatorBools();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            holdingLeft = false;
            isRunning = false;
            SetAnimatorBools();
        }
    }

    //Checks to see if you are touching the ground
    private void GroundCheck()
    {
        foreach(Transform raytransform in rayTransforms)
        {
            hit = Physics2D.Raycast(raytransform.position, direction, distance, whatIsGround);
            Debug.DrawRay(raytransform.position, direction);
            if (hit)
            {
                isJumping = false;
                isGrounded = true;
                isFalling = false;
                SetAnimatorBools();
                canJump = true;
                break;
            }
            else
            {
                isGrounded = false;
                if (!playerWallSliding.isWallSliding)
                {
                    canJump = false;
                }
                else
                {
                    canJump = true;
                }
                SetAnimatorBools();
                FallingCheck();
            }
        }
    }

}

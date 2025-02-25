using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private Animator anim;

    // Movement Variables
    private bool isRunning;
    private float movingInput;

    private bool canDoubleJump;
    private bool isSliding;

    public bool playerUnlocked;

    // Movement Settings
    [Header("Move info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private int jumpForce;

    // Slide Settings
    [Header("Slide info")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTime;
    [SerializeField] private float slideCooldown;
    private float slideCooldownCounter;
    private float slideTimeCounter;

    // Collision Settings
    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float ceillingCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;

    private bool isGrounded;
    private bool wallDetected;
    private bool ceillingDetected;

    [HideInInspector] public bool ledgeDetected;


    // Unity Lifecycle
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start() { }

    private void Update()
    {
        CheckCollison();
        AnimatorController();
        HandleTimers();

        movingInput = Input.GetAxis("Horizontal");

        if (playerUnlocked)
        {
            Movement();
        }

        if (isGrounded)
        {
            canDoubleJump = true;
        }

        CheckForSlide();
        CheckInput();
    }

    private void FixedUpdate() { }

    // Movement Handling
    private void Movement()
    {
        if (wallDetected) return;

        if (isSliding)
            rb.velocity = new Vector2(slideSpeed * movingInput, rb.velocity.y);
        else
            rb.velocity = new Vector2(moveSpeed * movingInput, rb.velocity.y);
    }

    private void JumpButton()
    {
        if (isSliding) return;

        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void SlideButton()
    {
        if (rb.velocity.x != 0 && slideCooldownCounter < 0)
        {
            isSliding = true;
            slideTimeCounter = slideTime;
            slideCooldownCounter = slideCooldown;
        }
    }

    private void HandleTimers()
    {
        slideTimeCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;
    }

    // Input Handling
    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            playerUnlocked = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            JumpButton();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SlideButton();
        }
    }


    private void CheckForSlide()
    {
        if (slideTimeCounter < 0 && !ceillingDetected)
        {
            isSliding = false;
        }
    }

    // Animation Handling
    private void AnimatorController()
    {
        isRunning = rb.velocity.x != 0;

        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isSliding", isSliding);

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetFloat("xVelocity", rb.velocity.x);
    }

    // Collision Handling
    private void CheckCollison()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        ceillingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceillingCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
    }

    // Debugging
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingCheckDistance));
        Gizmos.DrawCube(wallCheck.position, wallCheckSize);
    }
}

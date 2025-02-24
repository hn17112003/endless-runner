using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;

    private bool isRunning;

    public bool playerUnlocked;
    private float movingInput;

    private bool canDoubleJump;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private int jumpForce;

    [Header("Slide infor")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTime;
    [SerializeField] private float slideCooldown;
    private float slideCooldownCounter;
    private float slideTimeCounter;
    private bool isSliding;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float ceillingCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;

    private bool isGrounded;
    private bool wallDetected;
    private bool ceillingDetected;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Awake()
    {

    }

    void Update()
    {

        CheckCollison();
        AnimatorController();

        slideTimeCounter = slideTimeCounter - Time.deltaTime;
        slideCooldownCounter = slideCooldownCounter - Time.deltaTime;

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

    private void CheckForSlide()
    {
        if (slideTimeCounter < 0 && !ceillingDetected)
        {
            isSliding = false;
        }
    }

    private void Movement()
    {
        if (wallDetected)
        {
            return;
        }
        if (isSliding)
        {
            rb.velocity = new Vector2(slideSpeed * movingInput, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed * movingInput, rb.velocity.y);
        }
    }

    private void AnimatorController()
    {
        isRunning = rb.velocity.x != 0;

        //anim.SetBool("isRunning", isRunning);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isSliding", isSliding);

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetFloat("xVelocity", rb.velocity.x);

    }

    private void CheckCollison()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        ceillingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceillingCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
    }

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

    private void SlideButton()
    {
        if (rb.velocity.x != 0 && slideCooldownCounter < 0)
        {
            isSliding = true;
            slideTimeCounter = slideTime;
            slideCooldownCounter = slideCooldown;
        }

    }

    private void JumpButton()
    {
        if (isSliding)
        {
            return;
        }
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

    void FixedUpdate()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingCheckDistance));
        Gizmos.DrawCube(wallCheck.position, wallCheckSize);
    }

}

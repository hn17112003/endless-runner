using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private bool isRunning;
    public bool playerUnlocked;
    private bool isGrounded;
    private float movingInput;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private int jumpForce;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    void Start()
    {
        Debug.Log("Start");

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Awake()
    {
        Debug.Log("Awake");

    }

    void Update()
    {

        Debug.Log("Update");

        AnimatorController();

        movingInput = Input.GetAxis("Horizontal");

        if (playerUnlocked)
        {
            rb.velocity = new Vector2(moveSpeed * movingInput, rb.velocity.y);

        }

        CheckCollison();

        CheckInput();

    }

    private void AnimatorController()
    {
        isRunning = rb.velocity.x != 0;

        //anim.SetBool("isRunning", isRunning);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetFloat("xVelocity", rb.velocity.x);

    }

    private void CheckCollison()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        }
        if (Input.GetButtonDown("Fire2"))
        {
            playerUnlocked = true;
        }
    }

    void FixedUpdate()
    {
        Debug.Log("FixedUpdate");

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    }

}

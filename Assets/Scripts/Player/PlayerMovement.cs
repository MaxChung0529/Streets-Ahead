using UnityEngine;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator animator;

    private bool falling = false;
    private float gravity = 2f;
    private bool hitWall;

    [Header("Player")]
    [SerializeField] private LayerMask collisionMask;
    private float playerScale = 0.5f;
    private bool grounded;

    [Header("Jumping")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [Header("Dashing")]
    private float dashingVelocity = 25f;
    private float dashingTime = 0.2f;
    private float dashingCD = 1f;
    private bool isDashing;

    private bool isSliding = false;

    [Header("Status")]
    private PlayerStat stat;

    // Start is called before the first frame update
    void Start()
    {
        //Grab references
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stat = GetComponent<PlayerStat>();
    }

    // Update is called once per frame
    void Update()
    {
        //Block inputs if character is dashing or sliding
        if (isDashing || isSliding || stat.dead)
        {
            return;
        }

        float inputX = Input.GetAxis("Horizontal");
        bool jumpInput = Input.GetButton("Jump");

        //Horizontal movement
        rb.velocity = new Vector2(inputX * speed, rb.velocity.y);

        //Flip player to go left or right
        if (inputX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(inputX) * playerScale, playerScale, playerScale);
        }

        //Jump
        if (jumpInput && IsGrounded() && !hitWall)
        {
            Jump();
        }

        if (!IsGrounded() && !isSliding)
        {
            rb.gravityScale += rb.gravityScale * Time.deltaTime * 1.2f;
        }

        //Dash
        var dashInput = Input.GetKeyDown(KeyCode.LeftShift);

        if (dashInput && CanDash())
        {
            StartCoroutine(Dash());
        }

        animator.SetBool("isDashing", isDashing);

        //Detect if player is falling
        if (rb.velocity.y < 0 && !IsGrounded() && !hitWall)
        {
            rb.gravityScale = gravity * (float)2.5;
            falling = true;
            animator.SetTrigger("fall");
        }

        //Slide down the wall
        if (hitWall && !IsGrounded())
        {
            rb.velocity = new Vector2(0, -3);
            falling = false;
            isSliding = true;
        }

        //Set animator parameters
        animator.SetBool("run", inputX != 0f);
        animator.SetBool("grounded", grounded);
        animator.SetBool("falling", falling);
        animator.SetBool("hitWall", hitWall);
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        animator.ResetTrigger("jump");
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingVelocity, 0f);

        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        rb.gravityScale = originalGravity;
        yield return new WaitForSeconds(dashingCD);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetTrigger("jump");
        grounded = false;
    }

    public bool canAttack()
    {
        return !hitWall && IsGrounded();
    }

    private bool IsGrounded()
    {
        //return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionMask);
        return grounded;
    }

    private bool CanDash()
    {
        return !isDashing;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            falling = false;
            rb.gravityScale = gravity;

            hitWall = false;
            isSliding = false;

            animator.ResetTrigger("fall");
        }

        if (collision.gameObject.tag == "Wall")
        {
            hitWall = true;
            StopCoroutine(Dash());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            hitWall = false;
            falling = false;
            animator.ResetTrigger("fall");
            //Flip();
        }
    }
}

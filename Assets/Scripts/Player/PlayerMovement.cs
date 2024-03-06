using UnityEngine;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;

    private bool falling = false;
    private float gravity = 2f;
    private bool hitWall;

    [Header("Player")]
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask wallLayer;
    private float playerScale = 0.4f;
    private bool grounded;

    [Header("Jumping")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private float jumpingCD = 0.8f;
    private float lastJump = Mathf.Infinity;

    [Header("Dashing")]
    private float dashingVelocity = 18f;
    private float dashingTime = 0.2f;
    private float dashingCD = 1f;
    private bool isDashing;
    private Vector2 dashingDir;
    public bool dashReset = true;

    private bool isSliding = false;

    [Header("Status")]
    //private PlayerStat stat;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        //Grab references
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        active = true;
    }

    // Update is called once per frame
    void Update()
    {

        //Block inputs if character is dashing or sliding
        if (isDashing || isSliding || !active)
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

        //Ground check
        if (IsGrounded())
        {
            grounded = true;
            falling = false;
            rb.gravityScale = gravity;


            isSliding = false;

            animator.ResetTrigger("fall");
        }else
        {
            rb.gravityScale += rb.gravityScale * Time.deltaTime * 1.5f;
        }

        //Wall check
        onWall();

        //Jump
        if (jumpInput && IsGrounded() && !onWall() && lastJump >= jumpingCD)
        {
            lastJump = 0f;
            Jump();
        }

        if (!IsGrounded() && !isSliding)
        {
            rb.gravityScale += rb.gravityScale * Time.deltaTime * 1.2f;

            grounded = true;
            falling = false;
            rb.gravityScale = gravity;

            isSliding = false;

            animator.ResetTrigger("slide");
            animator.ResetTrigger("fall");

            //Detect if player is falling
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * (float)2.5;
                falling = true;
                animator.SetTrigger("fall");
            }
        }

        //Wall slide
        if (onWall() && !IsGrounded())
        {
            StartCoroutine(StopDashing());

            rb.velocity = new Vector2(0, -3);
            falling = false;
            animator.SetTrigger("slide");
            isSliding = true;
        }

        //Dash
        var dashInput = Input.GetKeyDown(KeyCode.LeftShift);

        if (dashInput && CanDash())
        {
            dashReset = false;

            float inputY = transform.localScale.y * 0.5f;

            dashingDir = new Vector2(inputX, inputY);
            if (inputX == 0f)
            {
                dashingDir = new Vector2(transform.localScale.x, 0);
            }
            StartCoroutine(Dash());
        }

        animator.SetBool("isDashing", isDashing);

        //Set animator parameters
        animator.SetBool("run", inputX != 0f);
        animator.SetBool("grounded", grounded);
        animator.SetBool("falling", falling);
        animator.SetBool("isSliding", isSliding);

        //Jump manage
        lastJump += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetTrigger("jump");
    }

    private IEnumerator Dash()
    {
        isDashing = true;

        animator.ResetTrigger("jump");
        float originalGravity = rb.gravityScale;

        rb.velocity = dashingDir.normalized * dashingVelocity;

        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        rb.gravityScale = originalGravity;
        yield return new WaitForSeconds(dashingCD);
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
    }

    public bool canAttack()
    {
        return !hitWall && IsGrounded();
    }

    private bool IsGrounded()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0,Vector2.down, 0.1f, groundLayer);

        //return rayCastHit.collider != null;
        grounded = rayCastHit.collider != null;

        if (grounded == true)
        {
            dashReset = true;
        }

        return grounded;
    }

    private bool onWall()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);

        Debug.Log(rayCastHit.collider != null);


        return rayCastHit.collider != null;
    }

    private bool CanDash()
    {
        return !isDashing && dashReset;
    }

    private void Die()
    {
        active = false;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            falling = false;
            rb.gravityScale = gravity;

            isSliding = false;

            animator.ResetTrigger("slide");
            animator.ResetTrigger("fall");
        }

        if (collision.gameObject.tag == "Enemies")
        {
            Die();
        }
    }
}

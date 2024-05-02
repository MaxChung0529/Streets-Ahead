using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;

    private bool falling = false;
    private bool hitWall;
    private float gravity;

    [Header("Moonwalk")]

    [Header("Player")]
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask wallLayer;
    [SerializeField] public LayerMask platformLayer;
    [SerializeField] public LayerMask utilLayer;
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

    [Header("Wall")]
    [SerializeField] private Transform wallCheck;
    private bool isSliding = false;

    // Start is called before the first frame update
    void Start()
    {
        //Grab references
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {

        gravity = PlayerManager.instance.gravity;
        //Block inputs if character is dashing
        if (isDashing)
        {
            return;
        }

        float inputX = Input.GetAxis("Horizontal");
        bool jumpInput = Input.GetButton("Jump");

        if (!isSliding)
        {
            //Horizontal movement
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);

            //Flip player to go left or right
            if (inputX != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(inputX) * playerScale, playerScale, playerScale);
            }
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

        //Wall slide
        if (onWall() && !IsGrounded())
        {
            StartCoroutine(StopDashing());


            float originalGravity = rb.gravityScale;

            rb.gravityScale = 0f;

            rb.velocity = new Vector2(0, -0.5f);
            falling = false;
            animator.SetTrigger("slide");
            isSliding = true;
        }

        //Jump
        if (jumpInput && (IsGrounded() || isSliding) && lastJump >= jumpingCD)
        {
            lastJump = 0f;
            animator.SetBool("isSliding", false);
            animator.ResetTrigger("slide");
            isSliding = false;
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
        return !hitWall;
    }

    private bool IsGrounded()
    {
        RaycastHit2D rayCastHitFloor = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0,Vector2.down, 0.1f, groundLayer);

        RaycastHit2D rayCastHitPlatform = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.down, 0.1f, platformLayer);

        RaycastHit2D rayCastHitUtil = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.down, 0.1f, utilLayer);

        //RaycastHit2D rayCastHitLeft = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.left, 0.1f, groundLayer);

        //return rayCastHit.collider != null;
        grounded = rayCastHitFloor.collider != null || rayCastHitPlatform.collider != null || rayCastHitUtil.collider != null;

        if (grounded == true)
        {
            dashReset = true;
        }

        return grounded;
    }

    private bool onWall()
    {
        RaycastHit2D rayCastHitWall = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);

        //RaycastHit2D rayCastHitLeft = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.left, 0.1f, platformLayer);
        //RaycastHit2D rayCastHitRight = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.right, 0.1f, platformLayer);

        //return Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallLayer);


        //return rayCastHitWall.collider != null || rayCastHitLeft.collider != null || rayCastHitRight.collider != null;
        return rayCastHitWall.collider != null;
    }

    private bool CanDash()
    {
        return !isDashing && dashReset && !isSliding;
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
    }
}

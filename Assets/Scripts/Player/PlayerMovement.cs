using UnityEngine;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    private Rigidbody2D body;
    private Animator animator;
    private bool grounded;
    private bool falling = false;
    private float gravity = 2;
    private bool airBourne;
    private bool hitWall;
    private bool facingRight = true;

    [Header("Dashing")]
    [SerializeField] private float dashingVelocity = 20f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCD = 1f;
    private bool isDashing;
    private bool canDash = true;

    private bool isSliding = false;

    // Start is called before the first frame update
    void Start()
    {
        //Grab references
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Block inputs if character is dashing or sliding
        if (isDashing || isSliding)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        //Dash
        var dashInput = Input.GetKeyDown(KeyCode.LeftShift);

        if (dashInput && canDash)
        {
            StartCoroutine(Dash());
        }

        animator.SetBool("isDashing", isDashing);

        if (grounded)
        {
            canDash = true;
        }


        //Flip player when go left or right
        if (horizontalInput > 0f && !facingRight)
        {
            //Go right
            Flip();
        }
        else if (horizontalInput < 0f && facingRight)
        {
            //Go left
            Flip();
        }

        //Jump
        if ((Input.GetKey(KeyCode.UpArrow)) && grounded && !hitWall)
        {
            Jump();
        }

        //Detect if player is falling
        if (body.velocity.y < 0 && airBourne && !hitWall)
        {
            body.gravityScale = gravity * (float)2.5;
            falling = true;
            animator.SetTrigger("fall");
        }

        //Slide down the wall
        if (hitWall)
        {
            body.velocity = new Vector2(0, -3);
            airBourne = false;
            falling = false;
            isSliding = true;
            animator.SetTrigger("idle");
        }

        //Set animator parameters
        animator.SetBool("run", horizontalInput != 0);
        animator.SetBool("grounded", grounded);
        animator.SetBool("falling", falling);
        animator.SetBool("airBourne", airBourne);
        animator.SetBool("hitWall", hitWall);
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
    }

    private void Flip()
    {
        Vector3 currentScale = body.transform.localScale;
        currentScale.x *= -1;
        body.transform.localScale = currentScale;

        facingRight = !facingRight; 
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        animator.ResetTrigger("jump");
        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;
        body.velocity = new Vector2(transform.localScale.x * dashingVelocity, 0f);

        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        body.gravityScale = originalGravity;
        yield return new WaitForSeconds(dashingCD);
        canDash = true;
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        animator.SetTrigger("jump");
        airBourne = true;
        grounded = false;
    }

    public bool canAttack()
    {
        return !hitWall && !airBourne;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            falling = false;
            airBourne = false;
            body.gravityScale = gravity;

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

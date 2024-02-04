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

    [Header("Dashing")]
    [SerializeField] private float dashingVelocity = 20f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCD = 1f;
    private bool isDashing;
    private bool canDash = true;

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

        if (isDashing)
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
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3((float)1, (float)1, (float)1);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3((float)-1, (float)1, (float)1);
        }

        //Jump
        if ((Input.GetKey(KeyCode.UpArrow)) && grounded)
        {
            Jump();
        }

        //Detect if player is falling
        if (body.velocity.y < 0)
        {
            body.gravityScale = gravity * (float)1.5;
            falling = true;
        }

        //Set animator parameters
        animator.SetBool("run", horizontalInput != 0);
        animator.SetBool("grounded", grounded);
        animator.SetBool("falling", falling);
        animator.SetBool("airBourne", airBourne);
        //animator.SetBool("hitWall", false);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            falling = false;
            grounded = true;
            airBourne = false;
            body.gravityScale = gravity;
        }
    }
}

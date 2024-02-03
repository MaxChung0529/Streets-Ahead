using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    private Rigidbody2D body;
    private Animator animator;
    private bool grounded;
    private bool falling = false;
    private float gravity = 1;

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
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

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
        //animator.SetBool("hitWall", false);
    }
    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        animator.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            falling = false;
            grounded = true;
        }
    }
}

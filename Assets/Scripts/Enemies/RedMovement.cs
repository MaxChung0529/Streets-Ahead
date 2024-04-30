using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMovement : MonoBehaviour
{


    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private bool dead = false;
    private bool collided = false;
    private Animator animator;
    [SerializeField] private CapsuleCollider2D capsuleCollider;
    [SerializeField] private LayerMask playerLayer;
    private int dir = -1;
    private Vector3 initScale;
    private Transform enemy;
    private Transform target;

    [Header("Knockback")]
    private float knockbackDuration = 1.5f;
    private float knockbackCounter = 0;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        initScale = rb.transform.localScale;
        enemy = rb.transform;
        target = FindAnyObjectByType<PlayerManager>().player;
    }

    // Update is called once per frame
    void Update()
    {
        //Celebrate if player is killed
        if (!GameObject.Find("Player").GetComponent<PlayerManager>().alive)
        {
            animator.SetTrigger("Celebrate");
            return;
        }

        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;
            if (speed > 0)
            {
                speed = -speed * 2f;
            }

            if (knockbackCounter <= 0)
            {
                speed = Mathf.Abs(speed * 0.5f);
                Debug.Log(speed);
            }
        }

        if (!dead && knockbackCounter <= 0)
        {
            //rb.velocity = (target.position - transform.position).normalized * speed;
            MoveInDir(dir);
        }

/*        if (seePlayer())
        {
            dead = true;
        }*/
    }

    public void MoveInDir(int direction)
    {
            //Face direction
            rb.transform.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);

            //Move in direction
            rb.transform.position = new Vector3(rb.transform.position.x + Time.deltaTime + direction * speed,
                rb.transform.position.y, rb.transform.position.z);
    }

    public void Sucked()
    {
        dead = true;
        animator.SetBool("Sucked", true);
    }

    public void knockBack()
    {
        knockbackCounter = knockbackDuration;

        Vector2 kbForce = new Vector2(-100 * (rb.transform.position.x + Time.deltaTime + dir * speed), rb.velocity.y);
        rb.AddForce(kbForce, ForceMode2D.Force);
    }

    private bool seePlayer()
    {
        RaycastHit2D hit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(capsuleCollider.bounds.center, capsuleCollider.bounds.size);
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            Sucked();
            capsuleCollider.enabled = false;
        }

        if (collision.gameObject.tag == "Wall")
        {
            dir *= -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Weapon")
        {
            dead = true;
            Sucked();

            //Let Red freefall
            //rb.constraints = RigidbodyConstraints2D.None;
            //rb.AddForce(Vector3.right * 25);
            //rb.AddForce(Vector3.up * 25);

            //gameObject.SetActive(false);
        }

    }

    

}

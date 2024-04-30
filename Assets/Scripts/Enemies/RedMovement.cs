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
    private CapsuleCollider2D capsuleCollider;
    private int dir = 1;
    private Vector3 initScale;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        initScale = rb.transform.localScale;
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

        if (!dead)
        {
            MoveInDir(dir);
        }
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
        Debug.Log("KnockBack");
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

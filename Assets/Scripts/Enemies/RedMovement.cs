using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMovement : MonoBehaviour, IAttackable
{


    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private bool dead = false;
    private bool collided = false;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Stop if player is killed
        if (!GameObject.Find("Player").GetComponent<PlayerManager>().alive)
        {
            animator.SetTrigger("Celebrate");
            return;
        }

        if (!dead && !collided)
        {
            Move();
        }
    }

    public void Move()
    {
        transform.Translate(-1 * speed, 0, 0);
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

        if (collision.gameObject.tag == "Wall")
        {
            collided = true;
        }

    }

    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMovement : MonoBehaviour, IAttackable
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private bool dead = false;
    private bool collided = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Stop if player is killed
        if (!GameObject.Find("Player").GetComponent<PlayerMovement>().active)
        {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Weapon")
        {
            dead = true;

            //Let Red freefall
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddForce(Vector3.right * 25);
            rb.AddForce(Vector3.up * 25);

            //gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Wall")
        {
            collided = true;
        }

    }

    

}

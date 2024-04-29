using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private bool hitWall;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
    }

    //Check if kunai hit any objects

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemies")
        {
            hit = true;
            animator.SetTrigger("hit");
            boxCollider.enabled = false;
        }else if (collision.gameObject.tag == "Wall")
        {
            Deactivate();
        }

    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        hitWall = false;
        boxCollider.enabled = true;

        //Flip kunai
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    //Deactivate kunai
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
    
}

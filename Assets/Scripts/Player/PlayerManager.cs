using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager: MonoBehaviour
{

    [SerializeField] private PlayerAttack attackScript;
    [SerializeField] private PlayerMovement movementScript;

    [Header("Player")]
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask wallLayer;
    public float playerScale = 0.4f;
    public bool grounded;
    public bool onGround;
    public bool hitWall;
    public Animator animator;
    public Rigidbody2D rb;
    public CapsuleCollider2D capsuleCollider;
    private TrailRenderer trail;
    public Transform player;

    private Vector3 respawnPt;
    public GameObject fallDetector;

    [Header("Stat")]
    public bool alive = true;

    [Header("Loot")]
    private int lotusCount = 0;

    [Header("Tools")]
    private int secondaryWeaponCount = 0;
    private bool shielded = false;


    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
        player = transform;
        respawnPt = transform.position;
    }

    void Update()
    {
        if (!alive)
        {
            gameObject.SetActive(false);
            return;
        }

        IsGrounded();

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private bool IsGrounded()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);

        //return rayCastHit.collider != null;
        onGround = rayCastHit.collider != null;

        if (onGround == true)
        {
            //dashReset = true;
        }

        return onGround;
    }

    public bool canSecondaryAttack()
    {
        secondaryWeaponCount--;
        return secondaryWeaponCount + 1 > 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FallDetector")
        {
            transform.position = respawnPt;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Lotus")
        {   
            LotusManager.instance.addLotus();
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Blackhole")
        {
            Debug.Log("Picked up");
            secondaryWeaponCount = 3;
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Shield")
        {
            shielded = true;
            collision.gameObject.SetActive(false);
            trail.enabled = true;
        }

        if (collision.gameObject.tag == "Enemies")
        {

            if (!shielded)
            {
                alive = false;
            }else
            {
                collision.gameObject.GetComponent<RedMovement>().KnockBack();
                shielded = false;
                trail.enabled = false;
            }
        }
    }
}

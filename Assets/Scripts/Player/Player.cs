using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour
{
    public bool onGround;
    public bool hitWall;
    public Animator animator;
    public Rigidbody2D rb;
    public CapsuleCollider2D capsuleCollider;

    [Header("Player")]
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask wallLayer;
    public float playerScale = 0.4f;
    public bool grounded;

    [Header("Status")]
    //private PlayerStat stat;
    public bool active;
    private bool alive;

    [Header("Loot")]
    private int lotusCount = 0;


    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void update()
    {
        IsGrounded();
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask blackholeLayer;
    [SerializeField] private LayerMask ignoredLayer;

    private Rigidbody2D rb;
    private bool dead = false;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;
    private BoxCollider2D boxCollider;
    private int dir = -1;
    private Vector3 initScale;
    private Transform enemy;
    private Transform target;
    private float originalSpeed;

    [Header("Knockback")]
    private float knockbackDuration = 1.5f;
    private float knockbackCounter = 0;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        originalSpeed = speed;

        initScale = rb.transform.localScale;
        enemy = rb.transform;
        target = FindAnyObjectByType<PlayerManager>().player;
    }

    // Update is called once per frame
    void Update()
    {

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
            }
        }
    }

    public void KnockBack()
    {
        knockbackCounter = knockbackDuration;

        Vector2 kbForce = new Vector2(-100 * (rb.transform.position.x + Time.deltaTime + dir * speed), rb.velocity.y);
        rb.AddForce(kbForce, ForceMode2D.Force);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red: MonoBehaviour
{
    private IRedState currentState = new RedPatrolState();

    [SerializeField] public float speed;
    [SerializeField] private LayerMask playerLayer;

    public BoxCollider2D boxCollider;
    public Rigidbody2D rb;
    public Vector3 initScale;
    public Animator animator;
    public bool dead = false;
    public int direction = -1;
    public float originalSpeed;

    [Header("Knockback")]
    private float knockbackDuration = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        initScale = rb.transform.localScale;
        currentState.Enter(this);
        originalSpeed = speed;
    }

    void Update()
    {
        UpdateState();
    }

    void UpdateState()
    {
        IRedState newState = currentState.Tick();
        if (newState != null)
        {
            //currentState.Exit(this);
            currentState = newState;
            newState.Enter(this);
        }
    }

    private IEnumerator KnockBack()
    {
        speed = -speed * 2f;
        Vector2 kbForce = new Vector2(-100 * (rb.transform.position.x + Time.deltaTime + direction * speed), rb.velocity.y);
        rb.AddForce(kbForce, ForceMode2D.Force);
        yield return new WaitForSeconds(knockbackDuration);
        speed = Mathf.Abs(speed * 0.5f);
    }

    public void StartKnockBack()
    {
        StartCoroutine(KnockBack());
    }

    public void MoveInDir()
    {
        //Face direction
        rb.transform.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);

        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }
    private void Deactivate()
    {
        animator.enabled = false;
    }

    public void Chase()
    {
        speed = originalSpeed * 3;
    }

    public void SlowDown()
    {
        speed /= 3;
    }

    public bool SeePlayer()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Wall")
        {
            direction *= -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Weapon")
        {
            dead = true;
        }

    }
}

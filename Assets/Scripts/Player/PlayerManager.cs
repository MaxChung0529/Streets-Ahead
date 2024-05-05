using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager: MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] private PlayerAttack attackScript;
    [SerializeField] private PlayerMovement movementScript;

    private LevelManager levelManager;

    [Header("Player")]
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask wallLayer;
    [SerializeField] public Transform firepoint;
    public float playerScale = 0.4f;
    public bool grounded;
    public bool onGround;
    public bool hitWall;
    public Animator animator;
    public Rigidbody2D rb;
    public CapsuleCollider2D capsuleCollider;
    public TrailRenderer trail;
    public Transform player;

    private float originalSpeed = 0f;
    public float speed = 15f;

    private Vector3 respawnPt;
    public GameObject fallDetector;

    public Tools holdingTool;
    private AbilityHUD hud;
    private string toolName;

    [Header("Stat")]
    public bool alive = true;

    [Header("Loot")]
    private int lotusCount = 0;

    [Header("Blackhole")]
    public int blackholeCount = 0;

    public bool shielded = false;

    [Header("Flash")]
    public bool spedUp = false;
    public float speedDuration = 0f;

    [Header("Moonwalk")]
    public bool moonWalk = false;
    public float moonWalkDuration = 0f;
    public float gravity = 2f;
    private float originalGravity = 0f;


    private void Start()
    {
        instance = this;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        trail = GetComponent<TrailRenderer>();

        hud = GameObject.Find("HUD").GetComponent<AbilityHUD>();

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            var hudOverlay = GameObject.Find("HUD");
            hudOverlay.SetActive(false);
        }


        trail.enabled = false;
        player = transform;
        levelManager = LevelManager.instance;

        originalSpeed = speed;
        originalGravity = gravity;

        rb.transform.position = levelManager.spawnPosition.position;

    }

    void Update()
    {

        if (!alive)
        {
            gameObject.SetActive(false);
            return;
        }

        attackScript.KunaiCooledDown();

        if (Input.GetKeyDown(KeyCode.Q)) {

            if (holdingTool != null)
            {
                holdingTool.Activate();
                holdingTool = null;
                hud.UpdateSecondary(toolName, false);
            }else if (blackholeCount > 0)
            {
                attackScript.PushBlackHole();
                if (blackholeCount > 0)
                {
                    hud.UpdateSecondary(toolName, true);
                }else
                {
                    hud.UpdateSecondary(toolName, false);
                }
            }
        }

        //IsGrounded();
        
        //SpeedUp activated
        if (spedUp)
        {
            speedDuration -= Time.deltaTime;

            if (speedDuration > 0)
            {
                speed = originalSpeed * 1.5f;
            }

            if (speedDuration <= 0)
            {
                speed = originalSpeed;
                spedUp = false;
                holdingTool = null;
            }
        }

        //Moonwalk activated
        if (moonWalk)
        {
            moonWalkDuration -= Time.deltaTime;

            if (moonWalkDuration > 0)
            {
                gravity = originalGravity * 0.5f;
            }

            if (moonWalkDuration <= 0)
            {
                gravity = originalGravity;
                moonWalk = false;
                holdingTool = null;
            }
        }

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    public void UpdateRespawn()
    {
        respawnPt = transform.position;
    }
    private bool IsGrounded()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);

        onGround = rayCastHit.collider != null;

        if (onGround == true)
        {
            //dashReset = true;
        }

        return onGround;
    }

    public bool canPushBlackHole()
    {   
        blackholeCount--;
        return blackholeCount + 1 > 0 && holdingTool == null;
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
            levelManager.addLotus();
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Blackhole")
        {
            blackholeCount = 3;
            toolName = "Blackhole";
            holdingTool = null;
            hud.UpdateSecondary(toolName, true);
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Shield")
        {
            holdingTool = new Shield();
            toolName = "Shield";
            hud.UpdateSecondary(toolName, true);
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Lightning")
        {
            holdingTool = new Lightning();
            toolName = "Lightning";
            hud.UpdateSecondary(toolName, true);
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Moonwalk")
        {
            holdingTool = new Moonwalk();
            toolName = "Moonwalk";
            hud.UpdateSecondary(toolName, true);
            collision.gameObject.SetActive(false);

        }

        if (collision.gameObject.tag == "Checkpoint")
        {
            levelManager.Check();
        }

        if (collision.gameObject.tag == "Enemies")
        {

            if (!shielded)
            {
                alive = false;
            }else
            {
                collision.gameObject.GetComponent<Red>().StartKnockBack();
                shielded = false;

                var checkTool = holdingTool as Shield;

                trail.enabled = false;
            }
        }
    }
}

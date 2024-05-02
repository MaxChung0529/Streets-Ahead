using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] private float attackCD;
    private Animator animator;
    private PlayerMovement playerMovement;
    private float cdTimer = Mathf.Infinity;

    [Header("Projectile")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] kunais;
    [SerializeField] private GameObject[] blackholes;

    private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        //Shoot kunai if E was pressed
        if ((Input.GetKeyDown(KeyCode.E) && cdTimer > attackCD && playerMovement.canAttack())){

            ShootKunai();

        }


        cdTimer += Time.deltaTime;
    }

    void ShootKunai()
    {
        cdTimer = 0;
        animator.SetTrigger("attack");

        int kunaiNum = findKunai();

        //Reset kunai position back to default
        kunais[kunaiNum].transform.position = firepoint.position;

        kunais[kunaiNum].GetComponent<Kunai>().SetDirection(Mathf.Sign(transform.localScale.x));

    }

    //Pooling kunais
    private int findKunai()
    {
        for (int i = 0; i < kunais.Length; i++)
        {
            if (!kunais[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

    public void PushBlackHole()
    {

        playerManager = PlayerManager.instance;
        if (cdTimer > attackCD && playerMovement.canAttack() && playerManager.canPushBlackHole())
        {

            cdTimer = 0;
            animator.SetTrigger("attack");
            int blackHoleNum = findBlackhole();

            blackholes[blackHoleNum].SetActive(true);
            blackholes[blackHoleNum].transform.position = firepoint.position;
            blackholes[blackHoleNum].GetComponent<BlackHole>().SetDirection(Mathf.Sign(transform.localScale.x));

        }
    }

    private int findBlackhole()
    {
        for (int i = 0; i < blackholes.Length; i++)
        {
            if (!blackholes[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}

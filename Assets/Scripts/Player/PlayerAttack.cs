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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Attack if E was pressed
        if ((Input.GetKeyDown(KeyCode.E) && cdTimer > attackCD && playerMovement.canAttack())){

            Attack();

        }

        cdTimer += Time.deltaTime;
    }

    void Attack()
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
        for (int i = 0; i< kunais.Length; i++)
        {
            if (!kunais[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}

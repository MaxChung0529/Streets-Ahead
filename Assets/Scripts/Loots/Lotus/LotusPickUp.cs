using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusPickUp : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LotusManager.instance.addLotus();
            gameObject.SetActive(false);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private float bounciness = 27f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            Vector2 upVector = Vector2.up * bounciness;
            rb.velocity = upVector;
        }
    }
}

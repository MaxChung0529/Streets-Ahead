using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy1 : MonoBehaviour
{
    [SerializeField] private GameObject door;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            boxCollider.enabled = false;
            door.SetActive(false);
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Blackhole")
        {
            gameObject.SetActive(false);
            door.SetActive(false);
        }


    }


}

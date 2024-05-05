using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy2 : MonoBehaviour
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
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Blackhole"))
        {
            gameObject.SetActive(false);
            door.SetActive(false);
        }
    }


}

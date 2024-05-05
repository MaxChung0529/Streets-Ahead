using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCheckPt : MonoBehaviour
{
    [SerializeField] private GameObject hud;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Collided");
            hud.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    private PlayerManager player;
    public GameObject overlay;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if (!player.alive)
        {
            //Time.timeScale = 0f;
            overlay.SetActive(true);

        }
    }
}

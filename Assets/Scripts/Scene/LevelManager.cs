using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Checkpoint")]
    public GameObject checkPoint;
    [SerializeField] public Sprite checkedFlag;

    public GameObject exitPortal;

    [Header("Lotus")]
    public TextMeshProUGUI lotusHeld;

    int lotusCount = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        lotusHeld.text = "X " + lotusCount.ToString();
    }

    public void addLotus()
    {
        lotusCount++;
        lotusHeld.text = "X " + lotusCount.ToString();
    }

    public void Check()
    {
        checkPoint.GetComponent<SpriteRenderer>().sprite = checkedFlag;
        checkPoint.GetComponent<BoxCollider2D>().enabled = false;
    }
}

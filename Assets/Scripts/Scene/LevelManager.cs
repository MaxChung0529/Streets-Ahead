using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public GameObject PopupUI;
    public PlayerManager player;

    [Header("Checkpoint")]
    public GameObject checkPoint;
    private bool reachedCheckPoint = false;
    [SerializeField] public Sprite checkedFlag;

    public GameObject exitPortal;
    public GameObject enterPortal;
    public Transform spawnPosition;

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
        PopupUI.SetActive(false);
        spawnPosition = enterPortal.transform;

        player = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    public void GoCheckPoint()
    {
        if (reachedCheckPoint)
        {
            spawnPosition = checkPoint.transform;
            
        }else
        {
            spawnPosition = enterPortal.transform;
        }
        RetryLevel();
    }

    public void RetryLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitRequest()
    {
        PopupUI.SetActive(true);
        PopupUI.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Menu()
    {
        SceneManager.LoadSceneAsync("Menu");
        Time.timeScale = 1f;
    }

    public void ConfirmExit()
    {
        Menu();
    }

    public void CancelExit()
    {
        PopupUI.SetActive(false);
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
        reachedCheckPoint = true;
        spawnPosition = checkPoint.transform;
        player.UpdateRespawn();
    }
}

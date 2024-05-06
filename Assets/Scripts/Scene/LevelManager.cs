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
    [SerializeField] private Sprite checkedFlag;

    public GameObject exitPortal;
    public GameObject enterPortal;
    public Transform spawnPosition;

    [Header("Lotus")]
    [SerializeField] private GameObject[] lotuses;
    [SerializeField] private Sprite pickedLotus;
    public int lotusCount = 0;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timer;
    private float timePassed = 0f;
    private float oldTime = 0f;
    public int minute = 0;
    public int second = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PopupUI.SetActive(false);
        spawnPosition = enterPortal.transform;

        player = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed - oldTime > 1f)
        {
            addSecond();
            oldTime = timePassed;
        }
    }

    private void addSecond()
    {
        if (second < 59)
        {
            second++;
        }else
        {
            second = 0;
            minute++;
        }
        if (minute <= 99)
        {
            timer.text = minute.ToString("D2") + ":" + second.ToString("D2");
        }
        else
        {
            timer.text = minute.ToString("D3") + ":" + second.ToString("D2");
        }
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
        lotuses[lotusCount - 1].GetComponent<SpriteRenderer>().sprite = pickedLotus;
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

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
    public GameObject gameSaved;

    [Header("Checkpoint")]
    public GameObject checkPoint;
    private bool reachedCheckPoint = false;
    [SerializeField] private Sprite checkedFlag;

    public GameObject exitPortal;
    public GameObject enterPortal;
    public Transform spawnPosition;

    [Header("Lotus")]
    [SerializeField] private GameObject[] lotuses;
    [SerializeField] private GameObject[] lotusesLoot;
    [SerializeField] private Sprite pickedLotus;
    public int lotusCount = 0;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    private float timePassed = 0f;
    private float oldTime = 0f;
    public int minute = 0;
    public int second = 0;

    private int currentLevel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        PopupUI.SetActive(false);
        player = GameObject.Find("Player").GetComponent<PlayerManager>();

        currentLevel = SceneManager.GetActiveScene().buildIndex - 1;

        var gameSave = SaveLoadSystem.LoadGame();

        if (gameSave.levelDatas.Count > 0)
        {
            foreach (LevelData level in gameSave.levelDatas)
            {
                if (level.level == currentLevel)
                {
                    SetScene(level);
                }
            }
        }
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
            timerText.text = minute.ToString("D2") + ":" + second.ToString("D2");
        }
        else
        {
            timerText.text = minute.ToString("D3") + ":" + second.ToString("D2");
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

    public void SetScene(LevelData lvl)
    {
        if (lvl.secondsPassed > 0)
        {
            minute = lvl.secondsPassed / 60;
            second = lvl.secondsPassed % 60;
        }else
        {
            minute = 0;
            second = 0;
        }
        var lotusNum = lvl.lotus.Count;
        for (int i = 0; i < lotusNum; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (lvl.lotus[i] == j)
                {
                    lotusesLoot[j].SetActive(false);
                    lotuses[j].GetComponent<SpriteRenderer>().sprite = pickedLotus;
                    lotusCount++;
                }
            }
        }
    }

    public void AddLotus(GameObject collectedLotus)
    {
        lotusCount++;
        var index = 0;

        for (int i = 0; i < lotusesLoot.Length; i++)
        {
            if (lotusesLoot[i] == collectedLotus)
            {
                index = i;
            }
        }

        lotuses[index].GetComponent<SpriteRenderer>().sprite = pickedLotus;
    }

    public void Check()
    {
        checkPoint.GetComponent<SpriteRenderer>().sprite = checkedFlag;
        checkPoint.GetComponent<BoxCollider2D>().enabled = false;
        reachedCheckPoint = true;
        spawnPosition = checkPoint.transform;
        player.UpdateRespawn();
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

    public void Okay()
    {
        gameSaved.SetActive(false);
    }

    public void ConfirmExit()
    {
        Menu();
    }

    public void CancelExit()
    {
        PopupUI.SetActive(false);
    }

    public void SaveGame()
    {
        GameData gameData = SaveLoadSystem.LoadGame();
        var tmpLevelDatas = new List<LevelData>();

        var pickedLotus = new List<int>();

        for (int i = 0; i < lotusesLoot.Length; i++)
        {
            if (lotusesLoot[i].activeInHierarchy == false)
            {
                pickedLotus.Add(i);
            }
        }

        var levelData = new LevelData(currentLevel, pickedLotus, minute * 60 + second);
        if (gameData.levelDatas.Count > 0)
        {
            foreach (LevelData lvl in gameData.levelDatas)
            {
                if (lvl.level == currentLevel)
                {
                    tmpLevelDatas.Add(levelData);
                }else
                {
                    tmpLevelDatas.Add(lvl);
                }
            }
            gameData.levelDatas = tmpLevelDatas;
        }else
        {
            gameData.levelDatas.Add(levelData);
        }
        gameData.deathCount += player.deathCount;
        gameData.position[0] = player.player.position.x;
        gameData.position[1] = player.player.position.y;
        gameData.position[2] = player.player.position.z;

        SaveLoadSystem.SaveGame(gameData);
        gameSaved.SetActive(true);
    }
}

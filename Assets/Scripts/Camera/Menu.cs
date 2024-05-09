using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{

    public GameObject confirmationWindow;
    public GameObject gameSaveWindow;
    public GameObject settingsOverlay;
    public TextMeshProUGUI gameSaveLevel;
    public TextMeshProUGUI gameSaveLotus;
    [SerializeField] GameObject loadGameBtn;

    public void StartNewGame()
    {
        Time.timeScale = 1;
        SaveLoadSystem.NewGame();
        SceneManager.LoadScene(1);
    }

    public void ShowGameSave()
    {
        gameSaveWindow.SetActive(true);
        var save = SaveLoadSystem.LoadGame();

        //Only allow loading game save when the level isn't finished
        if (save.levelDatas.Count > 0 && save.levelDatas[save.levelDatas.Count - 1].levelScore == 0)
        {
            Debug.Log(save.levelDatas[save.levelDatas.Count - 1].levelScore);
            if (save.levelDatas[save.levelDatas.Count - 1].level > 0)
            {
                gameSaveLevel.text = "Level: " + (save.levelDatas[save.levelDatas.Count - 1].level);
            }else
            {
                gameSaveLevel.text = "Level: Tutorial";
            }
            gameSaveLotus.text = "Lotus collected: " + save.levelDatas[save.levelDatas.Count - 1].lotus.Count;
        }else
        {
            Debug.Log(save.levelDatas[save.levelDatas.Count - 1].levelScore);
            gameSaveLevel.text = "No playable game save";
            gameSaveLotus.enabled = false;
            loadGameBtn.SetActive(false);
        }
    }

    public void Settings()
    {
        settingsOverlay.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsOverlay.SetActive(false);
    }

    public void CancelLoadGame()
    {
        gameSaveWindow.SetActive(false);
    }

    public void LoadGame()
    {
        GameData save = SaveLoadSystem.LoadGame();
        SceneManager.LoadScene(save.levelDatas[save.levelDatas.Count - 1].level + 1);
    }

    public void QuitRequest()
    {
        confirmationWindow.SetActive(true);
    }

    public void StayInGame()
    {
        confirmationWindow.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

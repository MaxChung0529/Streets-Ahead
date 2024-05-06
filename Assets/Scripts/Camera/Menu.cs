using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{

    public GameObject confirmationWindow;
    public GameObject gameSaveWindow;
    public TextMeshProUGUI gameSaveLevel;
    public TextMeshProUGUI gameSaveLotus;

    public void StartNewGame()
    {
        SaveLoadSystem.NewGame();
        SceneManager.LoadScene(1);
    }

    public void ShowGameSave()
    {
        gameSaveWindow.SetActive(true);
        var save = SaveLoadSystem.LoadGame();
        if (save.levelDatas.Count > 0) {
            if (save.levelDatas[save.levelDatas.Count - 1].level > 1)
            {
                gameSaveLevel.text = "Level: " + (save.levelDatas[save.levelDatas.Count - 1].level - 1);
            }else
            {
                gameSaveLevel.text = "Level: Tutorial";
            }
            gameSaveLotus.text = "Lotus collected: " + save.levelDatas[save.levelDatas.Count - 1].lotusCount;
        }
    }

    public void CancelLoadGame()
    {
        gameSaveWindow.SetActive(false);
    }

    public void LoadGame()
    {
        GameData save = SaveLoadSystem.LoadGame();
        SceneManager.LoadScene(save.levelDatas[save.levelDatas.Count - 1].level);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public GameObject PopupUI;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        PopupUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);

        //Resume game
        Time.timeScale = 1f;

        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);

        //Freeze game
        Time.timeScale = 0f;

        isPaused = true;
    }

    public void QuitRequest()
    {
        PopupUI.SetActive(true);
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
}

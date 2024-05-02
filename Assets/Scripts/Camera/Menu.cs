using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public GameObject confirmationWindow;

    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {

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

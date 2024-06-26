using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinishScene : MonoBehaviour
{

    public int thisScene;
    private BoxCollider2D boxCollider;
    [SerializeField] private GameObject finishedOverlay;
    [SerializeField] private GameObject[] finalLotuses;
    [SerializeField] private Sprite pickedLotus;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI score;
    public int totalScore;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        finishedOverlay.SetActive(false);
        totalScore = 0;
        thisScene = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            if (SceneManager.GetActiveScene().name != "Tutorial" && thisScene < 3)
            {
                Time.timeScale = 0;
                finishedOverlay.SetActive(true);
                var num = LevelManager.instance.lotusCount;

                levelText.text = "You are now a level " + num + " laser lotus!!!";

                for (int i = 0; i < num; i++)
                {
                    finalLotuses[i].GetComponent<SpriteRenderer>().sprite = pickedLotus;
                }

                var seconds = LevelManager.instance.minute * 60 + LevelManager.instance.second;
                var minuteUsed = (seconds / 60f).ToString("#.##");

                totalScore = Mathf.RoundToInt(num * 1000 * (1 / float.Parse(minuteUsed)));

                score.text = "Score: " + num + " Lotuses x " + minuteUsed + " minutes = " + totalScore;

            }
            else
            {
                NextScene();
            }

        }
    }

    public void NextScene()
    {
        if (SceneManager.GetActiveScene().name != "Tutorial" && thisScene < 3)
        {
            LevelManager.instance.SaveGame(totalScore);
        }else if (thisScene == 3)
        {

            var num = LevelManager.instance.lotusCount;
            var seconds = LevelManager.instance.minute * 60 + LevelManager.instance.second;
            var minuteUsed = (seconds / 60f).ToString("#.##");

            totalScore = Mathf.RoundToInt(num * 1000 * (1 / float.Parse(minuteUsed)));

            LevelManager.instance.SaveGame(totalScore);
        }

        var newSave = SaveLoadSystem.LoadGame();
        newSave.position = new float[] { 0f, 0f, 0f };
        newSave.totalScore += totalScore;
        Time.timeScale = 1;
        SceneManager.LoadScene(thisScene + 1);

        SaveLoadSystem.SaveGame(newSave);
    }

}

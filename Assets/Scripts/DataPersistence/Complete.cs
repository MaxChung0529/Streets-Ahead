using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Complete : MonoBehaviour
{

    [SerializeField] private GameObject[] lotuses;
    [SerializeField] private Sprite lotusCollected;
    [SerializeField] private TextMeshProUGUI level1Score;
    [SerializeField] private TextMeshProUGUI level2Score;
    [SerializeField] private TextMeshProUGUI lowerThan6Text;
    [SerializeField] private TextMeshProUGUI level6Text;
    [SerializeField] private TextMeshProUGUI btnText;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        var save = SaveLoadSystem.LoadGame();
        var lotusNum = 0;

        Debug.Log(save.levelDatas.Count);
        for (int j = 0; j < save.levelDatas.Count; j++)
        {
            //Fetching the number of lotuses collected during each level
            var lotusCollectedEachLvl = save.levelDatas[j].lotus.Count;
            lotusNum += lotusCollectedEachLvl;

            var minuteUsed = (save.levelDatas[j].secondsPassed / 60f).ToString("#.##");

            var score = Mathf.RoundToInt(lotusCollectedEachLvl * 1000 * (1 / float.Parse(minuteUsed)));
            
            switch (j)
            {
                case 0:
                    level1Score.text = score.ToString();
                    break;

                case 1:
                    level2Score.text = score.ToString();
                    break;
            }
        }

        for (int i = 0; i < lotusNum; i++)
        {
            lotuses[i].GetComponent<SpriteRenderer>().sprite = lotusCollected;
        }

        if (lotusNum < 6)
        {
            level6Text.enabled = false;
            lowerThan6Text.text = "Level " + lotusNum + " Laser Lotus huh? You have to do better Jeff!";
            btnText.text = "Fine! I will humour you";
        }else
        {
            lowerThan6Text.enabled = false;
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}

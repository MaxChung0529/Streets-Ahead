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

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        finishedOverlay.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            if (SceneManager.GetActiveScene().name != "Tutorial")
            {
                finishedOverlay.SetActive(true);
                var num = LevelManager.instance.lotusCount;

                levelText.text = "You are now a level " + num + " laser lotus!!!";

                for (int i = 0; i < num; i++)
                {
                    finalLotuses[i].GetComponent<SpriteRenderer>().sprite = pickedLotus;
                }
            }else
            {
                NextScene();
            }

        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene(thisScene + 1);
    }

}

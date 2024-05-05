using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScene : MonoBehaviour
{

    public int thisScene;
    private BoxCollider2D boxCollider;
    [SerializeField] private GameObject finishedOverlay;
    [SerializeField] private GameObject[] finalLotuses;
    [SerializeField] private Sprite pickedLotus;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        finishedOverlay.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Time.timeScale = 0;
            //NextScene();
            finishedOverlay.SetActive(true);
            var num = LevelManager.instance.lotusCount;

            for (int i = 0; i < num; i++)
            {
                finalLotuses[i].GetComponent<SpriteRenderer>().sprite = pickedLotus;
            }

        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene(thisScene + 1);
    }

}

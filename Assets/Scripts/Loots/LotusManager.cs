using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LotusManager : MonoBehaviour
{
    public static LotusManager instance;

    public TextMeshProUGUI lotusHeld;

    int lotusCount = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        lotusHeld = GetComponent<TextMeshProUGUI>();
        lotusHeld.text = lotusCount.ToString() + " Lotus Held";
    }

    public void addLotus()
    {
        lotusCount++;
        lotusHeld.text = lotusCount.ToString() + " Lotus Held";
    }

}

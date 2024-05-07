using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyAudio : MonoBehaviour
{

    [SerializeField] private List<AudioClip> bgm = new List<AudioClip>();

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }


}

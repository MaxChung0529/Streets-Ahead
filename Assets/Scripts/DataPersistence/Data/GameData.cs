using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public int deathCount;
    public float[] position;
    public int totalScore;
    public List<LevelData> levelDatas;

    public GameData()
    {
        levelDatas = new List<LevelData>();
        this.deathCount = 0;
        this.totalScore = 0;


        position = new float[3];

        this.position[0] = 0f;
        this.position[1] = 0f;
        this.position[2] = 0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{ 
    public int level;
    public int lotusCount;
    public int secondsPassed;

    public LevelData(int _level, int _lotusCount, int _secondsPassed)
    {
        level = _level;
        lotusCount = _lotusCount;
        secondsPassed = _secondsPassed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{ 
    public int level;
    public List<int> lotus;
    public int secondsPassed;

    public LevelData(int _level, List<int> lotuses, int _secondsPassed)
    {
        level = _level;
        lotus = lotuses;
        secondsPassed = _secondsPassed;
    }
}

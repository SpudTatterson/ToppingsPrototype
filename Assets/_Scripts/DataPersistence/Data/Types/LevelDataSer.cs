using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelDataSer
{
    public int levelID;
    public float bestTime;
    public float bestStarCount;

    public LevelDataSer(int levelID, float bestTime, float bestStarCount)
    {
        this.levelID = levelID;
        this.bestTime = bestTime;
        this.bestStarCount = bestStarCount;
    }
    public LevelDataSer(int levelID)
    {
        this.levelID = levelID;
        bestTime = 0;
        bestStarCount = 0;
    }
}

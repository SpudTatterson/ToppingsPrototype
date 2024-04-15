using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelDataSer
{
    public float bestTime;
    public float bestStarCount;

    public LevelDataSer(float bestTime, float bestStarCount)
    {
        this.bestTime = bestTime;
        this.bestStarCount = bestStarCount;
    }
    public LevelDataSer()
    {
        bestTime = 0;
        bestStarCount = 0;
    }
}

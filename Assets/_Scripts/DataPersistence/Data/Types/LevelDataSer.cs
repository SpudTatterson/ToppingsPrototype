using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelDataSer 
{
    public int buildIndex;
    public float bestTime;
    public float bestStarCount;
    
    public LevelDataSer(int buildIndex, float bestTime, float bestStarCount)
    {   
        this.buildIndex = buildIndex;
        this.bestTime = bestTime;   
        this.bestStarCount = bestStarCount;
    }
}

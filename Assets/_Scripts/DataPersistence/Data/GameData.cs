using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{

}
[System.Serializable]
public class SettingsData : Data
{
    [Header("Sound")]
    public float masterVolume;
    public float fxVolume;
    public float musicVolume;

    public SettingsData()
    {
        this.masterVolume = 1;
        this.fxVolume = 1;
        this.musicVolume = 1;
    }
}
[System.Serializable]
public class GameData : Data
{
    public List<LevelDataSer> levels = new List<LevelDataSer>();

    public LevelDataSer GetLevelData(int buildIndex)
    {
        if (levels.Count > buildIndex)
        {
            foreach (LevelDataSer data in levels)
            {
                if (data.levelID == buildIndex)
                    return data;
            }
        }
        if (levels.Count <= buildIndex)
        {    
            int initialMissingLevels = buildIndex - levels.Count + 1;
            int missingLevelIndex = levels.Count;
            for (int i = 0; i < initialMissingLevels; i++)
            {
                LevelDataSer data = new LevelDataSer(missingLevelIndex);
                levels.Add(data);
                missingLevelIndex++;
            }
        }
        LevelDataSer levelData = new LevelDataSer(buildIndex);
        levels[buildIndex] = levelData;
        return levelData;
    }
    public GameData()
    {
        this.levels = new List<LevelDataSer>();
    }
}

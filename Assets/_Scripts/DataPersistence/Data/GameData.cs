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

    public GameData()
    {
        this.levels = new List<LevelDataSer>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData 
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

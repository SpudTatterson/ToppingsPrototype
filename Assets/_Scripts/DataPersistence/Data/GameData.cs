using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Data
{

}
[System.Serializable]
public class SettingsData : Data
{
    [Header("Sound")]
    public float masterVolume;
    public float fxVolume;
    public float musicVolume;
    [Header("Minion Settings")]
    public DefaultClothing defaultClothing;
    [Header("Customizer Settings")]
    public SerializableDictionary<CustomizationType, int> customizationPicks;
    public SerializableDictionary<CustomizationType, ColorOption> customColors;

    public SettingsData()
    {
        this.masterVolume = 1;
        this.fxVolume = 1;
        this.musicVolume = 1;
        this.defaultClothing = new DefaultClothing();
        this.customizationPicks = new SerializableDictionary<CustomizationType, int>{
            {CustomizationType.Hat, 0},
            {CustomizationType.BackPack, 0},
            {CustomizationType.SkinColor, 1},
            {CustomizationType.ClothColor, 1}
        };
        this.customColors = new SerializableDictionary<CustomizationType, ColorOption>{
            {CustomizationType.SkinColor, new ColorOption("Custom", Color.white)},
            {CustomizationType.ClothColor, new ColorOption("Custom", Color.white)}
        };
        // this.customizerClothingColor = new ColorOption("Custom", Color.white);
        // this.customizerSkinColor = new ColorOption("Custom", Color.white);
    }
}
[System.Serializable]
public class GameData : Data
{
    public SerializableDictionary<string, LevelDataSer> levelDict = new SerializableDictionary<string, LevelDataSer>();

    public GameData()
    {
        levelDict = new SerializableDictionary<string, LevelDataSer>();
    }
}

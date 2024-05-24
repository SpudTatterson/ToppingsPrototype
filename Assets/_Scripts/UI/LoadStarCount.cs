using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadStarCount : MonoBehaviour, IPlayerDataPersistence
{
    [SerializeField] int levelBuildIndex;

    float bestScore;
    string id;
    public void LoadData(GameData data)
    {
        LevelDataSer levelData = new LevelDataSer();
        id = SceneManager.GetActiveScene().name;
        if (data.levelDict.ContainsKey(id))
            levelData = data.levelDict[id];
        else
            data.levelDict.Add(id, levelData);
        bestScore = levelData.bestStarCount;
    }

    public void SaveData(GameData data)
    {
    }
    void Awake()
    {
        
    }
    void Start()
    {
        GetComponent<Image>().fillAmount = bestScore;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadStarCount : MonoBehaviour, IPlayerDataPersistence
{
    [SerializeField] int levelBuildIndex;

    float bestScore;
    public void LoadData(GameData data)
    {
        bestScore = data.GetLevelData(levelBuildIndex).bestStarCount;
    }

    public void SaveData(GameData data)
    {
    }

    void Start()
    {
        GetComponent<Image>().fillAmount = bestScore;
    }

}

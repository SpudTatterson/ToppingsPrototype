using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadStarCount : MonoBehaviour
{
    [SerializeField] int levelBuildIndex;
    void Start()
    {
        string bestScoreKey = "BestScoreScene" + levelBuildIndex.ToString();
        float starCount = PlayerPrefs.GetFloat(bestScoreKey, 0);
        GetComponent<Image>().fillAmount = starCount;
    }
}

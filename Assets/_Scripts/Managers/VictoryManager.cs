using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    UIManager ui;
    LevelEndPoint endPoint;
    int initialMinionCount;
    int passedMinionCount;
    float bestTime;
    string bestTimeKey;
    float starCount;
    float bestScore;
    string highScoreKey;
    [SerializeField] bool minOneStar = true;
    void Start()
    {
        bestTimeKey = "BestTimeScene" + SceneManager.GetActiveScene().buildIndex.ToString();
        highScoreKey = "BestScoreScene" + SceneManager.GetActiveScene().buildIndex.ToString();
        bestTime = PlayerPrefs.GetFloat(bestTimeKey);
        bestScore = PlayerPrefs.GetFloat(highScoreKey, 0);
        ui = FindObjectOfType<UIManager>();
        endPoint = FindObjectOfType<LevelEndPoint>();
    }
    public void TriggerWin()
    {
        GetComponent<TimeManager>().toggle = false;
        Time.timeScale = 0f;
        ui.victoryScreen.SetActive(true);
        RetrieveNumbers();
        UpdateVictoryScreenUI();
    }
    void UpdateStars()
    {
        starCount = Mathf.InverseLerp(0, initialMinionCount, passedMinionCount);

        if (minOneStar && starCount < 0.33f) starCount = 0.33f;

        CheckForNewHighScore();

        ui.starFillUpBar.fillAmount = starCount;
        ui.bestStarFillUpBar.fillAmount = bestScore;
    }

    void CheckForNewHighScore()
    {
        if(starCount > bestScore)
        {
            SetNewHighScore();
        }
    }

    void SetNewHighScore()
    {
        PlayerPrefs.SetFloat(highScoreKey, starCount);
    }

    void UpdateVictoryScreenUI()
    {
        CheckForBestTime(Time.timeSinceLevelLoad);
        UpdateStars();
        //calculate percentage
        float passedMinionPercentage = passedMinionCount / (float)initialMinionCount * 100f;
        //set ui to numbers
        ui.alivePercentageText.text = passedMinionPercentage.ToString("F0") + "%";
        ui.aliveNumberText.text = passedMinionCount.ToString();
        ui.timeText.text = FormatTime(Time.timeSinceLevelLoad);
        ui.bestTimeText.text = FormatTime(bestTime);
    }

    void CheckForBestTime(float time)
    {
        if (time < bestTime || bestTime == 0)
        {
            bestTime = time;
            TriggerNewBestTime();
        }
    }

    void TriggerNewBestTime()
    {
        ui.newBestTimeScreen.SetActive(true);
        PlayerPrefs.SetFloat(bestTimeKey, bestTime);
    }

    private void RetrieveNumbers()//retrieve required numbers
    {
        passedMinionCount = endPoint.GetPassedMinionCount();
        initialMinionCount = endPoint.GetInitialMinionCount();
    }

    string FormatTime(float timeInSeconds)
    {
        // Calculate minutes and seconds from the total seconds
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        // Format and return the string
        return string.Format("{0}M {1}S", minutes, seconds);
    }
}

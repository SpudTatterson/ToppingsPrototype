using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    UIManager ui;
    LevelEndPoint endPoint;
    int initialMinionCount;
    int passedMinionCount;
    int minPassedForVictory;
    [SerializeField] bool minOneStar = true;
    void Start()
    {
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
        float starCount = Mathf.InverseLerp(0, initialMinionCount, passedMinionCount);

        if (minOneStar && starCount < 0.33f) starCount = 0.33f;

        ui.starFillUpBar.fillAmount = starCount;
    }
    void UpdateVictoryScreenUI()
    {
        //calculate percentage
        float passedMinionPercentage = passedMinionCount / (float)initialMinionCount * 100f;
        //set ui to numbers
        ui.alivePercentageText.text = passedMinionPercentage.ToString("F0") + "%";
        ui.aliveNumberText.text = passedMinionCount.ToString();
        ui.timeText.text = FormatTime(Time.time);
        UpdateStars();
    }

    private void RetrieveNumbers()//retrieve required numbers
    {
        minPassedForVictory = endPoint.GetMinPassedForVictory();
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

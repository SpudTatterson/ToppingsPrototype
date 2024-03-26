using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    UIManager ui;

    void Start()
    {
        ui = FindObjectOfType<UIManager>();
    }
    public void TriggerWin()
    {
        GetComponent<TimeManager>().toggle = false;
        Time.timeScale = 0f;
        ui.victoryScreen.SetActive(true);
        UpdateNumbers();
    }

    void UpdateNumbers()
    {
        LevelEndPoint endPoint =FindObjectOfType<LevelEndPoint>(); 
        int passedMinionCount = endPoint.GetPassedMinionCount();
        int initialMinionCount = endPoint.GetInitialMinionCount();
        float passedMinionPercentage = passedMinionCount / (float)initialMinionCount * 100f;
        ui.alivePercentageText.text = passedMinionPercentage.ToString("F0") + "%";
        ui.aliveNumberText.text = passedMinionCount.ToString();
        ui.timeText.text = FormatTime(Time.time);
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

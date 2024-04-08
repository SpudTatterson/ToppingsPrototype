using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDUpdater : MonoBehaviour
{
    public static HUDUpdater instance;
    float bestStarCount;
    float starCount;
    int initialMinionCount;
    int passedMinionCount;
    string time;
    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        bestStarCount = VictoryManager.instance.GetBestStarCount();
        UIManager.instance.bestStarFillUpBarHUD.fillAmount = bestStarCount;

        initialMinionCount = LevelEndPoint.instance.GetInitialMinionCount();

        UpdateStarHUD();
    }

    // Update is called once per frame
    void Update()
    {
        time = FormatTime(Time.timeSinceLevelLoad);
    
        UIManager.instance.timeHUDText.text = time;
    }

    public void UpdateStarHUD()
    {
        starCount = VictoryManager.instance.CalculateStarCount();
        passedMinionCount = LevelEndPoint.instance.GetPassedMinionCount();


        string minionCount = string.Format("{0}/{1}", passedMinionCount, initialMinionCount);

        UIManager.instance.starFillUpBarHUD.fillAmount = starCount;
        UIManager.instance.minionCountHUDText.text = minionCount;
    }

    string FormatTime(float timeInSeconds)
    {
        // Calculate minutes and seconds from the total seconds
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        // Format and return the string
        return string.Format("{0}:{1}", minutes, seconds);
    }
}

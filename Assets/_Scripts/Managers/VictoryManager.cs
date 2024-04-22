using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour, IPlayerDataPersistence
{
    public static VictoryManager instance;

    [Header("Settings")]
    [Tooltip("give the player a minimum of 1 star for completing the level")]
    [SerializeField] bool minOneStar = true;

    int initialMinionCount;
    int passedMinionCount;

    float bestTime;
    float starCount;
    float bestScore;

    LevelDataSer levelData = new LevelDataSer();
    string id;
    void Awake()
    {
        instance = this;
        id = SceneManager.GetActiveScene().name;
    }

    public void SaveData(GameData data)
    {
        if (data.levelDict.ContainsKey(id))
            data.levelDict[id] = levelData;
        else
            data.levelDict.Add(id, levelData);
    }

    public void LoadData(GameData data)
    {
        if(id == null) id = SceneManager.GetActiveScene().name;
        if (data.levelDict.ContainsKey(id))
            levelData = data.levelDict[id];
        else
            data.levelDict.Add(id, levelData);
        bestScore = levelData.bestStarCount;
        bestTime = levelData.bestTime;
    }

    public void TriggerWin() //this goes on the button that ends the level
    {
        GetComponent<TimeManager>().toggle = false;
        Time.timeScale = 0f;
        UIManager.instance.victoryScreen.SetActive(true);
        UpdateVictoryScreenUI();
        DataPersistenceManager.instance.SaveGame();
    }
    public void TriggerLoss()
    {
        Time.timeScale = 0f;
        UIManager.instance.loseScreen.SetActive(true);
    }
    void UpdateVictoryScreenUI()
    {
        CheckForBestTime(Time.timeSinceLevelLoad);
        UpdateStars();
        //calculate percentage
        float passedMinionPercentage = passedMinionCount / (float)initialMinionCount * 100f;
        //set ui to numbers
        UIManager.instance.alivePercentageText.text = passedMinionPercentage.ToString("F0") + "%";
        UIManager.instance.aliveNumberText.text = passedMinionCount.ToString();
        UIManager.instance.timeText.text = FormatTime(Time.timeSinceLevelLoad);
        UIManager.instance.bestTimeText.text = FormatTime(bestTime);
    }
    void UpdateStars()
    {
        CalculateStarCount();

        CheckForNewHighScore();

        UIManager.instance.starFillUpBar.fillAmount = starCount;
        UIManager.instance.bestStarFillUpBar.fillAmount = bestScore;
    }

    public float CalculateStarCount()
    {
        RetrieveNumbers();

        if (passedMinionCount < LevelEndPoint.instance.GetMinPassedForVictory())
            return 0f;

        starCount = Mathf.InverseLerp(0, initialMinionCount, passedMinionCount);

        if (minOneStar && starCount < 0.33f) starCount = 0.33f;

        return starCount;
    }
    private void RetrieveNumbers() //retrieve required numbers
    {
        passedMinionCount = LevelEndPoint.instance.GetPassedMinionCount();
        initialMinionCount = LevelEndPoint.instance.GetInitialMinionCount();
    }
    void CheckForNewHighScore()
    {
        if (starCount > bestScore)
        {
            SetNewHighScore();
        }
    }

    void SetNewHighScore()
    {
        levelData.bestStarCount = starCount;
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
        UIManager.instance.newBestTimeScreen.SetActive(true);
        levelData.bestTime = bestTime;
    }

    string FormatTime(float timeInSeconds)
    {
        // Calculate minutes and seconds from the total seconds
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        // Format and return the string
        return string.Format("{0}M {1}S", minutes, seconds);
    }

    public float GetBestStarCount()
    {
        return bestScore;
    }

}

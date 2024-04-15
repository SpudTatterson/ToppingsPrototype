using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    [Header("Settings")]
    [Tooltip("give the player a minimum of 1 star for completing the level")]
    [SerializeField] bool minOneStar = true;

    int initialMinionCount;
    int passedMinionCount;
    float bestTime;
    string bestTimeKey;
    float starCount;
    float bestScore;
    string highScoreKey;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // make scene unique keys for playerPrefs  
        bestTimeKey = "BestTimeScene" + SceneManager.GetActiveScene().buildIndex.ToString();
        highScoreKey = "BestScoreScene" + SceneManager.GetActiveScene().buildIndex.ToString();
        // get playerPrefs
        bestTime = PlayerPrefs.GetFloat(bestTimeKey);
        bestScore = PlayerPrefs.GetFloat(highScoreKey, 0);

    }
    public void TriggerWin() //this goes on the button that ends the level
    {
        GetComponent<TimeManager>().toggle = false;
        Time.timeScale = 0f;
        UIManager.instance.victoryScreen.SetActive(true);
        UpdateVictoryScreenUI();
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
        PlayerPrefs.SetFloat(highScoreKey, starCount);
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
        PlayerPrefs.SetFloat(bestTimeKey, bestTime);
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

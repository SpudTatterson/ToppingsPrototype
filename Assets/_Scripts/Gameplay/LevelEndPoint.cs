using UnityEngine;

public class LevelEndPoint : MonoBehaviour
{
    public static LevelEndPoint instance;
    [Header("Settings")]
    [SerializeField] string minionTag = "Lemming";
    [Tooltip("Minimum amount of minions that need to pass to qualify for the next level")]
    [SerializeField] int minPassedForVictory = 10;
    int passedMinionCount = 0;
    int initialMinionCount;

    void Awake()
    {
        instance = this;
        initialMinionCount = FindAnyObjectByType<LemmingSpawner>().lemmingCount;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(minionTag))
        {
            MinionPassed(other);
        }
    }
    void MinionPassed(Collider minion)
    {
        passedMinionCount++;
        HUDUpdater.instance.UpdateStarHUD();
        Destroy(minion.gameObject);
        if (CheckForVictory())
        {
            UIManager.instance.victoryButton.SetActive(true);
            if (LevelFinished())
            {
                VictoryManager.instance.TriggerWin();
            }
        }
    }

    bool CheckForVictory()
    {
        return passedMinionCount >= minPassedForVictory;
    }
    bool LevelFinished()
    {
        return passedMinionCount == initialMinionCount;
    }

    public int GetPassedMinionCount()
    {
        return passedMinionCount;
    }
    public int GetInitialMinionCount()
    {
        return initialMinionCount;
    }
    public int GetMinPassedForVictory()
    {
        return minPassedForVictory;
    }
}

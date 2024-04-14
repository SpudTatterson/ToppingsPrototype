using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    public static MinionManager instance;
    List<LemmingHealth> minions = new List<LemmingHealth>();
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("More Then one minion manager exists please remove one");
    }
    public void Add(LemmingHealth minion)
    {
        minions.Add(minion);
    }
    public void Remove(LemmingHealth minion)
    {
        minions.Remove(minion);
        CheckWinState();
    }
    void CheckWinState()
    {
        if(minions.Count != 0 && LevelEndPoint.instance.CheckForVictory())
        {
            UIManager.instance.victoryButton.SetActive(true);
        } 
        if(minions.Count == 0 && LevelEndPoint.instance.GetPassedMinionCount() == 0)
        {
            VictoryManager.instance.TriggerLoss();
        }
        else if(minions.Count == 0 && LevelEndPoint.instance.CheckForVictory())
        {
            VictoryManager.instance.TriggerWin();
        }
    }
}

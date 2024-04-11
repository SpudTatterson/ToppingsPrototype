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
    public void Add(LemmingHealth movement)
    {
        minions.Add(movement);
    }
    public void Remove(LemmingHealth movement)
    {
        minions.Remove(movement);
        CheckWinState();
    }
    void CheckWinState()
    {
        Debug.Log("Checking");
        Debug.Log(minions.Count.ToString());
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

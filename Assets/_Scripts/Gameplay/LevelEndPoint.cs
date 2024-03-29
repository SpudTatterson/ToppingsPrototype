using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndPoint : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] string minionTag = "Lemming";
    [Tooltip("Minimum amount of minions that need to pass to qualify for the next level")]
    [SerializeField] int minPassedForVictory = 10;
    int passedMinionCount = 0;
    int initialMinionCount;
    UIManager uiManager;

    void Start()
    {
        initialMinionCount = FindAnyObjectByType<LemmingSpawner>().lemmingCount;
        uiManager = FindAnyObjectByType<UIManager>();
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
        Destroy(minion.gameObject);
        if (CheckForVictory())
        {
            uiManager.victoryButton.SetActive(true);
            if (LevelFinished())
            {

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

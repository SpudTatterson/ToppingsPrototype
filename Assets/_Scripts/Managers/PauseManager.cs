using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    bool paused;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    void Toggle()
    {
        if (paused)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        UIManager.instance.pauseScreen.SetActive(true);
        PlacementManager.instance.ResetAllVariables();
        JobsSelector.instance.ResetVars();
        Time.timeScale = 0;
        paused = true;
    }

    public void UnPause()
    {
        UIManager.instance.pauseScreen.SetActive(false);
        UIManager.instance.settingsScreen.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }

}

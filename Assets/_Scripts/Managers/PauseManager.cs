using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    bool paused;
    UIManager UI;
    // Start is called before the first frame update
    void Start()
    {
        UI = GetComponent<UIManager>();
    }

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
        UI.PauseScreen.SetActive(true);
        Time.timeScale = 0;
        paused = true;
    }

    public void UnPause()
    {
        UI.PauseScreen.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }

}

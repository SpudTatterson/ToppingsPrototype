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
            UI.PauseScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void UnPause()
    {
        UI.PauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

}

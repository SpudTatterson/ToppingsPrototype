using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] float fastForwardMultiplier = 1;
    [SerializeField] float slowDownMultiplier = .1f;
    [SerializeField] GameObject jobsButton;
    [SerializeField] GameObject placeablesButton;

    private void Update()
    {
        ManipulateTime();
        Hotkeys();
    }

    private void ManipulateTime()
    {
        if(Input.GetKey(KeyCode.Space)) 
        {
            Time.timeScale = fastForwardMultiplier;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            Time.timeScale = slowDownMultiplier;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void Hotkeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            jobsButton.SetActive(true);
            placeablesButton.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            placeablesButton.SetActive(true);
            jobsButton.SetActive(false);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    [SerializeField] float fastForwardMultiplier = 1;
    [SerializeField] float slowDownMultiplier = .1f;
    [SerializeField] GameObject jobsButton;
    [SerializeField] GameObject placeablesButton;
    [HideInInspector]public bool toggle = true;

    private void Update()
    {
        //ManipulateTime(); i ruined it for now sorry
        Hotkeys();
    }

    private void ManipulateTime()
    {
        if(!toggle) return;
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

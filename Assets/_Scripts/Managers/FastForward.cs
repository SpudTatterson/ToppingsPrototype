using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    [SerializeField] float fastForwardMultiplier = 1;
    [SerializeField] float slowDownMultiplier = .1f;
    [HideInInspector]public bool toggle = true;

    private void Update()
    {
        //ManipulateTime(); i ruined it for now sorry
                         // nooooooooooooo!
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
}

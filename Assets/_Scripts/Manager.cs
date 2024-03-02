using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] float fastForwardMultiplier = 1;
    [SerializeField] float slowDownMultiplier = .1f;

    private void Update()
    {
        ManipulateTime();
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
}

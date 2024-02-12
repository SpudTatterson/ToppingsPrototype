using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] float fastForwardMultiplier = 1;

    private void Update()
    {
        FastForward();
    }

    private void FastForward()
    {
        if(Input.GetKey(KeyCode.Space)) 
        {
            Time.timeScale = fastForwardMultiplier;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}

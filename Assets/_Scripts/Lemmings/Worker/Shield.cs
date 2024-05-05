using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Worker
{
    [SerializeField] string animationIDName;

    private void Update()
    {
        jobTime -= Time.deltaTime;

        if (jobTime <= 0)
        {
            this.enabled = false;
        }
    }
}

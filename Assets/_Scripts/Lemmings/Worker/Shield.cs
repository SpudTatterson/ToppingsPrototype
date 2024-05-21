using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Worker
{
    new void OnEnable()
    {
        animator.SetBool("HoldShield", true);
        base.OnEnable();
    }

    new void OnDisable()
    {
        animator.SetBool("HoldShield", false);
        base.OnDisable();
    }

    private void Update()
    {
        jobTime -= Time.deltaTime;

        if (jobTime <= 0)
        {
            this.enabled = false;
        }
    }
}

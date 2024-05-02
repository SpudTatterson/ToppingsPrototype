using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Worker
{
    new void OnEnable()
    {
        animator.SetBool("HoldShield", true);
        Debug.Log("test");
        base.OnEnable();
    }

    new void OnDisable()
    {
        animator.SetBool("HoldShield", false);
        base.OnDisable();
    }
}

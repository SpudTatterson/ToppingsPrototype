using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    public GameObject baseLog;
    public GameObject slightlyDamaged;
    public GameObject damaged;
    public GameObject badlyDamaged;

    public int logHealth;

    private void Update()
    {
        SetLogGraphics(baseLog, 76, 100);
        SetLogGraphics(slightlyDamaged, 51, 75);
        SetLogGraphics(damaged, 26, 50);
        SetLogGraphics(badlyDamaged, 1, 25);
    }

    private void SetLogGraphics(GameObject logType, int minHP, int maxHP)
    {
        if(logHealth >= minHP && logHealth <= maxHP) 
        {
            logType.SetActive(true);
        }
        else
        {
            logType.SetActive(false);
        }
    }
}

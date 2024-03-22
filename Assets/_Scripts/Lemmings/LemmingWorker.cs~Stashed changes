using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemmingWorker : MonoBehaviour
{
    [SerializeField] Vector3 boxCastSize;

    [SerializeField] GameObject basicOutfit;
    [SerializeField] GameObject woodCutterOutfit;

    [HideInInspector] public bool woodCutter;

    private void Update()
    {
        WoodCutter();
    }

    public void SetLemmingToBasic()
    {
        basicOutfit.SetActive(true);
        woodCutterOutfit.SetActive(false);
    }

    public void SetWorkerOutfit()
    {
        basicOutfit.SetActive(false);
        if (woodCutter) { woodCutterOutfit.SetActive(true); }
    }

    private void WoodCutter()
    {
        if (woodCutter)
        {
            Ray ray = new Ray(transform.position + new Vector3(0, 1f, 0), transform.forward);
            RaycastHit hit;
            Physics.BoxCast(transform.position + new Vector3(0, 1f, 0), boxCastSize * 2f, transform.forward, out hit);
        }
    }
}

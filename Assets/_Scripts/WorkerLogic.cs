using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class WorkerLogic : MonoBehaviour
{


    [SerializeField] private GameObject basicOutfit;
    [SerializeField] private GameObject woodCutterOutfit;

    [HideInInspector] public bool woodCutter;
    [HideInInspector] private LemmingMovement movementScript;

    private void Start()
    {
        movementScript = GetComponent<LemmingMovement>();
    }

    private void Update()
    {
        WoodCutterLogic();
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

    private void WoodCutterLogic()
    {
        if (woodCutter)
        {
            Physics.BoxCast(transform.localPosition + transform.up, new Vector3(.4f, 1f, .4f), transform.forward, out RaycastHit hit, transform.localRotation, 1.5f);
            if (hit.collider != null && hit.collider.TryGetComponent(out Log logScript))
            {
                movementScript.walking = false;
                // Insert cutting log logic here.
            }
            else
            {
                movementScript.walking = true;
            }
        }
    }
}

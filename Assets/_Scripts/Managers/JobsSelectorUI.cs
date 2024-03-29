using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JobsSelectorUI : MonoBehaviour
{    
    [SerializeField] Button woodCutter;
    [SerializeField] Button shieldLemming;
    [SerializeField] LayerMask lemmingLayer;

    private bool woodCutterUI;
    private bool ShieldLemmingUI;

    EventSystem currentUI;
    RaycastHit hit;

    private void Start()
    {
        currentUI = EventSystem.current;
    }

    private void Update()
    {
        if (currentUI.currentSelectedGameObject != null)
        {
            if (currentUI.currentSelectedGameObject.name == woodCutter.name)
            {
                woodCutterUI = true;
                ShieldLemmingUI = false;
            }

            if (currentUI.currentSelectedGameObject.name == shieldLemming.name)
            {
                ShieldLemmingUI = true;
                woodCutterUI = false;
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, Mathf.Infinity, lemmingLayer);

        if (hit.collider == null) return;
        if (woodCutterUI)
        {
            if (hit.collider.TryGetComponent(out WorkerLogic workerLogicScript) && Input.GetKeyDown(KeyCode.Mouse0) && workerLogicScript.basicOutfit.activeSelf)
            {
                workerLogicScript.woodCutter = true;
                workerLogicScript.SetWorkerOutfit();
                woodCutterUI = false;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && !hit.collider.CompareTag("Lemming"))
            {
                woodCutterUI = false;
            }
        }

        if (ShieldLemmingUI)
        {
            if (hit.collider.TryGetComponent(out WorkerLogic workerLogicScript) && Input.GetKeyDown(KeyCode.Mouse0) && workerLogicScript.basicOutfit.activeSelf)
            {
                workerLogicScript.shieldLemming = true;
                workerLogicScript.SetWorkerOutfit();
                ShieldLemmingUI = false;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && !hit.collider.CompareTag("Lemming"))
            {
                ShieldLemmingUI = false;
            }
        }
    }
}
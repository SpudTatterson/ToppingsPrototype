using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JobsSelectorUI : MonoBehaviour
{
    [SerializeField] Button woodCutter;
    //[SerializeField] Button paratrooper;
    [SerializeField] Button shieldLemming;

    private bool woodCutterUI;
    //private bool paratrooperUI;
    private bool ShieldLemmingUI;

    EventSystem currentUI;
    RaycastHit hit;

    private void Start()
    {
        currentUI = EventSystem.current;
    }

    private void Update()
    {
        if (currentUI.currentSelectedGameObject == null) return;

        if (currentUI.currentSelectedGameObject != null)
        {
            if (currentUI.currentSelectedGameObject.name == woodCutter.name)
            {
                woodCutterUI = true;
                //paratrooperUI = false;
                ShieldLemmingUI = false;
            }

/*            if (currentUI.currentSelectedGameObject.name == paratrooper.name)
            {
                paratrooperUI = true;
                woodCutterUI = false;
                ShieldLemmingUI = false;
            }*/

            if (currentUI.currentSelectedGameObject.name == shieldLemming.name)
            {
                ShieldLemmingUI = true;
                //paratrooperUI = false;
                woodCutterUI = false;
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);       
        Physics.Raycast(ray, out hit);

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

/*        if (paratrooperUI)
        {
            if (hit.collider.TryGetComponent(out WorkerLogic workerLogicScript) && Input.GetKeyDown(KeyCode.Mouse0) && workerLogicScript.basicOutfit.activeSelf)
            {
                workerLogicScript.paratrooper = true;
                workerLogicScript.SetWorkerOutfit();
                paratrooperUI = false;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && !hit.collider.CompareTag("Lemming"))
            {
                paratrooperUI = false;
            }
        }*/

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JobsSelectorUI : MonoBehaviour
{
    [SerializeField] Button woodCutter;

    private bool woodCutterSelected;

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject.name == woodCutter.name)
            {
                woodCutterSelected = true;
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        if (woodCutterSelected)
        {
            if (hit.collider != null && hit.collider.CompareTag("Lemming") && Input.GetKeyDown(KeyCode.Mouse0))
            {
                hit.collider.GetComponent<WorkerLogic>().woodCutter = true;
                hit.collider.GetComponent<WorkerLogic>().SetWorkerOutfit();
                woodCutterSelected = false;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && !hit.collider.CompareTag("Lemming"))
            {
                woodCutterSelected = false;
            }
        }
    }
}

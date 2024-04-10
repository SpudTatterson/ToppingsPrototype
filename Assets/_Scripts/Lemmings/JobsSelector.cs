using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobsSelector : MonoBehaviour
{
    [SerializeField] LayerMask minionLayer;
    bool toggle = false;
    WorkerType type;
    void Update()
    {
        if (!toggle) return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Reset();
            Debug.Log("cancel");
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, minionLayer, QueryTriggerInteraction.Ignore) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (hit.transform.gameObject.GetComponentInChildren<Worker>().CheckIfAlreadyWorking()) return;
            Debug.Log("working");
            if (type == WorkerType.WoodWorker)
            {
                hit.transform.gameObject.GetComponentInChildren<WoodWorker>().enabled = true;
                Reset();
            }

            if (type == WorkerType.Shield)
            {
                hit.transform.gameObject.GetComponentInChildren<Shield>().enabled = true;
                Reset();
            }
        }


    }

    private void Reset()
    {
        toggle = false;
        type = WorkerType.Default;
    }

    public void SetWoodWorker()
    {
        toggle = true;
        type = WorkerType.WoodWorker;
    }
    public void SetShieldWorker()
    {
        toggle = true;
        type = WorkerType.Shield;
    }
}

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
            Worker worker = hit.transform.gameObject.GetComponentInChildren<Worker>();
            if (worker.CheckIfAlreadyWorking()) return;
            Debug.Log("working");
            
            List<Worker> workers = worker.GetWorkers();
            foreach (Worker workScript in workers)
            {
                if(workScript.workerType == type)
                    {
                        workScript.enabled = true;
                        Reset();
                        return;
                    }

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

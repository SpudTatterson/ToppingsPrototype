using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public enum WorkerType
{
    Default,
    WoodWorker,
    Shield,
    ParaTrooper
}


public class Worker : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] VisualEffect smokeVFX;
    [SerializeField] GameObject clothSet;
    [SerializeField] GameObject defaultClothSet;
    public WorkerType workerType;
    void OnEnable()
    {
        SetClothing();
    }
    public bool CheckIfAlreadyWorking()
    {
        GameObject parent = transform.parent.gameObject;
        List<Worker> workerList = parent.GetComponentsInChildren<Worker>().ToList<Worker>();
        workerList.Remove(this);
        foreach (Worker worker in workerList)
        {
            if(worker.enabled == true) return true;
        }
        return false;
    }

    public virtual void WorkerLogic()
    {

    }
        void Update()
    {
        WorkerLogic();
    }
    void OnDisable()
    {
        Finish();
    }
    void SetClothing()
    {
        smokeVFX.Play();
        clothSet.SetActive(true);
        defaultClothSet.SetActive(false);
    }
    void Finish()
    {
        smokeVFX.Play();
        clothSet.SetActive(false);
        defaultClothSet.SetActive(true);
    }

}

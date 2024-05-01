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


public abstract class Worker : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] VisualEffect smokeVFX;
    [SerializeField] ClothingSet clothingSet;
    [SerializeField] GameObject defaultClothSet;
    public WorkerType workerType;

    MinionCustomizer minionCustomizer;
    protected Animator animator;

    void Awake()
    {
        minionCustomizer = GetComponentInParent<MinionCustomizer>();
        animator = minionCustomizer.GetComponentInChildren<Animator>();
    }
    protected void OnEnable()
    {
        SetClothing();
    }
    public bool CheckIfAlreadyWorking()
    {
        List<Worker> workerList = GetWorkers();
        foreach (Worker worker in workerList)
        {
            if (worker.enabled == true) return true;
        }
        return false;
    }

    public List<Worker> GetWorkers()
    {
        GameObject parent = transform.parent.gameObject;
        List<Worker> workerList = parent.GetComponentsInChildren<Worker>().ToList<Worker>();
        return workerList;
    }

    public virtual void WorkerLogic()
    {

    }
    void Update()
    {
        WorkerLogic();
    }
    protected void OnDisable()
    {
        Finish();
    }
    void SetClothing()
    {
        smokeVFX.Play();
        minionCustomizer.UpdateClothing(clothingSet);
    }
    void Finish()
    {
        smokeVFX.Play();
        minionCustomizer.UpdateClothing(MinionManager.instance.GetDefaultClothing());
    }

}

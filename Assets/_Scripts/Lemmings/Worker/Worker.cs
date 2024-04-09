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
    public virtual void Work()
    {

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

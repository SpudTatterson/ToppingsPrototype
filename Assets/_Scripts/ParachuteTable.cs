using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachuteTable : Placeable
{
    public bool unlimitedParachutes;
    public List<GameObject> parachutes = new List<GameObject>();

    private void Update()
    {
        if (parachutes.Count <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GiveParachute(other);
    }

    private void GiveParachute(Collider other)
    {
        if (other.TryGetComponent(out WorkerLogic workerLogic) && parachutes.Count > 0)
        {
            if (workerLogic.paratrooper == true) return;
            workerLogic.paratrooper = true;
            workerLogic.SetWorkerOutfit();

            if (unlimitedParachutes) return;
            RemoveParachute();
        }
    }

    private void RemoveParachute()
    {
        if(parachutes.Count > 0)
        {
            var index = parachutes.Count - 1;
            parachutes[index].SetActive(false);
            parachutes.RemoveAt(index);
        }
    }
}

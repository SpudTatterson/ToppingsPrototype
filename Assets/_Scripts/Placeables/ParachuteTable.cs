using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachuteTable : Placeable
{

    [SerializeField] bool unlimitedParachutes;
    [SerializeField] string minionTag = "Lemming";
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
        if (other.CompareTag(minionTag))
            GiveParachute(other);
    }

    private void GiveParachute(Collider other)
    {
        Paratrooper paratrooper = other.GetComponentInChildren<Paratrooper>();
        if (paratrooper && parachutes.Count > 0)
        {
            if (unlimitedParachutes) return;
            RemoveParachute();
        }

    }

    private void RemoveParachute()
    {
        if (parachutes.Count > 0)
        {
            var index = parachutes.Count - 1;
            parachutes[index].SetActive(false);
            parachutes.RemoveAt(index);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Fan : MonoBehaviour
{
    [Header("Fan Settings")]
    [Space]
    [Tooltip("How strong the fan will be")]
    [SerializeField] float fanForce;
    [SerializeField] LayerMask ignoreThis;

    public List<GameObject> lemmings = new List<GameObject>();

    private void Start()
    {
        ignoreThis = ~ignoreThis;
    }

    private void Update()
    {
        foreach (var lemming in lemmings)
        {
            if (lemming == null) lemmings.Remove(lemming);
        }
    }

    private void FixedUpdate()
    {
        PushLemming();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Lemming"))
        {
            lemmings.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Lemming"))
        {
            lemmings.Remove(other.gameObject);
        }
    }

    private void PushLemming()
    {
        for (int i = 0; i < lemmings.Count; i++)
        {
            var rb = lemmings[i].GetComponent<Rigidbody>();

            Physics.Raycast(lemmings[i].transform.position + new Vector3(0, 1f, 0), -transform.forward, out RaycastHit hit, 10f, ignoreThis, QueryTriggerInteraction.Ignore);
            Debug.DrawRay(lemmings[i].transform.position + new Vector3(0, 1f, 0), -transform.forward * hit.distance, Color.red);

            if (hit.collider != null && hit.collider.gameObject.GetComponentInParent<Fan>() != null)
            {
                rb.AddForce(transform.forward * (fanForce / hit.distance) * Time.fixedDeltaTime);
            }
        }
    }
}

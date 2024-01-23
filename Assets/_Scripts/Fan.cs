using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Fan : MonoBehaviour
{
    [Header("Fan Settings")]
    [Space]
    [Tooltip("How strong the fan will be")]
    public float fanForce;

    List<GameObject> lemmings = new List<GameObject>();

    private void FixedUpdate()
    {
        for (int i = 0; i < lemmings.Count; i++)
        {
            var rb = lemmings[i].GetComponent<Rigidbody>();
            RaycastHit hit;

            Physics.Raycast(lemmings[i].transform.position, -transform.forward, out hit, 10f);
            Debug.DrawRay(lemmings[i].transform.position, -transform.forward * 10, Color.red);

            if (hit.collider.tag.Equals("Fan"))
            {
                rb.AddForce(transform.forward * (fanForce / hit.distance) * Time.deltaTime);
            }
        }
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
}

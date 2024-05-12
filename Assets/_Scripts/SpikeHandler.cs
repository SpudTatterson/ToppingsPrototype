using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHandler : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 impaledPosition;
    private Quaternion impaledRotation;
    private bool isImpaled;

    private LemmingHealth lemmingHealth;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lemmingHealth = GetComponentInParent<LemmingHealth>();
    }

    private void FixedUpdate()
    {
        if (isImpaled)
        {
            rb.MovePosition(impaledPosition);
            rb.MoveRotation(impaledRotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (lemmingHealth.dead)
        {
            if (other.GetComponent<SpikeTrap>())
            {
                rb.isKinematic = true;
                impaledPosition = rb.position;
                impaledRotation = rb.rotation;
                isImpaled = true;
            }
        }
    }
}

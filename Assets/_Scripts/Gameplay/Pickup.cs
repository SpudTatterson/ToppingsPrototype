using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] string playerTag = "Lemming";
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerTag))
        {
            PickUp();
        }
    }

    void PickUp()
    {
        // do stuff that needs to happen on picking up

        Destroy(gameObject);
    }
}

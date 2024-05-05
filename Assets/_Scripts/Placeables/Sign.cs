using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sign : Placeable
{
    private List<GameObject> lemmings = new List<GameObject>();
    private string lemmingTag = "Lemming";

    private void Update()
    {
        SetLemmingRotation();
    }

    private void OnTriggerEnter(Collider other)
    {
        TurnLemming(other);
    }

    private void TurnLemming(Collider other)
    {
        if (other.gameObject.CompareTag(lemmingTag))
        {
            var lemmingScript = other.gameObject.GetComponent<LemmingMovement>();

            lemmingScript.ResetTurnStats();

            Vector3 signDirection = transform.forward;
            Quaternion targetRotation = Quaternion.LookRotation(signDirection);

            lemmingScript.startRotationTest = lemmingScript.transform.rotation;
            lemmingScript.targetRotationTest = targetRotation;

            lemmings.Add(other.gameObject); // Adds lemming to a list so i can still access him after the collision
        }
    }

    private void SetLemmingRotation()
    {
        for (int i = 0; i < lemmings.Count; i++)
        {
            var lemmingScript = lemmings[i].GetComponent<LemmingMovement>();

            if (lemmingScript.turnComplete >= 1)
            {
                lemmings.RemoveAt(i);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignNew : MonoBehaviour
{
    private List<GameObject> lemmings = new List<GameObject>();

    private void Update()
    {
        SetLemmingRotation();
    }

    private void OnCollisionEnter(Collision collision)
    {
        KnockbackLemming(collision);
    }

    private void KnockbackLemming(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lemming"))
        {
            var lemmingScript = collision.gameObject.GetComponent<LemmingMovement>();

            lemmingScript.Knockback();

            Vector3 signDirection = transform.forward;
            Quaternion targetRotation = Quaternion.LookRotation(signDirection);

            lemmingScript.startRotationTest = lemmingScript.transform.rotation;
            lemmingScript.targetRotationTest = targetRotation;

            lemmings.Add(collision.gameObject); // Adds lemming to a list so i can still access him after the collision
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

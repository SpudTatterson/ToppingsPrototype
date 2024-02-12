using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    private float degree;

    List<GameObject> lemmings = new List<GameObject>();

    private void Start()
    {
        var collider = GetComponentInChildren<Collider>();

        if (collider.CompareTag("Right_Sign"))
        {
            degree = 90f;
        }

        if (collider.CompareTag("Left_Sign"))
        {
            degree = -90f;
        }
    }

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
            lemmings.Add(collision.gameObject); // Adds lemming to a list so i can still access him after the collision
        }
    }

    private void SetLemmingRotation()
    {
        for (int i = 0; i < lemmings.Count; i++)
        {
            var lemmingScript = lemmings[i].GetComponent<LemmingMovement>();
            lemmingScript.startRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
            lemmingScript.endRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + degree, transform.localEulerAngles.z);

            if (degree == 90f)
                lemmingScript.rotateRight = true;

            if (degree == -90f)
                lemmingScript.rotateLeft = true;

            if (lemmingScript.turnSpeedSide >= 1)
            {
                lemmings.RemoveAt(i);
            }
        }
    }
}
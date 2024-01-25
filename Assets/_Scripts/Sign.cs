using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    private bool right;
    private bool left;
    private float degree;

    List<GameObject> lemmings = new List<GameObject>();

    private void Start()
    {
        var collider = GetComponentInChildren<Collider>();

        if (collider.CompareTag("Right_Sign"))
        {
            right = true;
            degree = 90f;
        }

        if (collider.CompareTag("Left_Sign"))
        {
            left = true;
            degree = -90f;
        }
    }

    private void Update()
    {
        for (int i = 0; i < lemmings.Count; i++)
        {
            var lemmingScript = lemmings[i].GetComponent<LemmingMovement>();
            lemmingScript.startRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
            lemmingScript.endRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + degree, transform.localEulerAngles.z);

            if (right)
                lemmingScript.rotateRight = true;

            if (left)
                lemmingScript.rotateLeft = true;

            if (lemmingScript.turnSpeedSide >= 1)
                lemmings.RemoveAt(i);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lemming"))
        {
            KnockbackLemming(collision);
        }
    }

    private void KnockbackLemming(Collision collision)
    {
        var lemmingScript = collision.gameObject.GetComponent<LemmingMovement>();
        lemmingScript.Knockback();
        lemmings.Add(collision.gameObject);
    }
}
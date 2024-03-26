using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SignNew : MonoBehaviour
{
    public float direction;
    private float degree;
    private int trigger;

    List<GameObject> lemmings = new List<GameObject>();

    private void Start()
    {
        print(transform.rotation.eulerAngles);
        this.direction = transform.rotation.y;
        degree = 90f;
        trigger = 0;
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

            if (trigger == 0)
            {
            lemmingScript.startRotation = new Vector3(lemmingScript.transform.eulerAngles.x, lemmingScript.transform.eulerAngles.y, lemmingScript.transform.eulerAngles.z);
            lemmingScript.endRotation = new Vector3(transform.eulerAngles.x, direction, transform.eulerAngles.z);
            trigger++;
            }

            if (degree == 90f)
                lemmingScript.rotateRight = true;

            if (lemmingScript.turnSpeedSide >= 1)
            {
                lemmings.RemoveAt(i);
            }
        }
    }
}

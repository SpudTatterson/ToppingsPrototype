using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysics : MonoBehaviour
{
    [HideInInspector] public bool reflectRealistic;
    [HideInInspector] public bool reflectBackToShooter;
    [HideInInspector] public float bulletDamage;
    [HideInInspector] public float bulletSpeed;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hit);
        //Debug.DrawRay(transform.position, transform.forward * 2, Color.red);

        if (hit.collider == null) return;
        if (hit.collider.TryGetComponent(out LemmingHealth lemmingHealthScript) && hit.distance <= (bulletSpeed / 200))
        {
            lemmingHealthScript.health -= bulletDamage;
            Destroy(gameObject);
        }
        else if (hit.collider.CompareTag("Shield") && hit.distance <= (bulletSpeed / 200))
        {
            if (reflectRealistic)
            {
                var rb = gameObject.GetComponent<Rigidbody>();
                var direction = Vector3.Reflect(transform.forward, hit.normal);               
                transform.forward = direction;
                transform.position = hit.point + (transform.forward * 0.1f);               
                rb.velocity = Vector3.zero;
                rb.AddForce(transform.forward * bulletSpeed);
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.red; // This here is for testing only - remove if not needed
            }
            else if (reflectBackToShooter)
            {
                var rb = gameObject.GetComponent<Rigidbody>();
                var direction = startPosition - transform.position;
                transform.forward = direction;
                transform.position = hit.point + (transform.forward * 0.1f);
                rb.velocity = Vector3.zero;
                rb.AddForce(transform.forward * bulletSpeed);
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.red; // This here is for testing only - remove if not needed
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider)
        {
            Destroy(gameObject);
        }
    }
}

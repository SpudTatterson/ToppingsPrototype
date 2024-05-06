using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class BulletPhysics : MonoBehaviour
{
    [HideInInspector] public bool reflectRealistic;
    [HideInInspector] public bool reflectBackToShooter;
    [HideInInspector] public float bulletDamage;
    [HideInInspector] public float bulletSpeed;

    [SerializeField] public LayerMask seeThrough;

    private string shieldTag = "Shield";
    private Vector3 startPosition;
    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private RaycastHit hit;

    private void Start()
    {
        startPosition = transform.position;         // Gets the spawn position of the bullet
        previousPosition = transform.position;      // Gets the previous position
        seeThrough = ~seeThrough;                 // Takes the inputed layer and inverts it
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        currentPosition = transform.position;

        var distance = Vector3.Distance(previousPosition, currentPosition);

        var directionFromPreviousBullet = currentPosition - previousPosition;
        Ray ray = new Ray(previousPosition, directionFromPreviousBullet);
        Physics.Raycast(ray, out hit, distance, seeThrough);

        if (hit.collider == null)
        {
            var directionToPreviousBullet = previousPosition - currentPosition;
            ray = new Ray(currentPosition, directionToPreviousBullet);
            Physics.Raycast(ray, out hit, distance, seeThrough);
        }

        previousPosition = currentPosition;

        if (hit.collider == null) return;

        if (hit.collider.TryGetComponent(out LemmingHealth lemmingHealth))
        {
            if (lemmingHealth.health <= bulletDamage)
            {
                lemmingHealth.deathBullet = true;
                lemmingHealth.bulletForce = gameObject.GetComponent<Rigidbody>().velocity;
                lemmingHealth.bulletPos = gameObject.transform.position;
            }
            lemmingHealth.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }

        if (hit.collider.CompareTag(shieldTag))
        {
            ReflectBullet();
        }

        if (hit.collider.TryGetComponent(out Turret turret))
        {
            turret.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Turret turret))
        {
            turret.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag(shieldTag))
        {
            ReflectBullet();
        }
        else if (collision.collider.TryGetComponent(out LemmingHealth lemmingHealth))
        {
            lemmingHealth.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
        else if (collision.collider)
        {
            Destroy(gameObject);
        }
    }

    private void ReflectBullet()
    {
        if (reflectRealistic)
        {
            var direction = Vector3.Reflect(transform.forward, hit.normal);
            var position = hit.point - (transform.forward * .2f);
            var cloneBullet = Instantiate(gameObject, position, Quaternion.LookRotation(direction));
            cloneBullet.GetComponent<BulletPhysics>().seeThrough = ~seeThrough;
            Destroy(cloneBullet, 5f);
            Destroy(gameObject);
        }
        else if (reflectBackToShooter)
        {
            var direction = startPosition - transform.position;
            var position = hit.point - (transform.forward * .2f);
            var cloneBullet = Instantiate(gameObject, position, Quaternion.LookRotation(direction));
            cloneBullet.GetComponent<BulletPhysics>().seeThrough = ~seeThrough;
            Destroy(cloneBullet, 5f);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

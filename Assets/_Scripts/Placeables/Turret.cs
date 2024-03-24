using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class Turret : MonoBehaviour
{
    [SerializeField] private AudioClip[] BulletsSoundClips;
    [Header("Basic Settings")]
    [SerializeField] LayerMask lemmingLayer;
    [SerializeField] Transform turretHead;
    [SerializeField] GameObject lightBulb;
    [SerializeField] VisualEffect bulletSmoke;
    [SerializeField] float turretHealth;

    [Header("Turret Idle Status Settings")]
    [SerializeField] float radiusOfScan;
    [SerializeField] float rotationSpeed;
    [SerializeField] float scanCooldown;

    [Header("Turret Aiming Settings")]
    [SerializeField] float detectionRadius;
    [SerializeField] float aimRotationSpeed;
    [SerializeField] float delayToShoot;

    [Header("Turret Shooting Settings")]
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] LayerMask ignoreLayers;
    [SerializeField] float timeBetweenShots;
    [SerializeField] float bulletDamage;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletMass;
    [SerializeField] bool reflectRealistic;
    [SerializeField] bool reflectBackToShooter;
    [SerializeField][Range(0, .5f)] float bulletDispersion;
    [SerializeField][Range(0, 20f)] float accuracy;

    private GameObject targettedLemming;
    private bool randomBool;
    private bool shoot;
    private float lerpTime;
    private float timerToAim;
    private float timerToIdle;
    private float shotCooldown;
    private Quaternion lastIdlePos; 

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(turretHead.position, detectionRadius);
    }

    private void Start()
    {
        lerpTime = Random.Range(0, rotationSpeed);                            // Sets the starting rotation the turret wil be in.
        scanCooldown = Random.Range(0f, 0.15f);                               // Sets a random delay time between rotations.
        randomBool = Random.value > 0.5f;                                     // Sets the starting side that the turret will rotate towards. 50/50
        lastIdlePos = turretHead.rotation;
        timerToIdle = 1f;
        bullet.GetComponent<BulletPhysics>().bulletDamage = bulletDamage;
        bullet.GetComponent<BulletPhysics>().bulletSpeed = bulletSpeed;
        bullet.GetComponent<Rigidbody>().mass = bulletMass;
        bullet.GetComponent<BulletPhysics>().reflectRealistic = reflectRealistic;
        bullet.GetComponent<BulletPhysics>().reflectBackToShooter = reflectBackToShooter;
    }

    private void Update()
    {
        Spotting();

        if (targettedLemming != null)
        {
            AimAtTarget();
        }
        else
        {
            Idle();
        }

        if(turretHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Shoot();       
    }

    public void TakeDamage(float damage)
    {
        turretHealth -= damage;
    }

    private void Idle()
    {
        shoot = false;

        if (lastIdlePos != turretHead.rotation)
        {
            StatusLightColor(Color.yellow);
            turretHead.rotation = Quaternion.RotateTowards(turretHead.rotation, lastIdlePos, aimRotationSpeed * Time.deltaTime);
            timerToIdle = 0f;
            return;
        }

        timerToIdle += Time.deltaTime;
        if (timerToIdle < .5f) return;
        float percentageComplete = lerpTime / rotationSpeed;

        if (randomBool)
        {
            lerpTime += Time.deltaTime;
        }
        else if (!randomBool)
        {
            lerpTime -= Time.deltaTime;
        }

        if (percentageComplete >= 1f + scanCooldown)
        {
            randomBool = false;
        }
        else if (percentageComplete <= 0f - scanCooldown)
        {
            randomBool = true;
        }

        Vector3 startRot = new Vector3(0, radiusOfScan / 2, 0);
        Vector3 endRot = new Vector3(0, -radiusOfScan / 2, 0);

        StatusLightColor(Color.green);

        turretHead.localEulerAngles = Vector3.Lerp(startRot, endRot, Mathf.SmoothStep(0, 1, percentageComplete)); // Turns turret head with smoothing.
        lastIdlePos = turretHead.rotation;
        timerToAim = 0;
        shotCooldown = timeBetweenShots;
    }

    private void Spotting()
    {
        var lemmingColliders = Physics.OverlapSphere(transform.position, detectionRadius, lemmingLayer); // List of the lemming colliders that are in the spotting area.
        float closestDistance = Mathf.Infinity;
        GameObject closestLemming = null;

        for (int i = 0; i < lemmingColliders.Length; i++)
        {
            var lookDirection = (lemmingColliders[i].transform.position + Vector3.up) - turretHead.position;
            Physics.Raycast(new Ray(turretHead.position, lookDirection), out RaycastHit hit, Mathf.Infinity, ~ignoreLayers);

            if (hit.collider == null) continue;

            if (hit.collider == lemmingColliders[i])
            {
                float distanceToLemming = hit.distance;
                if(distanceToLemming < closestDistance)
                {
                    closestDistance = distanceToLemming;
                    closestLemming = lemmingColliders[i].gameObject;
                }
            }
        }

        if (targettedLemming == null && closestLemming != null) targettedLemming = closestLemming;
        if (targettedLemming == null) return;

        var direction = (targettedLemming.transform.position + Vector3.up) - turretHead.position;
        Physics.Raycast(new Ray(turretHead.position, direction), out RaycastHit hitLemming, Mathf.Infinity, ~ignoreLayers);

        if (targettedLemming != null && !lemmingColliders.Contains(targettedLemming.GetComponent<Collider>())) targettedLemming = null;
        if (targettedLemming != null && hitLemming.collider != targettedLemming.GetComponent<Collider>()) targettedLemming = null;
    }

    private void AimAtTarget()
    {
        timerToAim += Time.deltaTime;

        var predictedPos = predictedPosition(targettedLemming, turretHead, bullet, bulletSpeed);
        var desiredLookDirection = targettedLemming.transform.position - turretHead.position;       // The direction the turret will aim

        Physics.Raycast(new Ray(turretHead.position, desiredLookDirection), out RaycastHit hit, Mathf.Infinity, ~ignoreLayers);    // Shoots a ray in the direction of the target and stores collider data

        if (hit.collider == null) return;

        if (!shoot) StatusLightColor(Color.yellow);

        if (hit.collider.gameObject == targettedLemming && timerToAim > delayToShoot)
        {
            Quaternion RotateToTarget = Quaternion.LookRotation(predictedPos);
            turretHead.rotation = Quaternion.RotateTowards(turretHead.rotation, RotateToTarget, aimRotationSpeed * Time.deltaTime);
            turretHead.eulerAngles = new Vector3(0, turretHead.eulerAngles.y, 0);                       // Clamps the turret so it will only rotate on the Y axis

            var gap = RotateToTarget.eulerAngles.y - turretHead.eulerAngles.y;
            if (gap < 7 && gap > -7) shoot = true;
        }
    }

    private void Shoot()
    {
        if (shoot)
        {
            StatusLightColor(Color.red);
            shotCooldown += Time.deltaTime;
            
            if (shotCooldown >= timeBetweenShots)
            {               
                var cloneBullet = Instantiate(bullet, bulletSpawnPoint.position + PositionAccuracy(bulletDispersion), bulletSpawnPoint.rotation * RotationAccuracy(accuracy));
                cloneBullet.GetComponent<Rigidbody>().AddForce(cloneBullet.transform.forward * bulletSpeed * Time.fixedDeltaTime);
                Destroy(cloneBullet, 5f);
                MuzzleFlash();
                bulletSmoke.Play();
                shotCooldown = 0;
                SoundsFXManager.instance.PlayRandomSoundFXClip(BulletsSoundClips, transform, 1f);
            }
        }
    }

    private void StatusLightColor(Color color)
    {
        var renderer = lightBulb.GetComponent<Renderer>();
        renderer.material.SetColor("_BaseColor", color);
        //renderer.material.SetColor("_EmissionColor", color);
    }

    private Vector3 predictedPosition(GameObject target, Transform shooter, GameObject projectile, float bulletSpeed)
    {
        float magnitude = (bulletSpeed / projectile.GetComponent<Rigidbody>().mass) * Time.fixedDeltaTime;      // Calculates the magnitude of the projectile before it is being shot.

        var targetDirection = target.transform.position - shooter.position;                                     // Calculates the direction from the shooter to the target.
        Physics.Raycast(new Ray(shooter.position, targetDirection), out RaycastHit hit);                        // Shoots a ray to the target and stores collider data.
        float flightTime = hit.distance / magnitude;                                                            // Calculates the time it will take the bullet to reach its target.

        var predictedOffset = target.GetComponent<Rigidbody>().velocity * flightTime;                           // Calculates how much the target will move until the bullet reaches the target.
        var predictedPos = target.transform.position + predictedOffset;                                         // Calculates the position the target will be in with bullet flight time.
        var lookDirection = predictedPos - shooter.position;                                                    // Calculates the direction the shooter will look at.

        return lookDirection;                                                                                   // Returns the predicted target direction to shoot at.
    }

    private Vector3 PositionAccuracy(float accuracy)
    {
        float x = Random.Range(-accuracy, accuracy);
        float y = Random.Range(-accuracy, accuracy);
        float z = Random.Range(-accuracy, accuracy);
        var offset = new Vector3(x, y, z);

        return offset;
    }

    private Quaternion RotationAccuracy(float accuracy)
    {        
        var quaternionOffset = Quaternion.Euler(PositionAccuracy(accuracy));

        return quaternionOffset;
    }

    private void MuzzleFlash()
    {
        if (muzzleFlash.activeSelf == false)
        {
            muzzleFlash.SetActive(true);
            StartCoroutine(FlashEnd(muzzleFlash));
        }
        else
        {
            muzzleFlash.SetActive(false);
        }
    }

    IEnumerator FlashEnd(GameObject gameObject)
    {
        yield return new WaitForSeconds(.02f);
        gameObject.SetActive(false);
    }
}

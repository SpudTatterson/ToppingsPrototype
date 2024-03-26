using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class JumpPad : Placeable
{
    [SerializeField] private AudioClip[] JumpSoundClips;
    [SerializeField] Transform endPoint;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float speed = 20f;

    [Header("Display Controls")]
    [SerializeField] [Range(10, 100)] int LinePoints = 25;
    [SerializeField] [Range(0.01f, 0.25f)] float TimeBetweenPoints = 0.1f;
    [SerializeField] LayerMask groundLayer;
    Vector3 launchForce;
    void OnTriggerEnter(Collider other)
    {
        Launch(other.attachedRigidbody);
    }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        DrawProjection();
    }
    public override void SecondaryPlacement()
    {
        PlacementManager manager = FindObjectOfType<PlacementManager>();
        manager.SetNewItemToPlace(secondaryPlacable);

    }
    void Launch(Rigidbody rb)
    {
        SoundsFXManager.instance.PlayRandomSoundFXClip(JumpSoundClips, transform, 1f);
        rb.velocity = Vector3.zero;
        launchForce = CalculateLaunchVector();
        rb.AddForce(launchForce, ForceMode.VelocityChange);
    }
    private Vector3 CalculateLaunchVector()
    {
        Vector3 toTarget = endPoint.position - transform.position;

        // Set up the terms we need to solve the quadratic equations.
        float gSquared = Physics.gravity.sqrMagnitude;
        float b = speed * speed + Vector3.Dot(toTarget, Physics.gravity);
        float discriminant = b * b - gSquared * toTarget.sqrMagnitude;

        // Check whether the target is reachable at max speed or less.
        if (discriminant < 0)
        {
            // Target is too far away to hit at this speed.
            // Abort, or fire at max speed in its general direction?
            
            Debug.Log("too far");
            return new Vector3(0,0,1) + transform.position;
        }

        float discRoot = Mathf.Sqrt(discriminant);

        // Highest shot with the given max speed:
        float T_max = Mathf.Sqrt((b + discRoot) * 2f / gSquared);

        // Most direct shot with the given max speed:
        float T_min = Mathf.Sqrt((b - discRoot) * 2f / gSquared);

        float T = (T_max + T_min)/2;

        // Convert from time-to-hit to a launch velocity:

        Vector3 velocity = toTarget / T - Physics.gravity * T / 2f;

        return velocity;

    }

     private void DrawProjection()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
        Vector3 startPosition = transform.position;
        Vector3 startVelocity = CalculateLaunchVector();
        int i = 0;
        lineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < LinePoints; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;// calculates the x and z of the next position
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time); // offsets y of the point using kinematic equation 

            lineRenderer.SetPosition(i, point);

            Vector3 lastPosition = lineRenderer.GetPosition(i - 1);

            Vector3 direction = point - lastPosition;

            if(Physics.Raycast(lastPosition, direction.normalized, out RaycastHit hit, direction.magnitude, groundLayer))
            {
                lineRenderer.SetPosition(i, hit.point);
                lineRenderer.positionCount = i + 1;
                return;
            }
        }
    }
}

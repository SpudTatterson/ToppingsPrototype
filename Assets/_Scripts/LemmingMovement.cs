using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemmingMovement : MonoBehaviour
{
    [Header("LemmingSettings")]
    [Space]
    [Tooltip("How fast the lemming will reach his max speed")]
    public float accelerationSpeed;
    [Tooltip("How far the lemming will be knocked back when hitting an obstacle")]
    public float knockbackPower;
    [Tooltip("The max speed of the lemming")]
    public float maxWalkSpeed;
    [Tooltip("How fast the rotation in any direction will be in seconds")]
    public float RotationTime;
    [Tooltip("Insert here the tags that the lemming will hit and rotate 180 degrees back")]
    public string[] collidingObjects;

    private Rigidbody rb;
    [HideInInspector] public float knockbackTimer;
    [HideInInspector] public float turnSpeedSide;
    [HideInInspector] public bool walking;
    [HideInInspector] public bool rotateBack, rotateLeft, rotateRight;
    [HideInInspector] public Vector3 startRotation;
    [HideInInspector] public Vector3 endRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        walking = true;
    }

    private void Update()
    {
        knockbackTimer += Time.deltaTime;

        float turnSpeedBack = knockbackTimer / RotationTime;
        turnSpeedSide = knockbackTimer / (RotationTime / 2f);

        if (rotateBack)
        {
            transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, turnSpeedBack);
            if(turnSpeedBack >= 1)
            {
                walking = true;
                rotateBack = false;
            }
        }

        if (rotateRight)
        {
            transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, turnSpeedSide);
            if (turnSpeedSide >= 1)
            {
                walking = true;
                rotateRight = false;
            }
        }

        if (rotateLeft)
        {
            transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, turnSpeedSide);
            if (turnSpeedSide >= 1)
            {
                walking = true;
                rotateLeft = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxWalkSpeed && walking)
        {
            rb.AddRelativeForce(Vector3.forward * accelerationSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        for (int i = 0; i < collidingObjects.Length; i++)
        {
            if(other.collider.tag == collidingObjects[i])
            {
                Knockback();
                startRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
                endRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + 180f, transform.localEulerAngles.z);
                rotateBack = true;
            }
        }
    }

    public void Knockback()
    {
        walking = false;
        rb.AddRelativeForce(new Vector3(0, 0, -knockbackPower));
        knockbackTimer = 0;
    }
}

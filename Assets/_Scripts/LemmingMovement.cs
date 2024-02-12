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
    [Tooltip("Insert here the tags of the obstacles that the lemming will hit and rotate 180 degrees back")]
    public string[] collidingObjects;

    public Vector3 groundedOffset = new Vector3 (0,0.1f,0);
    public float maxDistanceOffGround = 0.2f;

    private Rigidbody rb;
    private float rotationTimer, delayVelocityCheck;

    [HideInInspector] public float turnSpeedSide;
    [HideInInspector] public bool walking;
    [HideInInspector] public bool rotateBack, rotateLeft, rotateRight;
    [HideInInspector] public Vector3 startRotation;
    [HideInInspector] public Vector3 endRotation;
    bool isGrounded;
    

    public Vector3 boxCastSize;
    private RaycastHit hit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        walking = true;
    }

    private void Update()
    {
        isGrounded = GroundCheck();

        rb.drag = isGrounded ? 1 : 0;

        LemmingRotation();
        knockbackTimer += Time.deltaTime;

        Physics.BoxCast(transform.localPosition + transform.up, boxCastSize, transform.forward, out hit, transform.localRotation, 1);
    }

    private void FixedUpdate()
    {
        if ((rb.velocity.magnitude < maxWalkSpeed) && walking && isGrounded)
        {
            rb.AddRelativeForce(Vector3.forward * accelerationSpeed * Time.fixedDeltaTime);
        }
    }

    bool GroundCheck()
    {
        Vector3 offset = transform.position + groundedOffset;
        return Physics.Raycast(offset, Vector3.down, maxDistanceOffGround);
    }
    private void OnCollisionEnter(Collision other)
    private void OnCollisionEnter(Collision collision)
    {
        TurnBackOnCollision(collision);
    }

    public void Knockback()
    {
        /* add a knockback effect relative to the facing direction og the Lemming */

        walking = false;
        rb.velocity = Vector3.zero;
        rb.AddRelativeForce(new Vector3(0, 0, -knockbackPower));
        rotationTimer = 0;
        delayVelocityCheck = 0; //This is here to make the rotation work better (Temporary untill I find a better solution)
    }

    private void TurnBackOnCollision(Collision collision)
    {
        /* Sets the current position and the position the Lemming will rotate to,
         This happens only if the lemming is colliding with the correctly tagged objects */        

        for (int i = 0; i < collidingObjects.Length; i++)
        {
            if (collision.collider.tag == collidingObjects[i] && hit.collider.name == collision.collider.name)
            {
                Knockback();
                startRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
                endRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + 180f, transform.localEulerAngles.z);
                rotateBack = true;
            }
        }
    }

    private void LemmingRotation()
    {
        /* Rotating the lemming after the knockback effect and only after the lemming
         has stopped moving */
         
        delayVelocityCheck += Time.deltaTime;
        turnSpeedSide = rotationTimer / RotationTime;

        RotationLogic(rotateBack);
        RotationLogic(rotateLeft);
        RotationLogic(rotateRight);

        if (walking)
        {
            rotateBack = false;
            rotateRight = false;
            rotateLeft = false;
        }
    }
    void OnDrawGizmos()
    {
        Vector3 offset = transform.position + groundedOffset;
        Gizmos.DrawRay(offset, Vector3.down * maxDistanceOffGround);
    }

    private void RotationLogic(bool direction)
    {
        if (direction & rb.velocity == Vector3.zero && delayVelocityCheck > 0.1f)
        {
            rotationTimer += Time.deltaTime;
            transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, turnSpeedSide);
            if (turnSpeedSide >= 1)
            {
                walking = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LemmingMovement : MonoBehaviour
{
    public float accelerationSpeed;
    public float knockbackPower;
    public float maxWalkSpeed;
    public float RotationTime;
    public string[] collidingObjects;
    public LayerMask ignoreThis;

    public VisualEffect groundStomp;
    private float stompTimer;

    public float force;

    public Vector3 groundedOffset = new Vector3(0, 0.1f, 0);
    public float maxDistanceOffGround = 0.2f;

    private Rigidbody rb;
    public float rotationTimer, delayVelocityCheck;

    [HideInInspector] public float turnSpeedSide;
    [HideInInspector] public bool walking;
    [HideInInspector] public bool rotateBack, rotateLeft, rotateRight;
    public Vector3 startRotation;
    public Vector3 endRotation;
    public bool isGrounded;
    public bool climbStairs;

    public Vector3 boxCastSize;
    public RaycastHit[] allHits;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        walking = true;
        stompTimer = 1;
        ignoreThis = ~ignoreThis;
    }

    private void Update()
    {
        if (isGrounded)
        {
            if(stompTimer == 0)
            {
                groundStomp.Play();
            }
            stompTimer += Time.time;
        }
        else
        {
            stompTimer = 0;
        }

        isGrounded = GroundCheck();

        rb.drag = isGrounded ? 1 : 0;

        LemmingRotation();
        Climb();

        allHits = Physics.BoxCastAll(transform.localPosition + transform.up, boxCastSize, transform.forward, transform.localRotation, 1, ignoreThis);
    }

    private void FixedUpdate()
    {
        if ((rb.velocity.magnitude < maxWalkSpeed) && walking && isGrounded && !climbStairs)
        {
            rb.AddRelativeForce(Vector3.forward * accelerationSpeed * Time.fixedDeltaTime);
        }

        if (climbStairs)
        {
            rb.AddForce(transform.up * force * Time.fixedDeltaTime);
            if ((rb.velocity.magnitude < maxWalkSpeed) && walking)
            {
                rb.AddRelativeForce(Vector3.forward * accelerationSpeed * Time.fixedDeltaTime);
            }
        }
    }

    bool GroundCheck()
    {
        Vector3 offset = transform.position + groundedOffset;
        return Physics.Raycast(offset, Vector3.down, maxDistanceOffGround);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TurnBackOnCollision(collision);
    }

    private void Climb()
    {
        Ray ray = new Ray(transform.position + new Vector3(0,0.1f,0), transform.forward);
        RaycastHit hitStair;
        Physics.Raycast(ray, out hitStair, 0.5f);

        if (hitStair.collider == null)
        {
            climbStairs = false;
            return;
        }

        if (hitStair.collider.CompareTag("Stair"))
        {
            climbStairs = true;
        }
        else
        {
            climbStairs = false;
        }
    }

    public void Knockback()
    {
        /* Adds a knockback effect relative to the facing direction og the Lemming */

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

        bool tagged = false;

        for (int i = 0; i < collidingObjects.Length; i++)
        {
            if (collision.collider.CompareTag(collidingObjects[i]))
            {
                tagged = true;
                break;
            }
        }

        if (allHits == null) return;

        for (int i = 0; i < allHits.Length; i++) 
        {
            if(collision.collider == allHits[i].collider && tagged)
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

    public void RotationLogicTest(Vector3 direction)
    {
        if (rb.velocity == Vector3.zero && delayVelocityCheck > 0.1f)
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

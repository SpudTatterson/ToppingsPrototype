using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class LemmingMovement : MonoBehaviour
{
    public float accelerationSpeed;
    public float maxWalkSpeed;
    public float climbForce;

    public float knockbackPower;
    public float RotationTime;
    public string[] collidingObjects;

    public LayerMask ignoreThis;
    [SerializeField] LayerMask groundLayer;
    public VisualEffect groundStomp;


    [SerializeField] Vector3 groundedOffset = new Vector3(0, 0.1f, 0);
    [SerializeField] float maxDistanceOffGround = 0.2f;
    [SerializeField] float stairCheckDistance = 0.7f;

    private float stompTimer;
    private Rigidbody rb;

    private float rotationTimer;
    private float delayVelocityCheck;
    [HideInInspector] public float turnComplete;

     public bool walking, turning;
    [HideInInspector] public bool climbStairs;
    [HideInInspector] public bool knockable;
    [HideInInspector] public bool isGrounded;

    public Vector3 boxCastSize;
    private RaycastHit[] allHits;
    [SerializeField] Animator animator;

    [HideInInspector] public Quaternion targetRotationTest;
    [HideInInspector] public Quaternion startRotationTest;

    // ------ DEPRECATED ------ //
    [HideInInspector] public Vector3 startRotation;
    [HideInInspector] public Vector3 endRotation;
    // ------ DEPRECATED ------ //
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        walking = true;
        knockable = true;
        stompTimer = 1;
        ignoreThis = ~ignoreThis;
    }

    private void Update()
    {
        if (isGrounded)
        {
            if (stompTimer == 0)
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


        animator.SetBool("Walking", walking);
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("UsingStairs", climbStairs);
        animator.SetBool("Turning", turning);

        LemmingRotation();
        TriggerRotationLogic();
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
            rb.AddForce(transform.up * climbForce * Time.fixedDeltaTime);
            if ((rb.velocity.magnitude < maxWalkSpeed) && walking)
            {
                rb.AddRelativeForce(Vector3.forward * accelerationSpeed * Time.fixedDeltaTime);
            }
        }
    }

    bool GroundCheck()
    {
        Vector3 offset = transform.position + groundedOffset;
        return Physics.Raycast(offset, Vector3.down, maxDistanceOffGround, groundLayer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TurnBackOnCollision(collision);
    }

    private void Climb()
    {
        Ray ray = new Ray(transform.position + groundedOffset, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * stairCheckDistance);
        RaycastHit hitStair;
        Physics.Raycast(ray, out hitStair, stairCheckDistance, groundLayer);

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
        knockable = false;
        walking = false;
        rb.velocity = Vector3.zero;
        rb.AddRelativeForce(new Vector3(0, 0, -knockbackPower));
        rotationTimer = 0;
        delayVelocityCheck = 0; //This is here to make the rotation work better (Temporary untill I find a better solution)
        ResetTurnStats();
    }

    public void ResetTurnStats()
    {
        walking = false;
        turning = true;
        rotationTimer = 0;
        delayVelocityCheck = 0;
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

        if (!tagged) return;
        if (allHits == null) return;

        for (int i = 0; i < allHits.Length; i++)
        {
            if (collision.collider == allHits[i].collider)
            {
                Knockback();
                startRotationTest = transform.rotation;
                Vector3 targetDirection = -transform.forward;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                targetRotationTest = targetRotation;
            }
        }
    }

    private void LemmingRotation()
    {
        /* Rotating the lemming after the knockback effect and only after the lemming
         has stopped moving */

        delayVelocityCheck += Time.deltaTime;
        turnComplete = rotationTimer / RotationTime;
        RotationLogic();
    }

    public void RotationLogic()
    {
        if (rb.velocity == Vector3.zero && delayVelocityCheck > 0.1f && !knockable)
        {
            rotationTimer += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotationTest, targetRotationTest, Mathf.SmoothStep(0, 1, turnComplete));
            if (turnComplete >= 1)
            {
                walking = true;
                knockable = true;
            }
        }
    }

    public void TriggerRotationLogic()
    {

        if (/*rb.velocity == Vector3.zero*//* &&*/ delayVelocityCheck > .5f & turning)
        {
            rotationTimer += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotationTest, targetRotationTest, Mathf.SmoothStep(0, 1, turnComplete));
        }

        if (turnComplete >= 1)
        {
            walking = true;
            turning = false;
        }
    }

    public void StopMovement()
    {
        rb.velocity = Vector3.zero;
    }
    void OnDrawGizmos()
    {
        Vector3 offset = transform.position + groundedOffset;
        Gizmos.DrawRay(offset, Vector3.down * maxDistanceOffGround);
    }
}

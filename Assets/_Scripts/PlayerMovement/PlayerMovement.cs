using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    float moveSpeed = 7f;
    Vector3 moveDir;

    [SerializeField] float walkSpeed = 7f;
    [SerializeField] float runSpeed = 10f;

    [SerializeField] float maxSlopeAngle = 40f;
    RaycastHit slopeHit;

    [SerializeField] float groundDrag = 5f;
    [SerializeField] float airDrag = 0.1f;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air
    }


    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpCoolDown = 0.2f;
    [SerializeField] float airMultiplier = 0.5f;
    bool canJump = true;

    [Header("Misc Settings")]
    [SerializeField] float rotationSpeed = 7f;
    [SerializeField] float customGravity = -9.8f;


    [Header("Ground Check")]
    [SerializeField] float groundOffset = 2f;
    [SerializeField] float groundedRadius;
    [SerializeField] LayerMask whatIsGround;
    bool isGrounded;

    [Header("Key Bindings")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("References")]
    [SerializeField] GameObject cam;
    Rigidbody rb;
    [SerializeField] Transform orientation;
    [SerializeField] CameraController cameraController;
    Animator animator;

    [Header("Inputs")]
    float horizInput;
    float verticalInput;



    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        if (!cameraController)
            cameraController = FindObjectOfType<CameraController>();
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        rb.AddForce(new Vector3(0, customGravity, 0));
        isGrounded = GroundCheck();

        GetInput();
        SpeedControl();
        RotatePlayer();
        //MoveHead();
        StateHandler();

        rb.drag = isGrounded ? groundDrag : airDrag;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }
    void GetInput()
    {
        horizInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && canJump && isGrounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }
    }
    void StateHandler()
    {
        animator.SetBool("Grounded", isGrounded);

        if (isGrounded && Input.GetKey(sprintKey))
        {
            animator.SetFloat("Speed", rb.velocity.magnitude);
            state = MovementState.sprinting;
            moveSpeed = runSpeed;
        }

        else if (isGrounded)
        {
            state = MovementState.walking;

            animator.SetFloat("Speed", rb.velocity.magnitude);
            moveSpeed = walkSpeed;
        }

        else
        {
            state = MovementState.air;
        }
    }
    // void MoveHead()
    // {
    //     Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
    //     Vector3 targetPosition;
    //     Vector3 targetDirection;
    //     Vector3 pointAlongRay = ray.origin + ray.direction * 20;
    //     targetPosition = VectorUtility.FlattenVector(pointAlongRay, headConstraint.position.y);
    //     targetDirection = VectorUtility.GetDirection(transform.position, targetPosition);
    //     float angle = Vector3.Angle(transform.forward, targetDirection);
    //     if (angle < 150) headConstraint.position = targetPosition;
    //     else
    //     {
    //         managers.ikRig.SwitchHeadAimRigSource();
    //     }

    // }
    void MovePlayer()
    {
        Vector3 inputDir = new Vector3(horizInput, 0, verticalInput);

        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        camForward.y = 0;
        camForward.Normalize();
        camRight.y = 0;
        camRight.Normalize();

        moveDir = inputDir.z * camForward + inputDir.x * camRight;

        if (moveDir != Vector3.zero) orientation.transform.forward = moveDir;

        Debug.DrawRay(transform.position + transform.up * 1, moveDir);

        if((moveDir != Vector3.zero || !isGrounded) && !cameraController.IsUsingFollowTarget())
            cameraController.ResetCamera();

        rb.useGravity = !OnSlope();

        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 5, ForceMode.Force);
            return;
        }

        if (isGrounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!isGrounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);


    }
    public void OnLand()
    {

    }
    public void OnFootstep()
    {

    }

    void Jump()
    {
        rb.velocity = VectorUtility.FlattenVector(rb.velocity);
        animator.SetBool("Jump", !canJump);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    void ResetJump()
    {
        canJump = true;
        animator.SetBool("Jump", !canJump);
    }
    void SpeedControl()
    {
        if (OnSlope())
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = VectorUtility.FlattenVector(rb.velocity);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }
    bool GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundOffset,
        transform.position.z);

        return Physics.CheckSphere(spherePosition, groundedRadius, whatIsGround, QueryTriggerInteraction.Ignore);
    }
    void RotatePlayer()
    {
        if (moveDir != Vector3.zero)
            transform.forward = Vector3.Slerp(transform.forward, moveDir.normalized, Time.deltaTime * rotationSpeed);
    }
    bool OnSlope()
    {
        if (!isGrounded) return false;
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 4f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal);
    }
    public Vector3 GiveMoveDir()
    {
        return moveDir;
    }
    public bool GiveGrounded()
    {
        return isGrounded;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundOffset,
        transform.position.z);
        Gizmos.DrawSphere(spherePosition, groundedRadius);
    }
}

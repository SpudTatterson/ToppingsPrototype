using UnityEngine;
using NaughtyAttributes;
using UnityEditor.Rendering;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] Vector2 minMaxDistance = new Vector2(10, 40);
    [SerializeField, DisableIf("useFollowTarget")] float speed = 0.1f;
    [SerializeField] float rotationIncrement = 45;
    [SerializeField] bool useFollowTarget = true;
    [SerializeField] float CamResetSpeed = 1f;
    [Header("Drag Settings")]
    [SerializeField] KeyCode dragToMoveCameraKeyCode = KeyCode.Mouse2;
    [SerializeField]float dragSmoothing = 2;
    [SerializeField] LayerMask draggableLayers;

    [Header("References")]
    [SerializeField] Transform cameraPivot;
    [SerializeField] Transform cameraHolder;
    [SerializeField] Camera cam;
    [SerializeField, EnableIf("useFollowTarget")] Transform followTarget;

    [Header("Inputs")]
    float mouseScroll;
    float horiz;
    float vert;
    [Header("Private Variables")]
    Vector3 dragOrigin;
    Vector3 dragDiff;
    bool isDragging;

    void Update()
    {
        GetInput();

        ScrollToZoom();
        DragToMove();
        Movement();
        Rotation();
    }
    void LateUpdate()
{
    if (isDragging)
    {
        // Calculate target position for smoother movement
        Vector3 targetPosition = dragOrigin - dragDiff;
        // Use Lerp or MoveTowards for smooth transition
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * dragSmoothing);
    }
}

    void Movement()
    {
        if (!useFollowTarget)
        {
            Vector3 movementDirection = new Vector3(vert, 0, -horiz);
            cameraPivot.Translate(movementDirection.normalized * speed);
        }
        else
        cameraPivot.position = followTarget.position;

    }
    void DragToMove()
    {
        if (Input.GetKey(dragToMoveCameraKeyCode))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray.origin, ray.direction, out hit, float.MaxValue, draggableLayers);
            if (!isDragging)
            {
                dragOrigin = hit.point;
                isDragging = true;
                useFollowTarget = false;
            }
            dragDiff = hit.point - transform.position;
        }
        else if (Input.GetKeyUp(dragToMoveCameraKeyCode))
        {
           isDragging = false; 
        }
            
    }
    void Rotation()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Quaternion targetRotation = Quaternion.Euler(cameraPivot.rotation.eulerAngles + new Vector3(0, rotationIncrement, 0));
            cameraPivot.rotation = targetRotation;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Quaternion targetRotation = Quaternion.Euler(cameraPivot.rotation.eulerAngles - new Vector3(0, rotationIncrement, 0));
            cameraPivot.rotation = targetRotation;
        }
    }

    void GetInput()
    {
        mouseScroll = Input.mouseScrollDelta.y;
        horiz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");
    }

    void ScrollToZoom()
    {
        float distance = CheckDistance();

        if (mouseScroll > 0 && distance > minMaxDistance.x)
        {
            cameraHolder.position = cameraHolder.position + cam.transform.forward;
        }
        else if (mouseScroll < 0 && distance < minMaxDistance.y)
        {
            cameraHolder.position = cameraHolder.position - cam.transform.forward;
        }
    }
    float CheckDistance()
    {
        return Vector3.Distance(cameraHolder.position, cameraPivot.position);
    }
    private IEnumerator ResetCameraCoroutine()
    {
        float elapsedTime = 0;
        Vector3 startingPosition = cameraPivot.position;
        while (elapsedTime < CamResetSpeed)
        {
            cameraPivot.position = Vector3.Lerp(startingPosition, followTarget.position, (elapsedTime / CamResetSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraPivot.position = followTarget.position; // Ensure it's exactly at the follow target's position
        isDragging = false;
        useFollowTarget = true;
    }
    public void ResetCamera()
    {
        StartCoroutine(ResetCameraCoroutine());
    }
    public bool IsUsingFollowTarget()
    {
        return useFollowTarget;
    }
}

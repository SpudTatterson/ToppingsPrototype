using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Vector2 minMaxDistance = new Vector2(10, 40);
    [SerializeField] float speed = 0.1f;
    [SerializeField] float rotationIncrement = 45;

    [Header("References")]
    [SerializeField] Transform cameraPivot;
    [SerializeField] Transform cameraHolder;
    [SerializeField] Camera cam;

    [Header("Inputs")]
    float mouseScroll;
    float horiz;
    float vert;


    void Update()
    {
        GetInput();

        ScrollToZoom();
        Movement();
        Rotation();
    }

    void Movement()
    {
        Vector3 movementDirection = new Vector3(vert, 0, -horiz);
        cameraPivot.Translate(movementDirection.normalized * speed);
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
}

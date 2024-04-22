using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToRotate : MonoBehaviour
{
    [SerializeField] LayerMask draggableLayers;
    [SerializeField] KeyCode dragKeyCode = KeyCode.Mouse0;
    [SerializeField] Transform objectToRotate;
    [SerializeField] float dragRotationSpeed;

    Vector3 lastMousePosition;
    bool isRotating = false;

    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetKeyDown(dragKeyCode) && Physics.Raycast(ray, out hit, 100f, draggableLayers))
        {
            lastMousePosition = Input.mousePosition;
            isRotating = true;
            Debug.Log("test1");
        }

        if (Input.GetKey(dragKeyCode) && isRotating)
        {
            Debug.Log("test2");
            // Calculate the difference in position
            Vector3 dragDiff = Input.mousePosition - lastMousePosition;

            // Apply rotation
            objectToRotate.Rotate(Vector3.up, -dragDiff.x * dragRotationSpeed * Time.deltaTime, Space.World);

            // Update lastMousePosition for the next frame
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetKeyUp(dragKeyCode))
        {
            Debug.Log("test3");
            isRotating = false;
        }
    }
}

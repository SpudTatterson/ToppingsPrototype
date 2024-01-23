using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemmingMovement : MonoBehaviour
{
    public Rigidbody rb;

    public float walkSpeed;
    public float rotationSpeed;
    public float knockbackPower;
    public float maxWalkSpeed;
    public float knockbackTimer;

    public bool walking;
    public bool rotatingRight;
    public bool rotatingLeft;
    public bool rotatingBack;

    public string[] collidingObjects;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        walking = true;
    }

    private void Update()
    {
        knockbackTimer += Time.deltaTime;

        if (rb.velocity.magnitude == 0 && knockbackTimer > 0.05)
        {
            if (rotatingRight)
            {
                transform.Rotate(Vector3.up, 90);
                walking = true;
                rotatingRight = false;
            }

            if (rotatingLeft)
            {
                transform.Rotate(Vector3.up, -90);
                walking = true;
                rotatingLeft = false;
            }

            if (rotatingBack)
            {
                transform.Rotate(Vector3.up, 180);
                walking = true;
                rotatingBack = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxWalkSpeed && walking)
        {
            rb.AddRelativeForce(Vector3.forward * walkSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag == "Right_Sign")
        {
            walking = false;
            rb.AddRelativeForce(new Vector3(0, 0, -knockbackPower));
            knockbackTimer = 0;
            rotatingRight = true;
        }

        if (other.collider.tag == "Left_Sign")
        {
            walking = false;
            rb.AddRelativeForce(new Vector3(0, 0, -knockbackPower));
            knockbackTimer = 0;
            rotatingLeft = true;
        }

        for(int i = 0; i < collidingObjects.Length; i++)
        {
            if(other.collider.tag == collidingObjects[i])
            {
                walking = false;
                rb.AddRelativeForce(new Vector3(0, 0, -knockbackPower));
                knockbackTimer = 0;
                rotatingBack = true;
            }
        }
    }
}

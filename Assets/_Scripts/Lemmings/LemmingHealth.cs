using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LemmingHealth : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float velocityForDeath;

    public float currentVelocity;
    private LemmingMovement lemmingMovement;

    private void Start()
    {
        lemmingMovement = GetComponent<LemmingMovement>();
        velocityForDeath = -velocityForDeath;
    }

    private void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }

        DeathOnFall();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void DeathOnFall()
    {
        var rb = GetComponent<Rigidbody>();

        if (lemmingMovement.isGrounded)
        {
            if(currentVelocity < velocityForDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentVelocity = 0;
            }
        }

        if (!lemmingMovement.isGrounded)
        {
            currentVelocity = rb.velocity.y;
        }
    }
}

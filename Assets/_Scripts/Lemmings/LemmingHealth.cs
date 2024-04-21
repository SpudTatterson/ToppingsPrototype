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

    public bool usingJumpPad = false;

    private void Start()
    {
        MinionManager.instance.Add(this.gameObject);
        lemmingMovement = GetComponent<LemmingMovement>();
        velocityForDeath = -velocityForDeath;
    }

    private void Update()
    {
        if (health <= 0)
        {
            Death();
        }

        DeathOnFall();
    }

    private void Death()
    {
        MinionManager.instance.Remove(this.gameObject);
        Destroy(gameObject);
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
            if (currentVelocity < velocityForDeath)
            {
                if(usingJumpPad) 
                {
                    usingJumpPad = false;
                    currentVelocity = 0;
                    return;
                }
                Death();
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

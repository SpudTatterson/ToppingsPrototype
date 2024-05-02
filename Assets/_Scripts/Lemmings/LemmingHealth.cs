using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LemmingHealth : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float velocityForDeath;
    [SerializeField] float timeToDestroy = 4;
    bool dead = false;

    public float currentVelocity;
    private LemmingMovement lemmingMovement;
    Animator animator;

    public bool usingJumpPad = false;

    private void Start()
    {
        MinionManager.instance.Add(this.gameObject);
        lemmingMovement = GetComponent<LemmingMovement>();
        animator = GetComponentInChildren<Animator>();
        velocityForDeath = -velocityForDeath;
    }

    private void Update()
    {
        if (health <= 0 && !dead)
        {
            Death();
        }

        DeathOnFall();
    }

    private void Death()
    {
        MinionManager.instance.Remove(this.gameObject);
        animator.SetTrigger("Death");
        lemmingMovement.walking = false;
        dead = true;
        Destroy(gameObject, timeToDestroy); 
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

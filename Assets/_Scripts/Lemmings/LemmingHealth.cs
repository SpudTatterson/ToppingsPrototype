using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LemmingHealth : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float velocityForDeath;

    public float currentVelocity;
    private LemmingMovement lemmingMovement;

    public bool usingJumpPad = false;
    public bool dead = false;

    List<Collider> ragdollParts = new List<Collider>();
    public Animator rigAnimator;

    void Awake()
    {
        MinionManager.instance.Add(this);
        SetRagdollParts();
    }
    private void Start()
    {
        lemmingMovement = GetComponent<LemmingMovement>();
        velocityForDeath = -velocityForDeath;
    }

    private void Update()
    {
        RestrictRagdoll();

        if (health <= 0)
        {
            Death();
        }

        FallCheck();
    }

    private void Death()
    {
        dead = true;
        ActivateRagdoll();
        MinionManager.instance.Remove(this);
    }

    public void ActivateRagdoll()
    {
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        lemmingMovement.enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
        rigAnimator.enabled = false;

        foreach (Collider collider in ragdollParts)
        {
            //collider.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collider.isTrigger = false;
        }
    }

    private void RestrictRagdoll()
    {
        if (!dead)
        {
            foreach (Collider collider in ragdollParts)
            {
                collider.gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity;
            }
        }
    }

    private void SetRagdollParts()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != this.gameObject)
            {
                collider.isTrigger = true;
                ragdollParts.Add(collider);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void FallCheck()

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

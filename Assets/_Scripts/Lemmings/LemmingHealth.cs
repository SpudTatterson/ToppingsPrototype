using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LemmingHealth : MonoBehaviour
{
    [SerializeField] public float health;
    [SerializeField] private float velocityForDeath;
    [SerializeField] float timeToDestroy = 4;
    bool dead = false;

    [HideInInspector] public bool deathBullet;
    [HideInInspector] public Vector3 bulletForce;
    [HideInInspector] public Vector3 bulletPos;

    public float currentVelocity;
    private LemmingMovement lemmingMovement;
    Animator animator;

    public bool usingJumpPad = false;

    List<Collider> ragdollParts = new List<Collider>();
    public Animator rigAnimator;

    void Awake()
    {
        SetRagdollParts();
    }
    private void Start()
    {
        MinionManager.instance.Add(this.gameObject);
        lemmingMovement = GetComponent<LemmingMovement>();
        animator = GetComponentInChildren<Animator>();
        velocityForDeath = -velocityForDeath;
    }

    private void Update()
    {
        RestrictRagdoll();

        if (health <= 0 && !dead)
        {
            Death();
        }

        FallCheck();
    }

    private void Death()
    {
        MinionManager.instance.Remove(this.gameObject);
        animator.SetTrigger("Death");
        lemmingMovement.walking = false;
        dead = true;
        Destroy(gameObject, timeToDestroy); 
        ActivateRagdoll();
    }

    public void ActivateRagdoll()
    {
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        lemmingMovement.enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
        rigAnimator.enabled = false;

        foreach (Collider collider in ragdollParts)
        {
            collider.isTrigger = false;
        }
    }

    private void RestrictRagdoll()
    {
        if (deathBullet)
        {
            foreach (Collider collider in ragdollParts)
            {
                collider.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(bulletForce * 60, bulletPos);
            }
            deathBullet = false;
        }
        else if (!dead)
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

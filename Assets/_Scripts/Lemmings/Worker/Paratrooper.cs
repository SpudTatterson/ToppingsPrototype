using UnityEngine;

public class Paratrooper : Worker
{
    [Header("Worker Settings")]
    [SerializeField] float parachuteFloat = 2f;
    [SerializeField][Range(1.01f, 1.1f)] private float velocityStop = 1.01f;
    LemmingMovement movement;
    float timer;
    void Start()
    {
        movement = GetComponentInParent<LemmingMovement>();

    }
    public override void WorkerLogic()
    {
        
        var rb = GetComponentInParent<Rigidbody>();

        if (movement.isGrounded == false && rb.velocity.y < -parachuteFloat)
        {
            timer += Time.fixedDeltaTime;
            rb.velocity = rb.velocity / velocityStop;
            Vector3 newVelocity = rb.velocity;
            newVelocity.y = -parachuteFloat;
            if (rb.velocity.y < -parachuteFloat) { rb.velocity = newVelocity; }
            // Insert parachute animation here.
        }
        else if (movement.isGrounded == true && timer > 0)
        {
            timer = 0;
            this.enabled = false;
        }
    }
}

using UnityEngine;

public class Paratrooper : Worker
{
    [Header("Worker Settings")]
    [SerializeField] float parachuteFloat = 2f;
    [SerializeField][Range(1.01f, 1.1f)] private float velocityStop = 1.01f;
    [SerializeField] GameObject parachute;
    LemmingMovement movement;
    float timer;
    GameObject parachuteGO;
    BoneContainer bones;
    void Start()
    {
        movement = GetComponentInParent<LemmingMovement>();
        bones = movement.transform.GetComponent<BoneContainer>();
    }
    public override void WorkerLogic()
    {
        var rb = GetComponentInParent<Rigidbody>();

        if (movement.isGrounded == false && rb.velocity.y < -parachuteFloat)
        {
            if(!parachuteGO)
            {
                parachuteGO = Instantiate(parachute, bones.back.position, Quaternion.identity, bones.back);
                parachuteGO.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
                
            timer += Time.fixedDeltaTime;
            rb.velocity = rb.velocity / velocityStop;
            Vector3 newVelocity = rb.velocity;
            newVelocity.y = -parachuteFloat;
            if (rb.velocity.y < -parachuteFloat) { rb.velocity = newVelocity; }
            // Insert parachute animation here.
        }
        else if (movement.isGrounded == true && timer > 0)
        {
            Destroy(parachuteGO);
            timer = 0;
            this.enabled = false;
        }

        if(movement.isGrounded == true)
        {
            jobTime -= Time.deltaTime;
        }
        else if(jobTime <= 0)
        {
            this.enabled = false;
        }
    }
}

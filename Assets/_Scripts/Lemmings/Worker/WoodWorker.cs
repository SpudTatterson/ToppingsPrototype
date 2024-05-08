using UnityEngine;

public class WoodWorker : Worker
{
    [Header("Work Settings")]
    [SerializeField] LayerMask ignoredLayers;
    [SerializeField] float castRadius = 2f;
    [SerializeField] float interactionDistance = 0.2f;
    [SerializeField] float timeBetweenHits = 1.5f;
    float timer;
    

    [SerializeField] private AudioClip[] ChoppingSoundClips;

    LemmingMovement movement;
    Log logScript;
    RaycastHit hit;
    bool foundLog = false;

    private void Start()
    {
       
        movement = GetComponentInParent<LemmingMovement>();
        ignoredLayers = ~ignoredLayers;
    }
    public override void WorkerLogic()
    {
        //Physics.BoxCast(transform.localPosition + transform.up, new Vector3(.4f, 0.99f, .4f), transform.forward, out RaycastHit hit, transform.localRotation, 1.5f, ignoredLayers);
        if (Physics.SphereCast(transform.position, castRadius, transform.forward, out hit, interactionDistance, ignoredLayers))
        {
            if (logScript == null) foundLog = hit.collider.TryGetComponent(out logScript);
            if (foundLog)
            {
                if (logScript.lemmingCutting == null) logScript.lemmingCutting = gameObject; // initialize 

                if (logScript.lemmingCutting == gameObject)
                {
                    print("cut");
                    movement.walking = false;
                    movement.StopMovement();
                    timer += Time.deltaTime;


                    // Insert cutting log animation here.
                }

                if (timer > timeBetweenHits)
                {
                    timer = 0;
                    logScript.logHealth -= 25;
                    SoundsFXManager.instance.PlayRandomSoundFXClip(ChoppingSoundClips, transform, 1f);
                    if (logScript.logHealth <= 0)
                    {
                        FinishJob();
                    }

                    if (timer > timeBetweenHits)
                    {
                        timer = 0;
                        logScript.logHealth -= 25;
                        SoundsFXManager.instance.PlayRandomSoundFXClip(ChoppingSoundClips, transform, 1f);
                        animator.SetBool("SwingAxe", true);
                        if (logScript.logHealth <= 0)
                        {
                            animator.SetBool("SwingAxe", false);
                            movement.walking = true;
                            this.enabled = false;
                        }
                    }
                }
            }
        }
        if(logScript == null)
        {
            jobTime -= Time.deltaTime;
        }

        if (jobTime <= 0)
        {
            FinishJob();
        }
    }

    private void FinishJob()
    {
        movement.walking = true;
        this.enabled = false;
    }
}

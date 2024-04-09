using UnityEngine;

public class WoodWorker : Worker
{
    [SerializeField] private LayerMask ignoredLayers;
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
    void Update()
    {
        Work();
    }
    public override void Work()
    {
        {
            //Physics.BoxCast(transform.localPosition + transform.up, new Vector3(.4f, 0.99f, .4f), transform.forward, out RaycastHit hit, transform.localRotation, 1.5f, ignoredLayers);
            
            Debug.DrawRay(transform.position, transform.forward * 0.5f);
            if (Physics.SphereCast(transform.position, 2f, transform.forward, out hit, 0.5f, ignoredLayers))
            {
                if(logScript == null) foundLog = hit.collider.TryGetComponent(out logScript);
                if (foundLog)
                {
                    if (logScript.lemmingCutting == null) logScript.lemmingCutting = gameObject; // initialize 

                    if (logScript.lemmingCutting == gameObject)
                    {
                        print("cut");
                        movement.walking = false;
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
                            movement.walking = true;
                            this.enabled = false;
                        }
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.VFX;

public class WorkerLogic : MonoBehaviour
{
    [SerializeField] private AudioClip[] ChopingSoundClips;

    
    [Header("Base Lemming Settings")]
    [Header("Effects")]
    [SerializeField] private VisualEffect smokePoof;

    [Header("Lemming Outfits")]
    [SerializeField] public GameObject basicOutfit;
    [SerializeField] private GameObject woodCutterOutfit;
    [SerializeField] private GameObject shieldLemmingOutfit;
    [SerializeField] private GameObject paratrooperOutfit;

    [Header("Wood Cutter Lemming Settings")]
    [SerializeField] private float hitCooldown;

    [Header("Paratrooper Lemming Settings")]
    [SerializeField] private float parachuteFloat;
    [SerializeField][Range(1.01f, 1.1f)] private float velocityStop;

    [HideInInspector] public bool woodCutter;
    [HideInInspector] public bool paratrooper;
    [HideInInspector] public bool shieldLemming;
    [HideInInspector] private LemmingMovement movementScript;

    float timer;

    private void Start()
    {
        timer = 0;
        movementScript = GetComponent<LemmingMovement>();
    }

    private void Update()
    {
        WoodCutterLogic();
        
    }
    private void FixedUpdate()
    {
        ParatrooperLogic();
    }

    public void SetLemmingToBasic()
    {
        basicOutfit.SetActive(true);
        woodCutterOutfit.SetActive(false);
        paratrooperOutfit.SetActive(false);
        shieldLemmingOutfit.SetActive(false);  
        smokePoof.Play();
    }

    public void SetWorkerOutfit()
    {
        basicOutfit.SetActive(false);
        if (woodCutter) { woodCutterOutfit.SetActive(true); }
        if (paratrooper) { paratrooperOutfit.SetActive(true); }
        if (shieldLemming) { shieldLemmingOutfit.SetActive(true); }
        smokePoof.Play();
    }

    private void WoodCutterLogic()
    {
        if (!woodCutter) return;

        Physics.BoxCast(transform.localPosition + transform.up, new Vector3(.4f, 1f, .4f), transform.forward, out RaycastHit hit, transform.localRotation, 1.5f);
        if (hit.collider == null) return;
        if (hit.collider.TryGetComponent(out Log logScript))
        {
            if(logScript.lemmingCutting == null) { logScript.lemmingCutting = gameObject; }

            if(logScript.lemmingCutting ==  gameObject)
            {
                movementScript.walking = false;
                timer += Time.deltaTime;
                // Insert cutting log animation here.
            }

            if(timer > hitCooldown)
            {
                timer = 0;
                logScript.logHealth -= 25;
                SoundsFXManager.instance.PlayRandomSoundFXClip(ChopingSoundClips, transform, 1f);
                if (logScript.logHealth <= 0)
                {
                    woodCutter = false;
                    movementScript.walking = true;
                    SetLemmingToBasic();
                }
            }
        }

    }

    private void ParatrooperLogic()
    {
        if (!paratrooper) return;
        
        var rb = GetComponent<Rigidbody>();

        if (movementScript.isGrounded == false && rb.velocity.y < -parachuteFloat)
        {
            timer += Time.fixedDeltaTime;
            rb.velocity = rb.velocity / velocityStop;
            Vector3 newVelocity = rb.velocity;
            newVelocity.y = -parachuteFloat;
            if (rb.velocity.y < -parachuteFloat) { rb.velocity = newVelocity; }
            // Insert parachute animation here.
        }
        else if (movementScript.isGrounded == true && timer > 0)
        {
            paratrooper = false;
            SetLemmingToBasic();
        }
        
    }
}

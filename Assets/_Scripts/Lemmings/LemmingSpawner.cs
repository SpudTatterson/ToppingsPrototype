using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemmingSpawner : MonoBehaviour
{
    [SerializeField] private AudioClip[] MinionSoundClips;
    [Header("Spawner Settings")]
    [Space]
    [Tooltip("The object that the spawner will spawn")]
    public GameObject lemming;
    [Tooltip("The position and rotation the object will be spawned")]
    public Transform spawnPoint;
    [Tooltip("The delay between lemming spawns in seconds")]
    public float delayBetweenSpawns;
    [Tooltip("How many lemming to spawn")]
    public int lemmingCount;

    private float timer;

    private void Start()
    {
        timer = delayBetweenSpawns - .5f; // Setting first spawn to be faster and not delayed
    }

    private void Update()
    {
        
        Spawner();
    }

    private void Spawner()
    {
        timer += Time.deltaTime;
        
        if (timer > delayBetweenSpawns && lemmingCount > 0)
        {
            SoundsFXManager.instance.PlayRandomSoundFXClip(MinionSoundClips, transform, 1f);
            Instantiate(lemming, spawnPoint.position, spawnPoint.rotation);
            timer = 0f;
            lemmingCount -= 1;
            
        }
    }
}

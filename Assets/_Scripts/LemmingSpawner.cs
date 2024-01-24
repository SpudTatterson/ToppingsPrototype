using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemmingSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Space]
    [Tooltip("The object that the spawner will spawn")]
    public GameObject lemming;
    [Tooltip("The position and rotation the object will be spawned")]
    public Transform spawnPoint;
    [Tooltip("the delay between lemming spawns")]
    public float delayBetweenSpawns;
    [Tooltip("How many lemming to spawn")]
    public int lemmingCount;

    float timer;

    private void Start()
    {
        timer = delayBetweenSpawns - .5f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > delayBetweenSpawns && lemmingCount > 0)
        {
            Instantiate(lemming, spawnPoint.position, spawnPoint.rotation);
            timer = 0f;
            lemmingCount -= 1;
        }
    }
}

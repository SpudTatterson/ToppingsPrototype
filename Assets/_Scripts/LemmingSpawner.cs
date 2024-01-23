using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemmingSpawner : MonoBehaviour
{
    public GameObject lemming;
    public Transform spawnPoint;

    public float delayBetweenSpawns;
    public int lemmingCount;

    public float timer;

    private void Start()
    {
        timer = delayBetweenSpawns - 1f;
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

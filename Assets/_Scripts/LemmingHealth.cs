using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemmingHealth : MonoBehaviour
{
    [SerializeField] public float health;

    private void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

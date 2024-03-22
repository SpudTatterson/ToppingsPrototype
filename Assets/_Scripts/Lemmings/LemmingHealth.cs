using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemmingHealth : MonoBehaviour
{
    [SerializeField] private float health;

    private void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}

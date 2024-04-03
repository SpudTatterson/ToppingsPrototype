using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private AudioClip[] DeathSoundClips;

    [SerializeField] string playerTag;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            SoundsFXManager.instance.PlayRandomSoundFXClip(DeathSoundClips, transform, 1f);

            Destroy(other.gameObject);
        }
    }
}

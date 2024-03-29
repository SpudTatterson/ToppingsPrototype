using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    const string masterKey = "MasterVolume";
    const string soundFXKey = "SoundFXVolume";
    const string musicKey = "MusicVolume";

    void Start()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(masterKey, Mathf.Log10(1) * 20f));
        SetSoundFXVolume(PlayerPrefs.GetFloat(soundFXKey, Mathf.Log10(1) * 20f));
        SetMusicVolume(PlayerPrefs.GetFloat(musicKey, Mathf.Log10(1) * 20f));
    }
    public void SetMasterVolume(float level)
    {
        Debug.Log(level);
        //  audioMixer.SetFloat("MasterVolume", level);
        audioMixer.SetFloat(masterKey, Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat(masterKey, level);
    }

    public void SetSoundFXVolume(float level)
    {
        // audioMixer.SetFloat("SoundFXVolume", level);
        audioMixer.SetFloat(soundFXKey, Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat(soundFXKey, level);
    }

    public void SetMusicVolume(float level)
    {
        //  audioMixer.SetFloat("MusicVolume", level);
        audioMixer.SetFloat(musicKey, Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat(musicKey, level);
    }
}

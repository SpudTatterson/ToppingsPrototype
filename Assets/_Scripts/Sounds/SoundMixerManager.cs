using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;


    const string masterKey = "MasterVolume";
    const string soundFXKey = "SoundFXVolume";
    const string musicKey = "MusicVolume";

    void Start()
    {
        UIManager.instance.masterVolume.value = PlayerPrefs.GetFloat(masterKey, 1);
        UIManager.instance.soundFXVolume.value = PlayerPrefs.GetFloat(soundFXKey, 1);
        UIManager.instance.musicVolume.value = PlayerPrefs.GetFloat(musicKey, 1);
    }
    public void SetMasterVolume(float level)
    {
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

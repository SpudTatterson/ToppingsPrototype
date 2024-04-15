using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour, ISettingDataPersistence
{
    [SerializeField] AudioMixer audioMixer;


    float masterVolume;
    float fxVolume;
    float musicVolume;

    const string masterKey = "MasterVolume";
    const string soundFXKey = "SoundFXVolume";
    const string musicKey = "MusicVolume";
    public void LoadData(SettingsData gameData)
    {
        masterVolume = gameData.masterVolume;
        fxVolume = gameData.fxVolume;
        musicVolume = gameData.musicVolume;
        Initialize();
    }
    public void SaveData(ref SettingsData gameData)
    {
        gameData.masterVolume = masterVolume;
        gameData.musicVolume = musicVolume;
        gameData.fxVolume = fxVolume;  
    }

    void Initialize()
    {
        UIManager.instance.masterVolume.value = masterVolume;
        UIManager.instance.soundFXVolume.value = fxVolume;
        UIManager.instance.musicVolume.value = musicVolume;
    }

    public void SetMasterVolume(float level)
    {
        //  audioMixer.SetFloat("MasterVolume", level);
        audioMixer.SetFloat(masterKey, Mathf.Log10(level) * 20f);
        masterVolume = level;
    }

    public void SetSoundFXVolume(float level)
    {
        // audioMixer.SetFloat("SoundFXVolume", level);
        audioMixer.SetFloat(soundFXKey, Mathf.Log10(level) * 20f);
        fxVolume = level;
    }

    public void SetMusicVolume(float level)
    {
        //  audioMixer.SetFloat("MusicVolume", level);
        audioMixer.SetFloat(musicKey, Mathf.Log10(level) * 20f);
        musicVolume = level;
    }
}

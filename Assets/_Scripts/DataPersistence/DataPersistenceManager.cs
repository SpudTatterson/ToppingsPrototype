using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager instance { get; private set; }

    [Header("Storage Config")]
    [SerializeField] string settingFileName = "settings.json";
    [SerializeField] string playerDataFileName = "playerData.json";

    // data objects
    SettingsData settingsData;
    GameData playerData;

    List<ISettingDataPersistence> settingsDataPersistenceObjects = new List<ISettingDataPersistence>();
    FileDataHandler<SettingsData> settingDataHandler;

    List<IPlayerDataPersistence> playerDataPersistenceObjects = new List<IPlayerDataPersistence>();
    FileDataHandler<GameData> playerDataHandler;
    void Awake()
    {
        instance = this;

        settingDataHandler = new FileDataHandler<SettingsData>(Application.persistentDataPath, settingFileName);
        settingsDataPersistenceObjects = GetSettingsDataPersistenceObjects();

        playerDataHandler = new FileDataHandler<GameData>(Application.persistentDataPath, playerDataFileName);
        playerDataPersistenceObjects = GetPlayerDataPersistenceObjects();

        LoadGame();
    }
    public void NewGame()
    {
        if (settingsData == null)
        {
            Debug.Log("settings file missing");
            settingsData = new SettingsData();
        }
        if (playerData == null)
        {
            Debug.Log("Game data file missing");
            playerData = new GameData();
        }
    }

    public void SaveGame()
    {
        SaveSettings();

        foreach (IPlayerDataPersistence data in playerDataPersistenceObjects)
        {
            data.SaveData(playerData);
        }

        playerDataHandler.Save(playerData);
    }

    public void SaveSettings()
    {
        foreach (ISettingDataPersistence data in settingsDataPersistenceObjects)
        {
            data.SaveData(settingsData);
        }

        settingDataHandler.Save(settingsData);
    }

    public void LoadGame()
    {
        LoadSettings();

        playerData = playerDataHandler.Load();

        if (playerData == null)
        {
            NewGame();
        }

        foreach (IPlayerDataPersistence item in playerDataPersistenceObjects)
        {
            item.LoadData(playerData);
        }
    }

    public void LoadSettings()
    {
        settingsData = settingDataHandler.Load();

        if (settingsData == null )
        {
            NewGame();
        }
        
        foreach (ISettingDataPersistence item in settingsDataPersistenceObjects)
        {
            item.LoadData(settingsData);
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
    List<ISettingDataPersistence> GetSettingsDataPersistenceObjects()
    {
        List<ISettingDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISettingDataPersistence>().ToList();
        return dataPersistenceObjects;
    }
    List<IPlayerDataPersistence> GetPlayerDataPersistenceObjects()
    {
        List<IPlayerDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerDataPersistence>().ToList();
        return dataPersistenceObjects;
    }
}

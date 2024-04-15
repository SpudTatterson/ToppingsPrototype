using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager instance { get; private set; }

    [Header("Storage Config")]
    [SerializeField] string settingFileName = "settings.json";
    [SerializeField] string levelDataFileName = "playerData";


    SettingsData settingsData;
    List<ISettingDataPersistence> settingsDataPersistenceObjects = new List<ISettingDataPersistence>();
    FileDataHandler settingDataHandler;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        settingDataHandler= new FileDataHandler(Application.persistentDataPath, settingFileName);
        settingsDataPersistenceObjects = GetSettingsDataPersistenceObjects();

        LoadGame();
    }
    public void NewGame()
    {
        settingsData = new SettingsData();
    }

    public void SaveGame()
    {
        foreach (IDataPersistence data in settingsDataPersistenceObjects)
        {
            data.SaveData(ref settingsData);
        }

        settingDataHandler.Save(settingsData);
    }
    public void LoadGame()
    {
        settingsData = settingDataHandler.Load();

        if (settingsData == null)
        {
            Debug.Log("No data starting new game");
            NewGame();
        }

        foreach (IDataPersistence item in settingsDataPersistenceObjects)
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
        List <ISettingDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISettingDataPersistence>().ToList();
        return dataPersistenceObjects;
    }
}

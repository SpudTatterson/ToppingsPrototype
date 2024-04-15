using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager instance { get; private set; }

    [Header("Storage Config")]
    [SerializeField] string fileName;


    GameData gameData;
    List<IDataPersistence> dataPersistenceObjects = new List<IDataPersistence>();
    FileDataHandler dataHandler;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        dataHandler= new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistenceObjects = GetDataPersistenceObjects();

        LoadGame();
    }
    public void NewGame()
    {
        gameData = new GameData();
    }

    public void SaveGame()
    {
        foreach (IDataPersistence data in dataPersistenceObjects)
        {
            data.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }
    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null)
        {
            Debug.Log("No data starting new game");
            NewGame();
        }

        foreach (IDataPersistence item in dataPersistenceObjects)
        {
            item.LoadData(gameData);            
        }
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    List<IDataPersistence> GetDataPersistenceObjects()
    {
        List <IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>().ToList();
        return dataPersistenceObjects;
    }
}

using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    string dataDirPath = "";
    string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public SettingsData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        //fullPath = fullPath.Replace("\\", "/");
        Debug.Log(fullPath);
        SettingsData loadedData = null;
        if (File.Exists(fullPath))
        {
            Debug.Log("File exists");
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // deserialize
                loadedData = JsonUtility.FromJson<SettingsData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("error when loading file:" + fullPath + e);
            }
        }
        return loadedData;
    }
    public void Save(SettingsData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        //fullPath = fullPath.Replace("\\", "/");
        Debug.Log(fullPath +"save path");
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("error when saving file:" + fullPath + e);
        }
    }
}

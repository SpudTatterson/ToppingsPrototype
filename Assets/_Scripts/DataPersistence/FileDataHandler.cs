using System;
using System.IO;
using UnityEngine;
//handles file loading and saving for any class that inherits from the data class
public class FileDataHandler<T> where T : Data
{
    string dataDirPath = "";
    string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public T Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        //fullPath = fullPath.Replace("\\", "/");
        Debug.Log(fullPath);
        T loadedData = null;
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
                loadedData = JsonUtility.FromJson<T>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("error when loading file:" + fullPath + e);
            }
        }
        return loadedData;
    }
    public void Save(T data)
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

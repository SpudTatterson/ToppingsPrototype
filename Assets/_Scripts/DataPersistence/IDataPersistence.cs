

public interface IDataPersistence 
{
    
}

public interface ISettingDataPersistence : IDataPersistence
{
    public void SaveData(SettingsData data);
    public void LoadData(SettingsData data);
}

public interface IPlayerDataPersistence : IDataPersistence
{
    public void SaveData(GameData data);
    public void LoadData(GameData data);
}